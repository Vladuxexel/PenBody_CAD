using Kompas6API5;
using System;
using System.Runtime.InteropServices;

namespace PenBody_Cad
{
    /// <summary>
    /// Класс управления подключением к КОМПАС-3D.
    /// </summary>
    public class CadConnector
    {
        /// <summary>
        /// Название точки входа в api5.
        /// </summary>
        private const string Api5Name = "KOMPAS.Application.5";

        /// <summary>
        /// Объект када.
        /// </summary>
        public KompasObject Kompas { get; set; }

        /// <summary>
        /// Запуск КОМПАС-3D.
        /// </summary>
        public void Connect()
        {
            var recievingResult = GetActiveKompas(out var kompas);
            if (!recievingResult)
            {
                var creationResult = CreateCompasInstance(out kompas);
                if (!creationResult)
                {
                    throw new ArgumentException("Не удалось создать новый экземпляр КОМПАС-3D.");
                }
            }
            kompas.Visible = true;
            kompas.ActivateControllerAPI();
            Kompas = kompas;
        }

        /// <summary>
        /// Подключение к активному экземпляру КОМПАС-3D.
        /// </summary>
        /// <param name="kompas">Ссылка на экземпляр КОМПАС-3D.</param>
        /// <returns>Результат подключения.</returns>
        private bool GetActiveKompas(out KompasObject kompas)
        {
            kompas = null;
            try
            {
                kompas = (KompasObject)Marshal.GetActiveObject(Api5Name);
                return true;
            }
            catch (COMException)
            {
                return false;
            }
        }

        /// <summary>
        /// Создание нового экземпляра КОМПАС-3D.
        /// </summary>
        /// <param name="kompas">Ссылка на экземпляр КОМПАС-3D.</param>
        /// <returns>Результат успешности создания.</returns>
        private bool CreateCompasInstance(out KompasObject kompas)
        {
            try
            {
                var type = Type.GetTypeFromProgID(Api5Name);
                kompas = (KompasObject)Activator.CreateInstance(type);
                return true;
            }
            catch (COMException)
            {
                kompas = null;
                return false;
            }
        }
    }
}
