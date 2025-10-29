namespace TodoTxt.UI.Bootstrap;
using Autofac;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoTxt.UI.App.CmdLineOptions>().As<TodoTxt.UI.App.CmdLineOptions>().SingleInstance();
        builder.RegisterType<TodoTxt.UI.App.App>().As<TodoTxt.UI.App.IApp>().SingleInstance();
        builder.RegisterType<TodoTxt.UI.Application>().As<Avalonia.Application>().SingleInstance();
    }
}