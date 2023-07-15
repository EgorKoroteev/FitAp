using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(TrainingProgramID), nameof(TrainingProgramID))]
    public class TrainingProgramDetailViewModel : BaseViewModel
    {
        public ObservableCollection<ModifyExercise> Exercises { get; }

        public Command StartTrain { get; }
        public Command DeleteTrainingProgramCommand { get; }
        public Command EditTrainingProgramCommand { get; }

        private string _TrainingProgramID;
        private string _Name;
        private string _Description;
        public string Id { get; set; }

        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }

        public string TrainingProgramID
        {
            get
            {
                return _TrainingProgramID;
            }
            set
            {
                _TrainingProgramID = value;
                LoadTrainingProgramID(value);
            }
        }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }
        public TrainingProgramDetailViewModel()
        {
            Exercises = new ObservableCollection<ModifyExercise>();

            EditTrainingProgramCommand = new Command(EditTrainingProgram);
            DeleteTrainingProgramCommand = new Command(DeleteTrainingProgram);

            StartTrain = new Command(OnStartTrain);
            BackCommand = new Command(OnBack);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public async void OnStartTrain()
        {
            await Shell.Current.GoToAsync($"{nameof(TrainingProgramSettingsView)}?{nameof(TrainingProgramSettingsViewModel.TrainingProgramSettingsID)}={TrainingProgramID}");
        }

        public async void LoadTrainingProgramID(string trainingProgramId)
        {
            try
            {
                var trainingProgram = await App.DatabaseHelper.GetTrainingProgramByIdAsync(Convert.ToInt32(trainingProgramId));

                Name = trainingProgram.Name;
                Description = trainingProgram.Description;

                var exers = await App.DatabaseHelper.GetExercisesByTrainingProgramAsync(Convert.ToInt32(trainingProgramId));
                Exercises.Clear();
                foreach (var exer in exers)
                {
                    Exercises.Add(exer);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Training program");
            }
        }

        public async void EditTrainingProgram()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyTrainingProgramView)}?{nameof(ModifyTrainingProgramViewModel.TrainingProgramID)}={TrainingProgramID}");
        }

        public async void DeleteTrainingProgram()
        {
            await App.DatabaseHelper.DeleteTrainingProgramAsync(Convert.ToInt32(TrainingProgramID));
            await Shell.Current.GoToAsync("..");
        }
    }
}
