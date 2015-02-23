using System.Web;
using System.Web.Mvc;
using Machine.Fakes;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class ConfigForAnHttpRequest
	{
		OnEstablish context = ctx =>
		{
			var httpRequest = ctx.An<HttpRequestBase>();
			httpRequest.WhenToldTo(x => x["X-Requested-With"]).Return((string)null);
			var httpContext = ctx.An<HttpContextBase>();
			httpContext.WhenToldTo(x => x.Request).Return(httpRequest);
			ctx.Configure(new ControllerContext { HttpContext = httpContext });
		};
	}
}
