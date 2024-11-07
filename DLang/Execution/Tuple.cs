using System.Text;

namespace DLang.Execution
{

    internal class Tuple
    {
        public readonly List<Value> Values;
        public readonly Dictionary<string, int> NamedIndices;
        public readonly Dictionary<int, string> IndicesNamed;

        public Tuple()
        {
            Values = [];
            NamedIndices = [];
            IndicesNamed = [];
        }

        public void Add(string identifier, Value value)
        {
            NamedIndices.Add(identifier, Values.Count);
            IndicesNamed.Add(Values.Count, identifier);
            Values.Add(value);
        }

        public void Add(Value value)
        {
            Values.Add(value);
        }

        public Value Get(Int128 index)
        {
            if (index < 0 || index >= Values.Count)
            {
                throw new ArgumentOutOfRangeException($"index {index} is out of tuple range");
            }

            return Values[(int)index];
        }

        public Value Get(string name)
        {
            if (!NamedIndices.TryGetValue(name, out int value))
            {
                throw new ArgumentOutOfRangeException($"key \"{name}\" does not exist in tuple");
            }

            return Values[value];
        }

        public static Tuple operator+(Tuple left, Tuple right)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('{');
            for (int i = 0; i < Values.Count; i++)
            {
                if (IndicesNamed.TryGetValue(i, out string? value))
                {
                    sb.Append(value);
                    sb.Append(" = ");
                }
                sb.Append(Values[i].ToString());

                if (i < Values.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append('}');

            return sb.ToString();
        }
    }

}
