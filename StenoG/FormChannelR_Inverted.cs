using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class FormChannelR_Inverted : FormChannelR
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage,
           int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int color = (sourceColor.R > 0) ? 0 : 255;
            return Color.FromArgb(color, color, color);
        }
    }
}
