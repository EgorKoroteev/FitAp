using SQLite;
using System;

namespace FitApp.Models
{
    public class Profile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Weight { get; set; }
        public DateTime EditDate { get; set; }
    }
}
