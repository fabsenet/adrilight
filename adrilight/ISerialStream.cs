namespace adrilight
{
    public interface ISerialStream
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}