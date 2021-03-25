using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class DiffWithOrigin : ProcessImg
    {
        Bitmap diff_img;

        public override Bitmap get_image_diff() { return diff_img; }

        protected override Color calculateNewPixelColor(Bitmap currImage,
            Bitmap originImage, int x, int y)
        {
            Color currColor = currImage.GetPixel(x, y);
            Color originColor = originImage.GetPixel(x, y);
            return Color.FromArgb(Math.Abs(currColor.R - originColor.R),
                Math.Abs(currColor.G - originColor.G),
                Math.Abs(currColor.B - originColor.B));
        }

        public override Bitmap processImage(Bitmap currImage, Bitmap originImage,
            BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(currImage.Width, currImage.Height);
            for (int i = 0; i < currImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < currImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(currImage,
                        originImage, i, j));
                }
            }
            diff_img = resultImage;
            return resultImage;
        }
    }
}
