namespace CalculatorApp.Operations
{
    public class AddOperation : IOperation
    {
        public string Symbol => "+";
        public double Execute(double left, double right) => left + right;
    }
}