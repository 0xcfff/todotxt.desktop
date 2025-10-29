using Autofac;
using TodoTxt.Core.CrossCutting.Logging;

namespace TodoTxt.Desktop.Bootstrap.Application;

public class ClientApp {
    private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

    public int Run(string[] args)
    {
        PreProcessCommandLine(args);

        var container = CreateContainer();
        var app = container.Resolve<TodoTxt.UI.App.IApp>();
        return app.Run(args);
    }

    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<TodoTxt.Core.Bootstrap.CoreModule>();
        builder.RegisterModule<TodoTxt.UI.Bootstrap.UIModule>();
        builder.RegisterModule<TodoTxt.UI.Bootstrap.AppModule>();
        return builder.Build();
    }

    private static void PreProcessCommandLine(string[] args){
        try {
            var opts = TodoTxt.UI.App.CmdLineOptions.Parse(args);
            LogConfigurer.ConfigureDefaultLogging(opts.IsDebug, opts.LogFileName);
        }
        catch (Exception) {
            LogConfigurer.ConfigureAppFaultLogging();
            throw;
        }
   }


}