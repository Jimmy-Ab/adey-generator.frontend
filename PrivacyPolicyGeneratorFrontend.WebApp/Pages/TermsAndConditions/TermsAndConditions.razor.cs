using System;
using PrivacyPolicyGeneratorFrontend.WebService.Enums;
using System.Text;
using PrivacyPolicyGeneratorFrontend.WebService.ViewModels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Pages.TermsAndConditions
{
    public partial class TermsAndConditions
    {
        [Inject]
        private NavigationManager? _navigationManager { get; set; }
        [Inject]
        private ILocalStorageService? localStore { get; set; }
        [Inject]
        private DialogService? DialogService { get; set; }

        [Parameter]
        public string? Payment { get; set; }
        public TermsAndConditionsQuestionsModel _TCQM = new TermsAndConditionsQuestionsModel();
        int selectedIndex = 0;

        public List<string> _companyTypes = new List<string>();
        public List<string> _cookieTypes = new List<string>();
        public List<string> _trackingTechTypes = new List<string>();
        public List<string> _audienceAge = new List<string>();
        public List<string> _platform = new List<string>();
        public List<string> _operator = new List<string>();
        public List<string> _thirdParties = new List<string>();
        public List<string> _notifyUpdate = new List<string>();
        public List<string> _newsLetterMethod = new List<string>();
        public List<string> _languages = new List<string>();


        public DateTime? dateValue = DateTime.Now;
        bool busy;
        bool _generated = false;
        string? _service = null;
        string _lang = null;


        protected async override void OnInitialized()
        {
            DialogService.OnOpen += Open;
            DialogService.OnClose += Close;
            _TCQM.Address = new Address();
            _TCQM.Services = new List<string>();
            if (Payment == "paid")
            {
                await localStore.SetItemAsync("TCPayment", Payment);
            }

            var savedLang = await localStore.GetItemAsync<string>("_lang");

            if (savedLang == "am-ET")
            {
                _companyTypes.Add("የህብረት ሽርክና ማህበር");
                _companyTypes.Add("ሁለት ዓይነት ኃላፊነት ያለበት የሽርክና ማህበር");
                _companyTypes.Add("ኃላፊነቱ የተወሰነ የሽርክና ማህበር");
                _companyTypes.Add("የአክሲዮን ማህበር");
                _companyTypes.Add("ኃላፊነቱ የተወሰነ የግል ማህበር");
                _companyTypes.Add("ባለአንድ አባል ኃላፉነቱ የተወሰነ የግል ማህበር");

                _cookieTypes.Add("የመጀመሪያ ወገን ኩኪዎች");
                _cookieTypes.Add("የሶስተኛ ወገን ኩኪዎች");
                _cookieTypes.Add("ጊዜያዊ ኩኪዎች");
                _cookieTypes.Add("ቋሚ ኩኪዎች");

                _trackingTechTypes.Add("ጉግል");
                _trackingTechTypes.Add("ቢኮን");

                _platform.Add("ድህረገፅ");
                _platform.Add("መተግበሪያ");
                _platform.Add("ሁለቱም");

                _operator.Add("ኩባንያ");
                _operator.Add("ግለሰብ");

                _thirdParties.Add("አቅራቢዎች");
                _thirdParties.Add("የንግድ አጋሮች");
                _thirdParties.Add("አገልግሎት አቅራቢዎች");
                _thirdParties.Add("ህጉን ያክብሩ");
                _thirdParties.Add("ደህንነትን ለመመርመር");
                _thirdParties.Add("የንግድ ግዢ ወይም ሽያጭን በተመለከተ");

                _notifyUpdate.Add("ኢሜይል");
                _notifyUpdate.Add("የግፋ ማሳወቂያዎች");
                _notifyUpdate.Add("ሁለቱም");

                _newsLetterMethod.Add("ኢሜይል");
                _newsLetterMethod.Add("መልእክት");
                _newsLetterMethod.Add("ሁለቱም");

                _languages.Add("እንግሊዝኛ");
                _languages.Add("አማርኛ");
            }
            else
            {

                foreach (CompanyType item in Enum.GetValues(typeof(CompanyType)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _companyTypes.Add(itemSpaced);
                }

                foreach (CookieType item in Enum.GetValues(typeof(CookieType)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _cookieTypes.Add(itemSpaced);
                }

                foreach (TrackingTechTypes item in Enum.GetValues(typeof(TrackingTechTypes)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _trackingTechTypes.Add(itemSpaced);
                }

                foreach (Platform item in Enum.GetValues(typeof(Platform)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _platform.Add(itemSpaced);
                }
                foreach (OperatorType item in Enum.GetValues(typeof(OperatorType)))
                {
                    _operator.Add(item.ToString());
                }

                foreach (ThirdParties item in Enum.GetValues(typeof(ThirdParties)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _thirdParties.Add(itemSpaced);
                }

                foreach (NotifyUpdate item in Enum.GetValues(typeof(NotifyUpdate)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _notifyUpdate.Add(itemSpaced);
                }

                foreach (NewsletterUpdate item in Enum.GetValues(typeof(NewsletterUpdate)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _newsLetterMethod.Add(itemSpaced);
                }

                foreach (Language item in Enum.GetValues(typeof(Language)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _languages.Add(itemSpaced);
                }
            }
            _audienceAge.Add(">13");
            _audienceAge.Add(">16");
            _audienceAge.Add(">18");

            var savedTCQM = await localStore.GetItemAsync<string>("_TCQM");

            if (savedTCQM != null)
            {
                _generated = await localStore.GetItemAsync<bool>("tcGenerated");
                _lang = await localStore.GetItemAsync<string>("tcLang");

                if (Payment == "paid")
                {
                    var jsonTCQM = JsonSerializer.Deserialize<TermsAndConditionsQuestionsModel>(savedTCQM);
                    _TCQM = jsonTCQM;
                    var savedIndex = await localStore.GetItemAsync<int>("_TCSelectedIndex");
                    selectedIndex = savedIndex;
                    StateHasChanged();
                }
                else
                {
                    if (_generated)
                    {
                        var jsonTCQM = JsonSerializer.Deserialize<TermsAndConditionsQuestionsModel>(savedTCQM);
                        _TCQM = jsonTCQM;
                        var savedIndex = await localStore.GetItemAsync<int>("_TCSelectedIndex");
                        selectedIndex = savedIndex;
                        StateHasChanged();
                    }
                    else
                    {

                        var dialogResult = await DialogService.Confirm(_localizer["RP1"], _localizer["Resume"], new ConfirmOptions() { OkButtonText = _localizer["Resume"], CancelButtonText = _localizer["Restart"] });
                        if (dialogResult == true)
                        {
                            var jsonTCQM = JsonSerializer.Deserialize<TermsAndConditionsQuestionsModel>(savedTCQM);
                            _TCQM = jsonTCQM;
                            var savedIndex = await localStore.GetItemAsync<int>("_TCSelectedIndex");
                            selectedIndex = savedIndex;
                            StateHasChanged();
                        }
                        else
                        {
                            _TCQM = new TermsAndConditionsQuestionsModel();
                            await localStore.RemoveItemAsync("_TCQM");
                            await localStore.RemoveItemAsync("tcGenerated");
                            await localStore.RemoveItemAsync("tcLang");
                            _generated = false;
                            _navigationManager.NavigateTo(_navigationManager.BaseUri + "terms-and-conditions", true);

                        }
                    }
                }
            }
            _navigationManager.NavigateTo(_navigationManager.BaseUri + "terms-and-conditions");
            StateHasChanged();
        }

        public void Dispose()
        {
            DialogService.OnOpen -= Open;
            DialogService.OnClose -= Close;
        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            Console.WriteLine("Dialog opened");
        }

        void Close(dynamic result)
        {
            Console.WriteLine($"Dialog closed");
        }
        public async Task OnPayClicked()
        {
            string jsonString = JsonSerializer.Serialize(_TCQM);
            await localStore.SetItemAsync("_TCQM", jsonString);
            await localStore.SetItemAsync("_TCSelectedIndex", selectedIndex);
        }
        async Task OnBusyClick()
        {
            busy = true;
            await Task.Delay(1000);

            string jsonString = JsonSerializer.Serialize(_TCQM);
            await localStore.SetItemAsync("_TCQM", jsonString);
            await localStore.SetItemAsync("_TCSelectedIndex", selectedIndex);
            await localStore.SetItemAsync("tcGenerated", true);
            await localStore.SetItemAsync("tcLang", _lang);
            busy = false;
            _generated = true;
        }

        void OnChangeAnswer()
        {
            _generated = false;
        }
        void OnCheckBoxChanged(bool? value, string field)
        {
            switch (field)
            {
                case "PrivacyPolicyYes":
                    _TCQM.HavePrivacyPolicy = true;
                    break;
                case "PrivacyPolicyNo":
                    _TCQM.HavePrivacyPolicy = false;
                    break;
                case "CreateAccountYes":
                    _TCQM.CreateAccount = true;
                    break;
                case "CreateAccountNo":
                    _TCQM.CreateAccount = false;
                    break;
                case "SharePostYes":
                    _TCQM.SharePost = true;
                    break;
                case "SharePostNo":
                    _TCQM.SharePost = false;
                    break;
                case "UseUserContentYes":
                    _TCQM.UseUsersContent = true;
                    break;
                case "UseUserContentNo":
                    _TCQM.UseUsersContent = false;
                    break;
                case "ReserveRightsYes":
                    _TCQM.ReserveRights = true;
                    break;
                case "ReserveRightsNo":
                    _TCQM.ReserveRights = false;
                    break;
                case "SendNewsLetterYes":
                    _TCQM.SendNewsletters = true;
                    break;
                case "SendNewsLetterNo":
                    _TCQM.SendNewsletters = false;
                    break;
                case "SendPushNotificationsYes":
                    _TCQM.SendPushNotifications = true;
                    break;
                case "SendPushNotificationsNo":
                    _TCQM.SendPushNotifications = false;
                    break;
                case "ThirdPartyLinksYes":
                    _TCQM.ThirdPartyLinks = true;
                    break;
                case "ThirdPartyLinksNo":
                    _TCQM.ThirdPartyLinks = false;
                    break;
                case "AgeLimitationYes":
                    _TCQM.AgeLimitation = true;
                    break;
                case "AgeLimitationNo":
                    _TCQM.AgeLimitation = false;
                    break;
                case "CommercialPurposeYes":
                    _TCQM.CommercialPurpose = true;
                    break;
                case "CommercialPurposeNo":
                    _TCQM.CommercialPurpose = false;
                    break;
                case "LiabilityLimitationYes":
                    _TCQM.LiabilityLimitation = true;
                    break;
                case "LiabilityLimitationNo":
                    _TCQM.LiabilityLimitation = false;
                    break;
                case "TerminateAccountsYes":
                    _TCQM.TerminateAccounts = true;
                    break;
                case "TerminateAccountsNo":
                    _TCQM.TerminateAccounts = false;
                    break;
                case "NoticeTerminationYes":
                    _TCQM.NoticeTermination = true;
                    break;
                case "NoticeTerminationNo":
                    _TCQM.NoticeTermination = false;
                    break;
                case "NotifyUpdateYes":
                    _TCQM.NotifyUpdate = true;
                    break;
                case "NotifyUpdateNo":
                    _TCQM.NotifyUpdate = false;
                    break;
                default:
                    break;
            }
        }

        string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        async void OnChange(int index)
        {
            string jsonString = JsonSerializer.Serialize(_TCQM);
            await localStore.SetItemAsync("_TCQM", jsonString);
            await localStore.SetItemAsync("_TCSelectedIndex", selectedIndex);
        }

        void OnAdd()
        {
            _TCQM.Services.Add(_service);
            _service = null;
        }
        async void OnLanguageChanged(object value, string op)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                if (strValue.ToString() == "English" || strValue.ToString() == "እንግሊዝኛ")
                {
                    _lang = "English";
                }
                if (strValue.ToString() == "Amharic" || strValue.ToString() == "አማርኛ")
                {
                    _lang = "Amharic";
                }

                await localStore.SetItemAsync("tcLang", _lang);
            }
            else
            {
                _lang = "English";
                await localStore.SetItemAsync("tcLang", _lang);
            }
        }
        void OnPlatformChanged(object value, string plat)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _TCQM.Platform = strValue.ToString();
            }
            else
            {
                _TCQM.Platform = null;
            }
        }

        void OnOperatorTypeChanged(object value, string op)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _TCQM.OperatorType = strValue.ToString();
            }
            else
            {
                _TCQM.OperatorType = null;
            }
        }

        void OnCompanyTypeChanged(object value, string com)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _TCQM.CompanyType = strValue.ToString();
            }
            else
            {
                _TCQM.CompanyType = null;
            }
        }
        void OnNewsletterMethodChanged(object value, string str)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _TCQM.NewsLetterMethod = strValue.ToString();
            }
            else
            {
                _TCQM.NewsLetterMethod = null;
            }
        }
        void OnAudienceAgeChanged(object value, string str)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _TCQM.Age = strValue.ToString();
            }
            else
            {
                _TCQM.Age = null;
            }
        }

        void OnNotifyUpdateChanged(object value, string str)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _TCQM.TermsAndUpdateMethod = strValue.ToString();
            }
            else
            {
                _TCQM.TermsAndUpdateMethod = null;
            }
        }
    }
}

