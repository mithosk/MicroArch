using AgileServiceBus.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SharingGateway.Filters
{
    public class BusExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RemoteException exception)
                switch (exception.Code)
                {
                    case "MAIN_OBJECT_NOT_FOUND":
                        context.Result = new NotFoundResult();
                        break;

                    case "REFERENCE_OBJECT_NOT_FOUND":
                        context.Result = new ForbidResult();
                        break;
                }
        }
    }
}