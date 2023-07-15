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
    [QueryProperty(nameof(TrainProgramId), nameof(TrainProgramId))]
    public class MuscleGroupsTrainingProgramViewModel : BaseViewModel
    {
        private string _TrainProgramId;

        public string TrainProgramId
        {
            get
            {
                return _TrainProgramId;
            }
            set
            {
                _TrainProgramId = value;
            }
        }

        private MuscleGroup _selectedItem;
        public ObservableCollection<MuscleGroup> MuscleGroups { get; }
        public Command AddExerciseCommand { get; }
        public Command<MuscleGroup> MuscleGroupTapped { get; }
        public MuscleGroupsTrainingProgramViewModel()
        {
            MuscleGroups = new ObservableCollection<MuscleGroup>();

            MuscleGroupTapped = new Command<MuscleGroup>(OnMuscleGroupSelected);

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

        async void OnMuscleGroupSelected(MuscleGroup muscleGroup)
        {
            if (muscleGroup == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ExercisesTrainingProgramView)}?{nameof(ExercisesTrainingProgramViewModel.MuscleGroupID)}={muscleGroup.Id}&trainProgramID={TrainProgramId}");
        }
    }
}
