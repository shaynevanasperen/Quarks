using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class FakeUrlHelper : UrlHelper
	{
		internal static Action<RouteCollection> RouteRegistration { get; set; }

		static readonly Action<RouteCollection> defaultRouteRegistration = routes =>
			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" });

		internal FakeUrlHelper()
			: this("http://localhost/") { }

		internal FakeUrlHelper(string currentUrl)
			: this(currentUrl, ConfigureRouteCollection()) { }

		internal FakeUrlHelper(RequestContext context)
			: base(context, ConfigureRouteCollection()) { }

		internal FakeUrlHelper(string currentUrl, RouteCollection routes)
			: base(FakeRequestContext.For(currentUrl), routes) { }

		internal static RouteCollection ConfigureRouteCollection()
		{
			var routes = new RouteCollection();
			var registerRoutes = RouteRegistration ?? defaultRouteRegistration;
			registerRoutes(routes);
			return routes;
		}
	}
}
