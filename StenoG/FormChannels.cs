using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class FormChannels : ProcessImg
    {
        protected Bitmap image_r;
        protected Bitmap image_g;
        protected Bitmap image_b;

        public override Bitmap get_image_r() { return image_r; }
        public override Bitmap get_image_g() { return image_g; }
        public override Bitmap get_image_b() { return image_b; }

        public override void processImage(Bitmap sourceImage,
            ref BackgroundWorker worker, bool no_return_value)
        {
            ProcessImg prc_im_r = new FormChannelR();
            image_r = prc_im_r.processImage(sourceImage, ref worker);
            ProcessImg prc_im_g = new FormChannelG();
            image_g = prc_im_g.processImage(sourceImage, ref worker);
            ProcessImg prc_im_b = new FormChannelB();
            image_b = prc_im_b.processImage(sourceImage, ref worker);
        }
    }
}
