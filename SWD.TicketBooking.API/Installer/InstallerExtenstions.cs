using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace SWD.TicketBooking.API.Installer
{
    public static class InstallerExtenstions
    {
        public static void InstallerServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installer = typeof(Program).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installer.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
