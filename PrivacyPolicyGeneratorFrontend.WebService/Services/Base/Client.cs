

namespace PrivacyPolicyGeneratorFrontend.WebService.Services
{
    public partial class Client : IClient
    {
        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }

    }
}
