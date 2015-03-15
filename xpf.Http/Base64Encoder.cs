using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xpf.Http
{
    public class Base64Encoder : IEncodeData
    {
        public string ContentEncoding
        {
            get { return "base64"; }
        }

        public async Task<Stream> Encode(string data)
        {
            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(new MemoryStream()))
            {
                await streamWriter.WriteAsync(data);
            }

            return stream;
        }

        public async Task<string> Decode(Stream data)
        {
            return await new StreamReader(data).ReadToEndAsync();
        }
    }
}


    //public class Base64StreamReader : Stream
    //{
    //    private static XmlReaderSettings InitXmlReaderSettings()
    //    {
    //        XmlReaderSettings settings = new XmlReaderSettings();
    //        settings.ConformanceLevel = ConformanceLevel.Auto;
    //        settings.CloseInput = false;
    //        return settings;
    //    }

    //    private Stream TheStream;
    //    private XmlReader xw;

    //    public Base64StreamReader(Stream stream)
    //    {
    //        this.TheStream = stream;
    //    }

    //    public override bool CanRead
    //    {
    //        get { return this.TheStream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return this.TheStream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return this.TheStream.CanWrite; }
    //    }

    //    public override void Flush()
    //    {
    //    }

    //    public void Close()
    //    {
    //        this.xw.Dispose();
    //    }

    //    public override long Length
    //    {
    //        get
    //        {
    //            return TheStream.Length;
    //        }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return TheStream.Position;
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    bool movedToContent = false;
    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        if (!movedToContent)
    //        {
    //            xw = XmlReader.Create(TheStream, InitXmlReaderSettings());
    //            xw.MoveToContent();

    //            movedToContent = true;
    //        }

    //        /* use 
    //         * int readed = xw.ReadElementContentAsBase64( buffer, offset, count );
    //         * for Base64 encoding
    //         */
    //        int readed = xw.ReadElementContentAsBinHex(buffer, offset, count);

    //        return readed;
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetLength(long value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //    }
    //}

    //public class Base64StreamWriter : Stream
    //{
    //    private static XmlWriterSettings InitXmlWriterSettings()
    //    {
    //        XmlWriterSettings settings = new XmlWriterSettings();
    //        settings.ConformanceLevel = ConformanceLevel.Auto;
    //        settings.Encoding = Encoding.ASCII;
    //        settings.OmitXmlDeclaration = true;
    //        settings.CloseOutput = true;
    //        return settings;
    //    }

    //    private Stream TheStream;
    //    private XmlWriter xw;

    //    public Base64StreamWriter(Stream Stream)
    //    {
    //        this.TheStream = Stream;

    //        xw = XmlWriter.Create(Stream, InitXmlWriterSettings());
    //        xw.WriteStartElement("data");
    //    }

    //    public override bool CanRead
    //    {
    //        get { return TheStream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return TheStream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return TheStream.CanWrite; }
    //    }

    //    public void Close()
    //    {
    //        if (xw.WriteState != WriteState.Closed)
    //        {
    //            xw.WriteEndElement();
    //            xw.Dispose();
    //        }
    //        base.Dispose();
    //    }

    //    public override void Flush()
    //    {
    //        xw.Flush();
    //    }

    //    public override long Length
    //    {
    //        get
    //        {
    //            return TheStream.Length;
    //        }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return TheStream.Position;
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetLength(long value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        /* use 
    //         * xw.WriteBase64( buffer, offset, count ); 
    //         * for Base64 encoding
    //         */
    //        xw.WriteBinHex(buffer, offset, count);
    //    }
    //}

//}