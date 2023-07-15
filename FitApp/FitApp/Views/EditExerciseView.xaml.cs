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
    public partial class EditExerciseView : ContentPage
    {
        private EditExerciseViewModel _viewModel;
        public EditExerciseView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new EditExerciseViewModel();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (_viewModel == null)
                return;

            if (_viewModel.SelectedMuscleGroups.Count <= e.CurrentSelection.Count)
            {
                _viewModel.SelectedMuscleGroups.Add(e.CurrentSelection[_viewModel.SelectedMuscleGroups.Count].ToString());
            }
            else
            {
                for (int i = 0; i < _viewModel.SelectedMuscleGroups.Count; i++)
                {
                    if (!e.CurrentSelection.Contains(_viewModel.SelectedMuscleGroups[i]))
                    {
                        _viewModel.SelectedMuscleGroups.Remove(_viewModel.SelectedMuscleGroups[i]);
                    }
                }

            }
        }
    }
}