namespace AsbaBank.Infrastructure.CommandScripts
{
    public abstract class ScriptBase
    {
        protected readonly JsonSerializer Serializer = new JsonSerializer();
        protected const string ScriptExtension = ".script";
    }
}