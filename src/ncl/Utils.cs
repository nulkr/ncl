using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

//http://stackoverflow.com/questions/1142802/how-to-use-localization-in-c-sharp
namespace ncl
{
    public static class MsgBox
    {
        public static void Show(string text)
        {
            MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void Error(string text)
        {
            MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static void Warning(string text)
        {
            MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static bool Query(string text)
        {
            return MessageBox.Show(text, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }

    public static class Utils
    {
        # region Strings

        /// Unicode string to ANSI String
        /// http://msdn.microsoft.com/ko-kr/library/kdcak6ye(v=vs.110).aspx
        public static string UnicodeToAnsi(string sUnicode)
        {
            // Create two different encodings.
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte array.
            byte[] unicodeBytes = unicode.GetBytes(sUnicode);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);

            // Convert the new byte[] into a char[] and then into a string.
            char[] asciiChars = new char[Encoding.ASCII.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            
            return new string(asciiChars);
        }

        /// WIN API Pointer to string
        /// <param name="P">IntPtr</param>
        /// <returns></returns>
        public static string PtrToStr(IntPtr P)
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringAuto(P);
        }

        /// Bitmap Data to Base64
        public static string BitmapToStr(Bitmap bmp)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream()) // BMP -> Base64
            {
                bmp.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// Load Bitmap from Base64
        public static Bitmap StrToBitmap(string s)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(s)))
            {
                ms.Position = 0;
                return new Bitmap(ms);
            }
        }

        # endregion

        #region Struct & array

        /// <summary>
        /// Struct to Byte Array
        /// http://bytes.com/topic/c-sharp/answers/236808-how-convert-structure-byte-array
        /// </summary>
        public static byte[] StructToBytes(object obj)
        {
            int len = Marshal.SizeOf(obj);

            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);

            Marshal.Copy(ptr, arr, 0, len);

            Marshal.FreeHGlobal(ptr);

            return arr;

        }
        /// <summary>
        /// Byte Array to Struct
        /// http://bytes.com/topic/c-sharp/answers/236808-how-convert-structure-byte-array
        /// </summary>
        public static void BytesToStruct(byte[] bytearray, ref object obj)
        {

            int len = Marshal.SizeOf(obj);

            IntPtr i = Marshal.AllocHGlobal(len);

            Marshal.Copy(bytearray, 0, i, len);

            obj = Marshal.PtrToStructure(i, obj.GetType());

            Marshal.FreeHGlobal(i);

        }
        /// <summary>
        /// http://stackoverflow.com/questions/415291/best-way-to-combine-two-or-more-byte-arrays-in-c-sharp
        /// </summary>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static byte[] CombineArray(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        #endregion

        # region Directories

        /// sBaseDir 에서 nUpCount 만큼 상위 이동한 Directory 이름을 반환
        /// <param name="sBase"></param>
        /// <param name="nUpCount"></param>
        /// <returns></returns>
        public static string GetDir(string sBaseDir, int nUpCount)
        {
            int nStartIndex = sBaseDir.Length - 1;
            if (sBaseDir.ElementAt(nStartIndex) == '\\') nStartIndex--;

            for (int i = 0; i < nUpCount; i++)
                nStartIndex = sBaseDir.LastIndexOf('\\', nStartIndex) - 1;

            return sBaseDir.Remove(nStartIndex + 2);
        }
        
        /// 상대 경로로 표현된 디렉토리를 AppDir를 기반으로하는 풀 디렉토리명으로 만들어 반환
        /// <param name="sRelativeDirName"></param>
        /// <returns></returns>
        public static string GetDir(string sRelativeDirName)
        {
            if (sRelativeDirName.IndexOf("..") == 0)
            {
                // http://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
                int nCnt = Regex.Matches(sRelativeDirName, "..").Count;

                return Utils.GetDir(App.Path, nCnt);
            }
            else
            {
                if (sRelativeDirName[0] == '.' || sRelativeDirName[0] == '\\')
                    return App.Path + sRelativeDirName.Substring(1);
                else
                    return App.Path + sRelativeDirName;
            }  
        }
        
        /// 상대 경로로 표현된 파일 이름의 풀 패쓰명을 반환
        /// <param name="sRelativeFileName"></param>
        /// <returns></returns>
        public static string GetFile(string sRelativeFileName)
        {
            if (sRelativeFileName.IndexOf("..\\") == 0)
            {
                // http://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-within-a-string
                int nCnt = Regex.Matches(sRelativeFileName, Regex.Escape("..\\")).Count;

                return Utils.GetDir(App.Path, nCnt) + sRelativeFileName.Substring(nCnt * 3);
            }
            else
            {
                if (sRelativeFileName[0] == '.' || sRelativeFileName[0] == '\\')
                    return App.Path + sRelativeFileName.Substring(1);
                else
                    return App.Path + sRelativeFileName;
            }
        }

