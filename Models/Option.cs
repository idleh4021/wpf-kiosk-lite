using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSK_LITE.Models
{
    public class Option 
    {
        public string OP_CD { get; set; }
        public string OP_NM { get; set; }
        public string MENU_CD { get; set; }
        public int PRICE { get; set; }

      
        public bool CHK { get; set; }
        public string USE_YN { get; set; }
  
    }

    public class OrderOption : Option
    {
        public string ORDER_NO { get; set; }
        public int ORDER_SEQ { get; set; }
    }
}
