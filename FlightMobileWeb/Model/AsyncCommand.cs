using System.Threading.Tasks;

namespace FlightMobileWeb.Models
{
    public enum Result { Ok, NotFound, TimeOut };
    public class AsyncCommand
    {
        public Command Command { get; private set; }
        public Task<Result> Task { get => Completion.Task; }
        public TaskCompletionSource<Result> Completion { get; private set; }
        public AsyncCommand(Command c)
        {
            Command = c;
            Completion = new TaskCompletionSource<Result>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }

}