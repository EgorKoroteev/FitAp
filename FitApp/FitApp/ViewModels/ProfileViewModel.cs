using FitApp.Models;
using FitApp.ViewModels.Base;
using FitApp.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private string _Weight;
        public string Weight
        {
            get => _Weight;
            set => SetProperty(ref _Weight, value);
        }

        public Command SaveCommand { get; }
        public Command HistoryCommand { get; }

        public ProfileViewModel()
        {
            HistoryCommand = new Command(OnHistoryCommand);
            SaveCommand = new Command(async () => await OnSave(), ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            Initialize();

        }

        private async Task Initialize()
        {
            var profile = await App.DatabaseHelper.GetLastProfileAsync();
            if (profile != null)
                Weight = profile.Weight;
            /*            await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Трапеции" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Плечи" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Грудь" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Трицепс" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Бицепс" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Предплечья" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Живот" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Спина" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Нижняя часть спины" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Ягодицы" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Передняя часть бедер" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Задняя часть бедер" });
                        await App.DatabaseHelper.AddMuscleGroupAsync(new MuscleGroup { Name = "Голень" });*/
            var a = await App.DatabaseHelper.GetExercisesAsync();
            var v = 0;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_Weight);
        }

        private async Task OnSave()
        {
            Profile newProfile = new Profile()
            {
                Weight = Weight,
                EditDate = DateTime.Now
            };

            await App.DatabaseHelper.AddProfileAsync(newProfile);
        }

        private async void OnHistoryCommand()
        {
            await Shell.Current.GoToAsync(nameof(ProfileHistoryView));
        }
    }
}
