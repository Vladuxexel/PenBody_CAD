using Kompas6API5;
using System;

namespace PenBody_Cad
{
    /// <summary>
    /// Класс управления подключением к Компас 3d.
    /// </summary>
    public class CadConnector
    {
        /// <summary>
        /// Объект када.
        /// </summary>
        public KompasObject Kompas { get; set; }

        /// <summary>
        /// Запуск Компас 3d и создание
        /// </summary>
        /// <returns></returns>
        public void Connect()
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
        }
    }
}
