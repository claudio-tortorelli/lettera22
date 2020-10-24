using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;


namespace Lettera22.Common
{
    /// <project>Lettera22</project>
    /// <copyright company="Claudio Tortorelli">
    /// Copyright (c) 2016 All Rights Reserved
    /// </copyright>        
    /// <author>Claudio Tortorelli</author>
    /// <email>claudio.tortorelli@gmail.com</email>
    /// <web>http://www.claudiotortorelli.it</web>
    /// <date>Sep 2016</date>
    /// <summary>
    /// A lot of utility functions
    /// </summary>
    /// https://choosealicense.com/licenses/mit/


    public static class Utils
    {
        private static int m_bConnectionAvailable = -1;

        /// <summary>
        /// Copy directories.
        /// </summary>
        /// <param name="sourceDirPath">The source dir path.</param>
        /// <param name="destDirName">Name of the destination dir.</param>
        /// <param name="isCopySubDirs">if set to <c>true</c> [is copy sub directories].</param>
        /// <returns></returns>
        /// http://stackoverflow.com/questions/18996330/copying-files-and-subdirectories-to-another-directory-with-existing-files
        /// 
        public static void DirectoryCopy(string sourceDirPath, string destDirName, bool bCopySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirPath);
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirPath);
            }
            DirectoryInfo parentDirectory = Directory.GetParent(directoryInfo.FullName);
            destDirName = System.IO.Path.Combine(parentDirectory.FullName, destDirName);

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);

                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                file.CopyTo(tempPath, false);
            }
            // If copying subdirectories, copy them and their contents to new location using recursive  function. 
            if (bCopySubDirs)
            {
                foreach (DirectoryInfo item in directories)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, item.Name);
                    DirectoryCopy(item.FullName, tempPath, bCopySubDirs);
                }
            }
        }

        /**
         * http://stackoverflow.com/questions/1266674/how-can-one-get-an-absolute-or-normalized-file-path-in-net
         * This should handle few scenarios like
         * uri and potential escaped characters in it, like
         * file:///C:/Test%20Project.exe -> C:\TEST PROJECT.EXE
         * path segments specified by dots to denote current or parent directory
         * c:\aaa\bbb\..\ccc -> C:\AAA\CCC
         * tilde shortened (long) paths
         * C:\Progra~1\ -> C:\PROGRAM FILES
         * inconsistent directory delimiter character
         * C:/Documents\abc.txt -> C:\DOCUMENTS\ABC.TXT
         * Other than those, it can ignore case, trailing \ directory delimiter character etc.
         * */
        public static string NormalizePath(string path, bool bFinalSlash = false)
        {
            try
            {
                if (path.StartsWith(".") || path.StartsWith("\\") || path.StartsWith("/"))
                {
                    if (path.StartsWith("/"))
                    {
                        path = Environment.CurrentDirectory + Path.PathSeparator + path.Substring(1);
                    }
                    else if (path.StartsWith("../") || path.StartsWith("..\\"))
                    {
                        path = Environment.CurrentDirectory + "/../" + path.Substring(3);
                    }
                    else if (path.StartsWith("./") || path.StartsWith(".\\"))
                    {
                        path = Environment.CurrentDirectory + Path.PathSeparator + path.Substring(2);
                    }
                }
                else if (!Path.IsPathRooted(path))
                {
                    path = Path.GetFullPath(path);
                }

                string norm;
                if (!bFinalSlash)
                {
                    norm = Path.GetFullPath(new Uri(path).LocalPath)
                           .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                           .ToLowerInvariant();
                }
                else
                {
                    norm = Path.GetFullPath(new Uri(path).LocalPath).ToLowerInvariant();
                }
                return norm;
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error("Unable to normalize " + path + ": " + ex.Message);
                throw ex;
            }            
        }

        public static string AddQuotesIfRequired(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            if (path.Contains(" "))
            {
                if (path.StartsWith("\""))
                    return path;
                return "\"" + path + "\"";
            }
            return path;            
        }

        public static string RemoveQuotes(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            path = path.Replace("\"", "");
            return path;
        }

        public static string ChangePathExtension(string inFilePath, string newExt)
        {
            string outFilePath = Path.ChangeExtension(inFilePath, newExt);
            return outFilePath;
        }

        public static byte[] GetFileMD5(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            byte[] hash = null;
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = md5.ComputeHash(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
            }
            return hash;
        }

        // http://stackoverflow.com/questions/1389570/c-sharp-byte-array-comparison
        public static bool AreMD5Equal(byte[] hash1, byte[] hash2)
        {
            if (hash1 == null || hash2 == null)
                return false;
            //return hash1.SequenceEqual(hash2);

            if (hash1 == hash2) 
                return true;
            if (hash1.Length != hash2.Length) 
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i]) 
                    return false;
            }
            return true;
        }

        public static string GetMD5String(byte[] md5)
        {
            return BitConverter.ToString(md5).Replace("-", "‌​").ToLower();
        }        

        public static string ByteToString(byte[] byteHash)
        {
            return BitConverter.ToString(byteHash).Replace("-", "");            
        }

        public static byte[] StringToByte(string strHash)
        {
            return Convert.FromBase64String(strHash);
        }
        
        /**
         * http://stackoverflow.com/questions/6309379/how-to-check-for-a-valid-base-64-encoded-string-in-c-sharp
         */ 
        public static bool IsBase64(this string base64String)
        {
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);                
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return false;
            }
            return true;
        }
        
        public static string StripHTMLTags(string source, List<string> excluded = null)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool bInside = false;
            string curTag = "";
            HTMLTag tagChecker = new HTMLTag();

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    if (i < source.Length - 1)
                    {
                        char letNext = source[i+1];
                        if (letNext != ' ')
                        {
                            curTag = "<";
                            bInside = true;
                            continue;
                        }
                    }
                }
                else if (let == '>' && bInside)
                {
                    curTag += ">";

                    if (!tagChecker.IsHTMLTag(curTag))
                    {
                        bInside = false;
                        continue;
                    }

                    Boolean bIsExcluded = excluded != null && (excluded.Contains(curTag) || excluded.Contains(curTag.ToUpper()));
                    if (bIsExcluded)
                    {
                        for (int j = 0; j < curTag.Length; j++)
                        {
                            array[arrayIndex] = curTag[j];
                            arrayIndex++;
                        }
                    }
                    
                    bInside = false;
                    continue;
                }
                if (!bInside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
                else
                    curTag += let;
            }
            
            string stripped = new string(array, 0, arrayIndex);
            stripped = stripped.Replace("&nbsp;", "");
            return stripped;
        }

        public static bool IsConnectionAvailable()
        {
            if (m_bConnectionAvailable != -1)
            {
                if (m_bConnectionAvailable == 0)
                    return false;
                return true;
            }
            try
            {
                Ping myPing = new Ping();
                String host = "8.8.8.8";
                byte[] buffer = new byte[32];
                int timeout = 1500;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                bool ret = (reply.Status == IPStatus.Success);

                m_bConnectionAvailable = 0;
                if (ret)
                    m_bConnectionAvailable = 1;
                return ret;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string EscapeHTMLTags(string source, List<string> excluded = null)
        {
            bool bInside = false;
            string curTag = "";
            HTMLTag tagChecker = new HTMLTag();
            string escaped = "";

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    if (i < source.Length - 1)
                    {
                        char letNext = source[i+1];
                        if (letNext != ' ')
                        {
                            curTag = "<";
                            bInside = true;
                            continue;
                        }
                    }
                }
                else if (let == '>' && bInside)
                {
                    curTag += ">";

                    if (!tagChecker.IsHTMLTag(curTag))
                    {
                        bInside = false;
                        continue;
                    }

                    Boolean bIsExcluded = excluded != null && (excluded.Contains(curTag) || excluded.Contains(curTag.ToUpper()));
                    if (!bIsExcluded)
                        escaped += tagChecker.GetEscapedTag(curTag);
                    else
                        escaped += curTag;
                    
                    bInside = false;
                    continue;
                }
                if (!bInside)
                    escaped += let;
                else
                    curTag += let;
            }            
            return escaped;
        }   

        /**
         * http://base64image.org/
         */
        public static string ImageToBase64String(string imagePath, ImageFormat format)
        {
            if (imagePath == null || imagePath.Length == 0)
                return "";

            try
            {
                Image imageIn = Image.FromFile(imagePath);
                if (imageIn == null)
                    return "";
                return ImageToBase64String(imageIn, format);
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return "";
            }
        }

        public static string ImageToBase64String(Image imageIn, ImageFormat format)
        {
            try
            {
                if (imageIn == null)
                    return "";

                using (MemoryStream ms = new MemoryStream())
                {
                    /* Convert this image back to a base64 string */
                    imageIn.Save(ms, format);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Globals.m_Logger.Error(ex.ToString());
                return "";
            }
        }

        public static Image ScaleImage(Image image, int maxSizePix, bool bPreserveExifRotation = true)
        {
            if (image == null)
                return null;
                        
            if (maxSizePix >= image.Width && maxSizePix >= image.Height)
                return image;// nothing to do

            if (bPreserveExifRotation)
                RotateImageByExifOrientationData(image);

            Bitmap scaled = null;
            double ratio = 1.0;
            int newWidth = 0;
            int newHeight = 0;
            if (image.Width > image.Height)
            {
                ratio = (double)maxSizePix / image.Width;
                newWidth = maxSizePix;
                newHeight = (int)(image.Height * ratio);
                scaled = new Bitmap(maxSizePix, newHeight);
            }
            else
            {
                ratio = (double)maxSizePix / image.Height;                
                newWidth = (int)(image.Width * ratio);
                newHeight = maxSizePix;
                scaled = new Bitmap(newWidth, maxSizePix);
            }

            scaled.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(scaled))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return scaled;
        }

        /// <summary>
        /// Rotate the given bitmap according to Exif Orientation data
        /// http://stackoverflow.com/questions/6222053/problem-reading-jpeg-metadata-orientation/38459903#38459903
        /// </summary>
        /// <param name="img">source image</param>
        /// <param name="updateExifData">set it to TRUE to update image Exif data after rotation (default is TRUE)</param>
        /// <returns>The RotateFlipType value corresponding to the applied rotation. If no rotation occurred, RotateFlipType.RotateNoneFlipNone will be returned.</returns>
        public static RotateFlipType RotateImageByExifOrientationData(Image img, bool updateExifData = false)
        {
            if (img == null)
                return RotateFlipType.RotateNoneFlipNone;

            int orientationId = 0x0112;
            var fType = RotateFlipType.RotateNoneFlipNone;
            if (img.PropertyIdList.Contains(orientationId))
            {
                var pItem = img.GetPropertyItem(orientationId);
                fType = GetRotateFlipTypeByExifOrientationData(pItem.Value[0]);
                if (fType != RotateFlipType.RotateNoneFlipNone)
                {
                    img.RotateFlip(fType);
                    // Remove Exif orientation tag (if requested)
                    if (updateExifData) img.RemovePropertyItem(orientationId);
                }
            }
            return fType;
        }

        /// <summary>
        /// Return the proper System.Drawing.RotateFlipType according to given orientation EXIF metadata
        /// </summary>
        /// <param name="orientation">Exif "Orientation"</param>
        /// <returns>the corresponding System.Drawing.RotateFlipType enum value</returns>
        private static RotateFlipType GetRotateFlipTypeByExifOrientationData(int orientation)
        {
            switch (orientation)
            {
                case 1:
                default:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
            }
        }

        /**
         * http://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
         */
        public static bool IsValidUrl(string url, bool bHttpsOnly = false)
        {
            if (url == null || url.Length == 0)
                return false;

            if (!url.StartsWith("http://") || !url.StartsWith("https://"))
                url = ("http://" + url);

            Uri uriResult;
            bool bResult = false;
            if (!bHttpsOnly)
                bResult = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            else
                bResult = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp);
            return bResult;
        }

        //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        public static string Sha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public static string GetHashSha256(string filePath)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] bytes = sha256Hash.ComputeHash(stream);

                    string result = "";
                    foreach (byte b in bytes) result += b.ToString("x2");
                    return result;
                }
            }
        }

        
    }
}
