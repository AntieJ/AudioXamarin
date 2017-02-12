using System.Threading.Tasks;

namespace Example_WorkingWithAudio
{
    interface INotificationReceiver
    {
        Task StartAsync();

        void Stop();
    }
}