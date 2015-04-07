using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nse.Entities
{
    public class DerivativeDataAccess
    {
        public void Persist(string derivativeName)
        {
            var derivative = new DerivativeType { DerviativeType = derivativeName };
            var context = new NseContext();
            context.DerivativeTypes.Add(derivative);
            context.SaveChanges();

        }
    }
}
