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
                bool empty = true;
                while (empty)
                {
                    string? line = Console.ReadLine();
                    if (line == null)
                    {
                        return null;
                    }

                    foreach (var token in line.Split())
                    {
                        if (token == "")
                        {
                            continue;
                        }

                        empty = false;
                        _tokens.Enqueue(token);
                    }
                }
            }

            return _tokens.Dequeue();
        }

    }

}
