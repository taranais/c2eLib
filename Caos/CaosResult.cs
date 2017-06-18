using System;

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
    }
}