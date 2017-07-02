using System;
using Xunit;
using c2eLib.Caos;

namespace test
{
    public class SharedMemoryInjectorTest : CaosInjectorTest
    {
        public override ICaosInjector CaosInjector()
        {
            return new SharedMemoryInjector(new BufferLayout(null),null);
        }           
    }
}