using System;
using System.ComponentModel;
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
        AppendInputCommand = new RelayCommand(param =>
        {
            if (param is not null)
            {
                CurrentExpression += param.ToString();
            }
        });

        CalculateCommand = new RelayCommand(_ =>
        {
            CurrentResult = EvaluateExpressionSafely(CurrentExpression);
        });

        ClearAllCommand = new RelayCommand(_ =>
        {
            CurrentExpression = string.Empty;
            CurrentResult = string.Empty;
        });

        ClearEntryCommand = new RelayCommand(_ =>
        {
            if (!string.IsNullOrEmpty(CurrentExpression))
            {
                var tokens = calculator.Tokenize(CurrentExpression);
                if (tokens.Any())
                {
                    tokens.RemoveAt(tokens.Count - 1); // 마지막 토큰 제거
                    CurrentExpression = string.Join("", tokens);
                }
            }
        });
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