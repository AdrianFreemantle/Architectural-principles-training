using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandScripts
{
    public class ScriptPlayer : ScriptBase
    {
        private readonly IPublishCommands commandPublisher;

        public ScriptPlayer(IPublishCommands commandPublisher)
        {
            this.commandPublisher = commandPublisher;
        }

        public IEnumerable<string> GetAvailableScripts()
        {
            return Directory.EnumerateFiles(".", "*" + ScriptExtension)
                            .Select(s => s.Replace(ScriptExtension, "").Replace(".\\", ""));
        }

        public void Play(string scriptName)
        {
            if (String.IsNullOrWhiteSpace(scriptName))
            {
                throw new ArgumentException("Please provide a valid script name");
            }

            string fileName = scriptName + ScriptExtension;

            RunScript(ReadScriptFile(fileName));
        }

        private IEnumerable<ICommand> ReadScriptFile(string fileName)
        {
            using (var reader = File.OpenRead(fileName))
            {
                return Serializer.Deserialize<Queue<ICommand>>(reader);
            }
        }

        private void RunScript(IEnumerable<ICommand> script)
        {
            foreach (var command in script)
            {
                commandPublisher.Publish(command);
            }
        }
    }
}