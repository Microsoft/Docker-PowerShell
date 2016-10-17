using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Docker.PowerShell.Support
{
    public static class PrivateKey
    {
        private const byte Padding = 0x00;
        private static readonly string s_KeySet = Guid.NewGuid().ToString();

        public static RSACryptoServiceProvider ReadFromPemFile(string pemFileName)
        {
            var allBytes = File.ReadAllBytes(pemFileName);
            var mem = new MemoryStream(allBytes);
            var startIndex = 0;
            var endIndex = 0;

            using (var rdr = new BinaryReader(mem))
            {
                if (!TryReadUntil(rdr, "-----BEGIN RSA PRIVATE KEY-----"))
                {
                    throw new Exception("Invalid file format expected. No begin tag.");
                }

                startIndex = (int)(mem.Position);

                const string endTag = "-----END RSA PRIVATE KEY-----";
                if (!TryReadUntil(rdr, endTag))
                {
                    throw new Exception("Invalid file format expected. No end tag.");
                }

                endIndex = (int)(mem.Position - endTag.Length - 2);
            }

            // Convert the bytes from base64;
            var convertedBytes = Convert.FromBase64String(Encoding.UTF8.GetString(allBytes, startIndex, endIndex - startIndex));
            mem = new MemoryStream(convertedBytes);
            using (var rdr = new BinaryReader(mem))
            {
                var val = rdr.ReadUInt16();
                if (val != 0x8230)
                {
                    throw new Exception("Invalid byte ordering.");
                }

                // Discard the next bits of the version.
                rdr.ReadUInt32();
                if (rdr.ReadByte() != Padding)
                {
                    throw new InvalidDataException("Invalid ASN.1 format.");
                }

                var rsa = new RSAParameters();
                rsa.Modulus = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.Exponent = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.D = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.P = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.Q = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.DP = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.DQ = rdr.ReadBytes(ReadIntegerCount(rdr));
                rsa.InverseQ = rdr.ReadBytes(ReadIntegerCount(rdr));

                var csp = new CspParameters(1);
                csp.KeyContainerName = s_KeySet;
                var rsaProvider = new RSACryptoServiceProvider(csp);
                rsaProvider.PersistKeyInCsp = false;
                rsaProvider.ImportParameters(rsa);

                return rsaProvider;
            }
        }

        /// <summary>
        /// Reads an integer count encoding in DER ASN.1 format.
        /// <summary>
        private static int ReadIntegerCount(BinaryReader rdr)
        {
            const byte highBitOctet = 0x80;
            const byte ASN1_INTEGER = 0x02;

            if (rdr.ReadByte() != ASN1_INTEGER)
            {
                throw new Exception("Integer tag expected.");
            }

            int count = 0;
            var val = rdr.ReadByte();
            if ((val & highBitOctet) == highBitOctet)
            {
                byte numOfOctets = (byte)(val - highBitOctet);
                Debug.Assert(numOfOctets <= 4);

                for (var i = 0; i < numOfOctets; i++)
                {
                    count <<= 8;
                    count += rdr.ReadByte();
                }
            }
            else
            {
                count = val;
            }

            while (rdr.ReadByte() == Padding)
            {
                count--;
            }

            // The last read was a valid byte. Go back here.
            rdr.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }

        /// <summary>
        /// Reads until the matching PEM tag is found.
        /// <summary>
        private static bool TryReadUntil(BinaryReader rdr, string tag)
        {
            char delim = '\n';
            char c;
            char[] line = new char[64];
            int index;

            try
            {
                do
                {
                    index = 0;
                    while ((c = rdr.ReadChar()) != delim)
                    {
                        line[index] = c;
                        index++;
                    }
                } while (new string(line, 0, index) != tag);

                return true;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }
    }
}