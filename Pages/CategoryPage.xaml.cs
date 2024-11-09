using KIOSK_LITE.Models;
using KIOSK_LITE.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KIOSK_LITE.Pages
{
    /// <summary>
    /// CateGoryPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CategoryPage : Page
    {
        public DataTable CategoryDt { get; set; }
        public List<string> ListUseYn { get; set; }

        public CategoryPage()
        {
            InitializeComponent();
        }

        public void SaveCategory()
        {
            CsvHelper.SaveCsv(nameof(BaseModel.CategoryDt), CategoryDt);
        }

        private void InitCatgDt()
        {
            //CategoryDt = CsvHelper.LoadCsvToDataTable(nameof(CategoryDt));
            CategoryDt = BaseModel.CategoryDt;
            if (CategoryDt == null)
            {
                CategoryDt = new DataTable();
                CategoryDt.Columns.Add("CT_CD");

                CategoryDt.Columns.Add("CT_NM");
                CategoryDt.Columns.Add("USE_YN");
                CategoryDt.Rows.Add("01", "주류", "Y");
            }
            grdCategory.ItemsSource = CategoryDt.DefaultView;
            cBoxUseYn.ItemsSource = ListUseYn;
            CategoryDt.TableNewRow += CategoryDt_TableNewRow;
        }

        private void CategoryDt_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {

            if (CategoryDt == null || CategoryDt.Rows.Count < 1)
            {
                e.Row["CT_CD"] = "001";
                e.Row["USE_YN"] = "Y";
                return;
            }
            string maxCtCd = CategoryDt.AsEnumerable().Max(r => r["CT_CD"]).ToString();
            int intMaxCtCd;
            if (string.IsNullOrEmpty(maxCtCd)) maxCtCd = "001";
            else
            {
                if (int.TryParse(maxCtCd, out intMaxCtCd))
                {
                    maxCtCd = string.Format("{0:D3}", intMaxCtCd + 1);
                }
                else maxCtCd = "001";
            }
            e.Row["CT_CD"] = maxCtCd;
            e.Row["USE_YN"] = "Y";

        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            InitProperty();
            InitCatgDt();
        }

        private void InitProperty()
        {
            ListUseYn = BaseModel.UseYn;
        }

        private void grdCategory_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            DataRowView rowView = grd.SelectedItem as DataRowView;
            if (string.IsNullOrEmpty(rowView["USE_YN"].ToString())) rowView["USE_YN"] = "Y";
        }

        //private void grdCategory_AddingNewItem(object sender, AddingNewItemEventArgs e)
        //{
        //    DataRow row = CategoryDt.NewRow();
        //    string maxCtCd = CategoryDt.AsEnumerable().Max(r => r["CT_CD"]).ToString();
        //    int intMaxCtCd;
        //    if (string.IsNullOrEmpty(maxCtCd)) maxCtCd = "001";
        //    else
        //    {
        //        if (int.TryParse(maxCtCd, out intMaxCtCd))
        //        {
        //            maxCtCd = string.Format("{0:D3}", intMaxCtCd + 1);
        //        }
        //    }
        //    row["CT_CD"] = maxCtCd;
        //    row["USE_YN"] = "Y";
        //    e.NewItem = row;

        //}
    }
}
