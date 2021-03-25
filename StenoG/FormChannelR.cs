using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class FormChannelR : FormChannelX
    {
        public override Bitmap processImage(Bitmap sourceImage,
            ref BackgroundWorker worker)
        {
            return processImage(sourceImage, ref worker, 33, 0);
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage,
            int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(sourceColor.R, sourceColor.R, sourceColor.R);
        }
    }
}
