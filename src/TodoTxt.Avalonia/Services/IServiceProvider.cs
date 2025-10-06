using System;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Simple service provider interface for dependency injection
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Registers a service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="instance">The service instance</param>
        void RegisterInstance<T>(T instance) where T : class;

        /// <summary>
        /// Registers a service factory
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="factory">The factory function to create the service</param>
        void RegisterFactory<T>(Func<T> factory) where T : class;

        /// <summary>
        /// Gets a service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance, or null if not registered</returns>
        T? GetService<T>() where T : class;

        /// <summary>
        /// Gets a required service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        /// <exception cref="InvalidOperationException">Thrown if service is not registered</exception>
        T GetRequiredService<T>() where T : class;

        /// <summary>
        /// Checks if a service is registered
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>True if the service is registered</returns>
        bool IsRegistered<T>() where T : class;

        /// <summary>
        /// Unregisters a service
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        void Unregister<T>() where T : class;
    }
}
