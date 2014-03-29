using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class rapoutController : ApiController
    {
        // GET api/rapout
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/rapout/5


        




        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        static item HttpGet(string url)
        {
            HttpWebRequest req = WebRequest.Create(url)
                                 as HttpWebRequest;
            string result = null;
            using (HttpWebResponse resp = req.GetResponse()
                                          as HttpWebResponse)
            {
                StreamReader reader =
                    new StreamReader(resp.GetResponseStream());
                string x = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<item>(x);
            }
        }

        public class item
        {
            public string stream_url;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static byte[] m_Bytes;

        public void Get(int id)
        {            
            item classRef = HttpGet("http://api.soundcloud.com/tracks/" + id + ".json?client_id=453f6b4d3bde6b9c15b0bb54c1addc31");
            WebRequest req = HttpWebRequest.Create(classRef.stream_url);
            using (Stream stream = req.GetResponse().GetResponseStream())
            {
                byte[] m_Bytes = ReadFully(stream);
            }

        }



        // POST api/rapout
        public void Post([FromBody]byte[] value)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(value,0,value.Length);
                // If you need it...
            string temp;
            byte[] data = ms.ToArray();
            string result1 = Path.GetTempPath();
            string result2 = Path.GetTempPath();
            string[] something = new string[2];
            something[0]=result1;
            something[1]=result2;
            File.WriteAllBytes(result1, data);
            File.WriteAllBytes(result2, m_Bytes);
            LameWavToMp3(result1, result1);
            Stream output= new MemoryStream();
            Combine(something, output);

        }

        private static void LameWavToMp3(string wavFile, string outmp3File)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"lame.exe";
            psi.Arguments = "-V2 " + wavFile + " " + outmp3File;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(psi);
            p.WaitForExit();
        }

        public static void Combine(string[] inputFiles, Stream output)
        {
            foreach (string file in inputFiles)
            {
                Mp3FileReader reader = new Mp3FileReader(file);
                if ((output.Position == 0) && (reader.Id3v2Tag != null))
                {
                    output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                }
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    output.Write(frame.RawData, 0, frame.RawData.Length);
                }
            }
        }

        // PUT api/rapout/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/rapout/5
        public void Delete(int id)
        {
        }
    }
}
