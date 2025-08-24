using System;
using System.Collections.Generic;
using System.Linq;

// MainCalculator
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
        var number = "";

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
            {
                number = "-";
            }

            else if (!char.IsWhiteSpace(c))
            {
                tokens.Add(c.ToString());
            }
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

    public string RemoveLastToken(string expression)
    {
        var tokens = Tokenize(expression);
        if (tokens.Any())
        {
            tokens.RemoveAt(tokens.Count - 1);
            return string.Join("", tokens);
        }
        return expression;
    }
}