namespace DLang.Execution
{

    internal class Stack
    {
        public readonly List<Namespace> Namespaces;
        public readonly int MaxDepth;

        public Stack(int maxDepth)
        {
            if (maxDepth <= 0)
            {
                throw new ArgumentException("negative staick maxDepth");
            }

            Namespaces = [];
            MaxDepth = maxDepth;
        }

        public void Push()
        {
            if (Namespaces.Count + 1 > MaxDepth)
            {
                throw new StackOverflowError($"stack overflow (max depth: {MaxDepth})");
            }

            Namespaces.Add(new());
        }

        public void Pop()
        {
            Namespaces.RemoveAt(Namespaces.Count - 1);
        }

        public Namespace Peek()
        {
            return Namespaces[^1];
        }

        public Value GetValue(string name)
        {
            foreach (Namespace ns in Namespaces.AsEnumerable().Reverse())
            {
                if (ns.Has(name))
                {
                    return ns.Get(name);
                }
            }

            throw new InvalidOperationException();
        }

        public void PutValue(string name, Value value)
        {
            Namespaces[^1].Put(name, value);
        }
    }

}
