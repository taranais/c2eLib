using System;
using Microsoft.Extensions.Logging;

namespace c2eLib.Caos
{
    /// TODO async


    /// <summary>
    ///
    /// </summary>
    public interface ICaosInjector
    {
        /// <summary>
        /// Configure and Starts CaosInjector
        /// </summary>
        /// <returns></returns>
        /// <param name="game">especified engine's game name</param>
        bool Init(string game);

        /// <summary>
        /// Stops CaosInjector
        /// </summary>
        /// <returns></returns>
        bool Stop();

        /// <summary>
        /// Send Caos over injector
        /// </summary>
        /// <param name="CaosAsString"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        CaosResult SendCaos(string CaosAsString, string Action);
    }
}