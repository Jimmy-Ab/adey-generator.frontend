using System;
namespace PrivacyPolicyGeneratorFrontend.WebService.ViewModels
{
    public class TypesOfDataCollected
    {
        public bool Name { get; set; }
        public bool Address { get; set; }
        public bool Email { get; set; }
        public bool PhoneNumber { get; set; }
        public bool Age { get; set; }
        public bool Gender { get; set; }
        public bool Payment { get; set; }
        public bool Location { get; set; }
        public bool Others { get; set; }
        public string? OtherDatas { get; set; }
    }
}

