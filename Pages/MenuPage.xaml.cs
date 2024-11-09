using KIOSK_LITE.Models;
using KIOSK_LITE.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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

namespace KIOSK_LITE.Pages
{
    /// <summary>
    /// MenuPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuPage : Page, INotifyPropertyChanged
    {
        public DataTable MenuDt { get; set; }
        public List<string> ListUseYn { get; set; }

        private byte[]? _imgFileName;

        public event PropertyChangedEventHandler? PropertyChanged;

        public byte[]? ImgFileName { get { return _imgFileName; } set { _imgFileName = value; if (_imgFileName == null) _imgFileName = GetDefaultImage(); OnPropertyChanged(nameof(ImgFileName)); } }
        private List<Category> _categories;
        public List<Category> Categories { get { return _categories; } set { _categories = value; OnPropertyChanged(nameof(Categories)); } }


        private byte[]? GetDefaultImage()
        {
            string filepath = Environment.CurrentDirectory + "\\Assets";
            string fileName = "defaultImage.png";
            return ImageHelper.ImageArray(fileName, filepath);
        }

        public MenuPage()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            InitProperty();
            InitMenuDt();
        }
        private void InitProperty()
        {
            ListUseYn = BaseModel.UseYn;
        }
        private void InitMenuDt()
        {
            //MenuDt = CsvHelper.LoadCsvToDataTable(nameof(MenuDt));
            MenuDt = BaseModel.MenuDt;
            if (MenuDt == null)
            {
                MenuDt = new DataTable();
                MenuDt.Columns.Add("MENU_CD");
                MenuDt.Columns.Add("MENU_NM");
                MenuDt.Columns.Add("CT_CD");
                MenuDt.Columns.Add("USE_YN");
                MenuDt.Columns.Add("PRICE");
                MenuDt.Columns.Add("IMG_PATH");
                MenuDt.Columns.Add("IMG_NM");
                MenuDt.Rows.Add("0001", "싸이버거", "001", "N", "4000", "", "");
            }
            grdMenu.ItemsSource = MenuDt.DefaultView;
            cBoxUseYn.ItemsSource = ListUseYn;
            LoadCategory();

            //cBoxCt.ItemsSource = Categories;
            MenuDt.TableNewRow += MenuDt_TableNewRow;
            //  MenuDt.RowChanged += MenuDt_RowChanged;
        }

        private void MenuDt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            //ImgFileName= e.Row["IMG_PATH"].ToString();
            ImgFileName = ImageHelper.ImageArray(e.Row["IMG_NM"].ToString());
        }

        private void MenuDt_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if (MenuDt == null || MenuDt.Rows.Count < 1)
            {
                e.Row["MENU_CD"] = "0001";
                e.Row["USE_YN"] = "Y";
                e.Row["IMG_NM"] = "";
                e.Row["IMG_PATH"] = "";
                return;
            }
            string maxMenuCd = MenuDt.AsEnumerable().Max(r => r["MENU_CD"]).ToString();
            int intmaxMenuCd;
            if (string.IsNullOrEmpty(maxMenuCd)) maxMenuCd = "0001";
            else
            {
                if (int.TryParse(maxMenuCd, out intmaxMenuCd))
                {
                    maxMenuCd = string.Format("{0:D4}", intmaxMenuCd + 1);
                }
                else maxMenuCd = "0001";
            }
            e.Row["MENU_CD"] = maxMenuCd;
            e.Row["USE_YN"] = "Y";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "이미지"; // Default file name
            //dialog.DefaultExt = ".jpg"; // Default file extension
            dialog.Filter = "ImageFiles|*.jpg;*.png"; // Filter files by extension

            if (grdMenu.SelectedItem == null || grdMenu.SelectedItem == CollectionView.NewItemPlaceholder) { MessageBox.Show("메뉴 이름을 먼저 입력해주세요"); return; }
            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                DataRowView row = (DataRowView)grdMenu.SelectedItem;

                // Open document
                string filePath = dialog.FileName;
                row["IMG_PATH"] = filePath;
                string ext = filePath.Split("\\").Last().Split(".").Last();
                string fileName = row["MENU_CD"].ToString() + "." + ext;
                row["IMG_NM"] = fileName;
                ImgFileName = ImageHelper.ImageArrayFromFullPath(filePath);

            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void SaveMenu()
        {
            CsvHelper.SaveCsv(nameof(BaseModel.MenuDt), MenuDt);
            SaveImg();

        }

        private void SaveImg()
        {
            //ImgFileName = b;
            foreach (var item in grdMenu.Items)
            {
                DataRowView row = item as DataRowView;
                if (row == null) continue;
                if (string.IsNullOrEmpty(row["IMG_PATH"].ToString())) continue;
                string saveFileName = row["IMG_NM"].ToString();
                string sourceFilePath = row["IMG_PATH"].ToString();
                ImageHelper.SaveImage(sourceFilePath, saveFileName);
            }
        }

        private void grdMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ImgFileName= e.Row["IMG_PATH"].ToString();
            if (grdMenu.SelectedItem == null || grdMenu.SelectedItem == CollectionView.NewItemPlaceholder) { ImgFileName = null; return; }
            DataRowView row = (DataRowView)grdMenu.SelectedItem;
            string filepath = "";
            filepath = ImageHelper.ImageFilePath(row["IMG_NM"].ToString());
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists) //저장된 이미지 존재하면
            {
                ImgFileName = ImageHelper.ImageArray(row["IMG_NM"].ToString());
                return;
            }
            else
            {
                filepath = row["IMG_PATH"].ToString();
                if (string.IsNullOrEmpty(filepath)) { ImgFileName = null; return; }
                FileInfo fi2 = new FileInfo(filepath);
                if (fi2.Exists)
                {
                    string fileName = filepath.Split("\\").Last();
                    string dirName = filepath.Replace($"\\{fileName}", "");
                    ImgFileName = ImageHelper.ImageArray(fileName, dirName);
                    //ImgFileName = filepath;
                    return;
                }
                else ImgFileName = null;
            }
            //ImgFileName=((DataRowView)grdMenu.SelectedItem)["IMG_PATH"].ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadCategory();
        }

        private void LoadCategory()
        {
            List<Category> categories = BaseModel.GetCategory();
            if (categories == null) categories= new List<Category>();
            
           
            Categories = categories.Where(r => r.USE_YN == "Y").ToList();
            cBoxCt.ItemsSource = Categories;
        }
    }
}
