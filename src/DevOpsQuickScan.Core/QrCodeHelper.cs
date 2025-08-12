using QRCoder;

namespace DevOpsQuickScan.Core;

public static class QrCodeHelper
{
    
    public static string GenerateQrPngDataUrl(string text)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        var qrBytes = qrCode.GetGraphic(20); // pixel size

        return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
    }
    
    public static string GenerateQrSvg(string text)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var svgQr = new SvgQRCode(qrData);
        return svgQr.GetGraphic(10); 
    }
}