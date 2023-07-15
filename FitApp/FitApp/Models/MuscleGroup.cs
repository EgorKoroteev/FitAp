using SQLite;

namespace FitApp.Models
{
    public class MuscleGroup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
