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
/// JSON 문자열로 전달된 숫자(예: "123.45")를
/// StringDouble로 변환하고,
/// StringDouble을 JSON 문자열로 출력합니다.
/// </summary>
public sealed class StringDoubleFormatter<TResolver>
    : BaseFormatter
    , IJsonFormatter<StringDouble, byte>
    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
{
    public static readonly StringDoubleFormatter<TResolver> Default
            = new StringDoubleFormatter<TResolver>();

    public StringDouble Deserialize(ref JsonReader<byte> reader)
    {
        if (reader.ReadIsNull())
            return default;

        // "..." 내부의 UTF‑8 바이트 스팬을 allocation‑free로 얻어옵니다.
        var utf8Span = reader.ReadUtf8StringSpan();

        // System.Buffers.Text.Utf8Parser로 직접 double 파싱
        if (!Utf8Parser.TryParse(utf8Span, out double value, out _, 'G'))
        {
            // 필요시 예외를 던지거나 기본값 반환
            throw new FormatException($"Invalid double value: {Encoding.UTF8.GetString(utf8Span)}");
        }

        return new StringDouble(value);
    }

    public void Serialize(ref JsonWriter<byte> writer, StringDouble value)
    {
        // null-ish 기본값이 아니라면
        // 내부 double 값을 ToString("G", Invariant)으로 문자열화 후 인용부호 출력
        writer.WriteString(value.ToString());
    }
}