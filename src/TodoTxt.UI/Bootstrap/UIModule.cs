namespace TodoTxt.UI.Bootstrap;
using Autofac;
using TodoTxt.UI.Services;

public class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // TODO: Move to CoreModule during settings service refactoring
        builder.RegisterType<ServiceProvider>().As<IServiceProvider>().SingleInstance();

        // Dialogs
        builder.RegisterType<TodoTxt.UI.Dialogs.DeleteConfirmationDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.AppendTextDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.SetPriorityDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.SetDueDateDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.PostponeDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.HelpDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.FilterDialog>().AsSelf().InstancePerDependency();
        builder.RegisterType<TodoTxt.UI.Dialogs.OptionsDialog>().AsSelf().InstancePerDependency();

        // Main Window
        builder.RegisterType<TodoTxt.UI.Views.MainWindow>().AsSelf().SingleInstance();
        builder.RegisterType<TodoTxt.UI.ViewModels.MainWindowViewModel>().AsSelf().SingleInstance();
    }
}