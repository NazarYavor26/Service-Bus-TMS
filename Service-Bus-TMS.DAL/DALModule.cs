﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service_Bus_TMS.DAL.DbContexts;
using Service_Bus_TMS.DAL.Repositories;

namespace Service_Bus_TMS.DAL;

public class DALModule
{
    public static void Load(IServiceCollection services, IConfiguration configuration)
    {
        // DAL Services
        services.AddDbContext<AppDbContext>();
        services.AddTransient<ITaskRepository, TaskRepository>();
    }
}