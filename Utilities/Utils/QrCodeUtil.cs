using QRCoder;
using System.Net;
using System.IO;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Utils
{
    public class QrCodeUtil
    {
        public static byte[] GenerateQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);
            //using (FileStream fs = new FileStream("qrcode.png", FileMode.Create))
            //{
            //    fs.Write(qrBytes, 0, qrBytes.Length);
            //}
            return qrCode.GetGraphic(20);
        }
        public static string GenerateQRCodeString(string message, string key)
        {
            // Convert message and key to byte arrays
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Create HMACSHA256 instance
            using (var hmac = new HMACSHA256(keyBytes))
            {
                // Compute the hash
                byte[] hashBytes = hmac.ComputeHash(messageBytes);

                // Convert hash to hexadecimal string
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}
