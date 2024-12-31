using QRCoder;
using System.Drawing;

namespace VaaradhiPay.Services.Implementations
{
    public class QRServices
    {
        public string GenerateQrCodeSvg(string data, string logoPath)
        {
            try
            {
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                    using (var qrCode = new SvgQRCode(qrCodeData))
                    {
                        var qrSvg = qrCode.GetGraphic(20);

                        if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                        {
                            try
                            {
                                var logoBytes = File.ReadAllBytes(logoPath);
                                var base64Logo = Convert.ToBase64String(logoBytes);

                                // Set the desired size for the logo
                                var logoSize = 250;
                                var translateAmount = logoSize / 2;

                                // Adjust the SVG to embed the logo with the new size
                                var svgWithLogo = qrSvg.Replace("</svg>",
                                   $"<rect x='50%' y='50%' height='{logoSize}' width='{logoSize}' fill='white' " +
                                   $"transform='translate(-{translateAmount},-{translateAmount})' />" +
                                   $"<image xlink:href='data:image/png;base64,{base64Logo}' " +
                                   $"x='50%' y='50%' height='{logoSize}' width='{logoSize}' " +
                                   $"transform='translate(-{translateAmount},-{translateAmount})' /></svg>");

                                return svgWithLogo;
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidOperationException("Failed to embed the logo in the QR code.", ex);
                            }
                        }

                        return qrSvg;
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }


    }
}


