using FinancialModeling.CustomHandler;
using FinancialModeling.Models;
using FinancialModeling.Repository;
using FinancialModeling.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Unity;

namespace FinancialModeling
{
    public static class WebApiConfig
    {
        static string webUrl = ConfigurationManager.AppSettings["WebUrl"].ToString();
        static bool addApiCors = Convert.ToBoolean(ConfigurationManager.AppSettings["AddApiCors"]);
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var container = new UnityContainer();
            container.RegisterType<FinancialModelingDbContext, FinancialModelingDbContext>();
            container.RegisterType<ILookupRepository, LookupRepository>();
            container.RegisterType<ILookupService, LookupService>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<IProjectionRepository, ProjectionRepository>();
            container.RegisterType<IProjectionService, ProjectionService>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IExportService, ExportService>();


            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Registering GlobalExceptionHandler
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            //Registering UnhandledExceptionLogger
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            if (addApiCors)
            {
                var cors = new EnableCorsAttribute(webUrl, "*", "*");
                config.EnableCors(cors);
            }

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
