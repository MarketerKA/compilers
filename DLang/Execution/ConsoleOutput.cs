namespace DLang.Execution
{

    internal class ConsoleOutput : Output
    {
        public ConsoleOutput() { }

        public void Write(string value)
        {
            Console.Write(value);
        }
    }

}
