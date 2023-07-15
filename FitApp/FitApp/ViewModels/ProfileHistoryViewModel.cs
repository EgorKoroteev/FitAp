using FitApp.Models;
using FitApp.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class ProfileHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<Profile> _profileList;

        public ObservableCollection<Profile> ProfileList
        {
            get { return _profileList; }
            set
            {
                _profileList = value;
                OnPropertyChanged();
            }
        }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public ProfileHistoryViewModel()
        {
            Title = "Истормя";
            ProfileList = new ObservableCollection<Profile>();
            BackCommand = new Command(OnBack);
            ExecuteLoadProfile();
        }
        public async Task ExecuteLoadProfile()
        {
            try
            {
                ProfileList.Clear();
                var profileList = await App.DatabaseHelper.GetProfilesAsync();

                for (int i = 0; i < profileList.Count; i++)
                {
                    ProfileList.Add(profileList[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
