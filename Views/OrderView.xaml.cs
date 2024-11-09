using KIOSK_LITE.Models;
using KIOSK_LITE.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Menu = KIOSK_LITE.Models.Menu;

namespace KIOSK_LITE.Views
{
    /// <summary>
    /// OrderView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderView : Window,INotifyPropertyChanged
    {
        private List<Category> _categoryList;

        public List<Category> CategoryList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(nameof(CategoryList)); } }
        public event PropertyChangedEventHandler? PropertyChanged;

        private Category _selectedCategory;
        public Category SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; FilterMenu(value); OnPropertyChanged(nameof(SelectedCategory)); } }

        private void FilterMenu(Category value)
        {
            string CT_CD = value.CT_CD;
            if(string.IsNullOrEmpty(CT_CD)) { this.MenuList = _allMenuList;return; }
            var MenuList = _allMenuList.Where(r=>r.CT_CD== CT_CD).ToList();
            this.MenuList = MenuList;

        }

        private List<Menu> _menuList;
        public List<Menu> MenuList { get => _menuList; set { _menuList = value; OnPropertyChanged(nameof(MenuList)); } }

        private List<Menu> _allMenuList;

        private ObservableCollection<Order> _orderList;
        public ObservableCollection<Order> OrderList { get => _orderList; set { _orderList = value; OnPropertyChanged(nameof(OrderList)); } }

        private ObservableCollection<OrderOption> _optionList;
        public ObservableCollection<OrderOption> OptionList { get => _optionList; set { _optionList = value; OnPropertyChanged(nameof(OptionList)); } }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public OrderView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Category> TmpCategoryList= BaseModel.GetCategory();
            if (TmpCategoryList == null) TmpCategoryList = new List<Category>();
            TmpCategoryList.Add(new Category { CT_CD = "", CT_NM = "전체", USE_YN = "Y" });
            CategoryList = TmpCategoryList;
            _allMenuList = BaseModel.GetMenu();
            ObservableCollection<Order> tempOrder= new ObservableCollection<Order>();
            OrderList = tempOrder;
            OptionList = new ObservableCollection<OrderOption>();
            OnPropertyChanged(nameof(OrderList));
            MenuList = _allMenuList;
            //foreach(Menu menu in MenuList)
            //{
            //    UC_MenuBtn btn = new UC_MenuBtn(menu);
            //    ItemMenu.Items.Add(btn);
            //}
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Menu menu = btn.DataContext as Menu;
            Order addOrder = new Order()
            { ORDER_NO="0",
                MENU_CD = menu.MENU_CD,
            MENU_NM= menu.MENU_NM,
                ORDER_SEQ =(OrderList==null || OrderList.Count==0)?0: OrderList.Max(r=>r.ORDER_SEQ)+1,
                PRICE = menu.PRICE,
                QTY = 1,
                TOT_PRICE = menu.PRICE
            };
            //List<Order> _list = OrderList;
            //_list.Add(addOrder);
            //OrderList = _list;
            OrderList.Add(addOrder);
            OnPropertyChanged(nameof(OrderList));
            //ItemOrder.Items.Add(new Button() { Content = "zz" });

        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.DataContext as Order;
            if (order.QTY <= 1) return;
            order.QTY -= 1;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.DataContext as Order;
            order.QTY+= 1;
        }

        private void btnDelOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.DataContext as Order;
            OrderList.Remove(order);
            ObservableCollection<OrderOption> options = new ObservableCollection<OrderOption>(OptionList.Where(r => r.ORDER_SEQ != order.ORDER_SEQ));
            OptionList = options;
        }

        private void btnOption_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.DataContext as Order;
            OrderOptionView view = new OrderOptionView(order,OptionList);
            view.ShowDialog();
            if ((bool)view.DialogResult)
            {
                var options = new ObservableCollection<OrderOption>(OptionList.Where(r => r.ORDER_SEQ != order.ORDER_SEQ));
                if (options == null) options = new ObservableCollection<OrderOption>();
                var selOptions = view.OptionList.Where(r => r.CHK == true);
                foreach(var val in selOptions)
                {
                    OrderOption orderOption = new OrderOption()
                    {
                        CHK = val.CHK,
                        MENU_CD = val.MENU_CD,
                        OP_CD = val.OP_CD,
                        OP_NM = val.OP_NM,
                        ORDER_SEQ = order.ORDER_SEQ,
                        PRICE = val.PRICE,
                        USE_YN = val.USE_YN
                    };
                    options.Add(orderOption);
                }
                OptionList = options;
                
            }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if(OrderList==null || OrderList.Count < 1)
            {
                MessageBox.Show("주문내역이 없습니다."); return;
            }
            PayView view = new PayView(OrderList, OptionList);
            view.ShowDialog();
            if (!(bool)view.DialogResult) return;
            int order_tot_price = view.ORDER_TOT_PRICE;
            string order_no = BaseModel.getNewOrderNo();

            DataTable orderHDt = CsvHelper.LoadOrderHCsvToDataTable();
            if (orderHDt == null)
            {
                orderHDt = new DataTable();
                orderHDt.Columns.Add("ORDER_NO");
                orderHDt.Columns.Add("TOT_PRICE");
                orderHDt.Columns.Add("SALE_TIME");
            }
            OrderH orderH = new OrderH() { ORDER_NO = order_no, TOT_PRICE  = order_tot_price,SALE_TIME=DateTime.Now };
            orderHDt.Rows.Add(orderH.ORDER_NO, orderH.TOT_PRICE,orderH.SALE_TIME);
            string strOrderH = JsonConvert.SerializeObject(orderH);
            //DataTable orderHDt = JsonConvert.DeserializeObject<DataTable>(strOrderH);
            CsvHelper.SaveOrderCsv("ORDER_H", orderHDt);
            
            string strOrderList = JsonConvert.SerializeObject(OrderList);
            DataTable oredrListDt = JsonConvert.DeserializeObject<DataTable>(strOrderList);
            foreach(DataRow row in oredrListDt.Rows) { row["ORDER_NO"] = order_no; }
            CsvHelper.SaveOrderCsv("D_" + order_no, oredrListDt);
            
            string strOptionList = JsonConvert.SerializeObject(OptionList);
            DataTable optionListDt = JsonConvert.DeserializeObject<DataTable>(strOptionList);
            foreach(DataRow row in optionListDt.Rows) { row["ORDER_NO"] = order_no; }
            CsvHelper.SaveOrderCsv("OP_" + order_no, optionListDt);

            //MessageBox.Show($"주문이 완료됐습니다. \n주문 번호 : {order_no}");
            NotiView noti = new NotiView("주문완료", "주문이 완료되었습니다.", $"주문 번호 :{order_no}");
            noti.ShowDialog();
            OrderList.Clear();
            OptionList.Clear();


        }
    }
}
