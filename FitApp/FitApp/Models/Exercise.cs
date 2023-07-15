using SQLite;

namespace FitApp.Models
{
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MuscleGroup { get; set; }
        public int Iteration { get; set; }
        public int Laps { get; set; }
        public int Weight { get; set; }

    }
}
