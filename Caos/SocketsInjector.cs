using System;
using Microsoft.Extensions.Logging;
using c2eLib;

namespace c2eLib.Caos
{
    ///     Linux  ¿ and mac ? version
    ///     TODO
    public class SocketsInjector : ICaosInjector
    {
        private readonly ILogger _logger;
        
        /// <summary>
        ///  gets Game's name
        /// </summary>
        /// <returns></returns>
        public string Game {get; private set;}

        public SocketsInjector(ILogger<SocketsInjector> logger = null) 
        {
            _logger = logger;
         }

        /// <inheritdoc/>
        public bool Init(string game){
            Game = game;
            _logger?.LogTrace(LoggingEvents.C2E_OPEN_CONNECTION, "Injector {0} Init", Game);
            return false;
        }

        /// <inheritdoc/>
        public bool Stop(){
            _logger?.LogTrace(LoggingEvents.C2E_CLOSE_CONNECTION, "Injector {0} Stop",Game);
            return true;
        }

        /// <inheritdoc/>
        public CaosResult SendCaos(string CaosAsString, string Action){
            _logger?.LogTrace(LoggingEvents.C2E_SEND_COMMAND, "{1} Caos for {0}",Game, Action);
            return null;
        }
    }
}