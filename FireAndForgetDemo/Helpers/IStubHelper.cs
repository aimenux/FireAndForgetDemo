using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireAndForgetDemo.Helpers
{
    public interface IStubHelper
    {
        Task<ICollection<int>> GetDataFromStableStorageAsync();
        Task<ICollection<int>> GetDataFromUnstableStorageAsync();
    }
}
