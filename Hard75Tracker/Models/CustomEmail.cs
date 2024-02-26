using System.ComponentModel.DataAnnotations;

namespace Hard75API.Models
{
    public class CustomEmail
    {
            public int emailID { get; set; }

            public string emailAddress { get; set; }

            public string subject { get; set; }
    
            public string body { get; set; }

    }


}

