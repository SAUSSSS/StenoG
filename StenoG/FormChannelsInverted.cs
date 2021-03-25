using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class FormChannelsInverted : FormChannels
    {
        public override void processImage(Bitmap sourceImage,
            ref BackgroundWorker worker, bool no_return_value)
        {
            ProcessImg prc_im_r = new FormChannelR_Inverted();
            image_r = prc_im_r.processImage(sourceImage, ref worker);
            ProcessImg prc_im_g = new FormChannelG_Inverted();
            image_g = prc_im_g.processImage(sourceImage, ref worker);
            ProcessImg prc_im_b = new FormChannelB_Inverted();
            image_b = prc_im_b.processImage(sourceImage, ref worker);
        }
    }
}
