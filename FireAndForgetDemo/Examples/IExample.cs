using System.Threading.Tasks;

namespace FireAndForgetDemo.Examples
{
    public interface IExample
    {
        string Description { get; }
        Task RunAsync();
    }
}
