using System;
using CalculatorApp.Operations;

namespace CalculatorApp
{
    public class CalculatorEngine
    {
        public double CurrentValue { get; private set; } = 0;
        public double Memory { get; private set; } = 0;

        private double _storedValue = 0;
        private IOperation _currentOperation = null;
        private bool _isNewEntry = true;

        public string InputDigit(string digit)
        {
            if (_isNewEntry)
            {
                CurrentValue = digit == "." ? 0 : double.Parse(digit);
                _isNewEntry = false;
            }
            else
            {
                string current = CurrentValue.ToString();
                if (current == "0" && digit != ".")
                    CurrentValue = double.Parse(digit);
                else
                    CurrentValue = double.Parse(current + digit);
            }
            return CurrentValue.ToString();
        }

        public string SetOperation(string symbol)
        {
            if (_currentOperation != null && !_isNewEntry)
                Calculate();

            _storedValue = CurrentValue;
            _currentOperation = OperationFactory.Get(symbol);
            _isNewEntry = true;
            return CurrentValue.ToString();
        }

        public string Calculate()
        {
            if (_currentOperation == null)
                return CurrentValue.ToString();

            double result = _currentOperation.Execute(_storedValue, CurrentValue);
            CurrentValue = result;
            _currentOperation = null;
            _isNewEntry = true;
            return result.ToString();
        }

        public string Clear()
        {
            CurrentValue = 0;
            _storedValue = 0;
            _currentOperation = null;
            _isNewEntry = true;
            return "0";
        }

        public string Backspace()
        {
            string s = CurrentValue.ToString();
            if (s.Length > 1)
                CurrentValue = double.Parse(s.Substring(0, s.Length - 1));
            else
                CurrentValue = 0;
            return CurrentValue.ToString();
        }

        public string Negate()
        {
            CurrentValue = -CurrentValue;
            return CurrentValue.ToString();
        }

        public string SquareRoot()
        {
            if (CurrentValue < 0)
                throw new ArgumentException("Корень из отрицательного числа!");
            CurrentValue = Math.Sqrt(CurrentValue);
            _isNewEntry = true;
            return CurrentValue.ToString();
        }

        public string Square()
        {
            CurrentValue *= CurrentValue;
            _isNewEntry = true;
            return CurrentValue.ToString();
        }

        public string Inverse()
        {
            if (CurrentValue == 0)
                throw new DivideByZeroException("Деление на ноль!");
            CurrentValue = 1 / CurrentValue;
            _isNewEntry = true;
            return CurrentValue.ToString();
        }

        public void MemorySave() => Memory = CurrentValue;
        public void MemoryAdd() => Memory += CurrentValue;
        public void MemorySubtract() => Memory -= CurrentValue;
        public string MemoryRecall() { CurrentValue = Memory; _isNewEntry = true; return Memory.ToString(); }
        public void MemoryClear() => Memory = 0;
    }
}