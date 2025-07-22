using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanJson.Structs;

/// <summary>
/// "string"으로부터 파싱된 값을 내부적으로 double로 저장하고,
/// double처럼 사용 가능한 구조체입니다.
/// </summary>
public readonly struct StringDouble : IEquatable<StringDouble>, IComparable<StringDouble>, IFormattable
{
    private readonly double _value;

    /// <summary>
    /// 문자열에서 파싱하려면 생성자를 직접 호출하세요.
    /// </summary>
    public StringDouble(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        _value = double.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// double 값으로 초기화할 때 사용합니다.
    /// </summary>
    public StringDouble(double value)
    {
        _value = value;
    }

    /// <summary>
    /// 내부 double 값을 반환합니다.
    /// </summary>
    public double Value => _value;

    // 암시적 변환: double → StringDouble
    public static implicit operator StringDouble(double d)
        => new StringDouble(d);

    // 암시적 변환: StringDouble → double
    public static implicit operator double(StringDouble sd)
        => sd._value;

    /// <summary>
    /// ToString은 double과 동일하게 InvariantCulture를 사용합니다.
    /// </summary>
    public override string ToString()
        => _value.ToString("G", CultureInfo.InvariantCulture);

    public string ToString(string format, IFormatProvider formatProvider)
        => _value.ToString(format, formatProvider);

    // 동등성 비교
    public bool Equals(StringDouble other)
        => _value.Equals(other._value);

    public override bool Equals(object obj)
        => obj is StringDouble sd && Equals(sd);

    public override int GetHashCode()
        => _value.GetHashCode();

    // 대소 비교
    public int CompareTo(StringDouble other)
        => _value.CompareTo(other._value);
}
