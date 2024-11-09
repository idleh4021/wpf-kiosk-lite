using KIOSK_LITE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KIOSK_LITE.Views
{
    /// <summary>
    /// PayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PayView : Window, INotifyPropertyChanged
    {
        private ObservableCollection<OrderForPay> _orderForPay= new ObservableCollection<OrderForPay>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<OrderForPay> OrderForPay { get => _orderForPay; set { _orderForPay = value; OnPropertyChanged(nameof(OrderForPay)); } }

        private int _order_tot_price;
        public int ORDER_TOT_PRICE { get => _order_tot_price; set { _order_tot_price = value;OnPropertyChanged(nameof(ORDER_TOT_PRICE)); } }
        public PayView(ObservableCollection<Order> orderList, ObservableCollection<OrderOption> optionList)
        {
            InitializeComponent();
            DataContext = this;
            MakePayList(orderList, optionList);
            SumOrderPrice();
        }

        private void SumOrderPrice()
        {
            ORDER_TOT_PRICE= OrderForPay.Sum(r => r.TOT_PRICE);
        }

        private void MakePayList(ObservableCollection<Order> orderList, ObservableCollection<OrderOption> optionList)
        {
            foreach (var item in orderList)
            {
                var options = optionList.Where(r => r.ORDER_SEQ == item.ORDER_SEQ && r.CHK == true);
                var desclist = options.Select(r=>r.OP_NM).ToList();
                string options_desc = string.Join(",", desclist);

                OrderForPay.Add(new Models.OrderForPay() 
                                            { MENU_NM = item.MENU_NM,
                                              OP_NM = options_desc,
                                              QTY = item.QTY,
                                              TOT_PRICE = (Convert.ToInt32(item.PRICE) + options.Sum(r=>r.PRICE))*item.QTY
                                            }
                );
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
