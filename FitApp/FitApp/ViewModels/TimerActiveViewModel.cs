using FitApp.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitApp.ViewModels
{
    public class TimerActiveViewModel : BaseViewModel
    {
        private int workTime;
        private int restTime;
        private int countRounds;
        private bool isWorking;
        private TimeSpan roundTime;
        private int currentRound;
        private int leftRounds;

        private CancellationTokenSource cancellationTokenSource;
        private Task timerTask;

        public TimerActiveViewModel()
        {
            Init();
            StartCommand = new Command(StartTimer);
            StopCommand = new Command(StopTimer);
            BackCommand = new Command(OnBack);

            // Инициализация значений по умолчанию
            isWorking = true; // Начинаем с работой
            roundTime = TimeSpan.FromSeconds(workTime); // Время одного раунда
            LeftRounds = countRounds - 0;
            currentRound = 1; // Начинаем с первого раунда
            
            // Запускаем таймер при инициализации ViewModel
            StartTimer();
        }

        private async Task Init()
        {
            var timer = await DataStoreTimer.GetItemAsync();
            workTime = timer.WorkTime;
            restTime = timer.RestTime;
            countRounds = timer.CountExercises;
        }

        public int LeftRounds
        {
            get => leftRounds;
            set => SetProperty(ref leftRounds, value);
        }

        public bool IsWorking
        {
            get { return isWorking; }
            set
            {
                if (isWorking != value)
                {
                    isWorking = value;
                    OnPropertyChanged(nameof(IsWorking));
                }
            }
        }

        public string TimerDisplay
        {
            get { return roundTime.ToString(@"mm\:ss"); }
        }

        public int CurrentRound
        {
            get { return currentRound; }
            set
            {
                if (currentRound != value)
                {
                    currentRound = value;
                    OnPropertyChanged(nameof(CurrentRound));
                }
            }
        }

        public Command BackCommand { get; }
        private async void OnBack()
        {
            cancellationTokenSource?.Cancel();
            await DataStoreTimer.DeleteItemAsync();
            await Shell.Current.GoToAsync("..");
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        private void StartTimer()
        {
            cancellationTokenSource?.Cancel(); // Отменяем предыдущий таймер

            cancellationTokenSource = new CancellationTokenSource();
            timerTask = RunTimerAsync(cancellationTokenSource.Token);
        }

        private void StopTimer()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task RunTimerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (countRounds >= CurrentRound)
                {
                    if (IsWorking)
                    {
                        // Работа
                        await Task.Delay(1000); // Задержка в 1 секунду
                        roundTime = roundTime.Subtract(TimeSpan.FromSeconds(1));
                        if (roundTime.TotalSeconds <= 0)
                        {
                            // Закончился раунд работы, переходим к отдыху
                            roundTime = TimeSpan.FromSeconds(restTime); // Время отдыха
                            CurrentRound++;

                            IsWorking = false;
                        }
                    }
                    else
                    {
                        // Отдых
                        await Task.Delay(1000); // Задержка в 1 секунду
                        roundTime = roundTime.Subtract(TimeSpan.FromSeconds(1));
                        if (roundTime.TotalSeconds <= 0)
                        {
                            // Закончился раунд отдыха, переходим к работе
                            roundTime = TimeSpan.FromSeconds(workTime); // Время работы
                            IsWorking = true;
                            LeftRounds = countRounds - CurrentRound + 1;
                        }
                    }
                }
                else
                {
                    cancellationTokenSource?.Cancel();
                    await DataStoreTimer.DeleteItemAsync();
                    await Shell.Current.GoToAsync("..");
                }

                OnPropertyChanged(nameof(TimerDisplay));
            }
        }
    }
}
