using System;
using System.Text;

namespace c2eLib
{
    public static class Utils{

        public static string utf16ToAscii(string str){
            Encoding iso = Encoding.ASCII;//.GetEncoding("ISO-8859-1");
            Encoding utf = Encoding.Unicode;
            byte[] utfBytes = utf.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf, iso, utfBytes);
            string msg = iso.GetString(isoBytes);
            return msg;            
        }

        public static string asciiToUtf16(string str){
            Encoding iso = Encoding.ASCII;//.GetEncoding("ISO-8859-1");
            Encoding utf = Encoding.Unicode;
            byte[] isoBytes = iso.GetBytes(str);
            byte[] utfBytes = Encoding.Convert( iso, utf, isoBytes);
            string msg = utf.GetString(utfBytes);
            return msg;            
        }
    }

}