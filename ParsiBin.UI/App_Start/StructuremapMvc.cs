// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
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

using System.Web.Http;
using System.Web.Mvc;
using StructureMap;
using ParsiBin.UI.DependencyResolution;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ParsiBin.UI.App_Start.StructuremapMvc), "Start")]

namespace ParsiBin.UI.App_Start {
    public static class StructuremapMvc {

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }
        public static void Start() {
			IContainer container = IoC.Initialize();
            //DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);

            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }
    }
}