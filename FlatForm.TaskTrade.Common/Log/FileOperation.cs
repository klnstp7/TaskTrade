using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Peacock.Common.IO.Log
{
    public class FileOperation
    {
        private static readonly object syncObj = new object();

        /// <summary>
        /// 判断是否存在指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CheckIsExistsFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 校验文件路径是否存在，如果不存在，则创建一个新的文件,并返回false。
        /// </summary>
        /// <param name="filePath">文件路径，含文件名</param>
        public static bool CheckAndCreateFile(string filePath)
        {
            bool isExixt = true;

            string path = filePath.Substring(0, filePath.LastIndexOf("\\"));

            //lock (syncObj)
            {
                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

                if (!File.Exists(filePath))
                {
                    isExixt = false;
                    File.Create(filePath).Close();
                }
            }

            return isExixt;
        }
        
        /// <summary>
        /// 合并多个物理路径。系统会自动检测两个路径之间的\分隔符，并去掉重复或增加新的\分隔符。
        /// </summary>
        /// <param name="prePath">第一个物理路径</param>
        /// <param name="paths">相对物理路径</param>
        /// <returns></returns>
        public static string CombinPhysicPath(string prePath, params string[] paths)
        {
            if (paths.Length <= 0) return prePath;

            string path = prePath.TrimEnd('\\');
            for (int i = 0; i < paths.Length; i++)
            {
                path = string.Format(@"{0}\{1}", path.TrimEnd('\\'), paths[i].Trim('\\'));
            }

            return path;
        }

        /// <summary>
        /// 批量拷贝文件，覆盖目标目录下的同名文件
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        /// <param name="fileNames">要拷贝的文件名（如：test.txt）</param>
        public static bool CopyFiles(string sourcePath, string destPath, List<string> fileNames)
        {
            return CopyFiles(sourcePath, destPath, fileNames, null, true);
        }

        /// <summary>
        /// 批量拷贝文件,如果目标路径不存在则创建目标路径。
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        /// <param name="fileNames">要拷贝的文件名（如：test.txt）</param>
        /// <param name="destFileNames">目标文件名，含扩展名</param>
        /// <param name="isOverride">是否覆盖同名文件</param>
        /// <returns>是否拷贝成功，如果不存在文件则返回false</returns>
        public static bool CopyFiles(string sourcePath, string destPath, List<string> fileNames, List<string> destFileNames, bool isOverride)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destPath) || fileNames == null || fileNames.Count < 0)
            {
                return false;
            }
            if (!Directory.Exists(sourcePath)) return false;
            if (!Directory.Exists(destPath)) { Directory.CreateDirectory(destPath); }
            if (destFileNames == null || destFileNames.Count <= 0) { destFileNames = fileNames; }

            int i = 0;
            foreach (string fileName in fileNames)
            {
                string sourceFileName = CombinPhysicPath(sourcePath, fileName);
                string destFileName = CombinPhysicPath(destPath, destFileNames[i++]);

                File.Copy(sourceFileName, destFileName, isOverride);
            }

            return true;
        }

        /// <summary>
        /// 从给定的文件路径中得到文件名。
        /// </summary>
        /// <param name="filePath">文件路径，含文件名。</param>
        /// <returns></returns>
        public static string GetFileNameFromPath(string filePath)
        {
            string fileName = "";

            int index = filePath.LastIndexOf('\\') >= 0 ? filePath.LastIndexOf('\\') : filePath.LastIndexOf('/');
            if (index < 0) return filePath;

            fileName = filePath.Substring(index + 1);

            return fileName;
        }

        /// <summary>
        /// 返回指定目录下所有的文件（不包括子文件夹）
        /// </summary>
        /// <param name="fileFolder">要搜索的目录</param>
        /// <returns></returns>
        public static string[] GetFiles(string fileFolder)
        {
            return GetFiles(fileFolder, null, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 根据指定条件获取指定目录下的文件
        /// </summary>
        /// <param name="fileFolder">要搜索的目录</param>
        /// <param name="searchPattern">文件匹配</param>
        /// <param name="searchOption">是否需要搜索子文件夹</param>
        /// <returns></returns>
        public static string[] GetFiles(string fileFolder, string searchPattern, SearchOption searchOption)
        {
            if (Directory.Exists(fileFolder))
            {
                if (string.IsNullOrEmpty(searchPattern))
                {
                    return Directory.GetFiles(fileFolder);
                }
                else if (searchPattern == null)
                {
                    return Directory.GetFiles(fileFolder, searchPattern);
                }
                else
                {
                    return Directory.GetFiles(fileFolder, searchPattern, searchOption);
                }
            }

            return null;
        }
    }
}
