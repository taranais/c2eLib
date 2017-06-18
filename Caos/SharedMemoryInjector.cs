using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using Microsoft.Extensions.Logging;
using c2eLib;

namespace c2eLib.Caos
{
    /// <summary>
    ///
    /// </summary>
    public class SharedMemoryInjector : ICaosInjector
    {
        private readonly ILogger _logger;
        private Mutex mutex;
        private MemoryMappedFile memfile;
        private MemoryMappedViewAccessor memViewAccessor;
        private EventWaitHandle resultEventHandle;
        private EventWaitHandle requestEventHandle;
        private BufferLayout _caosBuffer;
        private bool _started;

        /// <summary>
        ///  gets Game's name
        /// </summary>
        /// <returns></returns>
        private string Game {get; set;}

        /// <summary>
        /// Create CaosInjector for especified engine game
        /// </summary>
        
        public SharedMemoryInjector(BufferLayout caosBuffer, ILogger<SharedMemoryInjector> logger = null )  
        {
            _caosBuffer = caosBuffer;
            _logger = logger;
            _started = false; 
        }

        /// <inheritdoc/>
        public bool Init(string game){
            if(game.Equals(String.Empty))
            {
                // TODO check string game not empty
                // throw Exception
            }
            Game = game;
            try
            {
                _logger?.LogTrace(LoggingEvents.C2E_OPEN_CONNECTION, "Injector {0} Init", Game);
                mutex = Mutex.OpenExisting(Game + "_mutex");
                memfile = MemoryMappedFile.OpenExisting(Game + "_mem");
                memViewAccessor = memfile.CreateViewAccessor();
                resultEventHandle = EventWaitHandle.OpenExisting(Game + "_result");
                requestEventHandle = EventWaitHandle.OpenExisting(Game + "_request");
                _started = true;
            }
            catch(System.Threading.WaitHandleCannotBeOpenedException ex)
            {
                _logger?.LogWarning(LoggingEvents.C2E_OPEN_CONNECTION, ex , "Shared Memory not created for: {0}",Game);
                _started = false;
            }
            finally
            {
                _logger?.LogTrace(LoggingEvents.C2E_OPEN_CONNECTION, "Injector {0} Init : {1}", Game, _started);
            }
            return _started;
        }

        /// <inheritdoc/>
        public bool Stop(){
            if(!_started)
            {
                // TODO check if started 
                // throw Exception
            }
            bool succes= true;
            try{
                _logger?.LogTrace(LoggingEvents.C2E_CLOSE_CONNECTION, "Injector {0} Stop",Game);
                requestEventHandle.Dispose();
                resultEventHandle.Dispose();
                memViewAccessor.Dispose();
                memfile.Dispose();
                mutex.Dispose();
            }
            catch (System.NullReferenceException ex)
            {
                _logger?.LogWarning(LoggingEvents.C2E_CLOSE_CONNECTION, ex , "Shared Memory fail on Stop: {0}",Game);
                succes = false;
            }
            finally
            {
              _logger?.LogInformation(LoggingEvents.C2E_CLOSE_CONNECTION, "Injector {0} Stop : {1}", Game, succes);
            }
            return succes;
        }

        /// <inheritdoc/>
        public CaosResult SendCaos(string CaosAsString, string Action){
                if(!_started)
                {
                    // TODO check if started 
                    // throw Exception
                }
                _logger?.LogTrace(LoggingEvents.C2E_SEND_COMMAND, "{1} Caos for {0}",Game, Action);
                CaosResult caosResult = null;
                mutex.WaitOne(5000);
                _caosBuffer.PrepareBufferLayout(CaosAsString, Action);
                _caosBuffer.SetSharedMemory(memViewAccessor);
                requestEventHandle.Set();
                resultEventHandle.WaitOne(5000);
                _caosBuffer.GetSharedMemory(memViewAccessor);
                mutex.ReleaseMutex();
                caosResult = _caosBuffer.GetCaosResult();
                _logger?.LogInformation(LoggingEvents.C2E_SEND_COMMAND, "Caos result fail : {0} Content: {1} ",
                            caosResult.Failed,
                            System.Text.Encoding.ASCII.GetString(caosResult.Content));
                return caosResult;
        }
    }
}   