using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

namespace BDI3Mobile.Common
{
    public class KeyEncryption
    {
        private static readonly Dictionary<string, byte> ByteLookup = new Dictionary<string, byte>();
        private static readonly Dictionary<byte, string> HexLookup = new Dictionary<byte, string>();
        private static readonly TimeZoneInfo timeZoneInfo = TimeZoneInfo.Utc;
        static KeyEncryption()
        {
            for (var b = 0; b < 256; b++)
            {
                var hex = b.ToString("X2");
                ByteLookup[hex] = (byte)b;
                HexLookup[(byte)b] = hex;
            }

            var tz = TimeZoneInfo.Utc;
            foreach (var t in TimeZoneInfo.GetSystemTimeZones())
            {
                if (Device.RuntimePlatform == Device.iOS && tz.Equals(TimeZoneInfo.Utc))
                {
                    if (t.StandardName.Equals("PST"))
                        tz = t;
                }
                else if (t.DisplayName.Contains("Pacific") && tz.Equals(TimeZoneInfo.Utc))
                {
                    tz = t;
                }
            }
            timeZoneInfo = tz;
        }

        private static string KeyStr
        {
            get
            {
                var dateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
                var _keyStr = string.Format("@@!WJIV:{0}:TransportKey:{1}:{2}_{3}!@@", dateTime.Month,
                    dateTime.Day, dateTime.DayOfYear, dateTime.Year);
                if (_keyStr.Length > 32)
                    _keyStr = _keyStr.Substring(_keyStr.Length - 32, 32);
                return _keyStr;
            }
        }
        public static string Encrypt(string original, bool mobile = false)
        {
            if (original == null)
                return null;
            if (mobile)
                return original;
            var crypto = new AesCryptoServiceProvider { Padding = PaddingMode.ISO10126, KeySize = 256, Mode = CipherMode.CBC };
            var keyStr = KeyStr;
            var key =
                Encoding.ASCII.GetBytes(keyStr);
            var iv = Encoding.ASCII.GetBytes(keyStr.Substring(0, 16));
            using (var ms = new MemoryStream())
            {
                var cs = new CryptoStream(ms, crypto.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                var data = Encoding.Unicode.GetBytes(original);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ToHexString(ms.ToArray());
            }
        }

        public static int? DecryptAsInt(string data, bool mobile = false)
        {
            if (data?.Length < 11) // encrypted data is always at least 32 bytes and max int is 10 chars long anyway
            {
                if (int.TryParse(data, out var plainInt))
                    return plainInt;
            }
            var valid = int.TryParse(Decrypt(data, mobile), out var res);
            return valid ? res : (int?)null;
        }
        public static string Decrypt(string hexData, bool mobile = false)
        {
            if (string.IsNullOrEmpty(hexData))
                return hexData;
            if (mobile) return hexData;
            var crypto = new AesCryptoServiceProvider { Padding = PaddingMode.ISO10126, KeySize = 256, Mode = CipherMode.CBC };
            var keyStr = KeyStr;
            var key =
                Encoding.ASCII.GetBytes(keyStr);
            var iv = Encoding.ASCII.GetBytes(keyStr.Substring(0, 16));

            using (var ms = new MemoryStream(BytesFromHex(hexData)))
            {
                var cs = new CryptoStream(ms, crypto.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                var sr = new StreamReader(cs, Encoding.Unicode);
                var _data = sr.ReadToEnd();
                return _data;
            }
        }

        public static string ToHexString(byte[] data)
        {
            var sb = new StringBuilder();

            foreach (var t in data)
            {
                sb.Append(HexLookup[t]);
                //sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(HexLookup[t]);
                //sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] BytesFromHex(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ByteLookup[hex.Substring(i * 2, 2)];
                //bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;

        }
        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }

        public static string Encrypt(long? userId, bool mobile = false)
        {
            return Encrypt(Convert.ToString(userId), mobile);
        }
    }
}
