using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(MuscleGroupID), nameof(MuscleGroupID))]
    internal class ExercisesViewModel : BaseViewModel
    {
        private Exercise _selectedItem;
        private int _MuscleGroupID;
        public ObservableCollection<Exercise> Exercises { get; }
        public Command AddExerciseCommand { get; }
        public Command<Exercise> ExerciseTapped { get; }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public ExercisesViewModel()
        {

            Exercises = new ObservableCollection<Exercise>();

            ExerciseTapped = new Command<Exercise>(OnExerciseSelected);

            AddExerciseCommand = new Command(OnAddExercise);
            BackCommand = new Command(OnBack);
        }

        public int MuscleGroupID
        {
            get
            {
                return _MuscleGroupID;
            }
            set
            {
                _MuscleGroupID = value;
                LoadMuscleGroupID(value);
            }
        }

        public async void LoadMuscleGroupID(int muscleGroupID)
        {
            try
            {
                var group = await App.DatabaseHelper.GetMuscleGroupByIdAsync(MuscleGroupID);
                Title = group.Name;
                Exercises.Clear();

                var exers = await App.DatabaseHelper.GetExercisesByMuscleGroupAsync(MuscleGroupID);
                
                foreach (var exer in exers)
                {
                    Exercises.Add(exer);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Muscle Group");
            }
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
        private async void OnAddExercise(object obj)
        {
            await Shell.Current.GoToAsync(nameof(EditExerciseView));
        }
        async void OnExerciseSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ExerciseDetailView)}?{nameof(ExerciseDetailViewModel.MuscleGroupId)}={MuscleGroupID}&exerciseId={exercise.Id}");
        }
    }
}
