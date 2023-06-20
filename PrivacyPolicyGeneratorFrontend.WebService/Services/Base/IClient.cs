using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivacyPolicyGeneratorFrontend.WebService.Services
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }

    }

}
