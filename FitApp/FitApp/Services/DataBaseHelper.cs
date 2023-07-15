using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FitApp.Models;
using SQLite;

namespace FitApp.Database
{
    public class DataBaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DataBaseHelper(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Exercise>().Wait();
            _database.CreateTableAsync<MuscleGroup>().Wait();
            _database.CreateTableAsync<TrainingProgram>().Wait();
            _database.CreateTableAsync<TrainingHistory>().Wait();
            _database.CreateTableAsync<Profile>().Wait();
            _database.CreateTableAsync<TrainingExercises>().Wait();
            _database.CreateTableAsync<MuscleExercise>().Wait();
            _database.CreateTableAsync<ModifyExercise>().Wait();

            
        }

        public async Task Clear()
        {
            await _database.Table<Exercise>().DeleteAsync();
            await _database.Table<TrainingProgram>().DeleteAsync();
            await _database.Table<TrainingHistory>().DeleteAsync();
            await _database.Table<Profile>().DeleteAsync();
            await _database.Table<TrainingExercises>().DeleteAsync();
            await _database.Table<MuscleExercise>().DeleteAsync();
            await _database.Table<ModifyExercise>().DeleteAsync();
        }



        #region Упражнения

        public async Task<bool> AddExerciseAsync(Exercise exercise)
        {
            int rowsAffected = await _database.InsertAsync(exercise);
            if (rowsAffected > 0)
            {
                foreach (var muscle in exercise.MuscleGroup.Split(','))
                {
                    var group = await _database.Table<MuscleGroup>().FirstOrDefaultAsync(s => s.Name == muscle);
                    var muscleExercise = new MuscleExercise()
                    {
                        MuscleGroupId = group.Id,
                        ExerciseId = exercise.Id
                    };
                    await _database.InsertAsync(muscleExercise);
                }
                return true;
            }
            return rowsAffected > 0;
        }

        public async Task UpdateExerciseAsync(Exercise exercise, List<string> memoryMuscle, List<string> newMuscle)
        {
            await _database.UpdateAsync(exercise);

            List<string> exceptMemory = memoryMuscle.Except(newMuscle).ToList();

            var newExercise = await _database.Table<Exercise>().FirstOrDefaultAsync(s => s.Name == exercise.Name);

            foreach (var exer in exceptMemory)
            {
                MuscleGroup muscleGroup = await _database.Table<MuscleGroup>().FirstOrDefaultAsync(s => s.Name == exer);
                MuscleExercise muscleExercise = await _database.Table<MuscleExercise>().FirstOrDefaultAsync(a => a.MuscleGroupId == muscleGroup.Id && a.ExerciseId == newExercise.Id);
                await _database.DeleteAsync<MuscleExercise>(muscleExercise.Id);
            }

            exceptMemory = newMuscle.Except(memoryMuscle).ToList();

            foreach (var memory in exceptMemory)
            {
                MuscleGroup muscleGroup = await _database.Table<MuscleGroup>().FirstOrDefaultAsync(s => s.Name == memory);

                var muscleExercise = new MuscleExercise
                {
                    MuscleGroupId = muscleGroup.Id,
                    ExerciseId = exercise.Id
                };
                await _database.InsertAsync(muscleExercise);
            }




        }

        public async Task DeleteExerciseAsync(int id)
        {
            var muscleExer = await _database.Table<MuscleExercise>().FirstOrDefaultAsync(s => s.Id == id);
            await _database.DeleteAsync<Exercise>(muscleExer.ExerciseId);
        }

        public async Task<Exercise> GetExerciseAsync(int id)
        {
            var muscleExer = await _database.Table<MuscleExercise>().FirstOrDefaultAsync(s => s.Id == id);
            return await _database.GetAsync<Exercise>(muscleExer.ExerciseId);
        }

        public async Task<List<Exercise>> GetExercisesAsync()
        {
            /*            return await _database.Table<Exercise>()
                                              .OrderBy(x => x.Id) // Добавляем сортировку по идентификатору
                                              .ToListAsync();*/

            return await _database.QueryAsync<Exercise>("SELECT * FROM Exercise");
        }

        public async Task<int> GetExercisesCount()
        {
            return await _database.Table<Exercise>().CountAsync();
        }

        public async Task DeleteModifyExerciseAsync(int id)
        {
            await _database.DeleteAsync<ModifyExercise>(id);
        }

        #endregion

        #region Мускулы


        public async Task DeleteMuscleExerciseAsync(int id)
        {
            await _database.DeleteAsync<MuscleExercise>(id);
        }

