using System.ComponentModel.DataAnnotations;

namespace Hard75Shared
{
    public class TaskHistory
    {
        [Key]
        public int? taskHistoryID { get; set; }
        public int? taskID { get; set; }
        public int? userID { get; set; }
        public bool? isComplete { get; set; }
        public DateTime? dtGoal { get; set; }
        public DateTime? dtComplete { get; set; }
        public int? active { get; set; }
    }
}
