using FitApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp.Services
{
    public class MockDataStore : IDataStore<Timer>
    {
         
        readonly List<Timer> Timers;

        public MockDataStore()
        {
            Timers = new List<Timer>();
        }

        public int GetCount()
        {
            return Timers.Count;
        }
        public async Task<bool> AddItemAsync(Timer item)
        {
            Timers.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync()
        {

            Timers.Remove(Timers[0]);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteLastItemAsync()
        {
            Timers.RemoveAt(Timers.Count - 1);

            return await Task.FromResult(true);
        }

        public async Task<Timer> GetItemAsync()
        {
            return await Task.FromResult(Timers[0]);
        }

        public async Task<IEnumerable<Timer>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Timers);
        }


        public async Task<bool> UpdateItemAsync(Timer item)
        {
            var oldTimer = Timers.Where((Timer arg) => arg.Id == item.Id).FirstOrDefault();
            Timers.Remove(oldTimer);
            Timers.Add(item);

            return await Task.FromResult(true);
        }
    }
}

