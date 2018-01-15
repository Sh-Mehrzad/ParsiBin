// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapDependencyScope.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using System.Web;

namespace ParsiBin.UI.DependencyResolution
{
    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope : ServiceLocatorImplBase, IDependencyScope
    {
        #region Constants and Fields

        /// <summary>
        /// The container.
        /// </summary>
        //protected readonly IContainer Container;
        private const string NestedContainerKey = "Nested.Container.Key";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyScope"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        #endregion

        #region Public Properties

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer
        {
            get
            {
                return (IContainer)HttpContext.Items[NestedContainerKey];
            }
            set
            {
                HttpContext.Items[NestedContainerKey] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            DisposeNestedContainer();
            Container.Dispose();
        }

        public void DisposeNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                CurrentNestedContainer.Dispose();
                CurrentNestedContainer = null;
            }
        }

        public void CreateNestedContainer()
        {
            if (CurrentNestedContainer != null)
            {
                return;
            }
            CurrentNestedContainer = Container.GetNestedContainer();
        }

        /// <summary>
        /// The get services.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; System.Object].
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        #endregion

        #region Properties

        private HttpContextBase HttpContext
        {
            get
            {

                var ctx = Container.TryGetInstance<HttpContextBase>();
                return ctx ?? new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        ///        resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">
        /// Type of service requested.
        /// </param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        ///        the requested service instance.
        /// </summary>
        /// <param name="serviceType">
        /// Type of instance requested.
        /// </param>
        /// <param name="key">
        /// Name of registered service you want. May be null.
        /// </param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? this.Container.TryGetInstance(serviceType)
                           : this.Container.GetInstance(serviceType);
            }

            return this.Container.GetInstance(serviceType, key);
        }

        #endregion
    }
}
