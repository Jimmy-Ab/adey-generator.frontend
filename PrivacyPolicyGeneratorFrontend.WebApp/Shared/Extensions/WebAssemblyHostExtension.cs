using System;
using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Shared.Extensions
{
    public static class WebAssemblyHostExtension
    {
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
            var localStore = host.Services.GetRequiredService<ILocalStorageService>();
            var cultureFromLS = await localStore.GetItemAsync<string>("_lang");

            CultureInfo culture;

            if(cultureFromLS != null)
            {
                culture = new CultureInfo(cultureFromLS);
            }
            else
            {
                culture = new CultureInfo("en-US");
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}

