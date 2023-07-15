using SQLite;
using System;

namespace FitApp.Models
{
    public class TrainingHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTraining { get; set; }
    }
}
