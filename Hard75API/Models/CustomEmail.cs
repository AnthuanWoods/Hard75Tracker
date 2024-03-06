using System.ComponentModel.DataAnnotations;

namespace Hard75API.Models
{
    //class used to create and send MFA emails
    public class CustomEmail
    {
            public int emailID { get; set; } //ID

            public string emailAddress { get; set; } //User's email address

            public string subject { get; set; }// Subject Line
    
            public string body { get; set; }//Body (typically HTML)

            public string getEmail()
            {
                return emailAddress;
            }

    }


}

