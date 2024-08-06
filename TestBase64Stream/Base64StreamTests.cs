using Base64Stream;
using FluentAssertions;
using System.Reflection;
using System.Resources;

namespace TestBase64Stream
{
    public class Base64StreamTests
    {
        public Base64StreamTests() 
        {
        }

        Stream LoadResource(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(Base64StreamTests));
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{fileName}");
            return stream;
        }

        [Fact]
        public void TestEncodingBinaryFile()
        {
            // arrange
            var streamPdf = LoadResource("lorem-ipsum.pdf");
            var bytesPdf = new byte[streamPdf.Length];
            streamPdf.Read(bytesPdf, 0, (int)streamPdf.Length);
            streamPdf.Position = 0;
            var base64pdf = Convert.ToBase64String(bytesPdf);
            using var msEncoded = new MemoryStream();
            using var msDecoded = new MemoryStream();

            // act
            using var base64Stream = new Base64Stream.Base64Stream(msEncoded);
            streamPdf.CopyTo(base64Stream);
            base64Stream.Position = 0;
            base64Stream.CopyTo(msDecoded);

            // assert
            var bufferDecoded = msDecoded.ToArray();
            bufferDecoded.Should().Equal(bytesPdf);
        }
    }
}