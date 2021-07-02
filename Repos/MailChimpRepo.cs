using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{
    public interface IMailChimpRepo 
    {
        Task<bool> AddUserToList(MailChimpModel model);
        }
    public class MailChimpRepo : IMailChimpRepo
    {
        private readonly IConfiguration _configuration;
        private const string ListId = "866a037eff";
        private const int TemplateId = 10009662;

        private Setting _campaignSettings = new Setting
        {
            ReplyTo = "your@email.com",
            FromName = "The name that others will see when they receive the email",
            Title = "Your campaign title",
            SubjectLine = "The email subject",
        };

        public MailChimpRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        public async Task CreateAndSendCampaign(string html)
        {
            var mailChimpKey = _configuration["MailChimpApiKey"];
            IMailChimpManager _mailChimpManager = new MailChimpManager(mailChimpKey);
            var campaign =  _mailChimpManager.Campaigns.AddAsync(new Campaign
            {
                Settings = _campaignSettings,
                Recipients = new Recipient { ListId = ListId },
                Type = CampaignType.Regular
            }).Result; 
            var timeStr = DateTime.Now.ToString();
            var content = _mailChimpManager.Content.AddOrUpdateAsync(
             campaign.Id,
             new ContentRequest()
             {
                 Template = new ContentTemplate
                 {
                     Id = TemplateId,
                     Sections = new Dictionary<string, object>()
                {
                     { "preheader_leftcol_content", $"<p>{timeStr}</p>" }
                }
                 }
             }).Result; _mailChimpManager.Campaigns.SendAsync(campaign.Id).Wait();
        }

        public async Task UpdateUserInList(string email) {
            var mailChimpKey = _configuration["MailChimpApiKey"];
            IMailChimpManager _mailChimpManager = new MailChimpManager(mailChimpKey);
            // Get reference to existing user if you don't already have it
            var members = await _mailChimpManager.Members.GetAllAsync(ListId).ConfigureAwait(false);
            var member = members.First(x => x.EmailAddress == "abc@def.com");

            // Update the user
            member.MergeFields.Add("FNAME", "New first name");
            member.MergeFields.Add("LNAME", "New last name");
            await _mailChimpManager.Members.AddOrUpdateAsync(ListId, member);
        }

        public async Task<bool> AddUserToList(MailChimpModel model) {
            //var mailChimpKey = _configuration["MailChimpApiKey"];
            //MailChimpOptions options = new() { ApiKey = mailChimpKey, Limit = 50};
            //var _mailChimpManager = new MailChimpManager(options);
            //// Use the Status property if updating an existing member
            //var member = new Member {Tags = model.Tags, EmailAddress = model.Email, StatusIfNew = Status.Subscribed };
            //member.MergeFields.Add("FNAME", model.FirstName);
            ////member.MergeFields.Add("LNAME", "COW");
            //await _mailChimpManager.Members.AddOrUpdateAsync(ListId, member);
            return true;
        }
    }
}
