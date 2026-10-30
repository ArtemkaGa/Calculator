using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp.Operations
{
    public static class OperationFactory
    {
        private static readonly List<IOperation> _operations = new List<IOperation>
        {
            new AddOperation(),
            new SubtractOperation(),
            new MultiplyOperation(),
            new DivideOperation(),
            new PercentOperation()
        };

        public static IOperation Get(string symbol)
        {
            var op = _operations.FirstOrDefault(o => o.Symbol == symbol);
            if (op == null)
                throw new ArgumentException($"Неизвестная операция: {symbol}");
            return op;
        }

        public static bool IsOperation(string symbol) =>
            _operations.Any(o => o.Symbol == symbol);
    }
}