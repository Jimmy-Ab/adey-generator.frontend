
using System;
using Microsoft.AspNetCore.Components;
using PrivacyPolicyGeneratorFrontend.WebService.ViewModels;
using PrivacyPolicyGeneratorFrontend.WebService.Services;
using Microsoft.JSInterop;
using System.Text.Json;
using Blazored.LocalStorage;
using PrivacyPolicyGeneratorFrontend.WebService.Interfaces;
using Radzen;
using Radzen.Blazor.Rendering;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Pages.Imprint.Components
{
    public partial class GeneratedImprint
    {
        [Parameter]
        public ImprintQuestionsModel? _IQM { get; set; }
        [Parameter]
        public EventCallback OnChangeAnswer { get; set; }
        [Inject]
        public IJSRuntime? JsRuntime { get; set; }
        [Inject]
        private ILocalStorageService? localStore { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        public ITelebirrPaymentService TelebirrService { get; set; }
        // [Inject]
        // public IStringLocalizer<Resource> _localizer { get; set; }
        public TelebirrPaymentCommand _telebirrPaymentCommand { get; set; } = new();

        private bool paidForImprint = false;

        protected async override void OnInitialized()
        {

            string payment = await localStore.GetItemAsync<string>("IPayment");
            if (payment != null && payment == "paid")
            {
                paidForImprint = true;
            }
            else
            {
                var dialogResult = await DialogService.Confirm(_localizer["TBP1"], _localizer["TBP2"], new ConfirmOptions() { OkButtonText = _localizer["Pay"], CancelButtonText = _localizer["Cancel"] });
                if (dialogResult == true)
                {
                    await OnPayClicked();
                }
                else
                {
                    await OnChangeAnswer.InvokeAsync();
                }
            }
            StateHasChanged();
        }
        public async void OnChangeAnswerCliked()
        {
            await OnChangeAnswer.InvokeAsync();
        }

        public async Task OnDownloadClicked()
        {
            var filename = _IQM.CompanyName + "-Imprint-En.pdf";
            await JsRuntime.InvokeVoidAsync("DownloadPDF", filename);
        }


        public async Task OnPayClicked()
        {
            ShowBusyDialog(true);
            _telebirrPaymentCommand.NotifyUrl = _navigationManager.Uri + "/payment/paid";
            _telebirrPaymentCommand.ReturnUrl = _navigationManager.Uri + "/payment/paid";
            _telebirrPaymentCommand.Subject = "Imprint doc payment";
            _telebirrPaymentCommand.TotalAmount = "300";

            var response = await TelebirrService.MakeTelebirrPayment(_telebirrPaymentCommand);
            if (response.Succeeded)
            {
                if (response.Data.Code == 200)  
                {
                    DialogService.Close();
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success", Detail = response.Data.Message, Duration = 4000 });
                    _navigationManager.NavigateTo(response.Data.Data.ToPayUrl);
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Failed", Detail = response.Data.Message, Duration = 4000 });
                    await OnChangeAnswer.InvokeAsync();
                }
            }
            else
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Failed", Detail = response.Messages[0], Duration = 4000 });
                await OnChangeAnswer.InvokeAsync();
            }

        }

        async Task ShowBusyDialog(bool withMessageAsString)
        {
            if (withMessageAsString)
            {
                await BusyDialog("wait ...");
            }
        }
        async Task BusyDialog(string message)
        {
            await DialogService.OpenAsync("", ds =>
            {
                RenderFragment content = b =>
                {
                    b.OpenElement(0, "div");
                    b.AddAttribute(1, "class", "row");

                    b.OpenElement(2, "div");
                    b.AddAttribute(3, "class", "col-md-12");

                    b.AddContent(4, message);

                    b.CloseElement();
                    b.CloseElement();
                };
                return content;
            }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
        }

    }
}

