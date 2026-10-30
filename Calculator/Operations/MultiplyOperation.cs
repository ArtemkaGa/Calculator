namespace CalculatorApp.Operations
{
    public class MultiplyOperation : IOperation
    {
        public string Symbol => "*";
        public double Execute(double left, double right) => left * right;
    }
}