using System.ComponentModel.DataAnnotations;

namespace Hard75Shared
{
    public class Task
    {
        [Key]
        public int? taskID {  get; set; }
        public int? userID { get; set; }
        public int? categoryID { get; set; }
        public string? taskName { get; set; }
        public string? taskDescription { get; set; }
        public int? parentTaskID { get; set; }
        public int? active { get; set; }
    }
}
