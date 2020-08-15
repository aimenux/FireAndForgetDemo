using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FireAndForgetDemo.Helpers
{
    public interface ITaskHelper
    {
        void FireAndForgetTask(Task fireForgetTask, string fireForgetTaskName = null, [CallerMemberName] string callerName = null);
        void OnTerminatedTask(Task previousTask, string previousTaskName = null, [CallerMemberName] string callerName = null);
    }
}
