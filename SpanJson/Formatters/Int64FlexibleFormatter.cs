using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanJson.Formatters;


/// <summary>
/// JSON 숫자가 문자열로 묶여 있어도 (e.g. "1753159463402") 파싱해
/// long으로 반환합니다. 숫자 리터럴도 그대로 처리합니다.
/// </summary>
public sealed class Int64FlexibleFormatter<TResolver>
    : BaseFormatter
    , IJsonFormatter<long, byte>
    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
{
    public static readonly Int64FlexibleFormatter<TResolver> Default
            = new Int64FlexibleFormatter<TResolver>();

    public long Deserialize(ref JsonReader<byte> reader)
    {
        if (reader.ReadIsNull())
            return default;

        // rawValue에 숫자 또는 "숫자" 전체(따옴표 포함) 스팬을 가져온다
        var raw = reader.ReadUtf8RawValueSpan();

        ReadOnlySpan<byte> span;
        if (raw.Length >= 2 && raw[0] == (byte)'"')
        {
            // "12345" → 12345 부분만 슬라이스
            span = raw.Slice(1, raw.Length - 2);
        }
        else
        {
            // 숫자 리터럴 그대로
            span = raw;
        }

        // Utf8Parser로 직접 파싱
        if (!Utf8Parser.TryParse(span, out long value, out _, 'G'))
        {
            // 실패 시 예외
            throw new FormatException($"Invalid Int64 value: {Encoding.UTF8.GetString(span)}");
        }

        return value;
    }

    public void Serialize(ref JsonWriter<byte> writer, long value)
    {
        // 숫자 리터럴로 출력
        writer.WriteInt64(value);
    }
}