using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        private int _CountExercises;
        public int CountExercises
        {
            get => _CountExercises;
            set => SetProperty(ref _CountExercises, value);
        }

        private int _WorkTime;

        public int WorkTime
        {
            get => _WorkTime;
            set => SetProperty(ref _WorkTime, value);
        }

        private int _RestTime;

        public int RestTime
        {
            get => _RestTime;
            set => SetProperty(ref _RestTime, value);
        }


        public Command StartTimer { get; }
        public Command IncreaseCountExercisesCommand { get; }
        public Command ReduceCountExercisesCommand { get; }


        public Command IncreaseWorkTimeCommand { get; }
        public Command ReduceWorkTimeCommand { get; }


        public Command IncreaseRestTimeCommand{ get; }
        public Command ReduceRestTimeCommand { get; }

        public TimerViewModel()
        {
            StartTimer = new Command(OnStartTimer);
            IncreaseCountExercisesCommand = new Command(OnIncreaseCountExercises);
            ReduceCountExercisesCommand = new Command(OnReduceCountExercises);

            IncreaseWorkTimeCommand = new Command(OnIncreaseWorkTime);
            ReduceWorkTimeCommand = new Command(OnReduceWorkTime);

            IncreaseRestTimeCommand = new Command(OnIncreaseRestTime);
            ReduceRestTimeCommand = new Command(OnReduceRestTime);

            _CountExercises = 8;
            _WorkTime = 20;
            _RestTime = 10;

        }

        private async void OnStartTimer(object obj)
        {
            await DataStoreTimer.AddItemAsync(new Timer { CountExercises = CountExercises, WorkTime = WorkTime, RestTime = RestTime });
            await Shell.Current.GoToAsync(nameof(TimerActiveView));
        }

        private void OnIncreaseCountExercises(object obj)
        {
            CountExercises++;
        }
        private void OnReduceCountExercises(object obj)
        {
            CountExercises--;
        }

        private void OnIncreaseWorkTime(object obj)
        {
            WorkTime++;
        }
        private void OnReduceWorkTime(object obj)
        {
            WorkTime--;
        }

        private void OnIncreaseRestTime(object obj)
        {
            RestTime++;
        }
        private void OnReduceRestTime(object obj)
        {
            RestTime--;
        }
    }
}
