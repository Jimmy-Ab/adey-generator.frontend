using System;
using PrivacyPolicyGeneratorFrontend.WebApp.Shared.Resources;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        private Blazored.LocalStorage.ILocalStorageService? localStore { get; set; }

        string _selectedLang = "EN";

        protected override async Task OnInitializedAsync()
        {
            var lang = await localStore.GetItemAsync<string>("_lang");

            if (lang == "am-ET")
            {
                _selectedLang = "አማ";
            }
            else
            {
                _selectedLang = "EN";
            }

            StateHasChanged();
        }

        public void OnAmharicClicked()
        {
            localStore.SetItemAsync<string>("_lang", "am-ET");
            Console.WriteLine(_navigationManager.BaseUri);

            if (_navigationManager.Uri.Replace("#", "") == _navigationManager.BaseUri)
            {
                _navigationManager.NavigateTo(_navigationManager.BaseUri + "home", true);
            }
            else
            {
                _navigationManager.NavigateTo(_navigationManager.Uri, true);
            }
            Console.WriteLine(_navigationManager.Uri);
        }

        public void OnEnglishClicked()
        {
            localStore.SetItemAsync<string>("_lang", "en-US");
            Console.WriteLine(_navigationManager.Uri);

            if (_navigationManager.Uri.Replace("#", "") == _navigationManager.BaseUri)
            {
                _navigationManager.NavigateTo(_navigationManager.BaseUri + "home", true);
            }
            else
            {
                _navigationManager.NavigateTo(_navigationManager.Uri, true);
            }
        }

        void OnAboutUsClicked()
        {
            _navigationManager.NavigateTo("https://adey-meselesh.de/", true);
        }
    }
}