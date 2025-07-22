using SpanJson.Formatters;
using SpanJson.Structs;

using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanJson.Formatters;

/// <summary>
/// JSON이 "123.45"처럼 문자열로 넘어오거나
/// 123.45처럼 숫자 리터럴로 넘어와도
/// 모두 double로 역직렬화해 줍니다.
/// </summary>
public sealed class DoubleFlexibleFormatter<TResolver>
    : BaseFormatter
    , IJsonFormatter<double, byte>
    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
{
    public static readonly DoubleFlexibleFormatter<TResolver> Default
            = new DoubleFlexibleFormatter<TResolver>();

    public double Deserialize(ref JsonReader<byte> reader)
    {
        if (reader.ReadIsNull())
            return default;

        // 토큰 전체(raw value) 스팬을 가져옵니다.
        var raw = reader.ReadUtf8RawValueSpan();

        ReadOnlySpan<byte> span;
        if (raw.Length >= 2 && raw[0] == (byte)'"' && raw[^1] == (byte)'"')
        {
            // "123.45" → 안쪽 바이트 슬라이스
            span = raw.Slice(1, raw.Length - 2);
        }
        else
        {
            // 숫자 리터럴
            span = raw;
        }

        // Utf8Parser로 직접 파싱
        if (!Utf8Parser.TryParse(span, out double value, out _, 'G'))
        {
            throw new FormatException($"Invalid double value: {Encoding.UTF8.GetString(span)}");
        }
        return value;
    }

    public void Serialize(ref JsonWriter<byte> writer, double value)
    {
        // 숫자 리터럴로 출력합니다.
        writer.WriteDouble(value);
    }
}