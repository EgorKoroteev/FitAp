namespace FitApp.Models
{
    public class Timer
    {
        public string Id { get; set; }

        private int _CountExercises;

        public int CountExercises
        {
            get { return _CountExercises; }
            set { _CountExercises = value; }
        }

        private int _WorkTime;

        public int WorkTime
        {
            get { return _WorkTime; }
            set { _WorkTime = value; }
        }

        private int _RestTime;

        public int RestTime
        {
            get { return _RestTime; }
            set { _RestTime = value; }
        }


    }
}
