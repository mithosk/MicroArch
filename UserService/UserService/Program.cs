using Topshelf;

namespace UserService
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(hco =>
            {
                hco.Service<Service>(sce =>
                {
                    sce.ConstructUsing(hso => new Service());
                    sce.WhenStarted(ser => ser.Start());
                    sce.WhenStopped(ser => ser.Stop());
                });

                hco.RunAsLocalService();
            });
        }
    }
}