using KIOSK_LITE.Models;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KIOSK_LITE.Models;
using KIOSK_LITE.Repositories;

namespace KIOSK_LITE.Pages
{
    /// <summary>
    /// OptionPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Option> _options;
        public ObservableCollection<Option> Options { get => _options; set { _options = value; OnPropertyChanged(nameof(Options)); } }

        private DataTable _optionDt;
        public DataTable OptionDt { get => _optionDt; set { _optionDt = value; OnPropertyChanged(nameof(OptionDt)); } }

        public List<string> ListUseYn { get; set; }

        private List<Models.Menu> _menus;
        public List<Models.Menu> Menus { get => _menus; set { _menus = value; OnPropertyChanged(nameof(Menus)); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public OptionPage()
        {
            InitializeComponent();
            DataContext = this;
            Options = new ObservableCollection<Option>();
        }



        private void Page_Initialized(object sender, EventArgs e)
        {

            InitProperty();
            InitOptionsDt();
        }

        private void InitProperty()
        {
            ListUseYn = BaseModel.UseYn;
        }

        private void InitOptionsDt()
        {
            OptionDt = BaseModel.OptionDt;
            if (OptionDt == null)
            {
                OptionDt = new DataTable();

                OptionDt.Columns.Add("OP_CD");
                OptionDt.Columns.Add("OP_NM");
                OptionDt.Columns.Add("PRICE");
                OptionDt.Columns.Add("MENU_CD");
                OptionDt.Columns.Add("USE_YN");
                OptionDt.Rows.Add("01", "패티추가", "1000", "0001", "Y");
            }



            cBoxUseYn.ItemsSource = ListUseYn;
            OptionDt.TableNewRow += OptionsDt_TableNewRow;
            LoadMenu();
        }

        private void LoadMenu()
        {
            List<Models.Menu> menus= BaseModel.GetMenu();
            if (menus == null) menus = new List<Models.Menu>();
            Menus = menus.Where(r => r.USE_YN == "Y").ToList();
            cBoxMenu.ItemsSource = Menus;
        }

        private void OptionsDt_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if (OptionDt == null || OptionDt.Rows.Count < 1)
            {
                e.Row["OP_CD"] = "01";
                e.Row["USE_YN"] = "Y";
                e.Row["OP_NM"] = "";
                e.Row["MENU_CD"] = "";
                return;
            }
            string maxOptionsCd = OptionDt.AsEnumerable().Max(r => Convert.ToInt32(r["OP_CD"])).ToString();
            int intmaxOptionCd;
            if (string.IsNullOrEmpty(maxOptionsCd)) maxOptionsCd = "01";
            else
            {
                if (int.TryParse(maxOptionsCd, out intmaxOptionCd))
                {
                    maxOptionsCd = string.Format("{0:D2}", intmaxOptionCd + 1);
                }
                else maxOptionsCd = "01";
            }
            e.Row["OP_CD"] = maxOptionsCd;
            e.Row["USE_YN"] = "Y";
            e.Row["OP_NM"] = "";
            e.Row["MENU_CD"] = "";
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Options.Add(new Option() { MENU_CD = "0001", OP_CD = "01", OP_NM = "패티추가", USE_YN = "Y" });

        }

        public void SaveOption()
        {
            CsvHelper.SaveCsv(nameof(BaseModel.OptionDt), OptionDt);

        }
    }
}
