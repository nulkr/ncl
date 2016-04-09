using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ncl;

namespace ncl
{
    namespace Equipment
    {
        public class PmacVar
        {
            private Pmac _PmacRef;
            private Recipe _RecipeRef;

            public PmacVar(Pmac oPmac, Recipe oRecipe)
            {
                _PmacRef = oPmac;
                _RecipeRef = oRecipe;
            }

            public void Write(string sKey, double value)
            {
                SendToPmac(sKey + "={0:F5}", value);
            }
            public void Write(string sKey, int value)
            {
                SendToPmac(sKey + "={0:D}", value);
            }
            public void Write(string sKey, bool value)
            {
                SendToPmac(sKey + "={0:D}", value ? 1 : 0);
            }

            public double Read(string sKey)
            {
                string s = SendToPmac(sKey);
                return Convert.ToDouble(s);
            }
            public int ReadInt(string sKey)
            {
                string s = SendToPmac(sKey);
                return Convert.ToInt32(s);
            }
            public bool ReadBool(string sKey)
            {
                string s = SendToPmac(sKey);
                return (Convert.ToInt32(s) > 0 ? true : false);
            }

            private string SendToPmac(string sName, params object[] args)
            {
                return _PmacRef.Send(_RecipeRef[sName].Text, args);
            }
            private string SendToPmac(string sName)
            {
                return _PmacRef.Send(_RecipeRef[sName].Text);
            }
        }
    }
}