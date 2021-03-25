using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace StenoG
{
   abstract class ProcessImg
    {
        public virtual Bitmap get_image_r() { return null; }
        public virtual Bitmap get_image_g() { return null; }
        public virtual Bitmap get_image_b() { return null; }
        public virtual Bitmap get_image_raw() { return null; }
        public virtual Bitmap get_image_noised() { return null; }
        public virtual Bitmap get_image_LSB() { return null; }
        public virtual Bitmap get_image_diff() { return null; }

        protected virtual Color calculateNewPixelColor(Bitmap sourceImage,
            int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }

        protected virtual Color calculateNewPixelColor(Bitmap currImage,
            Bitmap originImage, int x, int y)
        {
            Color currColor = currImage.GetPixel(x, y);
            return Color.FromArgb(currColor.R, currColor.G, currColor.B);
        }

        public virtual Bitmap processImage(Bitmap currImage, Bitmap originImage,
            BackgroundWorker worker)
        {
            return currImage;
        }

        public virtual Bitmap processImage(Bitmap sourceImage,
            ref BackgroundWorker worker)
        {
            return sourceImage;
        }

        public virtual void processImage(Bitmap sourceImage,
            ref BackgroundWorker worker, bool no_return_value)
        {
            return;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
