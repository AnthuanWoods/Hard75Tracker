using System.ComponentModel.DataAnnotations;

namespace Hard75API.Models
{
    public class UserAccount
    {
        [Key]
        public int userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [MaxLength(100)]
        public string email { get; set; }
        public string pwd { get; set; }
        public string salt { get; set; }
        public int active { get; set; }
        public string mfaID { get; set; }

    }
}
