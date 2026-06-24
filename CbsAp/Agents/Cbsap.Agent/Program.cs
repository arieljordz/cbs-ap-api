using Cbsap.Agent;
using CbsAp.Application;
using CbsAp.Application.Features.AutoMatching;
using CbsAp.Infrastracture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System.Reflection;
using CbsAp.Infrastracture.Extensions;

System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        //services.Configure<QuartzOptions>(hostContext.Configuration.GetSection("Quartz"));

        // Bind the "Quartz" section from appsettings.json to Quartz options        
        // Add the required Quartz.NET services
        services.AddQuartz(q =>
        {
            // Use a Scoped container to create jobs. I'll touch on this later
            // q.UseMicrosoftDependencyInjectionJobFactory();
            //var jobKey = new JobKey("test");
            //q.AddJob<MyJob>(opts => opts.WithIdentity(jobKey));

            //// Create a trigger for the job
            //q.AddTrigger(opts => opts
            //    .ForJob(jobKey) // link to the HelloWorldJob
            //    .WithIdentity("Test-trigger") // give the trigger a unique name
            //                                           //.WithCronSchedule("0/5 * * * * ?")); // run every 
            //    .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()));

            // Configure the XML plugin metadata directly using property dictionary strings
            q.SetProperty("quartz.plugin.xml.type", "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins");
            q.SetProperty("quartz.plugin.xml.fileNames", "quartz_jobs.xml");

            // Optional: Fail app startup if the XML file is missing or corrupted
            q.SetProperty("quartz.plugin.xml.failOnFileNotFound", "true");
            //            var serviceProvider = services.BuildServiceProvider();



            // q.AddJobAndTrigger(serviceProvider, services, hostContext.Configuration);
        });

        services.Configure<QuartzOptions>(hostContext.Configuration.GetSection("Quartz"));
        services.AddQuartz(q => { /* No extra code required inside here anymore */ });

        var assembly = Assembly.GetExecutingAssembly();

        services
             .AddAppSettings(hostContext.Configuration)
            .AddApplication()
            .AddInfrastructure(hostContext.Configuration);

        //services.AddMediatR(configuration =>
        //configuration.RegisterServicesFromAssembly(assembly));

        //services. AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        //{
        //    // Resolve your custom AppSettings service from the container
        //    var appSettings = serviceProvider.GetRequiredService<AppSettings>();

        //    // Chain your EF Core configurations correctly
        //    options.UseLazyLoadingProxies()
        //           .UseSqlServer(appSettings.ConnectionString, builder =>
        //               builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
        //           .EnableSensitiveDataLogging();
        //});

        services.AddLogging(hostContext.Configuration);

        // Add the Quartz.NET hosted service

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);





        //services.AddSingleton(provider =>
        //{
        //    var schedulerFactory = new StdSchedulerFactory();
        //    var scheduler = schedulerFactory.GetScheduler().Result;

        //    scheduler.Start();

        //    var job = JobBuilder.Create<MyJob>()
        //                       .Build();

        //    var trigger = TriggerBuilder.Create()
        //                               //.WithIdentity("hourlyTrigger", "default")
        //                               .StartNow()
        //                               .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
        //                               .Build();

        //    scheduler.ScheduleJob(job, trigger);

        //    return scheduler;
        //});


        
    });


var host = builder

    .Build();

//builder.UseSerilog((context, configuration) =>
//    configuration.ReadFrom.Configuration(context.Configuration));




//Registering Logging
//var appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

// Bind the "Quartz" section from appsettings.json to Quartz options



host.Run();

