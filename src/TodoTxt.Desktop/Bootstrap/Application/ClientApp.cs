using Autofac;

namespace TodoTxt.Desktop.Bootstrap.Application;

public class ClientApp {
    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<TodoTxt.Core.Bootstrap.CoreModule>();
        builder.RegisterModule<TodoTxt.UI.Bootstrap.UIModule>();
        builder.RegisterModule<TodoTxt.UI.Bootstrap.AppModule>();
        return builder.Build();
    }

    public int Run(string[] args)
    {
        var container = CreateContainer();
        var app = container.Resolve<TodoTxt.UI.App.IApp>();
        return app.Run(args);
    }
}