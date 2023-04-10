namespace HangFireApp.Services
{
    public interface IHangFireJobService
    {
        void FireAndForgetJob();

        void RecurringJob();

        void DelayedJob();

        void ContinuationJob();
    }
}
