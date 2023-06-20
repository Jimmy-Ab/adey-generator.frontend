using System;
namespace PrivacyPolicyGeneratorFrontend.WebService.ViewModels
{
    public class TermsAndConditionsQuestionsModel
    {
        public string? Platform { get; set; }
        public string? OperatorType { get; set; }
        public string? CompanyType { get; set; }
        public string? BusinessName { get; set; }
        public Address? Address { get; set; }
        public string? WebsiteURL { get; set; }
        public string? WebsiteName { get; set; }
        public bool HavePrivacyPolicy { get; set; }
        public string? PrivacyPolicyLink { get; set; }
        public List<string>? Services { get; set; }
        public bool CreateAccount { get; set; }
        public bool SharePost { get; set; }
        public bool UseUsersContent { get; set; }
        public bool ReserveRights { get; set; }
        public bool SendNewsletters { get; set; }
        public string? NewsLetterMethod { get; set; }
        public bool SendPushNotifications { get; set; }
        public bool ThirdPartyLinks { get; set; }
        public bool AgeLimitation { get; set; }
        public string? Age { get; set; }
        public bool CommercialPurpose { get; set; }
        public bool OwnContentTrademarks { get; set; }
        public bool LiabilityLimitation { get; set; }
        public bool TerminateAccounts { get; set; }
        public bool NoticeTermination { get; set; }
        public bool NotifyUpdate { get; set; }
        public string? TermsAndUpdateMethod { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

