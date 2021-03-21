using Kompas6API5;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PenBody_Cad
{
    public class CadConnector
    {
        /// <summary>
        /// Объект када
        /// </summary>
        public KompasObject Kompas { get; set; }

        public ksDocument3D Connect()
        {
            if (Kompas == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                Kompas = (KompasObject)Activator.CreateInstance(type);
            }
            
            if (Kompas != null)
            {
                Kompas.Visible = true;
                Kompas.ActivateControllerAPI();
            }

            return (ksDocument3D)Kompas.Document3D();
        }
    }
}
