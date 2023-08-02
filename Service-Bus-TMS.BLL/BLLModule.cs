using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service_Bus_TMS.BLL.Services;
using Service_Bus_TMS.DAL;


namespace Service_Bus_TMS.BLL;

public class BLLModule
{
    public static void Load(IServiceCollection services, IConfiguration configuration)
    {
        // BLL Services
        services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
        services.AddTransient<ITaskService, TaskService>();

        DALModule.Load(services, configuration);
    }
}