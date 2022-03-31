using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using System.Web.Http;
using NSwag.AspNet.Owin;
using NSwag.Generation.Processors.Security;
using NSwag;
using Lawave.Security;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(Lawave.Startup))]
namespace Lawave
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            // Any connection or hub wire up and configuration should go here
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll); // You can modify the CorsOptions
                map.RunSignalR(new HubConfiguration { EnableJSONP = true });
            });

            var config = new HttpConfiguration();
            app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
            {
                //針對RPC-Style WebAPI，指定路由包含Action名稱
                settings.GeneratorSettings.DefaultUrlTemplate =
                    "api/{controller}/{action}/{id?}";
                //可加入客製化調整邏輯
                settings.PostProcess = document =>
                {
                    document.Info.Title = "WebAPI 範例";
                };
                //加入Api Key定義
                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("Go", new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Type into the textbox: Go {your JWT token}.",
                    In = OpenApiSecurityApiKeyLocation.Header,
                     Scheme = "Go" // 不填寫會影響 Filter 判斷錯誤
                }));
                //REF: https://github.com/RicoSuter/NSwag/issues/1304
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("Go"));
            });
            app.UseWebApi(config);
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

        }

        /// <summary>
        /// 啟用跨域及驗證配置
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureAuth(IAppBuilder app)
        {
            // 建立 OAuth 配置
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                Provider = new AuthorizationServerProvider()
            };

            // 啟用 OAuth2 bearer tokens 驗證並加入配置
            app.UseOAuthAuthorizationServer(oAuthOptions);
        }

    }
}
