using FitApp.Models;
using FitApp.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(TrainProgramID), "trainProgramID")]
    [QueryProperty(nameof(MuscleGroupID), nameof(MuscleGroupID))]
    public class ExercisesTrainingProgramViewModel : BaseViewModel
    {
        private Exercise _selectedItem;
        private string _MuscleGroupID;

        public string MuscleGroupID
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

        private string _TrainProgramID;

        public string TrainProgramID
        {
            get
            {
                return _TrainProgramID;
            }
            set
            {
                _TrainProgramID = value;
            }
        }

        public ObservableCollection<Exercise> Exercises { get; }
        public Command<Exercise> ExerciseTapped { get; }

        public ExercisesTrainingProgramViewModel()
        {
            Title = "Упражнения";
            Exercises = new ObservableCollection<Exercise>();

            ExerciseTapped = new Command<Exercise>(OnExerciseSelected);

            SelectedItem = null;
        }

        public async void LoadMuscleGroupID(string muscleGroupID)
        {
            try
            {
                Exercises.Clear();
                var exercises = await App.DatabaseHelper.GetExercisesByMuscleGroupAsync(Convert.ToInt32(muscleGroupID));
                foreach (var exer in exercises)
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

        async void OnExerciseSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            if (TrainProgramID != "")
            {
                /*var trainProgram = DataStoreTrainingProgram.GetItem(TrainProgramID);
                trainProgram.Exercises.Add(exercise);
                await Shell.Current.GoToAsync("../..");*/

                MessagingCenter.Send(this, "ModifyExerciseAdded", exercise);
                await Shell.Current.GoToAsync("../..");
            }
            else
            {
                MessagingCenter.Send(this, "ExerciseAdded", exercise);
                await Shell.Current.GoToAsync("../..");
            }
        }
    }
}
