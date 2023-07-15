using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(TrainingProgramSettingsID), nameof(TrainingProgramSettingsID))]
    public class TrainingProgramSettingsViewModel : BaseViewModel
    {
        private string _TrainingProgramSettingsID;
        private TrainingProgram _trainingProgram;


/*        private int _Laps;

        public int Laps
        {
            get { return _Laps; }
            set => SetProperty(ref _Laps, value);
        }

        private int _Iteration;

        public int Iteration
        {
            get { return _Iteration; }
            set => SetProperty(ref _Iteration, value);
        }

        private int _Weight;

        public int Weight
        {
            get { return _Weight; }
            set => SetProperty(ref _Weight, value);
        }*/


        public Command StartTrain { get; }

        public TrainingProgramSettingsViewModel()
        {
            StartTrain = new Command(async () => await OnStartTrain());
            Exercises = new ObservableCollection<ModifyExercise>();
        }

        public string TrainingProgramSettingsID
        {
            get { return _TrainingProgramSettingsID; }
            set
            {
                _TrainingProgramSettingsID = value;
                LoadTrainingProgramID(value);
            }
        }

        private async void LoadTrainingProgramID(string trainingProgramId)
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

                OnPropertyChanged(nameof(Exercises));
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Training program");
            }
        }

        public ObservableCollection<ModifyExercise> Exercises
        {
            get;
            set;
            //get { return _trainingProgram?.Exercises; }
        }

        public async Task OnStartTrain()
        {
            if (_trainingProgram == null)
                return;

            await App.DatabaseHelper.UpdateModifyExercisesAsync(Exercises);
            await Shell.Current.GoToAsync($"{nameof(TrainingProgramActiveView)}?{nameof(TrainingProgramActiveViewModel.TrainingProgramActiveID)}={TrainingProgramSettingsID}");
        }
    }
}
