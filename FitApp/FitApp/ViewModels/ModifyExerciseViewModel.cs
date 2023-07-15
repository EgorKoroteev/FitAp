using FitApp.Models;
using FitApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(ExerciseID), "exerciseID")]
    [QueryProperty(nameof(MuscleGroupId), nameof(MuscleGroupId))]
    public class ModifyExerciseViewModel : BaseViewModel
    {
        private ObservableCollection<object> _PreSelectedMuscleGroups;
        public ObservableCollection<object> PreSelectedMuscleGroups
        {
            get => _PreSelectedMuscleGroups;
            set => SetProperty(ref _PreSelectedMuscleGroups, value);
        }

        private string _MuscleGroupId;
        private string _ExerciseID;
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

        private ObservableCollection<string> muscleGroups;
        private List<string> selectedMuscleGroups;


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

        public async void LoadMuscleGroupId(string muscleGroupId)
        {
            try
            {
                var groups = await App.DatabaseHelper.GetMuscleGroupsAsync();
                foreach (var group in groups)
                {
                    muscleGroups.Add(group.Name);
                }

                    var exer = await App.DatabaseHelper.GetExerciseAsync(Convert.ToInt32(ExerciseID));
                Name = exer.Name;
                Description = exer.Description;


                PreMuscleGroups = exer.MuscleGroup.Split(',').ToList();


                PreSelectedMuscleGroups = new ObservableCollection<object>();
                for (int i = 0; i < PreMuscleGroups.Count; i++)
                {
                    PreSelectedMuscleGroups.Add(PreMuscleGroups[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Exercise");
            }
        }


        public ObservableCollection<string> MuscleGroups
        {
            get => muscleGroups;
            set => SetProperty(ref muscleGroups, value);
        }

        public ModifyExerciseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            muscleGroups = new ObservableCollection<string>();
            PreMuscleGroups = new List<string>();

            // var groups = DataStore.GetItems();

            //foreach (var group in groups)
            //{
            //   muscleGroups.Add(group.Name);
            // }
        }

        public List<string> PreMuscleGroups
        {
            get => selectedMuscleGroups;
            set => SetProperty(ref selectedMuscleGroups, value);
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_Name)
                && !String.IsNullOrWhiteSpace(_Description);
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
                MuscleGroup = string.Join(",", PreSelectedMuscleGroups)
            };

            await App.DatabaseHelper.UpdateExerciseAsync(newExercise, PreMuscleGroups, newExercise.MuscleGroup.Split(',').ToList());
            await App.DatabaseHelper.AddExerciseAsync(newExercise);

            await Shell.Current.GoToAsync("..");


            /*foreach (var group in PreMuscleGroups)
            {
                foreach (var pre in PreSelectedMuscleGroups)
                {
                    if (group == (string)pre)
                    {
                        // var groupMuscle = DataStore.GetItemName(group);
                        *//*groupMuscle.Exercises.Remove(groupMuscle.Exercises.FirstOrDefault(s => s.Id == newExercise.Id));
                        groupMuscle.Exercises.Add(newExercise);*//*
                    }
                    else
                    {
                        //  var groupMuscle = DataStore.GetItemName(group);
                        // groupMuscle.Exercises.Remove(groupMuscle.Exercises.FirstOrDefault(s => s.Id == newExercise.Id));
                    }
                }
            }

            await Shell.Current.GoToAsync("..");*/
        }
    }
}
