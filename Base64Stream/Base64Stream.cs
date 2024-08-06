using System;
using System.Security;

namespace Base64Stream
{
    public class Base64Stream : Stream
    {
        // System.Convert
        internal static readonly char[] base64Table = new char[65]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '+', '/', '='
        };

        private readonly Stream _source;

        public override bool CanRead => _source.CanRead;

        public override bool CanSeek => _source.CanSeek;

        public override bool CanWrite => _source.CanWrite;

        public override long Length => _source.Length;

        public override long Position { get => _source.Position; set => _source.Position = value; }

        public Base64Stream(Stream source)
        {
            ArgumentNullException.ThrowIfNull(source);
            _source = source;
        }

        public override void Flush()
        {
            _source.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _source.Read(buffer,offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _source.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _source.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _source.Write(buffer, offset, count);
        }

        [SecurityCritical]
        private unsafe static int ConvertToBase64Array(char* outChars, byte* inData, int offset, int length, bool insertLineBreaks)
        {
            int lengthmod3 = length % 3;
            int calcLength = offset + (length - lengthmod3);
            int j = 0;
            int charcount = 0;
            fixed (char* base64 = base64Table)
            {
                int i;
                for (i = offset; i < calcLength; i += 3)
                {
                    if (insertLineBreaks)
                    {
                        if (charcount == 76)
                        {
                            outChars[j++] = '\r';
                            outChars[j++] = '\n';
                            charcount = 0;
                        }
                        charcount += 4;
                    }
                    outChars[j] = base64[(inData[i] & 0xFC) >> 2];
                    outChars[j + 1] = base64[((inData[i] & 3) << 4) | ((inData[i + 1] & 0xF0) >> 4)];
                    outChars[j + 2] = base64[((inData[i + 1] & 0xF) << 2) | ((inData[i + 2] & 0xC0) >> 6)];
                    outChars[j + 3] = base64[inData[i + 2] & 0x3F];
                    j += 4;
                }
                i = calcLength;
                if (insertLineBreaks && lengthmod3 != 0 && charcount == 76)
                {
                    outChars[j++] = '\r';
                    outChars[j++] = '\n';
                }
                switch (lengthmod3)
                {
                    case 2:
                        outChars[j] = base64[(inData[i] & 0xFC) >> 2];
                        outChars[j + 1] = base64[((inData[i] & 3) << 4) | ((inData[i + 1] & 0xF0) >> 4)];
                        outChars[j + 2] = base64[(inData[i + 1] & 0xF) << 2];
                        outChars[j + 3] = base64[64];
                        j += 4;
                        break;
                    case 1:
                        outChars[j] = base64[(inData[i] & 0xFC) >> 2];
                        outChars[j + 1] = base64[(inData[i] & 3) << 4];
                        outChars[j + 2] = base64[64];
                        outChars[j + 3] = base64[64];
                        j += 4;
                        break;
                }
            }
            return j;
        }

    }
}
