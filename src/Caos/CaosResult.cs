using System;
using System.IO;
using System.Text;

namespace c2eLib.Caos
{

    /// <summary>
    ///
    /// </summary>
    public class CaosResult
    {
        public bool     Failed                  { get; private set; }
        public int      ProcessID               { get; private set; }
        public byte[]   Content                 { get; private set; }

        public CaosResult(int failed,  byte[] content, int processID)
        {
            Failed      = Convert.ToBoolean(failed);
            Content     = content;
            ProcessID   = processID;
        }
         public string ContentAsString(){
            StringBuilder sb = new StringBuilder();
            using(StreamReader rdr =  new StreamReader(new MemoryStream(Content), Encoding.ASCII)){
                Int32 nc;
                while((nc = rdr.Read()) != -1) {
                    Char c = (Char)nc;
                    if( c != '\0' ) sb.Append( c );
                }
            }
            return sb.ToString();
        }
    }
}