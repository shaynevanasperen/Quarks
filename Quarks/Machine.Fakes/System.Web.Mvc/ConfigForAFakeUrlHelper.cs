using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Machine.Fakes;
using Quarks.System.Web;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class ConfigForAFakeUrlHelper
	{
		public ConfigForAFakeUrlHelper()
		{
			_currentUrl = "http://localhost/";
		}

		internal ConfigForAFakeUrlHelper(string currentUrl)
		{
			_currentUrl = currentUrl;
		}

		OnEstablish context = ctx =>
		{
			createHttpRequest(ctx);
			createHttpContext(ctx);
			createRequestContext(ctx);
			createUrlHelper(ctx);
		};

		static string _currentUrl;

		static void createUrlHelper(IFakeAccessor context)
		{
			context.Configure(FakeUrlHelper.ConfigureRouteCollection());
			context.Configure(new UrlHelper(context.The<RequestContext>(), context.The<RouteCollection>()));
		}

		static void createHttpRequest(IFakeAccessor context)
		{
			// MvcContrib.FakeHttpRequest doesn't implement RawUrl.
			var uri = new Uri(_currentUrl);
			context.The<HttpRequestBase>().WhenToldTo(x => x.Url).Return(uri);
			context.The<HttpRequestBase>().WhenToldTo(x => x.RawUrl)
				.Return(uri.AbsoluteUri.Substring(uri.GetLeftPart(UriPartial.Authority).Length));
			// Used by PathHelpers.GenerateClientUrl(...)
			context.The<HttpRequestBase>().WhenToldTo(x => x.ApplicationPath).Return("/");
		}

		static void createHttpContext(IFakeAccessor context)
		{
			context.The<HttpContextBase>().WhenToldTo(x => x.Request).Return(context.The<HttpRequestBase>());
			// Used by RouteCollection.GetUrlWithApplicationPath(...)
			context.The<HttpContextBase>().WhenToldTo(x => x.Response).Return(new FakeHttpResponse());
		}

		static void createRequestContext(IFakeAccessor context)
		{
			var routeData = context.An<RouteData>();
			var requestContext = context.The<RequestContext>();
			requestContext.WhenToldTo(x => x.HttpContext).Return(context.The<HttpContextBase>());
			// Used by UrlHelper.GenerateUrl(...)
			requestContext.WhenToldTo(x => x.RouteData).Return(routeData);
		}
	}
}
