using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

public class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly MainCalculator calculator = new MainCalculator();

    private string currentExpression = string.Empty;
    private string currentResult = string.Empty;

    public string CurrentExpression
    {
        get => currentExpression;
        set
        {
            currentExpression = value;
            OnPropertyChanged(nameof(CurrentExpression));
        }
    }

    public string CurrentResult
    {
        get => currentResult;
        set
        {
            currentResult = value;
            OnPropertyChanged(nameof(CurrentResult));
        }
    }

    public ICommand AppendInputCommand { get; }
    public ICommand CalculateCommand { get; }
    public ICommand ClearAllCommand { get; }
    public ICommand ClearEntryCommand { get; }

    public CalculatorViewModel()
    {
        AppendInputCommand = new RelayCommand(AppendInput);
        CalculateCommand = new RelayCommand(_ => Calculate());
        ClearAllCommand = new RelayCommand(_ => ClearAll());
        ClearEntryCommand = new RelayCommand(_ => ClearEntry());
    }

    private void AppendInput(object? param)
    {
        if (param is not null)
        {
            CurrentExpression += param.ToString();
        }
    }

    private void Calculate()
    {
        CurrentResult = EvaluateExpressionSafely(CurrentExpression);
    }

    private void ClearAll()
    {
        CurrentExpression = string.Empty;
        CurrentResult = string.Empty;
    }

    private void ClearEntry()
    {
        if (!string.IsNullOrEmpty(CurrentExpression))
        {
            CurrentExpression = calculator.RemoveLastToken(CurrentExpression);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private string EvaluateExpressionSafely(string expression)
    {
        try
        {
            decimal result = calculator.EvaluateExpression(expression);
            return $"{result}";
        }
        catch
        {
            return "Error";
        }
    }
}