using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PrivacyPolicyGeneratorFrontend.WebService.Enums;
using PrivacyPolicyGeneratorFrontend.WebService.ViewModels;
using Radzen;

namespace PrivacyPolicyGeneratorFrontend.WebApp.Pages.Imprint
{
    public partial class Imprint
    {
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        private ILocalStorageService? localStore { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public string? Payment { get; set; }

        public ImprintQuestionsModel _IQM = new ImprintQuestionsModel();

        public List<string> _companyTypes = new List<string>();
        public List<string> _languages = new List<string>();

        bool busy;
        bool busyAm;

        bool _generated = false;
        string _lang = null;

        protected async override void OnInitialized()
        {

            DialogService.OnOpen += Open;
            DialogService.OnClose += Close;
            _IQM.CompanyAddress = new Address();
            if (Payment == "paid")
            {
                await localStore.SetItemAsync("IPayment", Payment);
            }
            _generated = await localStore.GetItemAsync<bool>("imprintGenerated");
            var savedLang = await localStore.GetItemAsync<string>("_lang");
            _lang = await localStore.GetItemAsync<string>("imprintLang");
            StateHasChanged();

            if (savedLang == "am-ET")
            {
                _companyTypes.Add("የህብረት ሽርክና ማህበር");
                _companyTypes.Add("ሁለት ዓይነት ኃላፊነት ያለበት የሽርክና ማህበር");
                _companyTypes.Add("ኃላፊነቱ የተወሰነ የሽርክና ማህበር");
                _companyTypes.Add("የአክሲዮን ማህበር");
                _companyTypes.Add("ኃላፊነቱ የተወሰነ የግል ማህበር");
                _companyTypes.Add("ባለአንድ አባል ኃላፉነቱ የተወሰነ የግል ማህበር");

                _languages.Add("እንግሊዝኛ");
                _languages.Add("አማርኛ");
            }
            else
            {
                foreach (CompanyType item in Enum.GetValues(typeof(CompanyType)))
                {
                    var spacedItem = AddSpacesToSentence(item.ToString(), true);
                    _companyTypes.Add(spacedItem);
                }

                foreach (Language item in Enum.GetValues(typeof(Language)))
                {
                    var itemSpaced = AddSpacesToSentence(item.ToString(), true);
                    _languages.Add(itemSpaced);
                }
            }


            var savedIQM = await localStore.GetItemAsync<string>("_IQM");

            if (savedIQM != null)
            {
                if (Payment == "paid")
                {
                    var jsonIQM = JsonSerializer.Deserialize<ImprintQuestionsModel>(savedIQM);

                    _IQM = jsonIQM;
                    StateHasChanged();
                }
                else
                {
                    if (_generated)
                    {
                        var jsonIQM = JsonSerializer.Deserialize<ImprintQuestionsModel>(savedIQM);

                        _IQM = jsonIQM;
                        StateHasChanged();
                    }
                    else
                    {
                        var dialogResult = await DialogService.Confirm(_localizer["RP1"], _localizer["Resume"], new ConfirmOptions() { OkButtonText = _localizer["Resume"], CancelButtonText = _localizer["Restart"] });
                        if (dialogResult == true)
                        {
                            var jsonIQM = JsonSerializer.Deserialize<ImprintQuestionsModel>(savedIQM);

                            _IQM = jsonIQM;
                            StateHasChanged();
                        }
                        else
                        {
                            _IQM = new ImprintQuestionsModel();
                            await localStore.RemoveItemAsync("_IQM");
                            await localStore.RemoveItemAsync("imprintGenerated");
                            await localStore.RemoveItemAsync("imprintLang");

                            _generated = false;
                            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
                        }
                    }
                }
                    
            }
            _navigationManager.NavigateTo(_navigationManager.BaseUri + "imprint");
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
            string jsonString = JsonSerializer.Serialize(_IQM);
            await localStore.SetItemAsync("_IQM", jsonString);
        }

        async Task OnBusyClick()
        {
            busy = true;
            await Task.Delay(1000);
            

            if (_lang == "Amharic")
            {
                switch (_IQM.CompanyType)
                {
                    case "General Partnership":
                        _IQM.CompanyType = "የህብረት ሽርክና ማህበር";
                        break;
                    case "Limited Partnership":
                        _IQM.CompanyType = "ሁለት ዓይነት ኃላፊነት ያለበት የሽርክና ማህበር";
                        break;
                    case "Limited Liability Partnership":
                        _IQM.CompanyType = "ኃላፊነቱ የተወሰነ የሽርክና ማህበር";
                        break;
                    case "Share Company":
                        _IQM.CompanyType = "የአክሲዮን ማህበር";
                        break;
                    case "Private Limited Company":
                        _IQM.CompanyType = "ኃላፊነቱ የተወሰነ የግል ማህበር";
                        break;
                    case "One Person Private Limited Company":
                        _IQM.CompanyType = "ባለአንድ አባል ኃላፉነቱ የተወሰነ የግል ማህበር";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (_IQM.CompanyType)
                {
                    case "የህብረት ሽርክና ማህበር":
                        _IQM.CompanyType = "General Partnership";
                        break;
                    case "ሁለት ዓይነት ኃላፊነት ያለበት የሽርክና ማህበር":
                        _IQM.CompanyType = "Limited Partnership";
                        break;
                    case "ኃላፊነቱ የተወሰነ የሽርክና ማህበር":
                        _IQM.CompanyType = "Limited Liability Partnership";
                        break;
                    case "የአክሲዮን ማህበር":
                        _IQM.CompanyType = "Share Company";
                        break;
                    case "ኃላፊነቱ የተወሰነ የግል ማህበር":
                        _IQM.CompanyType = "Private Limited Company";
                        break;
                    case "ባለአንድ አባል ኃላፉነቱ የተወሰነ የግል ማህበር    ":
                        _IQM.CompanyType = "One Person Private Limited Company";
                        break;
                    default:
                        break;
                }
            }

            string jsonString = JsonSerializer.Serialize(_IQM);
            await localStore.SetItemAsync("_IQM", jsonString);
            await localStore.SetItemAsync("imprintGenerated", true);
            await localStore.SetItemAsync("imprintLang", _lang);
            busy = false;

            _generated = true;

            StateHasChanged();
        }


        void OnChangeAnswer()
        {
            _generated = false;
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

        async void OnLanguageChanged(object value, string op)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                if(strValue.ToString() == "English" || strValue.ToString() == "እንግሊዝኛ"){
                    _lang = "English";
                }
                if(strValue.ToString() == "Amharic" || strValue.ToString() == "አማርኛ"){
                    _lang = "Amharic";
                }

                await localStore.SetItemAsync("imprintLang", _lang);
            }
            else
            {
                _lang = "English";
                await localStore.SetItemAsync("imprintLang", _lang);
            }
        }

        void OnCompanyTypeChanged(object value, string com)
        {
            var strValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            if (strValue != null)
            {
                _IQM.CompanyType = strValue.ToString();
            }
            else
            {
                _IQM.CompanyType = null;
            }
        }

        void OnCheckBoxChanged(bool? value, string field)
        {
            switch (field)
            {
                case "OtherWebsiteLinkYes":
                    _IQM.OtherWebsiteLink = true;
                    break;
                case "OtherWebsiteLinkNo":
                    _IQM.OtherWebsiteLink = false;
                    break;
                default:
                    break;
            }
        }
    }
}

