using System;
using System.Collections.Generic;

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