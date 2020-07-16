namespace Mubbi.Marketplace.AccessControl.Area.Account.Models
{
    public class UserInfo
    {
        public int? UserId { get; set; }
        public string Provider { get; set; }
        public string SubjectId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