        /// 상대 경로로 표현된 파일 이름의 FileInfo를 반환
        /// <param name="sRelativeFileName"></param>
        /// <returns></returns>
        public static System.IO.FileInfo GetFileInfo(string sRelativeFileName)
        {
            return new System.IO.FileInfo(GetFile(sRelativeFileName));
        }

        #endregion

        #region Math

        public static int Round(double value)
        {
            return (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        public static double ToRadian(double angleDeg)
        {
            return Math.PI / 180.0 * angleDeg;
        }
        public static double ToDegree(double angleDeg)
        {
            return 180.0 / Math.PI * angleDeg;
        }

        public static bool InRange(int value, int min, int max)
        {
            return (value >= min && value <= max);
        }
        public static bool InRange(double value, double min, double max)
        {
            return (value >= min && value <= max);
        }
        public static int EnsureRange(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public static double EnsureRange(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static bool GetBit32(UInt32 value, int bitIndex)
        {
            return (value & (1 << bitIndex)) != 0;
        }
        public static void SetBit32(ref UInt32 value, int bitIndex, bool state)
        {
            if (state)
                value |= ((UInt32)1 << bitIndex);
            else
                value &= ((UInt32)1 << bitIndex) ^ 0xFFFFFFFF;
        }
        public static void ToggleBit32(ref UInt32 value, int bitIndex)
        {
            value ^= (UInt32)1 << bitIndex;
        }
        public static bool GetBit32(Int32 value, int bitIndex)
        {
            return GetBit32((UInt32)value, bitIndex);
        }
        public static void SetBit32(ref Int32 value, int bitIndex, bool state)
        {
            SetBit32(ref value, bitIndex, state);
        }
        public static void ToggleBit32(ref Int32 value, int bitIndex)
        {
            ToggleBit32(ref value, bitIndex);
        }

        #endregion

        #region Bitmap

        /// Alpha Blending Drawing
        /// http://www.codeproject.com/Articles/5034/How-to-implement-Alpha-blending
        /// https://msdn.microsoft.com/ko-kr/library/system.drawing.imaging.colormatrix(v=vs.110).aspx
        /// <param name="aGraphics">Dest Graphics</param>
        /// <param name="aImage">Source Image</param>
        /// <param name="alpha">Alpha Blending Value (0..1)</param>
        public static void DrawAlpha(Graphics aDstGraphics, Image aSrcImage, int aDstX, int aDstY, float alpha)
        {
            ColorMatrix cm = new ColorMatrix(new float[][] {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            });
            cm.Matrix33 = alpha;

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);

            aDstGraphics.DrawImage(aSrcImage, new Rectangle(aDstX, aDstY, aSrcImage.Width, aSrcImage.Height), 0, 0, aSrcImage.Width, aSrcImage.Height, GraphicsUnit.Pixel, ia);
        }
        public static void DrawAlpha(Image aDstImage, Image aSrcImage, int aDstX, int aDstY, float alpha)
        {
            using (Graphics g = Graphics.FromImage(aDstImage))
            {
                Utils.DrawAlpha(g, aSrcImage, aDstX, aDstY, alpha);
            }
        }

        #endregion

        #region Geometry
        
        /// 두점을 대각으로 가지는 Rectangle 생성
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Rectangle CreateRect_From2Point(int x1, int y1, int x2, int y2)
        {
            int x = Math.Min(x1, x2);
            int y = Math.Min(y1, y2);

            return new Rectangle(x, y, Math.Abs(x1 - x2 + 1), Math.Abs(y1 - y2+ 1));
        }
        public static Rectangle CreateRect_From2Point(Point p1, Point p2)
        {
            return CreateRect_From2Point(p1.X, p1.Y, p2.X, p2.Y);
        }

        #endregion

        #region classes

        public static object LoadObjectFromJson(Type type, string filename)
        {
            // http://stackoverflow.com/questions/1076730/datacontractserializer-doesnt-call-my-constructor
            // It creates the object as empty memory
            using (var fs = new FileStream(filename, FileMode.Open))
                return (new DataContractJsonSerializer(type)).ReadObject(fs);
        }
        public static void SaveObjectToJson(object o, string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
                (new DataContractJsonSerializer(o.GetType())).WriteObject(fs, o);
        }
        #endregion
    }

  
}
