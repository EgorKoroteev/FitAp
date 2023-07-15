using SQLite;
using System.Collections.ObjectModel;

namespace FitApp.Models
{
    public class ModifyExercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Laps { get; set; }
        public int Iteration { get; set; }
        public int Weight { get; set; }

        [Ignore]
        public ObservableCollection<Lap> LapsArray { get; set; }
    }
}
