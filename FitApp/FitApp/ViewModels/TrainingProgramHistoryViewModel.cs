using FitApp.Models;
using FitApp.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class TrainingProgramHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<TrainingHistory> _historyList;

        public ObservableCollection<TrainingHistory> HistoryList
        {
            get { return _historyList; }
            set
            {
                _historyList = value;
                OnPropertyChanged();
            }
        }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        public TrainingProgramHistoryViewModel()
        {
            HistoryList = new ObservableCollection<TrainingHistory>();
            BackCommand = new Command(OnBack);
            ExecuteLoadTrainingHistory();
        }
        public async Task ExecuteLoadTrainingHistory()
        {
            try
            {
                HistoryList.Clear();
                var historyList = await App.DatabaseHelper.GetTrainHistoriesAsync();
                foreach (var history in historyList)
                {
                    HistoryList.Add(history);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
