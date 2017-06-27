using System;
using System.Text;

namespace c2eLib
{
    public static class Utils{

        public static string utf16ToLatin1(string str){
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf = Encoding.Unicode;
            byte[] utfBytes = utf.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf, iso, utfBytes);
            string msg = iso.GetString(isoBytes);
            return msg;            
        }

        public static string latin1ToUtf16(string str){
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf = Encoding.Unicode;
            byte[] isoBytes = iso.GetBytes(str);
            byte[] utfBytes = Encoding.Convert( iso, utf, isoBytes);
            string msg = utf.GetString(utfBytes);
            return msg;            
        }
    }

}