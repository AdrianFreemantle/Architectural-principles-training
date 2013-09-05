using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AsbaBank.Core.Commands;

namespace AsbaBank.Infrastructure.CommandScripts
{
    public class ScriptRecorder : ScriptBase
    {
        private readonly Queue<ICommand> script;
        private bool recording;

        public ScriptRecorder()
        {
            script = new Queue<ICommand>();
        }

        public void BeginRecording()
        {
            recording = true;
        }

        public void AddCommand(ICommand command)
        {
            if (recording)
            {
                script.Enqueue(command);
            }
        }

        public void Save(string scriptName)
        {
            if (String.IsNullOrWhiteSpace(scriptName))
            {
                throw new ArgumentException("Please provide a valid script name");
            }

            recording = false;
            string fileName = scriptName + ScriptExtension;
            WriteScriptFile(fileName);
        }

        private void WriteScriptFile(string fileName)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                Serializer.Serialize(writer, script);
            }
        }
    }
}