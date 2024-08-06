# Webrox.Base64Stream

Use the [repo on GitHub](https://github.com/Poppyto/Webrox.Base64Stream) to create issues and feature requests.

## Features

Implement a more readable way to Encode and Decode Stream in Base64, based on Microsoft System.Security.Cryptography.CryptoStream

### Sample - Encode Stream in Base64
```
using Webrox.Base64Stream;

using (var msEncoded = new MemoryStream())
{
    using (var base64EncoderStream = new Base64EncoderStream(msEncoded, leaveOpen: true))
    {
        streamInput.CopyTo(base64StreamEncode);
    }

    // if you dont want to Dispose base64EncoderStream, 
    // don't forget to call base64StreamEncode.FlushFinalBlock(); 
    // to write the remaining bytes after CopyTo

}
```
### Sample - Decode Base64 Stream in an other Stream 
```
using Webrox.Base64Stream;

using (var msDecoded = new MemoryStream())
{
    using (var base64DecoderStream = new Base64DecoderStream(msEncoded, leaveOpen: true))
    {
        streamInputBase64.CopyTo(base64DecoderStream);
    }
}
```