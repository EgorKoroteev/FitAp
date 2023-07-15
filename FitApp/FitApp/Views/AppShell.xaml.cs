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
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MuscleGroupsTrainingProgramView), typeof(MuscleGroupsTrainingProgramView));
            Routing.RegisterRoute(nameof(MuscleGroupView), typeof(MuscleGroupView));
            Routing.RegisterRoute(nameof(TrainHistoryView), typeof(TrainHistoryView));
            Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));
            Routing.RegisterRoute(nameof(ProfileHistoryView), typeof(ProfileHistoryView));
            Routing.RegisterRoute(nameof(TrainingProgramActiveView), typeof(TrainingProgramActiveView));
            Routing.RegisterRoute(nameof(TrainingProgramSettingsView), typeof(TrainingProgramSettingsView));
            Routing.RegisterRoute(nameof(TimerActiveView), typeof(TimerActiveView));
            Routing.RegisterRoute(nameof(ModifyTrainingProgramView), typeof(ModifyTrainingProgramView));
            Routing.RegisterRoute(nameof(AddNewTrainingProgramView), typeof(AddNewTrainingProgramView));
            Routing.RegisterRoute(nameof(ExercisesTrainingProgramView), typeof(ExercisesTrainingProgramView));
            Routing.RegisterRoute(nameof(TrainingProgramDetailView), typeof(TrainingProgramDetailView));

            Routing.RegisterRoute(nameof(ModifyExerciseView), typeof(ModifyExerciseView));
            Routing.RegisterRoute(nameof(ExerciseDetailView), typeof(ExerciseDetailView));
            Routing.RegisterRoute(nameof(EditExerciseView), typeof(EditExerciseView));
            Routing.RegisterRoute(nameof(ExercisesView), typeof(ExercisesView));
        }
    }
}