using System;

namespace CalculatorApp.Operations
{
    public class DivideOperation : IOperation
    {
        public string Symbol => "/";

        public double Execute(double left, double right)
        {
            if (right == 0)
                throw new DivideByZeroException("Деление на ноль!");
            return left / right;
        }
    }
}