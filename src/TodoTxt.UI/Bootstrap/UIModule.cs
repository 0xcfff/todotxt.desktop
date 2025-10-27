namespace TodoTxt.UI.Bootstrap;
using Autofac;
using TodoTxt.UI.Services;

public class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // TODO: Move to CoreModule during settings service refactoring
        builder.RegisterType<ServiceProvider>().As<IServiceProvider>().SingleInstance();

        // the application 
        builder.RegisterType<TodoTxt.UI.Application>().As<Avalonia.Application>().SingleInstance();
    }
}