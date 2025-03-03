namespace TaskManagerPet.Models
{
    public class Tasks
    {
        public int Id { get; set; }

        public required string TaskName { get; set; }

        public required string TaskDescription { get; set; }

        public required string TaskStatus { get; set; }

        public DateTime TaskTime { get; set; }
    }
}
