using Kompas6API5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_Cad
{
    public class PenBodyBuilder
    {
        public void Build(PenBody penBody)
        {
            var cadConnector = new CadConnector();
            var document = cadConnector.Connect();
            document.Create(false, true);
            document = (ksDocument3D)cadConnector.Kompas.ActiveDocument3D();
        }
    }
}
