﻿using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPowerTools.Routing
{
    public static class Extensions
    {
        /// <summary>
        /// Removes the namespace root from the provided string
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="namespace">Usually the namespace of the controller</param>
        /// <returns></returns>
 
        public static string StripNamespaceRoot(this RoutingConventionsSettings settings, string @namespace)
         {
             return @namespace.Remove(0, settings.NamespaceRoot.Length);
         }

        /// <summary>
        /// Gets the controller name without the "Controler" suffix
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetControllerName(this ActionCall action)
        {
            var name = action.Controller.Name;
            return name.Substring(0, name.Length - 10);
        }


        /// <summary>
        /// Scans assembly and registers policies. Uses dependency resolver
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="res">Null means it uses the current dependecy resolver</param>
        public static RoutingConventions RegisterConventions(this RoutingConventions policy, Assembly asm, IDependencyResolver res = null)
        {
            if (res == null)
            {
                res = DependencyResolver.Current;
            }
            
            foreach(var builder in asm.GetTypesDerivedFrom<IBuildRoutes>())
            {
                policy.Add(res.GetService(builder) as IBuildRoutes);
            }
            
            foreach(var modifier in asm.GetTypesDerivedFrom<IModifyRoute>())
            {
                policy.Add(res.GetService(modifier) as IModifyRoute);
            }

            policy.LoadModule(asm.GetTypesDerivedFrom<RoutingConventionsModule>(true).Select(t=>(RoutingConventionsModule)res.GetService(t)).ToArray());
            
            return policy;
        }

        /// <summary>
        /// Register types deriving from Controller class
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static RoutingConventions RegisterControllers(this RoutingConventions policy,Assembly asm)
        {
            return RegisterController(policy,asm.GetControllerTypes().ToArray());          
        }
        ///// <summary>
        ///// Register as actions types matching a criteria
        ///// </summary>
        ///// <param name="policy"></param>
        ///// <param name="asm"></param>
        ///// <param name="match"></param>
        //public static RoutingConventions RegisterController(this RoutingConventions policy, Assembly asm, Func<Type, bool> match)
        //{
        //    RegisterController(policy, asm.GetTypes().Where(match).ToArray());
        //    return policy;
        //}

        /// <summary>
        /// Registers actions from controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="policy"></param>
        public static RoutingConventions RegisterController<T>(this RoutingConventions policy) where T : Controller
        {
            policy.RegisterController(typeof(T));
            return policy;
        }

        /// <summary>
        /// Registers actions from the provided types.
        /// All types should be Controllers
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="controllers"></param>
        public static RoutingConventions RegisterController(this RoutingConventions policy, params Type[] controllers)
        {
            foreach (var c in controllers)
            {
                c.GetActionMethods().ForEach(m =>
                {
                    policy.AddAction(new ActionCall(m, policy.Settings));
                }); 
            }
            return policy;
        }

        public static RoutingConventions RegisterHandlerConvention(this RoutingConventions policy)
        {
            policy.Add(new HandlerRouteConvention());
            return policy;
        }


        public static void ConstrainToGet(this Route route)
        {
            route.Constrain("GET");
        }


        public static void ConstrainToPost(this Route route)
        {
            route.Constrain("POST");
        }

        static void Constrain(this Route route, string method)
        {
            route.MustNotBeNull();
            route.Constraints["method"] = method;
        }

        public static IConfigureAction Always(this IConfigureRoutingConventions cfg)
        {
            return cfg.If(d => true);
        }
    }
}