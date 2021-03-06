﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ncl
{
    public class IniFile
    {
        #region property

        public string FileName { get; set; }

        public int MaxBufferSize { get; set; }

        #endregion property

        #region constructor

        public IniFile(string iniFileName, int maxBufferSize = 255)
        {
            FileName = iniFileName;
            MaxBufferSize = maxBufferSize;
        }

        #endregion constructor

        #region API

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string file);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string file);

        #endregion API

        #region writeing

        public void Write(string section, string key, string sValue)
        {
            WritePrivateProfileString(section, key, sValue, FileName);
        }
        public void Write(string section, string key, double dValue)
        {
            WritePrivateProfileString(section, key, dValue.ToString("F5"), FileName);
        }
        public void Write(string section, string key, Color color)
        {
            WritePrivateProfileString(section, key, ColorTranslator.ToHtml(color), FileName);
        }
        public void Write(string section, string key, TimeSpan defaultValue)
        {
            Write(section, key, defaultValue.Ticks);
        }
        public void Write<T>(string section, string key, T e)
        {
            WritePrivateProfileString(section, key, e.ToString(), FileName);
        }

        public void Write(string section, object o)
        {
            IList<PropertyInfo> props = new List<PropertyInfo>(o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty));

            foreach (PropertyInfo prop in props)
                if (prop.CanRead && prop.CanWrite && prop.GetCustomAttributes(true).Contains(BrowsableAttribute.Yes))
                {
                    Write(section, prop.Name, prop.GetValue(o, null).ToString());
                }
        }

        #endregion writeing

        #region reading

        public string Read(string section, string key, string sDefault)
        {
            StringBuilder sb = new StringBuilder(MaxBufferSize);
            if (GetPrivateProfileString(section, key, sDefault, sb, MaxBufferSize, FileName) > 0)
                return sb.ToString();
            else
                return sDefault;
        }
        public int Read(string section, string key, int defaultValue)
        {
            return Convert.ToInt32(Read(section, key, defaultValue.ToString()));
        }
        public TimeSpan Read(string section, string key, TimeSpan defaultValue)
        {
            return TimeSpan.FromTicks( Read(section, key, defaultValue.Ticks) );
        }
        public DateTime Read(string section, string key, DateTime defaultValue)
        {
            return Convert.ToDateTime(Read(section, key, defaultValue.ToString()));
        }
        public long Read(string section, string key, long defaultValue)
        {
            return Convert.ToInt64(Read(section, key, defaultValue.ToString()));
        }
        public double Read(string section, string key, double defaultValue)
        {
            return Convert.ToDouble(Read(section, key, defaultValue.ToString("F5")));
        }
        public bool Read(string section, string key, bool defaultValue)
        {
            return Convert.ToBoolean(Read(section, key, defaultValue.ToString()));
        }
        public Color Read(string section, string key, Color defaultValue)
        {
            return ColorTranslator.FromHtml(Read(section, key, ColorTranslator.ToHtml(defaultValue)));
        }
        public T Read<T>(string section, string key, Enum defaultValue)
        {
            return (T)Enum.Parse(typeof(T), Read(section, key, defaultValue.ToString()));
        }

        public void Read(string section, object o)
        {
            IList<PropertyInfo> props = new List<PropertyInfo>(o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty));

            foreach (PropertyInfo prop in props)
                if (prop.CanRead && prop.CanWrite && prop.GetCustomAttributes(true).Contains(BrowsableAttribute.Yes))
                {
                    Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    StringBuilder sb = new StringBuilder(MaxBufferSize);
                    if (GetPrivateProfileString(section, prop.Name, "", sb, MaxBufferSize, FileName) > 0)
                    {
                        if (t.IsEnum)
                        {
                            prop.SetValue(o, Enum.Parse(t, sb.ToString()), null);
                        }
                        else
                        {
                            object safeValue = (sb.ToString() == null) ? null : Convert.ChangeType(sb.ToString(), t);

                            prop.SetValue(o, safeValue, null);
                        }
                    }
                }
        }

        #endregion reading
    }
}