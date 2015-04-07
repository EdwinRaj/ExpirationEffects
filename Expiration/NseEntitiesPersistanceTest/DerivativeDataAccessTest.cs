using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NseEntities;
using NUnit.Framework;

namespace NseEntitiesPersistanceTest
{
    [TestFixture]
    public class DerivativeDataAccessTest
    {
        private volatile Type _dependency;

        public DerivativeDataAccessTest()
        {
            _dependency = typeof (System.Data.Entity.SqlServer.SqlProviderServices);
        }

        [Test]
        public void TestMultiThreading()
        {
            var dataAccess = new DerivativeDataAccess();
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                ParameterizedThreadStart threadStart = delegate(object o) { dataAccess.Persist((string)o); };
                threads[i] = new Thread(threadStart);
            }

            for (int i = 0; i < 10; i++)
            {
                threads[i].Start("Derivative=" + i);
            }


            Thread.CurrentThread.Join();

            
        }
    }
}
