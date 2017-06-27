using System;
using Xunit;

namespace test
{
    [TestClass]
    public class SharedMemoryInjectorTest : CaosInjectorTest
    {
        public override ICaosInjector CaosInjectorTest()
        {
            return new SharedMemoryInjector(new BufferLayout(null),null);
        }           
    }
}