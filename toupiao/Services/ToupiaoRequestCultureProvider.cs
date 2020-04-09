using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace toupiao.Services
{
    public class ToupiaoRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var CULTURE_String="CULTURE";
            var CultureCookie = httpContext.Request.Cookies[CULTURE_String]?.ToString() ?? "";  

            var _SupportedUICultures = httpContext.RequestServices
                    .GetService<IOptions<RequestLocalizationOptions>>()
                    .Value.SupportedUICultures.Select(c => c.Name).ToList();

            CultureCookie =( string.IsNullOrEmpty(CultureCookie) || !_SupportedUICultures.Contains(CultureCookie))?"zh-Hans":CultureCookie;  

            return Task.FromResult(new ProviderCultureResult(CultureCookie));
        }
    }

}