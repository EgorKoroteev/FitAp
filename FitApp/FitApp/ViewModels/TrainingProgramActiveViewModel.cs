using FitApp.Models;
using FitApp.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(TrainingProgramActiveID), nameof(TrainingProgramActiveID))]
    public class TrainingProgramActiveViewModel : BaseViewModel
    {
        #region Секундомер

        private TimeSpan _stopwatchTime;
        public TimeSpan StopwatchTime
        {
            get { return _stopwatchTime; }
            set { SetProperty(ref _stopwatchTime, value); }
        }

        private Timer _stopwatchTimer;
        private const int TimerInterval = 1000;

        public void StartStopwatch()
        {
            _stopwatchTimer = new Timer(TimerInterval);
            _stopwatchTimer.Elapsed += OnStopwatchTimerElapsed;
            _stopwatchTimer.Start();
        }

        private void OnStopwatchTimerElapsed(object sender, ElapsedEventArgs e)
        {
            StopwatchTime = StopwatchTime.Add(TimeSpan.FromSeconds(1));
        }



        #endregion

        #region Команды

        public ICommand EndTrainingProgramCommand => new Command(OnEndTrainingProgram);
        private async void OnEndTrainingProgram()
        {
            _stopwatchTimer.Stop();
            var history = new TrainingHistory
            {
                Name = _trainingProgram.Name,
                DateTraining = DateTime.Now
            };

            await App.DatabaseHelper.AddTrainHistoryAsync(history);

            await Shell.Current.GoToAsync("../../..");
        }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            _stopwatchTimer.Stop();

            await Shell.Current.GoToAsync("..");
        }

        #endregion

        private int _CurrentPosition;
        private ObservableCollection<ModifyExercise> _exercises;
        private TrainingProgram _trainingProgram;

        public int CurrentPosition
        {
            get { return _CurrentPosition; }
            set
            {
                _CurrentPosition = value;
                OnPropertyChanged(nameof(CurrentPosition));
                OnPropertyChanged(nameof(CurrentExercise));
            }
        }

        public ModifyExercise CurrentExercise
        {
            get { return _exercises?.Count > CurrentPosition ? _exercises[CurrentPosition] : null; }
        }

        public ObservableCollection<ModifyExercise> Exercises
        {
            get { return _exercises; }
            set { SetProperty(ref _exercises, value); }
        }

        public TrainingProgramActiveViewModel()
        {
            Exercises = new ObservableCollection<ModifyExercise>();
            BackCommand = new Command(OnBack);
        }

        private string _TrainingProgramActiveID;

        public string TrainingProgramActiveID
        {
            get { return _TrainingProgramActiveID; }
            set
            {
                _TrainingProgramActiveID = value;
                LoadTrainingProgramID(value);
            }
        }

        public async void LoadTrainingProgramID(string trainingProgramId)
        {
            try
            {
                _trainingProgram = await App.DatabaseHelper.GetTrainingProgramByIdAsync(Convert.ToInt32(trainingProgramId));

                var exers = await App.DatabaseHelper.GetExercisesByTrainingProgramAsync(Convert.ToInt32(trainingProgramId));

                Exercises.Clear();

                foreach (var exer in exers)
                {
                    Exercises.Add(exer);
                }

                foreach (var exer in Exercises)
                {
                    exer.LapsArray = new ObservableCollection<Lap>();
                    for (int i = 0; i < exer.Laps; i++)
                    {
                        exer.LapsArray.Add(new Lap { Iteraiton = exer.Iteration, Weight = exer.Weight, IsChecked = false });
                    }
                }
                                  /*_trainingProgram = await DataStoreTrainingProgram.GetItemAsync(trainingProgramId);
                if (_trainingProgram != null)
                {
                    Exercises = new ObservableCollection<Exercise>(_trainingProgram.Exercises);
                    *//*foreach (var exercise in Exercises)
                    {
                        exercise.LapsArray = new ObservableCollection<Lap>();
                        for (int i = 0; i < exercise.Laps; i++)
                        {
                              exercise.LapsArray.Add(new Lap { Done = true });
                        }
                    }*//*
                }
                else
                {
                    Exercises.Clear();
                }*/
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Training program");
            }
        }
    }
}
