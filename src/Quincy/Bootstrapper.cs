using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using StructureMap;

namespace Quincy
{
    public abstract class Bootstrapper : BootstrapperBase
    {
        private IContainer container;

        protected Bootstrapper(bool useApplication = true)
            : base(useApplication)
        {
        }

        protected override void Configure()
        {
            container = ConfigureContainer();
        }

        protected abstract IContainer ConfigureContainer();

        protected override object GetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? container.GetInstance(serviceType)
                : container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType).OfType<object>();
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblyPath = GetAssemblyPath();
            foreach (var file in EnumerateValidFiles(assemblyPath, "*.dll").Concat(EnumerateValidFiles(assemblyPath, "*.exe")))
            {
                Assembly assembly = null;
                try
                {
                    assembly = AppDomain.CurrentDomain.Load(Path.GetFileNameWithoutExtension(file));
                }
                catch (Exception)
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(file);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                if (assembly != null)
                    yield return assembly;
            }
        }

        protected virtual string GetAssemblyPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private IEnumerable<string> EnumerateValidFiles(string assemblyPath, string searchPattern)
        {
            return Directory.EnumerateFiles(assemblyPath, searchPattern).Where(IsFileValid);
        }

        protected virtual bool IsFileValid(string filePath)
        {
            return true;
        }
    }
}