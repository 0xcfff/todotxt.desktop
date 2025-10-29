namespace TodoTxt.UI.MarkupExtensions;
using Autofac;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;
public class DIExtension : MarkupExtension
{
    internal static Autofac.IComponentContext? Container;
    public string? Name { get; set; }
    public Type? Type { get; set; }
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Container == null)
        {
            throw new InvalidOperationException("DI Extension is not initialized");
        }

        if (Name != null && Type != null)
        {
            return Container.ResolveNamed(Name!, Type!);
        }
        if (Type != null)
        {
            return Container.Resolve(Type!);
        }
        throw new InvalidOperationException("Type or Type and Name should be set");
    }

    internal static IDisposable BeginDIScope(Autofac.IComponentContext container)
    {
        return new DIScope(container);
    }

    class DIScope : IDisposable
    {
        private readonly IComponentContext _container;
        public DIScope(IComponentContext container)
        {
            _container = container;
            DIExtension.Container = _container;
        }

        public void Dispose()
        {
            Interlocked.CompareExchange(ref DIExtension.Container, null, _container);
        }
    }
}

public static class DIExtensionExtensions
{
    /// <summary>
    /// Begin a new DI scope based on the provided container
    /// </summary>
    /// <param name="container">The container to be used as the scope</param>
    /// <returns></returns>
    public static IDisposable BeginXamlDIScope(this IComponentContext container)
    {
        return DIExtension.BeginDIScope(container);
    }
}