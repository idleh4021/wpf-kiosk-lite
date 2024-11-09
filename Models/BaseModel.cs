using KIOSK_LITE.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSK_LITE.Models
{
    public static class BaseModel
    {
        public static DataTable CategoryDt { get { return CsvHelper.LoadCsvToDataTable(nameof(CategoryDt)); } }

        public static DataTable MenuDt { get { return CsvHelper.LoadCsvToDataTable(nameof(MenuDt)); } }
        public static DataTable OptionDt { get { return CsvHelper.LoadCsvToDataTable(nameof(OptionDt)); } }


        private static List<string> _useYn;
        public static List<string> UseYn { get { return _useYn; } }
        public static List<Menu> GetMenu()
        {
            DataTable dt = MenuDt;
            string json = JsonConvert.SerializeObject(dt);
            List<Menu> Menus = JsonConvert.DeserializeObject<List<Menu>>(json);
            return Menus;
        }

        public static ObservableCollection<Option> GetOption(string MENU_CD)
        {
            DataRow[] rows = OptionDt.Select($"MENU_CD = '{MENU_CD}'");
            if (rows.Length == 0) return null;
            DataTable dt = rows.CopyToDataTable();
            string json = JsonConvert.SerializeObject(dt);
            ObservableCollection<Option> options = JsonConvert.DeserializeObject<ObservableCollection<Option>>(json);
            return options;
        }

        public static List<Category> GetCategory()
        {
            DataTable dt = CategoryDt;
            string json = JsonConvert.SerializeObject(dt);
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(json);
            return categories;
        }
        public static ObservableCollection<Category> GetObCategory()
        {
            DataTable dt = CategoryDt;
            string json = JsonConvert.SerializeObject(dt);
            ObservableCollection<Category> categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
            return categories;
        }

        public static string getNewOrderNo()
        {
            //DataTable dt = CsvHelper.LoadCsvToDataTable("ORDER_H");
            DataTable dt = CsvHelper.LoadOrderHCsvToDataTable();
            if (dt == null || dt.Rows.Count == 0) return "001";
            object maxOrderNo = dt.Compute("MAX(ORDER_NO)", "");
            string newOrderNo = string.Format("{0:000}", Convert.ToInt32(maxOrderNo) + 1);
            return newOrderNo;
        }

        static BaseModel()
        {
            _useYn = new List<string>();
            _useYn.Add("Y");
            _useYn.Add("N");
        }
    }
}
