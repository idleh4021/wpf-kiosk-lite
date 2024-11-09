using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSK_LITE.Repositories
{
    public static class CsvHelper
    {
        static string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\KIOSK\\CSV";

        public static void SaveCsv(string Name, DataTable dt)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists) dir.Create();

            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(dirPath + "\\" + Name + ".csv", sb.ToString(), Encoding.UTF8);
        }

        public static void SaveOrderCsv(string Name, DataTable dt)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath+"\\ORDER");
            if (!dir.Exists) dir.Create();

            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(dirPath + "\\ORDER" + "\\" + Name + ".csv", sb.ToString(), Encoding.UTF8);
        }
        public static DataTable LoadOrderHCsvToDataTable()
        {
            DataTable dataTable = new DataTable();
            string fileName = dirPath + "\\ORDER" + "\\" + "ORDER_H" + ".csv";
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists) return null;
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(fs))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }
                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(',');
                    dataTable.Rows.Add(rows);
                }
            }
            return dataTable;
        }
        public static DataTable LoadCsvToDataTable(string Name)
        {
            DataTable dataTable = new DataTable();
            string fileName = dirPath + "\\" + Name + ".csv";
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists) return null;
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(fs))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }
                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(',');
                    dataTable.Rows.Add(rows);
                }
            }
            return dataTable;
        }

        public static void CopyTestData()
        {
            DirectoryInfo appStartPath = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TEST");
            DirectoryInfo targetPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\KIOSK");
            if (!targetPath.Exists) targetPath.Create();
            else { targetPath.Delete(true); }
            CopyDirectory(appStartPath.FullName, targetPath.FullName);
        }

        public static void CopyDirectory(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyDirectory(folder, dest);
            }
        }
    }
}