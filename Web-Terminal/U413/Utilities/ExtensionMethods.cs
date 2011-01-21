using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebTerminal.Models;
using System.Text.RegularExpressions;

namespace WebTerminal.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsShort(this string arg)
        {
            try
            {
                Convert.ToInt16(arg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInt(this string arg)
        {
            try
            {
                Convert.ToInt32(arg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsLong(this string arg)
        {
            try
            {
                Convert.ToInt64(arg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static short ToShort(this string arg)
        {
            return Convert.ToInt16(arg);
        }

        public static int ToInt(this string arg)
        {
            return Convert.ToInt32(arg);
        }

        public static long ToLong(this string arg)
        {
            return Convert.ToInt64(arg);
        }
    }
}