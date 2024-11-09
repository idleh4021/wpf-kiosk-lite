using KIOSK_LITE.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KIOSK_LITE.Models
{
    public class Menu
    {

        public string MENU_CD { get; set; }
        public string MENU_NM { get; set; }
        public string CT_CD { get; set; }
        public string USE_YN { get; set; }
        public int PRICE { get; set; }
        public string IMG_PATH { get; set; }
        public string IMG_NM { get; set; }
        public byte[] IMAGE
        {
            get
            {
                return ImageHelper.ImageArray(IMG_NM);
            }
        }

    }
}
