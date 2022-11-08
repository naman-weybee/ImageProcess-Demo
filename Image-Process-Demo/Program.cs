using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace Image_Process_Demo
{
    class Program
    {
        private static string basePath = "D:\\.NET\\SocialCRM-ImageProcessDemo\\Image-Process-Demo\\images\\";
        private static string mergePath = "D:\\.NET\\SocialCRM-ImageProcessDemo\\Image-Process-Demo\\mergeImages\\";
        static void Main(string[] args)
        {
            string email = "test@gmail.com";
            string mobileNo = "4567986564";
            string tag = "FrandshipDay";

            string emailColor = "#00FF00";
            string mobileNoColor = "#0000FF";
            string tagColor = "#FF0000";

            int emailAlign = 1;
            int mobileNoAlign = 1;
            int tagAlign = 1;

            Font emailFont = new Font("Roboto", 25, FontStyle.Regular, GraphicsUnit.Pixel);
            Font mobileNoFont = new Font("Cinzel Decorative", 50, FontStyle.Regular, GraphicsUnit.Pixel);
            Font tagFont = new Font("Edwardian Script ITC", 30, FontStyle.Regular, GraphicsUnit.Pixel);

            Image bgImage = Image.FromFile(basePath + "bgImage.jpg");
            Image squareLogo = Image.FromFile(basePath + "squareLogo.jpg");
            Image rectangleLogo = Image.FromFile(basePath + "rectangleLogo.jpg");
            Image packageLogo = Image.FromFile(basePath + "packageLogo.jpg");

            Program program = new Program();
            //program.WaterMark(new Bitmap(basePath + "bgImage.jpg"));
            program.MergeImages(bgImage, squareLogo, rectangleLogo, packageLogo, email, mobileNo, tag, emailFont, mobileNoFont, tagFont, emailColor, mobileNoColor, tagColor, emailAlign, mobileNoAlign, tagAlign);
        }

        public Bitmap MergeImages(Image bgImage, Image squareLogo, Image rectangleLogo, Image packageLogo, string email, string mobileNo, string tag, Font emailFont, Font mobileNoFont, Font tagFont, string emailColor, string mobileNoColor, string tagColor, int emailAlign, int mobileNoAlign, int tagAlign)
        {
            Image emailImage = TextToImage(email, emailFont, emailColor);
            Image mobileNoImage = TextToImage(mobileNo, mobileNoFont, mobileNoColor);
            Image tagImage = TextToImage(tag, tagFont, tagColor);
            Bitmap bitmap = new Bitmap(bgImage.Width, bgImage.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(bgImage, 0, 0);
                g.DrawImage(squareLogo, 10, 10, 100, 100);
                g.DrawImage(rectangleLogo, 780, 10, 200, 200);
                g.DrawImage(packageLogo, 400, 400, 200, 200);

                var X_emailAlign = 0;
                var Y_emailAlign = 0;
                var X_mobileNoAlign = 0;
                var Y_mobileNoAlign = 0;
                var X_tagAlign = 0;
                var Y_tagAlign = 0;

                //Email
                if (emailAlign == 1)
                {
                    X_emailAlign = 10;
                    Y_emailAlign = 1000 - emailImage.Height - 10;
                }
                else if (emailAlign == 2)
                {
                    X_emailAlign = 10 - emailImage.Width / 2;
                    Y_emailAlign = 1000 - emailImage.Height - 10;
                }
                else if (emailAlign == 2)
                {
                    X_emailAlign = 10 - emailImage.Width;
                    Y_emailAlign = 1000 - emailImage.Height - 10;
                }

                //MobileNo
                if (mobileNoAlign == 1)
                {
                    X_mobileNoAlign = (1000 - mobileNoImage.Width) / 2;
                    Y_mobileNoAlign = 500;
                }
                else if (mobileNoAlign == 2)
                {
                    X_mobileNoAlign = 400 - mobileNoImage.Width / 2;
                    Y_mobileNoAlign = 550;
                }
                else if (mobileNoAlign == 3)
                {
                    X_mobileNoAlign = 400 - mobileNoImage.Width;
                    Y_mobileNoAlign = 550;
                }

                //Tag
                if (tagAlign == 1)
                {
                    X_tagAlign = 1000 - tagImage.Width - 10;
                    Y_tagAlign = 1000 - tagImage.Height - 10;
                }
                else if (tagAlign == 2)
                {
                    X_tagAlign = (1000 - tagImage.Width - 10) - tagImage.Width / 2;
                    Y_tagAlign = 1000 - tagImage.Height - 10;
                }
                else if (tagAlign == 3)
                {
                    X_tagAlign = (1000 - tagImage.Width - 10) - tagImage.Width;
                    Y_tagAlign = 1000 - tagImage.Height - 10;
                }

                g.DrawImage(emailImage, X_emailAlign, Y_emailAlign);
                g.DrawImage(mobileNoImage, X_mobileNoAlign, Y_mobileNoAlign);
                g.DrawImage(tagImage, X_tagAlign, Y_tagAlign);
            }

            Image img = bitmap;
            string file = Path.GetFileNameWithoutExtension("Template_" + Path.GetRandomFileName()) + ".jpg";
            img.Save(mergePath + file);
            WaterMark(new Bitmap(mergePath + file));
            return bitmap;
        }

        public Bitmap TextToImage(string Text, Font font, string hashColor)
        {
            Color color = ColorTranslator.FromHtml(hashColor);
            int r = Convert.ToInt16(color.R);
            int g = Convert.ToInt16(color.G);
            int b = Convert.ToInt16(color.B);

            Bitmap bitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bitmap);
            int width = (int)graphics.MeasureString(Text, font).Width;
            int height = (int)graphics.MeasureString(Text, font).Height;
            bitmap = new Bitmap(bitmap, new Size(width, height));
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.DrawString(Text, font, new SolidBrush(Color.FromArgb(r, g, b)), 0, 0);
            graphics.Flush();
            graphics.Dispose();
            return bitmap;
        }

        public void WaterMark(Image image)
        {
            string watermarkText = "@WeyBee";
            Bitmap bitmap = new Bitmap(image);
            Graphics grp = Graphics.FromImage(bitmap);

            Brush brush = new SolidBrush(Color.FromArgb(99, Color.Gray));
            Font font = new Font("Arial", 60, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF textSize = new SizeF();
            textSize = grp.MeasureString(watermarkText, font);

            grp.TranslateTransform(0, 0);
            grp.RotateTransform(-25);
            int MaxH = 1500;
            int MaxW = 1000;
            for (int w = -100; w < MaxW; w = w + 500)
            {
                for (int h = -100; h < MaxH; h = h + 250)
                {
                    grp.DrawString(watermarkText, font, brush, w - ((int)textSize.Width / 2), h);
                }
            }
            grp.ResetTransform();
            bitmap.Save(mergePath + "TemplateWithWaterMark_" + Path.GetRandomFileName() + ".jpg", ImageFormat.Jpeg);
        }
    }
}
