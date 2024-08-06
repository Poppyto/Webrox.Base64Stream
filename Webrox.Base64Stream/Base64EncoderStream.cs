using System.IO;
using System.Security.Cryptography;

namespace Webrox
{
    public class Base64EncoderStream : CryptoStream
    {
        public Base64EncoderStream(Stream stream, bool leaveOpen)
             : base(stream, new ToBase64Transform(), CryptoStreamMode.Write, leaveOpen)
        {
        }
    }
}