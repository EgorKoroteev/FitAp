using FitApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingProgramActiveView : ContentPage
    {
        public TrainingProgramActiveViewModel _viewModel;
        public TrainingProgramActiveView()
        {
            InitializeComponent();

            _viewModel = new TrainingProgramActiveViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.StartStopwatch();
        }
    }
}