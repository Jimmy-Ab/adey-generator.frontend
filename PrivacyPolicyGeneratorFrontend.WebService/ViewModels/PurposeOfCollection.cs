using System;
namespace PrivacyPolicyGeneratorFrontend.WebService.ViewModels
{
    public class PurposeOfCollection
    {
        public bool Run { get; set; }
        public bool Provide { get; set; }
        public bool Improve { get; set; }
        public bool Communicate { get; set; }
        public bool Payment { get; set; }
        public bool Research { get; set; }
        public bool Security { get; set; }
        public bool Ads { get; set; }
        public bool Others { get; set; }
        public string? OtherPurposes { get; set; }
    }
}

