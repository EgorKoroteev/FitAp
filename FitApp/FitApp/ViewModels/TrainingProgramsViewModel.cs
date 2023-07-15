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
    public class TrainingProgramsViewModel : BaseViewModel
    {
        private TrainingProgram _selectedItem;
        public ObservableCollection<TrainingProgram> TrainingPrograms { get; }

        public Command AddTrainingProgramCommand { get; }
        public Command<TrainingProgram> TrainingProgramTapped { get; }
        public Command TrainingHistoryCommand { get; }

        public TrainingProgramsViewModel()
        {
            Title = "Тренировки";
            TrainingPrograms = new ObservableCollection<TrainingProgram>();


            TrainingProgramTapped = new Command<TrainingProgram>(OnTrainingProgramSelected);

            AddTrainingProgramCommand = new Command(OnAddTrainingProgram);

            TrainingHistoryCommand = new Command(OnTrainingHistory);


        }

        public async Task ExecuteLoadTrainingProgramsCommand()
        {
            try
            {
                TrainingPrograms.Clear();
                var trainingPrograms = await App.DatabaseHelper.GetTrainingProgramsAsync();
                foreach (var trainingProgram in trainingPrograms)
                {
                    TrainingPrograms.Add(trainingProgram);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public TrainingProgram SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnTrainingProgramSelected(value);
            }
        }

        private async void OnAddTrainingProgram(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddNewTrainingProgramView));
        }

        async void OnTrainingProgramSelected(TrainingProgram trainingProgram)
        {
            if (trainingProgram == null)
                return;


            await Shell.Current.GoToAsync($"{nameof(TrainingProgramDetailView)}?{nameof(TrainingProgramDetailViewModel.TrainingProgramID)}={trainingProgram.Id}");
        }

        private async void OnTrainingHistory()
        {
            await Shell.Current.GoToAsync(nameof(TrainHistoryView));
        }
    }
}
