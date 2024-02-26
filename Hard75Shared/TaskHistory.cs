using System.ComponentModel.DataAnnotations;

namespace Hard75Shared
{
    public class TaskHistory
    {
        [Key]
        public int? taskID { get; set; }
        public int? userID { get; set; }
        public bool? isComplete { get; set; }
        public DateTime? lastUpdated { get; set; }
        public int? active { get; set; }
    }
}
