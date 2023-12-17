using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using Interface;
using QRCoder;

namespace Service;

public class GjxService : IGjxService
{
    public async Task<string> GetEwm(string txt)
    {
        //创建一个QRCodeGenerator的实例，这是QRCoder库中用于生成QR码的核心类
        var qrCodeGenerator = new QRCodeGenerator();

        //CreateQrCode()中第一个参数是文本内容（这里我放的是我csdn的链接）
        //第二个参数是错误矫正等级
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(
            txt,
            QRCodeGenerator.ECCLevel.H);

        //使用前面生成的qrCodeData来创建一个QRCode的实例，这个实例将用于生成实际的二维码图像
        QRCode qrCode = new QRCode(qrCodeData);

        //GetGraphic方法的参数指定了二维码的像素大小、前景色、背景色以及是否要绘制一个白色的边框。
        //在这个例子中，每个QR码模块的大小被设置为15像素，前景色为黑色，背景色为白色，并且绘制了白色边框
        Bitmap bitmap = qrCode.GetGraphic(15,
            Color.Black,
            Color.White, true);

        // 将Bitmap保存到内存流中
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // 保存图像到流中，使用PNG格式
            bitmap.Save(memoryStream, ImageFormat.Png);

            // 将流的位置重置到开始位置，以便读取其内容
            memoryStream.Position = 0;

            // 读取流中的所有字节
            byte[] byteImage = memoryStream.ToArray();

            // 将字节数组转换为Base64字符串
            string base64String = Convert.ToBase64String(byteImage);

            return "data:image/png;base64,"+base64String;
        }
    }
}