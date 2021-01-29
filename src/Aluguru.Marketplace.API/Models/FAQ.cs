using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;

namespace Aluguru.Marketplace.API.Models
{
    public class FAQ : AggregateRoot
    {
        public FAQ(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        public string Question { get; set; }
        public string Answer { get; set; }

        protected override void ValidateEntity()
        {
            Ensure.NotNullOrEmpty(Question, "Question cannot be null or empty");
            Ensure.NotNullOrEmpty(Answer, "Answer cannot be null or empty");
        }
    }
}
