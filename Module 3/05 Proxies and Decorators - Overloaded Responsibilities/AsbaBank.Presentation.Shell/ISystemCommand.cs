namespace AsbaBank.Presentation.Shell
{
    public interface ISystemCommand
    {
        string Usage { get; }
        string Key { get; }
        void Execute(string[] args);
    }
}