namespace CalculatorApp.Operations
{
    public class SubtractOperation : IOperation
    {
        public string Symbol => "-";
        public double Execute(double left, double right) => left - right;
    }
}