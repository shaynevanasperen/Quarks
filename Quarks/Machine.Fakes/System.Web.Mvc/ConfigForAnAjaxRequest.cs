using System.Web;
using System.Web.Mvc;
using Machine.Fakes;

namespace Quarks.Machine.Fakes.System.Web.Mvc
{
	class ConfigForAnAjaxRequest
	{
		OnEstablish context = ctx =>
		{
			var ajaxRequest = ctx.An<HttpRequestBase>();
			ajaxRequest.WhenToldTo(x => x["X-Requested-With"]).Return("XMLHttpRequest");
			var httpContext = ctx.An<HttpContextBase>();
			httpContext.WhenToldTo(x => x.Request).Return(ajaxRequest);
			ctx.Configure(new ControllerContext { HttpContext = httpContext });
		};
	}
}
