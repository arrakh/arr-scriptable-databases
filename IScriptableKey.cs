using System;

namespace Arr.ScriptableDatabases
{
    public interface IScriptableKey
    {
        public string Id { get; }
    }

    public interface IScriptableKey<out T> where T : IEquatable<T>
    {
        public T Key { get; }
    }
}