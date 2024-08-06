using FluentAssertions;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using Webrox;

namespace TestBase64Stream
{
    public class Base64StreamTests
    {
        Stream LoadResource(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(Base64StreamTests));
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Assets.{fileName}");
            return stream;
        }

        [InlineData("lorem-ipsum.pdf")]
        [InlineData("1_5MB.pdf")]
        [InlineData("dataText.txt")]        
        [Theory]
        public void GivenFile_EncodeDecodeBase64_OutputMatchesInput(string fileNameInline)
        {
            // Arrange
            var streamPdf = LoadResource(fileNameInline);
            
            var bytesPdf = new byte[streamPdf.Length];
            streamPdf.Read(bytesPdf, 0, (int)streamPdf.Length);
            streamPdf.Position = 0;
            var base64pdf = Convert.ToBase64String(bytesPdf);
            using var msEncoded = new MemoryStream();
            using var msDecoded = new MemoryStream();

            // Act
            using (var base64StreamEncode = new Base64EncoderStream(msEncoded, true))
            {
                streamPdf.CopyTo(base64StreamEncode);
            }

            // if you dont want to Dispose base64EncoderStream, 
            // don't forget to call base64StreamEncode.FlushFinalBlock(); 
            // to write the remaining bytes after CopyTo


            msEncoded.Position = 0;

            using (var base64StreamDecode = new Base64DecoderStream(msEncoded, true))
            {
                base64StreamDecode.CopyTo(msDecoded);
            }

            // Assert
            var decodedArray = msDecoded.ToArray();
            var bufferDecoded = decodedArray;
            bufferDecoded.Length.Should().Be(bytesPdf.Length);
            bufferDecoded.Should().Equal(bytesPdf);
        }
    }
}