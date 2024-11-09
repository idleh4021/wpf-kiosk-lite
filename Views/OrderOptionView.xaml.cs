using KIOSK_LITE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// OrderOptionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderOptionView : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Option> _optionList;
        public ObservableCollection<Option> OptionList { get => _optionList; set { _optionList = value; OnPropertyChanged(nameof(OptionList)); } }

        private int _tot_price;
        public int TOT_PRICE { get => _tot_price; set { _tot_price = value; OnPropertyChanged(nameof(TOT_PRICE)); } }

        private string _menu_nm;
        public string MENU_NM { get => _menu_nm; set { _menu_nm = value; OnPropertyChanged(nameof(MENU_NM)); } }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OrderOptionView()
        {
            InitializeComponent();
            DataContext = this;
        }
        public OrderOptionView(Order order,ObservableCollection<OrderOption> options)
        {
            InitializeComponent();
            DataContext = this;
            MENU_NM = order.MENU_NM;
            OptionList = BaseModel.GetOption(order.MENU_CD);
            var selOptions = options.Where(r =>  r.ORDER_SEQ ==order.ORDER_SEQ&& r.CHK == true );
            if (selOptions.Count() > 0)
            {
                foreach (var selOption in selOptions)
                {
                    var op = OptionList.FirstOrDefault(r => r.OP_CD == selOption.OP_CD);
                    op.CHK = true;
                }
                TOT_PRICE = OptionList.Where(r => r.CHK == true).Sum(r => r.PRICE);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TOT_PRICE = OptionList.Where(r=>r.CHK==true).Sum(r => r.PRICE);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (OptionList == null || OptionList.Count == 0) { MessageBox.Show("선택된 제품은 옵션이 없습니다."); this.DialogResult = false; Close(); }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
           
        }
    }
}
