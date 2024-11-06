namespace DLang.Execution
{

    internal class ConsoleInput : Input
    {
        private readonly Queue<string> _tokens;

        public ConsoleInput()
        {
            _tokens = new();
        }

        public string? Next()
        {
            if (_tokens.Count == 0)
            {
                string? line = Console.ReadLine();
                if (line == null)
                {
                    return null;
                }

                foreach (var token in line.Split())
                {
                    _tokens.Enqueue(token);
                }
            }

            return _tokens.Dequeue();
        }

    }

}
