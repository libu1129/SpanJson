using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dubu;

using SpanJson.Helpers;
using SpanJson.Resolvers;

namespace SpanJson.Formatters;

/// <summary>
/// ArraySegment/<T/> 전용 포매터
/// </summary>
public sealed class ArraySegmentFormatter<T, TSymbol, TResolver>
    : BaseFormatter
    , IJsonFormatter<ArraySegment<T>, TSymbol>
    where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
    where TSymbol : struct
{
    public static readonly ArraySegmentFormatter<T, TSymbol, TResolver> Default
            = new ArraySegmentFormatter<T, TSymbol, TResolver>();

    // 요소 포매터
    private static readonly IJsonFormatter<T, TSymbol> ElementFormatter =
            StandardResolvers.GetResolver<TSymbol, TResolver>().GetFormatter<T>();

    // 순환 참조 체크
    private static readonly bool IsRecursionCandidate = RecursionCandidate<T>.IsRecursionCandidate;

    public ArraySegment<T> Deserialize(ref JsonReader<TSymbol> reader)
    {
        if (reader.ReadIsNull())
            return default;

        // [ 시작
        reader.ReadBeginArrayOrThrow();

        // 임시 버퍼(풀에서 빌린 뒤 ArraySegment에 바로 쓴다)
        T[] poolBuffer = ArrayPool<T>.Shared.Rent(4);
        int count = 0;

        // 요소 읽기
        while (!reader.TryReadIsEndArrayOrValueSeparator(ref count))
        {
            if (count == poolBuffer.Length)
                FormatterUtils.GrowArray(ref poolBuffer);

            poolBuffer[count - 1] = ElementFormatter.Deserialize(ref reader);
        }

        // ArraySegment로 래핑(반환 시점에 반드시 ArrayPool<T>.Shared.Return 해야 함)
        return new ArraySegment<T>(poolBuffer, 0, count);
    }

    public void Serialize(ref JsonWriter<TSymbol> writer, ArraySegment<T> value)
    {
        if (value.Array == null)
        {
            writer.WriteNull();
            return;
        }

        if (IsRecursionCandidate)
            writer.IncrementDepth();

        writer.WriteBeginArray();
        int len = value.Count;
        if (len > 0)
        {
            // 첫 요소
            SerializeRuntimeDecisionInternal<T, TSymbol, TResolver>(
                ref writer,
                value.Array[value.Offset],
                ElementFormatter);

            // 나머지 요소
            for (int i = 1; i < len; i++)
            {
                writer.WriteValueSeparator();
                SerializeRuntimeDecisionInternal<T, TSymbol, TResolver>(
                    ref writer,
                    value.Array[value.Offset + i],
                    ElementFormatter);
            }
        }

        if (IsRecursionCandidate)
            writer.DecrementDepth();

        writer.WriteEndArray();
    }
}