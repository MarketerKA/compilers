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
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (line == null)
                    {
                        return null;
                    }

                    var tokens = line.Split();
                    if (tokens.Length == 0)
                    {
                        continue;
                    }

                    foreach (var token in line.Split())
                    {
                        _tokens.Enqueue(token);
                    }

                    break;
                }
            }

            return _tokens.Dequeue();
        }

    }

}
