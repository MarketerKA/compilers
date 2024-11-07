using System.Text;

namespace DLang.Execution
{

    internal class Array
    {
        public readonly SortedDictionary<Int128, Value> Values;
        private Int128 _maxIndex;

        public Array()
        {
            Values = [];
            _maxIndex = 0;
        }

        public void Add(Value value)
        {
            _maxIndex!++;
            Values.Add(_maxIndex, value);
        }

        public Value Get(Int128 index)
        {
            if (index <= 0)
            {
                throw new IndexOutOfRangeException($"attempted to index an array with index that is less than 1 ({index})");
            }

            if (!Values.ContainsKey(index))
            {
                Values.Add(index, new Value());
                if (index > _maxIndex)
                {
                    _maxIndex = index;
                }
            }

            return Values[(int)index];
        }

        public static Array operator +(Array a, Array b)
        {
            Array result = new();
            foreach (var item in a.Values)
            {
                Value val = new();
                result.Values.Add(item.Key, val);
                val.Assign(item.Value);

                if (item.Key > result._maxIndex)
                {
                    result._maxIndex = item.Key;
                }
            }
            foreach (var item in b.Values)
            {
                Int128 index = item.Key + a._maxIndex;
                Value val = new();
                result.Values.Add(index, val);
                val.Assign(item.Value);

                if (index > result._maxIndex)
                {
                    result._maxIndex = index;
                }
            }

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('[');
            int i = 0;
            foreach (var pair in Values)
            {
                sb.Append($"[{pair.Key}] = {pair.Value}");

                if (i < Values.Count - 1) sb.Append(", ");
                i++;
            }
            sb.Append(']');
            return sb.ToString();
        }
    }

}
