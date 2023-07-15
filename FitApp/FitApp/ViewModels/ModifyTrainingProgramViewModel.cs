using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    [QueryProperty(nameof(TrainingProgramID), nameof(TrainingProgramID))]
    public class ModifyTrainingProgramViewModel : BaseViewModel
    {
        public ModifyTrainingProgramViewModel()
        {
            Exercises = new ObservableCollection<ModifyExercise>();

            MemoryExercises = new ObservableCollection<ModifyExercise>();

            ExerciseTapped = new Command<ModifyExercise>(OnExerciseSelected);

            AddExerciseCommand = new Command(OnAddExercise);


            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            MessagingCenter.Subscribe<ExercisesTrainingProgramViewModel, Exercise>(this, "ModifyExerciseAdded", (sender, exercise) =>
            {
                _newModifyExercise = new ModifyExercise();
                _newModifyExercise.Name = exercise.Name;
                _newModifyExercise.Description = exercise.Description;
                //Exercises.Add(new ModifyExercise { Name = exercise.Name, Description = exercise.Description});
            });
        }

        private ModifyExercise _newModifyExercise { get; set; }

        private ModifyExercise _selectedItem;

        public TrainingProgram TrainingProgramStore { get; set; }

        public ObservableCollection<ModifyExercise> Exercises { get; }

        public ObservableCollection<ModifyExercise> MemoryExercises { get; }

        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<ModifyExercise> ExerciseTapped { get; }
        private string _TrainingProgramID;
        private string _Name;
        private string _Description;
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

        public async void LoadTrainingProgramID(string trainingProgramId)
        {
            try
            {
                TrainingProgramStore = await App.DatabaseHelper.GetTrainingProgramByIdAsync(Convert.ToInt32(trainingProgramId));
                Name = TrainingProgramStore.Name;
                Description = TrainingProgramStore.Description;

                var exercises = await App.DatabaseHelper.GetExercisesByTrainingProgramAsync(Convert.ToInt32(trainingProgramId));
                Exercises.Clear();
                MemoryExercises.Clear();
                foreach (var exer in exercises)
                {
                    Exercises.Add(exer);
                    MemoryExercises.Add(exer);
                }

                if (_newModifyExercise != null)
                {
                    Exercises.Add(_newModifyExercise);

                    _newModifyExercise = null;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Training Program");
            }
        }



        public ModifyExercise SelectedItem
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
            await Shell.Current.GoToAsync($"{nameof(MuscleGroupsTrainingProgramView)}?{nameof(MuscleGroupsTrainingProgramViewModel.TrainProgramId)}={TrainingProgramID}");
        }
        void OnExerciseSelected(ModifyExercise exercise)
        {
            if (exercise == null)
                return;

            Exercises.Remove(exercise);

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
            TrainingProgramStore.Name = Name;
            TrainingProgramStore.Description = Description;
            List<ModifyExercise> modifyExercises = new List<ModifyExercise>();

            List<ModifyExercise> memoryModifyExercises = new List<ModifyExercise>();

            foreach (var exer in Exercises)
            {
                modifyExercises.Add(exer);
            }

            foreach (var memory in MemoryExercises)
            {
                memoryModifyExercises.Add(memory);
            }

            await App.DatabaseHelper.UpdateTrainingProgramAsync(TrainingProgramStore, memoryModifyExercises, modifyExercises);

            await Shell.Current.GoToAsync("..");
        }
    }
}
