using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitApp.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync();
        Task<bool> DeleteLastItemAsync();
        Task<T> GetItemAsync();
    }
}
