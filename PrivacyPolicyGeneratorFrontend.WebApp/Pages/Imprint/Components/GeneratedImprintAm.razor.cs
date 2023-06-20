using System;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PrivacyPolicyGeneratorFrontend.WebService.Interfaces;
using PrivacyPolicyGeneratorFrontend.WebService.Services;
using PrivacyPolicyGeneratorFrontend.WebService.ViewModels;
using Radzen;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Pages.Imprint.Components
{
    public partial class GeneratedImprintAm
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
            var filename = _IQM.CompanyName + "-Am-Imprint.pdf";
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
            DialogService.Close();

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

