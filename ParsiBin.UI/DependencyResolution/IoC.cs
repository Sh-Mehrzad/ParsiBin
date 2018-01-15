// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
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


using ParsiBin.DataLayer;
using ServiceLayer.EFServices;
using ServiceLayer.Interfaces;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Threading;
using System.Web.Security;

namespace ParsiBin.UI.DependencyResolution {
    public static class IoC {

        //private static readonly Lazy<Container> _containerBuilder =
        //   new Lazy<Container>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);


        
        public static IContainer Initialize() {
            var container = new Container(x => 
            {
                x.For<IUnitOfWork>().Singleton().Use(new ParsContext());
                x.For<ITournament>().Singleton().Use<TournamentService>();
                x.For<ISportType>().Singleton().Use<SportTypeService>();
                x.For<IParticipant>().Singleton().Use<ParticipantService>();
                x.For<IGroup>().Singleton().Use<GroupService>();
                x.For<ICountry>().Singleton().Use<CountryService>();
                x.For<IReferee>().Singleton().Use<RefereeService>();
                x.For<ICity>().Singleton().Use<CityService>();
                x.For<Istadium>().Singleton().Use<StadiumService>();
                x.For<IMatch>().Singleton().Use<MatchService>();
                x.For<IPartInGroup>().Singleton().Use<PartInGroupService>();
                x.For<IpartInMatch>().Singleton().Use<PartInMatchService>();
                x.For<IScoreTitle>().Singleton().Use<ScoreTitleService>();
                x.For<IMatchScore>().Singleton().Use<MatchScoreService>();
                x.For<IUser>().Singleton().Use<UserService>();
                x.For<IverifyUser>().Singleton().Use<verifyUserService>();
                x.For<IRoleProvider>().Singleton().Use<RoleProviderService>();
                x.For<IUserPrediction>().Singleton().Use<UserPredictionService>();
            });
            //return new Container(x => x.For<ITournament>().Singleton().Use<TournamentService>());            
            return container;
        }
    }
}