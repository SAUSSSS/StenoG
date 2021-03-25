using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class FormChannelX : ProcessImg
    {
        public Bitmap processImage(Bitmap sourceImage, ref BackgroundWorker worker,
           int mult, int summand)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress(Clamp((int)((float)i / resultImage.Width
                    * mult + summand), 0, 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage,
                        i, j));
                }
            }
            return resultImage;
        }
    }
}
