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
            if (index < 1 || index > Values.Count)
            {
                throw new IndexOutOfRangeException($"index {index} is out of tuple range");
            }

            return Values[(int)index - 1];
        }

        public Value Get(string name)
        {
            if (!NamedIndices.TryGetValue(name, out int value))
            {
                throw new IndexOutOfRangeException($"key \"{name}\" does not exist in tuple");
            }

            return Values[value];
        }

        public static Tuple operator+(Tuple left, Tuple right)
        {
            Tuple result = new();

            foreach (var item in left.NamedIndices)
            {
                if (right.NamedIndices.ContainsKey(item.Key))
                {
                    throw new ValueError($"element name collision in tuple addition (\"{item.Key}\")");
                }
            }

            for (int i = 0; i < left.Values.Count; i++)
            {
                Value val = new();
                result.Values.Add(val);
                
                if (left.IndicesNamed.TryGetValue(i, out string? value))
                {
                    result.IndicesNamed.Add(i, value);
                    result.NamedIndices.Add(value, i);
                }

                val.Assign(left.Values[i]);
            }

            int end = result.Values.Count;

            for (int i = 0; i < right.Values.Count; i++)
            {
                int index = i + end;
                Value val = new();
                result.Values.Add(val);

                if (right.IndicesNamed.TryGetValue(i, out string? value))
                {
                    result.IndicesNamed.Add(index, value);
                    result.NamedIndices.Add(value, index);
                }

                val.Assign(right.Values[i]);
            }

            return result;
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
