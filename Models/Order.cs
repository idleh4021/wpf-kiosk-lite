using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSK_LITE.Models
{
    public class Order : INotifyPropertyChanged
    {
        public string ORDER_NO { get; set; }
        public int ORDER_SEQ { get; set; }

        public int PRICE { get; set; }

        public string MENU_CD { get; set; }

        public string MENU_NM { get; set; }


        private int _qty;
        public int QTY { get { return _qty; } set { _qty = value; TOT_PRICE = (_qty * PRICE); OnPropertyChanged(nameof(QTY)); } }
        public int TOT_PRICE { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderH
    {
        public string ORDER_NO { get; set; }
        public decimal TOT_PRICE { get; set; }
        public DateTime SALE_TIME { get; set; }
    }

    public class OrderForPay
    {
        public string MENU_NM { get; set; }
        public string OP_NM { get; set; }
        public int QTY { get; set; }
        public int TOT_PRICE { get; set; }
    }
}
