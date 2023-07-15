using SQLite;

namespace FitApp.Models
{
    public class MuscleExercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MuscleGroupId { get; set; }

        public int ExerciseId { get; set; }
    }
}
