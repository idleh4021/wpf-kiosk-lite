using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIOSK_LITE.Repositories
{
    public static class ImageHelper
    {
        static string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\KIOSK\\Image";

        public static void SaveImage(string sourceFilePath, string targetFileName)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists) dir.Create();
            System.IO.File.Copy(sourceFilePath, dirPath + "\\" + targetFileName, true);
        }
        public static string ImageFilePath(string fileName)
        {
            return dirPath + "\\" + fileName;
        }

        public static byte[]? ImageArray(string fileName)
        {
            string filepath = ImageFilePath(fileName);
            FileInfo file = new FileInfo(filepath);
            if (!file.Exists) { return null; }
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                // 이미지를 읽어서 MemoryStream에 쓰기
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);

                    // MemoryStream의 내용을 byte 배열로 변환하여 반환
                    return ms.ToArray();
                }
            }
        }
        public static byte[]? ImageArray(string fileName, string filePath)
        {
            string fileFullPath = filePath + "\\" + fileName;
            FileInfo file = new FileInfo(fileFullPath);
            if (!file.Exists) { return null; }
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
            {
                // 이미지를 읽어서 MemoryStream에 쓰기
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);

                    // MemoryStream의 내용을 byte 배열로 변환하여 반환
                    return ms.ToArray();
                }
            }
        }

        public static byte[]? ImageArrayFromFullPath(string filePath)
        {

            FileInfo file = new FileInfo(filePath);
            if (!file.Exists) { return null; }
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // 이미지를 읽어서 MemoryStream에 쓰기
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);

                    // MemoryStream의 내용을 byte 배열로 변환하여 반환
                    return ms.ToArray();
                }
            }
        }

    }
}
