using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanJson.Structs;

/// <summary>
/// 내부적으로 string 값을 감싸고, string과 동일한 API를 제공하는 구조체입니다.
/// </summary>
/// <summary>
/// 내부적으로 string 값을 감싸며,
/// string으로 암시적 변환만 제공하는 구조체입니다.
/// </summary>
public readonly struct PooledString : IEquatable<PooledString>
{
    private readonly string _value;

    public PooledString(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    // 암시적 변환: string → CachedString
    public static implicit operator PooledString(string s)
        => s is null ? throw new ArgumentNullException(nameof(s)) : new PooledString(s);

    // 암시적 변환: CachedString → string
    public static implicit operator string(PooledString cs)
        => cs._value;

    public override string ToString()
        => _value;

    public override bool Equals(object obj)
        => obj is PooledString other && Equals(other);

    public bool Equals(PooledString other)
        => string.Equals(_value, other._value, StringComparison.Ordinal);

    public override int GetHashCode()
        => _value.GetHashCode();
}
