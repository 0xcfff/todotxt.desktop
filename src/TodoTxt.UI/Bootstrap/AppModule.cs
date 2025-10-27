namespace TodoTxt.UI.Bootstrap;
using Autofac;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoTxt.UI.App.App>().As<TodoTxt.UI.App.IApp>().SingleInstance();
    }
}