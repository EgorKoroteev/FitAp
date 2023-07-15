using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(ExerciseID), "exerciseId")]
    [QueryProperty(nameof(MuscleGroupId), nameof(MuscleGroupId))]
    public class ExerciseDetailViewModel : BaseViewModel
    {
        private string _ExerciseID;
        private string _MuscleGroupId;
        private string _Name;
        private string _Description;
        private string _MuscleGroup;
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

        public string MuscleGroup
        {
            get => _MuscleGroup;
            set => SetProperty(ref _MuscleGroup, value);
        }

        public string MuscleGroupId
        {
            get
            {
                return _MuscleGroupId;
            }
            set
            {
                _MuscleGroupId = value;
                LoadMuscleGroupId(value);
            }
        }

        public string ExerciseID
        {
            get
            {
                return _ExerciseID;
            }
            set
            {
                _ExerciseID = value;
            }
        }
        public ExerciseDetailViewModel()
        {
            EditExerciseCommand = new Command(EditExercise);
            DeleteExerciseCommand = new Command(DeleteExercise);
            BackCommand = new Command(OnBack);
        }
        public Command DeleteExerciseCommand { get; }
        public Command EditExerciseCommand { get; }
        public Command BackCommand { get; }
        private async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadMuscleGroupId(string muscleGroupId)
        {
            try
            {
                var exer = await App.DatabaseHelper.GetExerciseAsync(Convert.ToInt32(ExerciseID));
                Name = exer.Name;
                Description = exer.Description;
                MuscleGroup = exer.MuscleGroup;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Exercise");
            }
        }
        public async void EditExercise()
        {
            await Shell.Current.GoToAsync($"{nameof(ModifyExerciseView)}?{nameof(ModifyExerciseViewModel.MuscleGroupId)}={MuscleGroupId}&exerciseID={ExerciseID}");
        }
        public async void DeleteExercise()
        {
            //await App.DatabaseHelper.DeleteMuscleExerciseAsync(Convert.ToInt32(ExerciseID));
            await App.DatabaseHelper.DeleteExerciseAsync(Convert.ToInt32(ExerciseID));
            await Shell.Current.GoToAsync("..");
        }
    }
}
