using System;
using System.Text;
using Xunit;
using c2eLib.Caos;

namespace test
{
    public abstract class CaosInjectorTest
    {
        /// <summary>
        /// Override this method to implement the tests
        /// </summary>
        /// <returns></returns>
        public abstract ICaosInjector CaosInjector();
 
    

        [Fact]
        public void Init_Test()
        {
            var injector = CaosInjector();
            if(injector.Init("Docking Station")){
                Assert.True(true);
            }
            else{
                Assert.True(false);
            }
            injector.Stop();
        }

        [Fact]
        public void Stop_Test()
        {
            var injector = CaosInjector();
            injector.Init("Docking Station");
            if(injector.Stop()){
                Assert.True(true);
            }
            else{
                Assert.True(false);
            }
        }

        [Fact]
        public void SendCaos_Test()
        {
            Encoding iso =  Encoding.ASCII;// GetEncoding("ISO-8859-1");
            Encoding utf = Encoding.Unicode;
            string value ="99";
            byte[] utfBytes = utf.GetBytes(value);
            byte[] isoBytes = Encoding.Convert(utf, iso, utfBytes);
            string msgIso = iso.GetString(isoBytes);

            var injector = CaosInjector();
            injector.Init("Docking Station");
            
            var resultado = injector.SendCaos("outv 99","execute");
            string caosString = iso.GetString(resultado.Content);
            string caosTrimed = caosString.Trim('\0');
            Assert.Equal(msgIso, caosTrimed );
            
            injector.Stop();
        }
    }
}
