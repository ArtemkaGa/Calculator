using System.Collections.Generic;

namespace CalculatorApp
{
    public class HistoryManager
    {
        private readonly List<string> _history = new List<string>();

        public void Add(string entry)
        {
            _history.Insert(0, entry);
            if (_history.Count > 50)
                _history.RemoveAt(_history.Count - 1);
        }

        public List<string> GetAll() => new List<string>(_history);
        public void Clear() => _history.Clear();
    }
}