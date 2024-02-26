using System.ComponentModel.DataAnnotations;

namespace Hard75Shared
{
    public class Category
    {
        public int userID { get; set; }
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public int active {  get; set; }
    }
}
