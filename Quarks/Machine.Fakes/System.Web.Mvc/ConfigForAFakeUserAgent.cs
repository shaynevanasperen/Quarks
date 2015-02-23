using System.Web;
using System.Web.Mvc;
using Machine.Fakes;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class ConfigForAFakeUserAgent
	{
		static string _userAgent;

		internal ConfigForAFakeUserAgent(string userAgent)
		{
			_userAgent = userAgent;
		}

		OnEstablish context = ctx =>
		{
			var request = ctx.An<HttpRequestBase>();
			request.WhenToldTo(req => req.UserAgent).Return(_userAgent);
			var httpContext = ctx.An<HttpContextBase>();
			httpContext.WhenToldTo(x => x.Request).Return(request);
			ctx.Configure(new ControllerContext { HttpContext = httpContext });
		};
	}
}
