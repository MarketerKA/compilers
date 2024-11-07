using System.Text;

namespace DLang.Execution
{

    internal class Array
    {
        public readonly SortedDictionary<Int128, Value> Values;
        private Int128 _maxIndex;
        private Int128 _minIndex;
        private bool _contiguous;

        public Array()
        {
            Values = [];
            _maxIndex = 0;
            _minIndex = 1;
            _contiguous = true;
        }

        public void Add(Value value)
        {
            _maxIndex++;
            Values.Add(_maxIndex, value);
        }

        public Value Get(Int128 index)
        {
            if (!Values.ContainsKey(index))
            {
                Values.Add(index, new Value());
                if (index > _maxIndex)
                {
                    if (index - 1 != _maxIndex)
                    {
                        _contiguous = false;
                    }

                    _maxIndex = index;
                }

                if (index < _minIndex)
                {
                    _contiguous = false;
                    _minIndex = index;
                }
            }

            return Values[(int)index];
        }

        public static Array operator +(Array a, Array b)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append('[');
            int i = 0;
            foreach (var pair in Values)
            {
                if (!_contiguous)
                    sb.Append($"{pair.Key}: {pair.Value}");
                else
                    sb.Append(pair.Value.ToString());

                if (i < Values.Count - 1) sb.Append(", ");
                i++;
            }
            sb.Append(']');
            return sb.ToString();
        }
    }

}
