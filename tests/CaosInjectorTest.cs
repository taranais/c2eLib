using System;
using Xunit;

namespace test
{
    public abstarct class CaosInjectorTest
    {
        /// <summary>
        /// Override this method to implement the tests
        /// </summary>
        /// <returns></returns>
        public abstract ICaosInjector CaosInjectorTest();
 
    

        [TestMethod]
        public void Init_Test()
        {
            var injector = CaosInjectorTest();
            if(injector.Start()){
                Assert.AreEqual(true, true);
            }
            else{
                Assert.AreEqual(false, true);
            }
            injector.Stop();
        }

        [TestMethod]
        public void Stop_Test()
        {
            var injector = CaosInjectorTest();
            injector.Start();
            if(injector.Stop()){
                Assert.AreEqual(true, true);
            }
            else{
                Assert.AreEqual(false, true);
            }
        }

        [TestMethod]
        public void SendCaos_Test()
        {
            var injector = CaosInjectorTest();
            injector.Start();
            var resultado = injcetor.SendCaos("outv 99","execute");
            injector.Stop();
        }
    }
}
