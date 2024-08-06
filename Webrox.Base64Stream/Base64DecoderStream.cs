using System.IO;
using System.Security.Cryptography;

namespace Webrox
{
    public class Base64DecoderStream : CryptoStream
    {
        public Base64DecoderStream(Stream stream, bool leaveOpen)
             : base(stream, new FromBase64Transform(), CryptoStreamMode.Read, leaveOpen)
        {            
        }
    }
}
