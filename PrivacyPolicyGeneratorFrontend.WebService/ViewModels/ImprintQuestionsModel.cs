using System;
namespace PrivacyPolicyGeneratorFrontend.WebService.ViewModels
{
    public class ImprintQuestionsModel
    {
        public string? CompanyName { get; set; }
        public string? CompanyType { get; set; }
        public Address? CompanyAddress { get; set; }
        public string? AdditionalAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? BusinessRegisterNumber { get; set; }
        public string? VATID { get; set; }
        public string? TaxNumber { get; set; }
        public string? Representer { get; set; }
        public bool OtherWebsiteLink { get; set; }
    }
}

