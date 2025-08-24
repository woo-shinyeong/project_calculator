using System;
using System.Collections.Generic;

// AdvancedCalculator 
public class AdvancedCalculator : BaseCalculator
{
    public decimal Value { get; private set; }

    public AdvancedCalculator(decimal initialValue = 0)
    {
        Value = initialValue;
    }

    public override decimal Calculate(IReadOnlyList<string> postfix)
    {
        var stack = new Stack<decimal>();

        foreach (var token in postfix)
        {
            if (decimal.TryParse(token, out decimal number))
            {
                stack.Push(number);
                continue;
            }

            if (stack.Count < 2) throw new InvalidOperationException("잘못된 식입니다.");

            decimal b = stack.Pop();
            decimal a = stack.Pop();

            stack.Push(token switch
            {
                "+" => Add(a, b),
                "-" => Subtract(a, b),
                "*" => Multiply(a, b),
                "/" => Divide(a, b),
                _ => throw new InvalidOperationException($"지원하지 않는 연산자: {token}")
            });
        }

        if (stack.Count != 1) throw new InvalidOperationException("잘못된 식입니다.");

        Value = stack.Pop();
        return Value;
    }

    // Operator overloading
    public static AdvancedCalculator operator +(AdvancedCalculator a, AdvancedCalculator b)
        => new AdvancedCalculator(a.Value + b.Value);
    public static AdvancedCalculator operator -(AdvancedCalculator a, AdvancedCalculator b)
        => new AdvancedCalculator(a.Value - b.Value);
    public static AdvancedCalculator operator *(AdvancedCalculator a, AdvancedCalculator b)
        => new AdvancedCalculator(a.Value * b.Value);
    public static AdvancedCalculator operator /(AdvancedCalculator a, AdvancedCalculator b)
        => new AdvancedCalculator(a.Divide(a.Value, b.Value));
}