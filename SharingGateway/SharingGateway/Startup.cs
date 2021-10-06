using AgileServiceBus.Additionals;
using AgileServiceBus.Drivers;
using AgileServiceBus.Interfaces;
using AgileServiceBus.Tracing;
using AgileServiceBus.Utilities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SharingGateway.Validators;
using System.Text;

namespace SharingGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //cors
            services.AddCors(coo => coo.AddPolicy("CorsPolicy", cpb =>
            {
                cpb.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            //controllers
            services.AddControllers()
                .AddNewtonsoftJson(mnj => JsonConverter.SetUp(mnj.SerializerSettings))
                .AddFluentValidation(fvm =>
                {
                    fvm.RegisterValidatorsFromAssemblyContaining<AuthorizationValidator>();
                    fvm.RegisterValidatorsFromAssemblyContaining<POIFilterValidator>();
                    fvm.RegisterValidatorsFromAssemblyContaining<StoryValidator>();
                });

            //swagger
            services.AddSwaggerGen();

            //jwt
            services.AddAuthentication(aou =>
            {
                aou.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                aou.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jbo =>
            {
                jbo.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateActor = false,
                    ValidateIssuerSigningKey = false,
                    ValidateTokenReplay = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.Get("JWT_KEY"))),
                    ValidAudience = Env.Get("JWT_AUDIENCE")
                };
            });

            //tracer
            services.AddSingleton<Tracer, DefaultTracer>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ITraceScope>(spe =>
            {
                ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)spe.GetService<IActionContextAccessor>().ActionContext.ActionDescriptor;
                string controllerName = actionDescriptor.ControllerTypeInfo.FullName + "." + actionDescriptor.MethodInfo.Name;

                return new TraceScope(controllerName, spe.GetService<Tracer>());
            });

            //bus
            services.AddSingleton<IGatewayBus>(spe => new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //development
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //cors
            app.UseCors("CorsPolicy");

            //controllers
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(sui =>
            {
                sui.SwaggerEndpoint("/swagger/v1/swagger.json", "SharingGateway");
                sui.RoutePrefix = string.Empty;
            });
        }
    }
}