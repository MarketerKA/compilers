namespace DLang.Analysis
{

    internal class NamespaceStack
    {
        private readonly List<HashSet<string>> _stack;
        private readonly List<bool> _functionStack;

        public NamespaceStack()
        {
            _stack = [];
            _functionStack = [];
        }

        public bool NameExists(string name)
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("Attempted to check name with no existing namespaces");
            }

            foreach (var @namespace in _stack)
            {
                if (@namespace.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool NameExistsInCurrentNamespace(string name)
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("Attempted to check name with no existing namespaces");
            }

            if (_stack[^1].Contains(name))
            {
                return true;
            }

            return false;
        }

        public bool IsFunctionBody()
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                if (_functionStack[i])
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(string name)
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("Attempted to add name with no existing namespaces");
            }

            _stack[^1].Add(name);
        }

        public void Push(bool isFunctionBody = false)
        {
            _stack.Add([]);
            _functionStack.Add(isFunctionBody);
        }

        public void Pop()
        {
            _stack.RemoveAt(_stack.Count - 1);
            _functionStack.RemoveAt(_functionStack.Count - 1);
        }
    }

}
