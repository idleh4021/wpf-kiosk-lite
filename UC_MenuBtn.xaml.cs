using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Menu = KIOSK_LITE.Models.Menu;


namespace KIOSK_LITE
{
    /// <summary>
    /// UC_MenuBtn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UC_MenuBtn : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Menu _menu;
        public Menu Menu { get => _menu; set { _menu = value; OnPropertyChanged(nameof(Menu)); } }

        public string MENU_NM { get => _menu.MENU_NM; }
        public byte[] IMAGE { get => _menu.IMAGE; }
        public int PRICE { get => _menu.PRICE; }

        public string MENU_CD { get => _menu.MENU_CD; }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public UC_MenuBtn()
        {
            InitializeComponent();
            DataContext = this;
        }

        public UC_MenuBtn(Menu menu)
        {
            InitializeComponent();
            DataContext = this;
            Menu = menu;
        }



    }
}
