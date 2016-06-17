using System.IO;

namespace Peacock.Common.Helper
{
    public class FileStreamHelper
    {
        /// <summary>
        /// 获取网站根目录
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationRootDir()
        {
            return System.Web.HttpRuntime.BinDirectory.Substring(0, System.Web.HttpRuntime.BinDirectory.TrimEnd('\\').LastIndexOf("\\")) + "\\";
        }

        /// <summary>
        /// 获得目录的物理路径
        /// </summary>
        /// <param name="sPath">文件路径</param>
        /// <returns></returns>
        public static string GetPathAtApplication(string sPath)
        {
            string result = "";
            if (sPath.StartsWith("~"))
            {
                result = sPath.Replace("~/", GetApplicationRootDir()).Replace("/", "\\");
            }
            else
            {
                result = GetApplicationRootDir() + sPath;
            }
            return result;
        }

        /// <summary>
        /// 判断文件夹是否存在，不存在则新建
        /// </summary>
        /// <param name="sFolderPath">文件夹路径</param>
        public static void IsExistsDirectory(string sFolderPath)
        {
            if (!Directory.Exists(sFolderPath))
            {
                Directory.CreateDirectory(sFolderPath);
            }
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static bool IsExits(string sFilePath)
        {
            bool result = File.Exists(sFilePath);
            return result;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sSourceFilePath">源文件路径</param>
        /// <param name="sTargetFilePath">目标文件路径</param>
        public static void CopyFile(string sSourceFilePath, string sTargetFilePath)
        {
            if (IsExits(sSourceFilePath))
            {
                File.Copy(sSourceFilePath, sTargetFilePath, true);
            }
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public static string FileFormat(string sFileName)
        {
            string result = string.Empty;
            result = Path.GetExtension(sFileName).Replace(".", "");
            return result;
        }

        /// <summary>
        /// 用于MVC文件下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetContentType(string sFileName)
        {
            string contentType = "application/octet-stream"; //"application/x-zip-compressed";
            string ext = Path.GetExtension(sFileName).ToLower();
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (registryKey != null && registryKey.GetValue("Content Type") != null)
            {
                contentType = registryKey.GetValue("Content Type").ToString();
            }
            return contentType;

        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="fullFilePath">文件完成路径</param>
        /// <returns></returns>
        public static bool CheckFileExeit(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
