using System.ComponentModel.DataAnnotations;

namespace Hard75Shared
{
    //Shared class used to store user information
    public class UserAccount
    {
        [Key]
        public int? userID { get; set; } //Primary Key
        public string? firstName { get; set; }//First name of the User
        public string? lastName { get; set; }//Last name of the User
        public string? email { get; set; }//Email Address of the User
        public string? pwd { get; set; }//Password of the User
        public string?pwd2 { get; set; }//Confirmation Password of the user. Used for registration only and not stored in DB.
        public string? salt { get; set; }//Salt of the User
        public int? active { get; set; }//Active field. Decides whether user account is active.
        public string? mfaID { get; set; }//MFA ID, will be used when multiple MFA methods available.

    }
}
