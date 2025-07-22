using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanJson
{
    public ref partial struct JsonReader<TSymbol> where TSymbol : struct
    {
        public readonly ReadOnlySpan<char> _chars;
        public readonly ReadOnlySpan<byte> _bytes;
        public readonly int _length;

        private int _pos;
        public int pos => _pos;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonReader(in ReadOnlySpan<TSymbol> input)
        {
            _length = input.Length;
            _pos = 0;

            if (typeof(TSymbol) == typeof(char))
            {
                _chars = MemoryMarshal.Cast<TSymbol, char>(input);
                _bytes = null;
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                _bytes = MemoryMarshal.Cast<TSymbol, byte>(input);
                _chars = null;
            }
            else
            {
                ThrowNotSupportedException();
                _chars = default;
                _bytes = default;
            }
        }

        public int Position => _pos;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowJsonParserException(JsonParserException.ParserError error, JsonParserException.ValueType type)
        {
            throw new JsonParserException(error, type, _pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowJsonParserException(JsonParserException.ParserError error)
        {
            throw new JsonParserException(error, _pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowJsonParserException(JsonParserException.ParserError error, int pos)
        {
            throw new JsonParserException(error, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowJsonParserException(JsonParserException.ParserError error, JsonParserException.ValueType type, int pos)
        {
            throw new JsonParserException(error, type, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBeginArrayOrThrow()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                ReadUtf16BeginArrayOrThrow();
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                ReadUtf8BeginArrayOrThrow();
            }
            else
            {
                ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowNotSupportedException()
        {
            throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadIsEndArrayOrValueSeparator(ref int count)
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return TryReadUtf16IsEndArrayOrValueSeparator(ref count);
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return TryReadUtf8IsEndArrayOrValueSeparator(ref count);
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ReadDynamic()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return ReadUtf16Dynamic();
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return ReadUtf8Dynamic();
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadIsNull()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return ReadUtf16IsNull();
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return ReadUtf8IsNull();
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadEscapedName()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return ReadUtf16EscapedName();
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return ReadUtf8EscapedName();
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadEscapedNameSpan()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16EscapedNameSpan());
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8EscapedNameSpan());
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadVerbatimNameSpan()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                SkipWhitespaceUtf16();
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16VerbatimNameSpan());
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                SkipWhitespaceUtf8();
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8VerbatimNameSpan());
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadIsEndObjectOrValueSeparator(ref int count)
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return TryReadUtf16IsEndObjectOrValueSeparator(ref count);
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return TryReadUtf8IsEndObjectOrValueSeparator(ref count);
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBeginObjectOrThrow()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                ReadUtf16BeginObjectOrThrow();
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                ReadUtf8BeginObjectOrThrow();
            }
            else
            {
                ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadEndObjectOrThrow()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                ReadUtf16EndObjectOrThrow();
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                ReadUtf8EndObjectOrThrow();
            }
            else
            {
                ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadEndArrayOrThrow()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                ReadUtf16EndArrayOrThrow();
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                ReadUtf8EndArrayOrThrow();
            }
            else
            {
                ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadStringSpan()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16StringSpan());
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8StringSpan());
            }

            ThrowNotSupportedException();
            return default;
        }

        /// <summary>
        /// Doesn't skip whitespace, just for copying around in a token loop
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadVerbatimStringSpan()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16StringSpanInternal(out _));
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8StringSpanInternal(out _));
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipNextSegment()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                SkipNextUtf16Segment();
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                SkipNextUtf8Segment();
            }
            else
            {
                ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonToken ReadNextToken()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return ReadUtf16NextToken();
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return ReadUtf8NextToken();
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadNumberSpan()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16NumberInternal());
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8NumberInternal());
            }

            ThrowNotSupportedException();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSymbolOrThrow(TSymbol symbol)
        {
            if (typeof(TSymbol) == typeof(char))
            {
                ReadUtf16SymbolOrThrow(Unsafe.As<TSymbol, char>(ref symbol));
            }
            else if (typeof(TSymbol) == typeof(byte))
            {
                ReadUtf8SymbolOrThrow(Unsafe.As<TSymbol, byte>(ref symbol));
            }
            else
            {
                ThrowNotSupportedException();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadBase64EncodedArray()
        {
            if (typeof(TSymbol) == typeof(char))
            {
                return ReadUtf16Base64EncodedArray();
            }

            if (typeof(TSymbol) == typeof(byte))
            {
                return ReadUtf8Base64EncodedArray();
            }

            ThrowNotSupportedException();
            return default;
        }


        /// <summary>
        /// 현재 위치의 공백을 건너뛰고, 다음 한 개의 JSON 값(문자열,숫자,객체,배열,리터럴)을 원시 바이트 시퀀스로 읽어와 ReadOnlySpan/<byte/>로 반환합니다. 내부 _pos는 값의 바로 다음 위치로 이동합니다.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8RawValueSpan()
        {
            // (1) 공백 있을 수도 없을 수도 있으니 무조건 스킵
            SkipWhitespaceUtf8();

            // (2) 토큰 시작 인덱스 기록
            int start = _pos;
            byte marker = _bytes[_pos++];

            // (3) 토큰별로 읽기
            switch (marker)
            {
                // --- 문자열: 따옴표 안에서 이스케이프 건너뛰며 마지막 따옴표까지 ---
                case (byte)'"':
                    while (_pos < _length)
                    {
                        byte b = _bytes[_pos++];
                        if (b == (byte)'"') break;
                        if (b == (byte)'\\' && _pos < _length) _pos++;
                    }
                    break;

                // --- 객체: 중첩된 { } 추적, 내부 문자열도 처리 ---
                case (byte)'{':
                    int objDepth = 1;
                    while (objDepth > 0 && _pos < _length)
                    {
                        byte b = _bytes[_pos++];
                        if (b == (byte)'"')
                        {
                            // 문자열 내부 건너뛰기
                            while (_pos < _length)
                            {
                                byte bb = _bytes[_pos++];
                                if (bb == (byte)'"') break;
                                if (bb == (byte)'\\' && _pos < _length) _pos++;
                            }
                        }
                        else if (b == (byte)'{') objDepth++;
                        else if (b == (byte)'}') objDepth--;
                    }
                    break;

                // --- 배열: 중첩된 [ ] 추적, 내부 문자열도 처리 ---
                case (byte)'[':
                    int arrDepth = 1;
                    while (arrDepth > 0 && _pos < _length)
                    {
                        byte b = _bytes[_pos++];
                        if (b == (byte)'"')
                        {
                            while (_pos < _length)
                            {
                                byte bb = _bytes[_pos++];
                                if (bb == (byte)'"') break;
                                if (bb == (byte)'\\' && _pos < _length) _pos++;
                            }
                        }
                        else if (b == (byte)'[') arrDepth++;
                        else if (b == (byte)']') arrDepth--;
                    }
                    break;

                // --- 숫자: 부호·소수·지수 표기까지 모두 읽기 ---
                default:
                    if (marker == (byte)'-' || (marker >= (byte)'0' && marker <= (byte)'9'))
                    {
                        while (_pos < _length)
                        {
                            byte c = _bytes[_pos];
                            if ((c >= (byte)'0' && c <= (byte)'9')
                                || c == (byte)'+'
                                || c == (byte)'-'
                                || c == (byte)'.'
                                || c == (byte)'e'
                                || c == (byte)'E')
                            {
                                _pos++;
                                continue;
                            }
                            break;
                        }
                    }
                    // --- true, false, null 리터럴 ---
                    else if (marker == (byte)'t'
                             && _pos + 2 < _length
                             && _bytes[_pos] == (byte)'r'
                             && _bytes[_pos + 1] == (byte)'u'
                             && _bytes[_pos + 2] == (byte)'e')
                    {
                        _pos += 3; // 'r','u','e'
                    }
                    else if (marker == (byte)'f'
                             && _pos + 3 < _length
                             && _bytes[_pos] == (byte)'a'
                             && _bytes[_pos + 1] == (byte)'l'
                             && _bytes[_pos + 2] == (byte)'s'
                             && _bytes[_pos + 3] == (byte)'e')
                    {
                        _pos += 4; // 'a','l','s','e'
                    }
                    else if (marker == (byte)'n'
                             && _pos + 2 < _length
                             && _bytes[_pos] == (byte)'u'
                             && _bytes[_pos + 1] == (byte)'l'
                             && _bytes[_pos + 2] == (byte)'l')
                    {
                        _pos += 3; // 'u','l','l'
                    }
                    else
                    {
                        // 예기치 않은 토큰
                        ThrowJsonParserException(JsonParserException.ParserError.InvalidEncoding);
                    }
                    break;
            }

            // (4) 슬라이스 반환
            int len = _pos - start;
            return _bytes.Slice(start, len);
        }

    }
}