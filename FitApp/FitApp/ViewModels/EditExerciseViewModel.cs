using FitApp.Models;
using FitApp.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class EditExerciseViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private ObservableCollection<string> muscleGroups;
        private List<string> selectedMuscleGroups;

        public EditExerciseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();

            muscleGroups = new ObservableCollection<string>();
            SelectedMuscleGroups = new List<string>();

            Init();
        }

        private async Task Init()
        {
            var groups = await App.DatabaseHelper.GetMuscleGroupsAsync();
            foreach (var group in groups)
            {
                muscleGroups.Add(group.Name);
            }
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ObservableCollection<string> MuscleGroups
        {
            get => muscleGroups;
            set => SetProperty(ref muscleGroups, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Exercise newExercise = new Exercise()
            {
                Name = Name,
                Description = Description,
                MuscleGroup = string.Join(",", SelectedMuscleGroups)
            };

            await App.DatabaseHelper.AddExerciseAsync(newExercise);

            await Shell.Current.GoToAsync("..");
        }

        public List<string> SelectedMuscleGroups
        {
            get => selectedMuscleGroups;
            set => SetProperty(ref selectedMuscleGroups, value);
        }
    }
}
