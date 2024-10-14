namespace DLang
{

    internal class ArgsError : Exception
    {
        public ArgsError(string message) : base(message) { }

        public ArgsError(string message, Exception innerException) : base(message, innerException) { }
    }

    internal class Args
    {
        public readonly string InputFilePath;

        public Args(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Not enough arguments");
            }

            InputFilePath = args[0];
        }
    }

}
