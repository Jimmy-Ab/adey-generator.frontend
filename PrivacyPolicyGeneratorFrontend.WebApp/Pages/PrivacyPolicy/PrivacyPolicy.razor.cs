using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PrivacyPolicyGeneratorFrontend.WebService.Enums;
using PrivacyPolicyGeneratorFrontend.WebService.ViewModels;
using Radzen;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Pages.PrivacyPolicy
{
    public partial class PrivacyPolicy
    {
        [Inject]
        private NavigationManager? _navigationManager { get; set; }
        [Inject]
        private ILocalStorageService? localStore { get; set; }
        [Inject]
        private DialogService? DialogService { get; set; }

        [Parameter]
        public string? Payment { get; set; }

        public PrivacyPolicyQuestionsModel _PPQM = new PrivacyPolicyQuestionsModel();
        int selectedIndex = 0;

        public List<string> _companyTypes = new List<string>();
        public List<string> _cookieTypes = new List<string>();
        public List<string> _trackingTechTypes = new List<string>();
        public List<string> _audienceAge = new List<string>();
        public List<string> _platform = new List<string>();
        public List<string> _operator = new List<string>();
        public List<string> _thirdParties = new List<string>();
        public List<string> _languages = new List<string>();


        public DateTime? dateValue = DateTime.Now;
        bool busy;
        bool _generated = false;

        string? _lang;
        string? savedLang;

        protected async override void OnInitialized()
        {
            DialogService.OnOpen += Open;
            DialogService.OnClose += Close;

            _PPQM.Address = new Address();
            _PPQM.TypesOfDataCollected = new TypesOfDataCollected();
            _PPQM.PurposeOfCollection = new PurposeOfCollection();
            _PPQM.Contact = new Contact();

            if (Payment == "paid")
            {
                await localStore.SetItemAsync("PPPayment", Payment);
            }

            savedLang = await localStore.GetItemAsync<string>("_lang");
            
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

                foreach (Language item in Enum.GetValues(typeof(Language)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _languages.Add(itemSpaced);
                }
            }
            


            _audienceAge.Add(">13");
            _audienceAge.Add(">16");
            _audienceAge.Add(">18");

            var savedPPQM = await localStore.GetItemAsync<string>("_PPQM");

            if (savedPPQM != null)
            {
                _generated = await localStore.GetItemAsync<bool>("ppGenerated");
                _lang = await localStore.GetItemAsync<string>("ppLang");

                if (Payment == "paid")
                {
                    var jsonPPQM = JsonSerializer.Deserialize<PrivacyPolicyQuestionsModel>(savedPPQM);
                    _PPQM = jsonPPQM;
                    var savedIndex = await localStore.GetItemAsync<int>("_PPSelectedIndex");
                    selectedIndex = savedIndex;
                    StateHasChanged();
                }
                else
                {
                    if (_generated)
                    {
                        var jsonPPQM = JsonSerializer.Deserialize<PrivacyPolicyQuestionsModel>(savedPPQM);
                        _PPQM = jsonPPQM;
                        var savedIndex = await localStore.GetItemAsync<int>("_PPSelectedIndex");
                        selectedIndex = savedIndex;
                        StateHasChanged();
                    }
                    else
                    {
                        var dialogResult = await DialogService.Confirm(_localizer["RP1"], _localizer["Resume"], new ConfirmOptions() { OkButtonText = _localizer["Resume"], CancelButtonText = _localizer["Restart"] });
                        if (dialogResult == true)       
                        {
                            var jsonPPQM = JsonSerializer.Deserialize<PrivacyPolicyQuestionsModel>(savedPPQM);
                            _PPQM = jsonPPQM;
                            var savedIndex = await localStore.GetItemAsync<int>("_PPSelectedIndex");
                            selectedIndex = savedIndex;
                            StateHasChanged();
                        }
                        else
                        {
                            _PPQM = new PrivacyPolicyQuestionsModel();
                            await localStore.RemoveItemAsync("_PPQM");
                            await localStore.RemoveItemAsync("ppGenerated");
                            await localStore.RemoveItemAsync("ppLang");
                            _generated = false;
                            _navigationManager.NavigateTo(_navigationManager.BaseUri + "privacy-policy", true);
                        }
                    }
                }
            }
            _navigationManager.NavigateTo(_navigationManager.BaseUri + "privacy-policy");
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
            string jsonString = JsonSerializer.Serialize(_PPQM);
            await localStore.SetItemAsync("_PPQM", jsonString);
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

        async Task OnBusyClick()
        {
            busy = true;
            await Task.Delay(1000);
           
            string jsonString = JsonSerializer.Serialize(_PPQM);
            await localStore.SetItemAsync("_PPQM", jsonString);
            await localStore.SetItemAsync("_PPSelectedIndex", selectedIndex);
            await localStore.SetItemAsync("ppGenerated", true);
            await localStore.SetItemAsync("ppLang", _lang);

            busy = false;
            _generated = true;
        }
        async void OnChange(int index)
        {
            string jsonString = JsonSerializer.Serialize(_PPQM);
            await localStore.SetItemAsync("_PPQM", jsonString);
            await localStore.SetItemAsync("_PPSelectedIndex", selectedIndex);
        }

        void OnChangeAnswer()
        {
            _generated = false;
        }

        void OnCheckBoxChanged(bool? value, string field)
        {
            switch (field)
            {
                case "CollectPersonalInformationYes":
                    _PPQM.CollectPersonalInformation = true;
                    break;
                case "CollectPersonalInformationNo":
                    _PPQM.CollectPersonalInformation = false;
                    break;
                case "Name":
                    _PPQM.TypesOfDataCollected.Name = (bool)value;
                    break;

                case "Address":
                    _PPQM.TypesOfDataCollected.Address = (bool)value;
                    break;
                case "Email":
                    _PPQM.TypesOfDataCollected.Email = (bool)value;
                    break;
                case "Location":
                    _PPQM.TypesOfDataCollected.Location = (bool)value;
                    break;

                case "Phone":
                    _PPQM.TypesOfDataCollected.PhoneNumber = (bool)value;
                    break;
                case "Age":
                    _PPQM.TypesOfDataCollected.Age = (bool)value;
                    break;
                case "Gender":
                    _PPQM.TypesOfDataCollected.Gender = (bool)value;
                    break;
                case "Payment":
                    _PPQM.TypesOfDataCollected.Payment = (bool)value;
                    break;
                case "Others":
                    _PPQM.TypesOfDataCollected.Others = (bool)value;
                    break;

                case "Run":
                    _PPQM.PurposeOfCollection.Run = (bool)value;
                    break;

                case "Provide":
                    _PPQM.PurposeOfCollection.Provide = (bool)value;
                    break;

                case "Improve":
                    _PPQM.PurposeOfCollection.Improve = (bool)value;
                    break;
                case "Communicate":
                    _PPQM.PurposeOfCollection.Communicate = (bool)value;
                    break;
                case "PaymentPurpose":
                    _PPQM.PurposeOfCollection.Payment = (bool)value;
                    break;
                case "Research":
                    _PPQM.PurposeOfCollection.Research = (bool)value;
                    break;
                case "Security":
                    _PPQM.PurposeOfCollection.Security = (bool)value;
                    break;
                case "Ads":
                    _PPQM.PurposeOfCollection.Ads = (bool)value;
                    break;
                case "OtherPersonalData":
                    _PPQM.PurposeOfCollection.Others = (bool)value;
                    break;
                case "ShareDataYes":
                    _PPQM.ShareData = true;
                    break;
                case "ShareDataNo":
                    _PPQM.ShareData = false;
                    break;
                case "UseCookiesYes":
                    _PPQM.UseCookies = true;
                    break;
                case "UseCookiesNo":
                    _PPQM.UseCookies = false;
                    break;
                case "UseRemarketingForAdvertisingYes":
                    _PPQM.UseRemarketingForAdvertising = true;
                    break;
                case "UseRemarketingForAdvertisingNo":
                    _PPQM.UseRemarketingForAdvertising = false;
                    break;
                case "UseTrackingTechsYes":
                    _PPQM.UseTrackingTechs = true;
                    break;
                case "UseTrackingTechsNo":
                    _PPQM.UseTrackingTechs = false;
                    break;
                case "ProvideHyperLinksYes":
                    _PPQM.ProvideHyperlinks = true;
                    break;
                case "ProvideHyperLinksNo":
                    _PPQM.ProvideHyperlinks = false;
                    break;
                default:
                    break;

            }
        }
        void OnPlatformChanged(object value, string plat)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _PPQM.Platform = strValue.ToString();
            }
            else
            {
                _PPQM.Platform = null;
            }
        }
        void OnOperatorTypeChanged(object value, string op)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _PPQM.OperatorType = strValue.ToString();
            }
            else
            {
                _PPQM.OperatorType = null;
            }
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

                await localStore.SetItemAsync("ppLang", _lang);
            }
            else
            {
                _lang = "English";
                await localStore.SetItemAsync("ppLang", _lang);
            }
        }

        void OnCompanyTypeChanged(object value, string com)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _PPQM.CompanyType = strValue.ToString();
            }
            else
            {
                _PPQM.CompanyType = null;
            }
        }
        void OnThirdPartyChanged(object value, string str)
        {

            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _PPQM.ThirdParties = strValue.ToString();
            }
            else
            {
                _PPQM.ThirdParties = null;
            }
        }
        void OnCookieTypeChanged(object value, string str)
        {

            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _PPQM.CookieType = strValue.ToString();
            }
            else
            {
                _PPQM.CookieType = null;
            }
        }

        void OnTechTypeChanged(object value, string str)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _PPQM.TrackingTechTypes = strValue.ToString();
            }
            else
            {
                _PPQM.TrackingTechTypes = null;
            }
        }

        void OnAudienceAgeChanged(object value, string str)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;

            if (strValue != null)
            {
                _PPQM.AudienceAge = strValue.ToString();
            }
            else
            {
                _PPQM.AudienceAge = null;
            }
        }

        void OnEffectiveDateChanged(DateTime? value, string format)
        {
            _PPQM.EffectiveDate = (DateTime)value;
        }
    }
}

