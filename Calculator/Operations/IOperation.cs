namespace CalculatorApp.Operations
{
    public interface IOperation
    {
        string Symbol { get; }
        double Execute(double left, double right);
    }
}