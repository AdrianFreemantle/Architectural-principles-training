using System;

namespace AsbaBank.Presentation.Shell.SystemCommands
{
    public class RunScript : ISystemCommand
    {
        public string Usage { get { return String.Format("{0} <ScriptName>", Key); } }
        public string Key { get { return "RunScript"; } }

        public void Execute(string[] args)
        {
            Console.WriteLine("Running script...");
            var scriptPlayer = Environment.GetScriptPlayer();
            scriptPlayer.Play(args[0]);
            Console.WriteLine("Script completed.");
        }
    }
}