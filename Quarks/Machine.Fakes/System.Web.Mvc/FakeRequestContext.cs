using System;
using System.Web;
using System.Web.Routing;
using Machine.Fakes;
using Quarks.System.Web;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class FakeRequestContext : WithFakes
	{
		/// <summary>
		/// Return a fake RequestContext that can be used to create a stub UrlHelper
		/// </summary>
		/// <param name="url">Url to fake a request for.</param>
		internal static RequestContext For(string url)
		{
			if (_specificationController == null)
				throw new InvalidOperationException(
					"Test context must inherit 'WithFakes' to use FakeUrlHelper or FakeRequestContext.");

			// MvcContrib.FakeHttpRequest doesn't implement RawUrl.
			var request = An<HttpRequestBase>();
			var uri = new Uri(url);
			request.WhenToldTo(x => x.Url).Return(uri);
			request.WhenToldTo(x => x.RawUrl).Return(uri.AbsoluteUri.Substring(uri.GetLeftPart(UriPartial.Authority).Length));
			// Used by PathHelpers.GenerateClientUrl(...)
			request.WhenToldTo(x => x.ApplicationPath).Return("/");

			var httpContext = An<HttpContextBase>();
			httpContext.WhenToldTo(x => x.Request).Return(request);
			// Used by RouteCollection.GetUrlWithApplicationPath(...)
			httpContext.WhenToldTo(x => x.Response).Return(new FakeHttpResponse());

			var routeData = An<RouteData>();
			var requestContext = An<RequestContext>();
			requestContext.WhenToldTo(x => x.HttpContext).Return(httpContext);
			// Used by UrlHelper.GenerateUrl(...)
			requestContext.WhenToldTo(x => x.RouteData).Return(routeData);

			return requestContext;
		}
	}

	class FakeRequestContext<T> : WithSubject<T> where T : class
	{
		/// <summary>
		/// Return a fake RequestContext that can be used to create a stub UrlHelper
		/// </summary>
		/// <param name="url">Url to fake a request for.</param>
		internal static RequestContext For(string url)
		{
			if (_specificationController == null)
				throw new InvalidOperationException(
					"Test context must inherit 'WithSubject' to use FakeUrlHelper or FakeRequestContext.");

			// MvcContrib.FakeHttpRequest doesn't implement RawUrl.
			var request = An<HttpRequestBase>();
			var uri = new Uri(url);
			request.WhenToldTo(x => x.Url).Return(uri);
			request.WhenToldTo(x => x.RawUrl).Return(uri.AbsoluteUri.Substring(uri.GetLeftPart(UriPartial.Authority).Length));
			// Used by PathHelpers.GenerateClientUrl(...)
			request.WhenToldTo(x => x.ApplicationPath).Return("/");

			var httpContext = An<HttpContextBase>();
			httpContext.WhenToldTo(x => x.Request).Return(request);
			// Used by RouteCollection.GetUrlWithApplicationPath(...)
			httpContext.WhenToldTo(x => x.Response).Return(new FakeHttpResponse());

			var routeData = An<RouteData>();
			var requestContext = An<RequestContext>();
			requestContext.WhenToldTo(x => x.HttpContext).Return(httpContext);
			// Used by UrlHelper.GenerateUrl(...)
			requestContext.WhenToldTo(x => x.RouteData).Return(routeData);

			return requestContext;
		}
	}
}