        public async Task<bool> AddMuscleGroupAsync(MuscleGroup muscleGroup)
        {
            int rowsAffected = await _database.InsertAsync(muscleGroup);
            return rowsAffected > 0;
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsAsync()
        {
            return await _database.Table<MuscleGroup>().ToListAsync();
        }

        public async Task<MuscleGroup> GetMuscleGroupByIdAsync(int muscleGroupId)
        {
            return await _database.Table<MuscleGroup>().FirstOrDefaultAsync(s => s.Id == muscleGroupId);
        }


        public async Task<MuscleGroup> GetMuscleGroupByNameAsync(string name)
        {
            return await _database.Table<MuscleGroup>().FirstOrDefaultAsync(s => s.Name == name);
        }


        /*public async Task<bool> AddMuscleExerciseAsync(MuscleExercise muscleExercise)
        {
            int rowsAffected = await _database.InsertAsync(muscleExercise);
            return rowsAffected > 0;
        }*/

        // Возвращает массив упражнений для определенной группы мышц
        public async Task<List<Exercise>> GetExercisesByMuscleGroupAsync(int muscleGroupId)
        {
            var query = "SELECT * FROM Exercise " +
                        "INNER JOIN MuscleExercise ON Exercise.Id = MuscleExercise.ExerciseId " +
                        "WHERE MuscleExercise.MuscleGroupId = ?";
            return await _database.QueryAsync<Exercise>(query, muscleGroupId);
        }


        /*public async Task<List<MuscleExercise>> GetMuscleExercisesAsync()
        {
            return await _database.Table<MuscleExercise>().ToListAsync();
        }*/

        #endregion

        #region Тренировки

        public async Task<bool> AddTrainingProgramAsync(TrainingProgram program, List<ModifyExercise> exercises)
        {
            int rowsAffected = await _database.InsertAsync(program);
            if (rowsAffected > 0)
            {
                await _database.InsertAllAsync(exercises);
                // Добавляем связи между программой и упражнениями в таблицу TrainingExercises
                foreach (var exercise in exercises)
                {
                    var trainingExercises = new TrainingExercises
                    {
                        ProgramId = program.Id,
                        ExerciseId = exercise.Id
                    };
                    await _database.InsertAsync(trainingExercises);
                }

                return true;
            }
            return false;

        }

        public async Task<List<TrainingProgram>> GetTrainingProgramsAsync()
        {
            return await _database.Table<TrainingProgram>().ToListAsync();
        }

        public async Task<TrainingProgram> GetTrainingProgramByIdAsync(int trainingProgramId)
        {
            return await _database.Table<TrainingProgram>().FirstOrDefaultAsync(s => s.Id == trainingProgramId);
        }

        public async Task UpdateTrainingProgramAsync(TrainingProgram trainingProgram, List<ModifyExercise> memoryModifyExercises, List<ModifyExercise> modifyExercise)
        {
            await _database.UpdateAsync(trainingProgram);

            List<ModifyExercise> exceptMemory = memoryModifyExercises.Except(modifyExercise).ToList();

            foreach (var exer in exceptMemory)
            {
                TrainingExercises trainingExercises = await _database.Table<TrainingExercises>().FirstOrDefaultAsync(s => s.ExerciseId == exer.Id);
                await _database.DeleteAsync<TrainingExercises>(trainingExercises.Id);
                await _database.DeleteAsync<ModifyExercise>(exer.Id);

            }

            exceptMemory = modifyExercise.Except(memoryModifyExercises).ToList();
            var a = 0;
            foreach (var memory in exceptMemory)
            {
                await _database.InsertAsync(memory);
                var trainingExercises = new TrainingExercises
                {
                    ProgramId = trainingProgram.Id,
                    ExerciseId = memory.Id
                };
                await _database.InsertAsync(trainingExercises);
            }
        }

        //
        public async Task DeleteTrainingProgramAsync(int trainingExercisesId)
        {
            await _database.DeleteAsync<TrainingProgram>(trainingExercisesId);
        }

        // Возвращает массив упражнений для определенной тренировки
        public async Task<List<ModifyExercise>> GetExercisesByTrainingProgramAsync(int trainingProgramId)
        {
            var query = "SELECT * FROM ModifyExercise " +
                        "INNER JOIN TrainingExercises ON ModifyExercise.Id = TrainingExercises.ExerciseId " +
                        "WHERE TrainingExercises.ProgramId = ?";
            return await _database.QueryAsync<ModifyExercise>(query, trainingProgramId);
        }

        public async Task UpdateModifyExercisesAsync(ObservableCollection<ModifyExercise> modifyExercises)
        {
            await _database.UpdateAllAsync(modifyExercises);
        }
        #endregion

        #region Вес

        public async Task<bool> AddProfileAsync(Profile profile)
        {
            int rowsAffected = await _database.InsertAsync(profile);
            return rowsAffected > 0;
        }

        public async Task UpdateProfileAsync(Profile profile)
        {
            await _database.UpdateAsync(profile);
        }

        public async Task DeleteProfileAsync(string id)
        {
            await _database.DeleteAsync<Profile>(id);
        }

        public async Task<Profile> GetProfileAsync(string id)
        {
            return await _database.GetAsync<Profile>(id);
        }

        public async Task<List<Profile>> GetProfilesAsync()
        {
            return await _database.Table<Profile>().ToListAsync();
        }

        public async Task<Profile> GetLastProfileAsync()
        {
            return await _database.Table<Profile>()
                                   .OrderByDescending(x => x.Id)
                                   .FirstOrDefaultAsync();
        }

        #endregion

        #region История тренировок

        public async Task<bool> AddTrainHistoryAsync(TrainingHistory trainingHistory)
        {
            int rowsAffected = await _database.InsertAsync(trainingHistory);
            return rowsAffected > 0;
        }

        public async Task<List<TrainingHistory>> GetTrainHistoriesAsync()
        {
            return await _database.Table<TrainingHistory>().ToListAsync();
        }

        #endregion
    }
}
