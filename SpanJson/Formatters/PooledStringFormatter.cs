using CommunityToolkit.HighPerformance.Buffers;

using SpanJson.Structs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanJson.Formatters;


/// <summary>
/// UTF‑8(byte) 전용 PooledString 포매터.
/// IJsonFormatter&lt;PooledString, byte&gt; 인터페이스 구현에 맞춰 메서드 시그니처를 조정했습니다.
/// </summary>
public sealed class PooledStringFormatter<TResolver>
    : BaseFormatter
    , IJsonFormatter<PooledString, byte>
    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
{
    // 싱글턴 인스턴스
    public static readonly PooledStringFormatter<TResolver> Default
            = new PooledStringFormatter<TResolver>();

    // CommunityToolkit.StringPool
    private static readonly StringPool s_pool = new StringPool();

    // <-- IJsonFormatter<PooledString, byte> 에서 요구하는 메서드 시그니처 -->
    public PooledString Deserialize(ref JsonReader<byte> reader)
    {
        if (reader.ReadIsNull())
            return default;

        string pooled = s_pool.GetOrAdd(reader.ReadUtf8StringSpan(), encoding: Encoding.UTF8);
        return new PooledString(pooled);
    }

    public void Serialize(ref JsonWriter<byte> writer, PooledString value)
    {
        if (value.Equals(default(PooledString)))
        {
            writer.WriteNull();
        }
        else
        {
            // 암시적 변환으로 내부 string 꺼내서 기록
            writer.WriteString((string)value);
        }
    }
}

///// <summary>
///// PooledString 전용 JSON (de)serializer
///// </summary>
//public sealed class PooledStringFormatter<TSymbol, TResolver>
//    : BaseFormatter
//    , IJsonFormatter<PooledString, TSymbol>
//    where TSymbol : struct
//    where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
//{
//    // 싱글턴 인스턴스
//    public static readonly PooledStringFormatter<TSymbol, TResolver> Default
//            = new PooledStringFormatter<TSymbol, TResolver>();

//    // StringPool 인스턴스 (풀링)
//    private static readonly StringPool s_pool = new StringPool();

//    public PooledString Deserialize(ref JsonReader<TSymbol> reader)
//    {
//        if (reader.ReadIsNull())
//            return default;

//        // JSON 문자열 읽기
//        string raw = reader.ReadString();
//        // StringPool으로 풀링
//        string pooled = s_pool.GetOrAdd(reader.ReadStringSpan());
//        return new PooledString(pooled);
//    }

//    public void Serialize(ref JsonWriter<TSymbol> writer, PooledString value)
//    {
//        // 기본값 (null) 처리
//        if (value.Equals(default(PooledString)))
//        {
//            writer.WriteNull();
//        }
//        else
//        {
//            // 암시적 변환으로 string 얻어 기록
//            writer.WriteString((string)value);
//        }
//    }
//}