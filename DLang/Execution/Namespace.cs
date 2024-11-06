namespace DLang.Execution
{

    internal class Namespace
    {
        public readonly Dictionary<string, Value> Names;

        public Namespace()
        {
            Names = [];
        }

        public void Put(string name, Value value)
        {
            Names.Add(name, value);
        }

        public bool Has(string name)
        {
            return Names.ContainsKey(name);
        }

        public Value Get(string name)
        {
            return Names[name];
        }
    }

}
