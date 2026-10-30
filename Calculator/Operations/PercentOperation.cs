namespace CalculatorApp.Operations
{
    public class PercentOperation : IOperation
    {
        public string Symbol => "%";
        public double Execute(double left, double right) => left * right / 100;
    }
}