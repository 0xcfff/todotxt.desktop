using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Simple implementation of IServiceProvider for dependency injection
    /// </summary>
    public class ServiceProvider : IServiceProvider, IDisposable
    {
        private readonly Dictionary<Type, object> _instances = new();
        private readonly Dictionary<Type, Func<object>> _factories = new();
        private bool _disposed = false;

        /// <summary>
        /// Registers a service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="instance">The service instance</param>
        public void RegisterInstance<T>(T instance) where T : class
        {
            ThrowIfDisposed();
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var serviceType = typeof(T);
            _instances[serviceType] = instance;
            _factories.Remove(serviceType); // Remove any existing factory
        }

        /// <summary>
        /// Registers a service factory
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="factory">The factory function to create the service</param>
        public void RegisterFactory<T>(Func<T> factory) where T : class
        {
            ThrowIfDisposed();
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var serviceType = typeof(T);
            _factories[serviceType] = () => factory();
            _instances.Remove(serviceType); // Remove any existing instance
        }

        /// <summary>
        /// Gets a service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance, or null if not registered</returns>
        public T? GetService<T>() where T : class
        {
            ThrowIfDisposed();
            var serviceType = typeof(T);

            // Check for registered instance first
            if (_instances.TryGetValue(serviceType, out var instance))
            {
                return (T)instance;
            }

            // Check for registered factory
            if (_factories.TryGetValue(serviceType, out var factory))
            {
                return (T)factory();
            }

            return null;
        }

        /// <summary>
        /// Gets a required service instance
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance</returns>
        /// <exception cref="InvalidOperationException">Thrown if service is not registered</exception>
        public T GetRequiredService<T>() where T : class
        {
            var service = GetService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");
            }
            return service;
        }

        /// <summary>
        /// Checks if a service is registered
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>True if the service is registered</returns>
        public bool IsRegistered<T>() where T : class
        {
            ThrowIfDisposed();
            var serviceType = typeof(T);
            return _instances.ContainsKey(serviceType) || _factories.ContainsKey(serviceType);
        }

        /// <summary>
        /// Unregisters a service
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        public void Unregister<T>() where T : class
        {
            ThrowIfDisposed();
            var serviceType = typeof(T);
            _instances.Remove(serviceType);
            _factories.Remove(serviceType);
        }

        /// <summary>
        /// Disposes of the service provider and all registered services
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                // Dispose of all registered instances that implement IDisposable
                foreach (var instance in _instances.Values)
                {
                    if (instance is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                _instances.Clear();
                _factories.Clear();
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ServiceProvider));
            }
        }
    }
}

