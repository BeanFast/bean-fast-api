using QRCoder;
using System.Net;
using System.IO;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            using (FileStream fs = new FileStream("qrcode.png", FileMode.Create))
            {
                fs.Write(qrBytes, 0, qrBytes.Length);
            }
            return qrCode.GetGraphic(20);
        }
    }
}
