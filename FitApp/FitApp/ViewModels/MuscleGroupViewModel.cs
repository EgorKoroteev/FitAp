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
    public class MuscleGroupViewModel : BaseViewModel
    {
        private MuscleGroup _selectedItem;
        public ObservableCollection<MuscleGroup> MuscleGroups { get; }
        public Command AddExerciseCommand { get; }
        public Command<MuscleGroup> MuscleGroupTapped { get; }
        public MuscleGroupViewModel()
        {
            MuscleGroups = new ObservableCollection<MuscleGroup>();

            MuscleGroupTapped = new Command<MuscleGroup>(OnMuscleGroupSelected);

            AddExerciseCommand = new Command(OnAddExercise);

            ExecuteLoadExercisesCommand();
        }
        public async Task ExecuteLoadExercisesCommand()
        {
            try
            {
                MuscleGroups.Clear();
                var muscleGroup = await App.DatabaseHelper.GetMuscleGroupsAsync();

                foreach (var muscle in muscleGroup)
                {
                    MuscleGroups.Add(muscle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public MuscleGroup SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnMuscleGroupSelected(value);
            }
        }

        private async void OnAddExercise(object obj)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(EditExerciseView));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        async void OnMuscleGroupSelected(MuscleGroup muscleGroup)
        {
            if (muscleGroup == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ExercisesView)}?{nameof(ExercisesViewModel.MuscleGroupID)}={muscleGroup.Id}");
        }
    }
}
