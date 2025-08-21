using System;
using System.Collections.Generic;
using System.Linq;

// BaseCalculator (상속 구조)
public class BaseCalculator
{
    // 기본 사칙연산
    public virtual decimal Add(decimal a, decimal b) => a + b;
    public virtual decimal Subtract(decimal a, decimal b) => a - b;
    public virtual decimal Multiply(decimal a, decimal b) => a * b;
    public virtual decimal Divide(decimal a, decimal b)
    {
        if (b == 0) throw new DivideByZeroException();
        return a / b;
    }

    public virtual decimal Calculate(IReadOnlyList<string> postfix)
    {
        throw new NotImplementedException("자식 클래스에서 구현 필요");
    }
}


// AdvancedCalculator 
public class AdvancedCalculator : BaseCalculator
{
    public decimal Value { get; private set; }

    public AdvancedCalculator(decimal initialValue = 0)
    {
        Value = initialValue;
    }

    //BaseCalculator override
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


public class MainCalculator : AdvancedCalculator
{
    private readonly Dictionary<string, int> precedence = new()
    {
        { "+", 1 }, { "-", 1 }, { "*", 2 }, { "/", 2 }
    };

    public MainCalculator(decimal initialValue = 0) : base(initialValue) { }

    // 숫자/연산자 토큰 분리
    public List<string> Tokenize(string expr)
    {
        var tokens = new List<string>();
        string number = "";

        for (int i = 0; i < expr.Length; i++)
        {
            char c = expr[i];

            if (char.IsDigit(c) || c == '.')
            {
                number += c;
                continue;
            }
            if (!string.IsNullOrEmpty(number))
            {
                tokens.Add(number);
                number = "";
            }

            if (c == '-' && (i == 0 || expr[i - 1] == '('))
                number = "-";
            else if (!char.IsWhiteSpace(c))
                tokens.Add(c.ToString());
        }

        if (!string.IsNullOrEmpty(number)) tokens.Add(number);
        return tokens;
    }


    // 후위표기 변환
    public List<string> ConvertToPostfix(List<string> tokens)
    {
        var output = new List<string>();
        var stack = new Stack<string>();

        foreach (var token in tokens)
        {
            if (decimal.TryParse(token, out _))
            {
                output.Add(token);
                continue;
            }

            if (precedence.ContainsKey(token))
            {
                while (stack.Any() && precedence.ContainsKey(stack.Peek()) &&
                       precedence[token] <= precedence[stack.Peek()])
                    output.Add(stack.Pop());
                stack.Push(token);
            }
            else if (token == "(")
                stack.Push(token);
            else if (token == ")")
            {
                while (stack.Any() && stack.Peek() != "(") output.Add(stack.Pop());
                if (stack.Any()) stack.Pop();
            }
        }

        while (stack.Any()) output.Add(stack.Pop());
        return output;
    }

    // 문자열 식 계산
    public decimal EvaluateExpression(string expression)
    {
        var tokens = Tokenize(expression);
        var postfix = ConvertToPostfix(tokens);
        return Calculate(postfix); 
    }
}