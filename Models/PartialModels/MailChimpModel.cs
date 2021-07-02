using MailChimp.Net.Models;
using System.Collections.Generic;

namespace UnchainedBackend.Models.PartialModels
{
    public class MailChimpModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Html { get; set; }
        public List<MemberTag> Tags { get; set; }
    }
}
