using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class AddNewTrainingProgramViewModel : BaseViewModel
    {
        private Exercise _selectedItem;
        public ObservableCollection<Exercise> Exercises { get; }

        public Command AddExerciseCommand { get; }
        public Command<Exercise> ExerciseTapped { get; }
        private string _Name;
        private string _Description;
        public AddNewTrainingProgramViewModel()
        {
            Exercises = new ObservableCollection<Exercise>();

            ExerciseTapped = new Command<Exercise>(OnExerciseSelected);

            AddExerciseCommand = new Command(OnAddExercise);

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            MessagingCenter.Subscribe<ExercisesTrainingProgramViewModel, Exercise>(this, "ExerciseAdded", (sender, exercise) =>
            {
                Exercises.Add(exercise);
            });
        }

        public Exercise SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnExerciseSelected(value);
            }
        }
        //
        private async void OnAddExercise(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(MuscleGroupsTrainingProgramView)}?{nameof(MuscleGroupsTrainingProgramViewModel.TrainProgramId)}=");
        }
        void OnExerciseSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            Exercises.Remove(exercise);
        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(_Name)
                && !string.IsNullOrWhiteSpace(_Description);
        }
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
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave()
        {
            TrainingProgram newTrainingProgram = new TrainingProgram()
            {

                Name = Name,
                Description = Description
            };

            List<ModifyExercise> modifyExercises = new List<ModifyExercise>();

            foreach (var exer in Exercises)
            {
                var modifyExercise = new ModifyExercise()
                {
                    Name = exer.Name,
                    Description = exer.Description,
                    Laps = 0,
                    Iteration = 0,
                    Weight = 0
                };
                modifyExercises.Add(modifyExercise);
            }
            await App.DatabaseHelper.AddTrainingProgramAsync(newTrainingProgram, modifyExercises);
            await Shell.Current.GoToAsync("..");
        }
    }
}
