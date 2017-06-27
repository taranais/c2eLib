using System;
using System.Text;
using System.IO.MemoryMappedFiles;
using Microsoft.Extensions.Logging;


namespace c2eLib.Caos
{
    /// <summary>
    /// SharedMemory data srtucture
    /// </summary>
    public class BufferLayout{
        private readonly ILogger _logger;

        public string   c2e                     { get; private set; }
        public int      ProcessID               { get; private set; }
        public int      ResultCode              { get; private set; }
        public uint     Size                    { get; private set; }
        public byte[]   Data                    { get; private set; }

        public uint     SharedMemoryBufferSize  { get; private set; }

        public BufferLayout(ILogger<BufferLayout> logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets control chars
        /// </summary>
        /// <param name="MemViewAccessor"></param>
        /// <returns></returns>
        private string GetControlArray(MemoryMappedViewAccessor MemViewAccessor)
        {
            string ControlCharSet = string.Empty;
            byte[] byteArrayControl = new byte[4];
            for (int i = 0; i<4 ;i++){
                byteArrayControl[i] = MemViewAccessor.ReadByte(i);
            }
            ControlCharSet = Encoding.ASCII.GetString(byteArrayControl);
            return ControlCharSet;
        }

        /// <summary>
        /// Prepare srtucture to write on SharedMemory
        /// </summary>
        /// <param name="CaosAsString"></param>
        /// <param name="Action"></param>
        public void PrepareBufferLayout(string CaosAsString, string Action) {
            _logger?.LogTrace("Prepare buffer layout");
            Data= GeneratePlayload(CaosAsString,Action);
            Size = Convert.ToUInt32(Data.Length);
        }

        /// <summary>
        /// Generate byte array to write on SharedMemory
        /// </summary>
        /// <param name="CaosAsString"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        private byte[] GeneratePlayload(string CaosAsString, string Action){
            byte[] action = Encoding.ASCII.GetBytes(Action);
            byte[] caos = Encoding.ASCII.GetBytes(CaosAsString);
            byte[] rv = new byte[action.Length + caos.Length + 2];
            System.Buffer.BlockCopy(action, 0, rv, 0, action.Length);
            System.Buffer.SetByte(rv, action.Length, 13);
            System.Buffer.BlockCopy(caos, 0 , rv, action.Length + 1, caos.Length);
            System.Buffer.SetByte(rv, rv.Length -1, 0);
            return rv;
        }

        /// <summary>
        /// Write on SharedMemory
        /// </summary>
        /// <param name="MemViewAccessor"></param>
        public void SetSharedMemory(MemoryMappedViewAccessor MemViewAccessor) {
            _logger?.LogTrace("Write Shared Memory");
            MemViewAccessor.Write(12, Size);
            for (int i = 0; i < Size; i++)
            {
                MemViewAccessor.Write(24 + i,Data[i]);
            }
        }

        /// <summary>
        /// Read SharedMemory
        /// </summary>
        /// <param name="MemViewAccessor"></param>
        public void GetSharedMemory(MemoryMappedViewAccessor MemViewAccessor)
        {
            _logger?.LogTrace("Read Shared Memory");
            c2e                     = GetControlArray(MemViewAccessor);
            ProcessID               = MemViewAccessor.ReadInt16(4);
            ResultCode              = MemViewAccessor.ReadInt16(8);
            Size                    = MemViewAccessor.ReadUInt16(12);
            SharedMemoryBufferSize  = MemViewAccessor.ReadUInt16(16);
            Data = new byte[Size];
            for (int i = 0; i < Size; i++)
            {
                Data[i] = MemViewAccessor.ReadByte(24 + i);
            }
        }
        /// <summary>
        /// get control string
        /// </summary>
        /// <returns></returns>
        private static string stringCode(){
            string tagCode = "c2e@";
            // return stringToASCII(tagCode);
            return c2eLib.Utils.utf16ToLatin1(tagCode);
        }

        /// <summary>
        /// Converts to ASCII
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string stringToASCII(string code){
            byte[] utf = System.Text.Encoding.ASCII.GetBytes(code);
            return System.Text.Encoding.ASCII.GetString(utf);
        }


        /// <summary>
        /// Get CaosResult from readed data
        /// </summary>
        /// <returns></returns>
        public CaosResult GetCaosResult()
        {
            if (!c2e.Equals(stringCode()))
            {
                // TODO own exception
                Exception exc = new InvalidOperationException(" either the buffer is corrupt or you're looking in the wrong place");
                _logger?.LogTrace("Buffer corrupt: {0}",c2e);
                throw exc;
            }
            else{
                _logger?.LogTrace("Buffer tagged: {0}",c2e);
            }
            return new CaosResult(ResultCode, Data, ProcessID);
        }
    }
}