using SQLite;

namespace FitApp.Models
{
    public class TrainingExercises
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        public int ProgramId { get; set; }


        public int ExerciseId { get; set; }
    }
}
