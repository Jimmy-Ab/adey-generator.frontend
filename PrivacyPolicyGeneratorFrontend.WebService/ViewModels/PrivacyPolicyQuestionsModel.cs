using System;
using PrivacyPolicyGeneratorFrontend.WebService.Enums;

namespace PrivacyPolicyGeneratorFrontend.WebService.ViewModels
{
    public class PrivacyPolicyQuestionsModel
    {
        public string? Platform { get; set; }
        public string? WebsiteURL { get; set; }
        public string? WebsiteName { get; set; }
        public string? CompanyType { get; set; }
        public string? BusinessName { get; set; }
        public Address? Address { get; set; }
        public string? OperatorType { get; set; }
        public bool CollectPersonalInformation { get; set; } = true;
        public TypesOfDataCollected? TypesOfDataCollected { get; set; }
        public PurposeOfCollection? PurposeOfCollection { get; set; }
        public bool ShareData { get; set; }  
        public string? ThirdParties { get; set; }
        public bool UseCookies { get; set; }
        public string? CookieType { get; set; }
        public bool UseRemarketingForAdvertising { get; set; }
        public bool UseTrackingTechs { get; set; }
        public string? TrackingTechTypes { get; set; }
        public bool ProvideHyperlinks { get; set; }
        public string? AudienceAge { get; set; }
        public Contact? Contact { get; set; }
        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    }
}

