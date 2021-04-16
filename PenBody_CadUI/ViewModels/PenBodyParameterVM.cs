using GalaSoft.MvvmLight;
using PenBody_Cad.Enums;

namespace PenBody_CadUI.ViewModels
{
    /// <summary>
    /// Вью-модель параметра.
    /// </summary>
    public class PenBodyParameterVM : ViewModelBase
    {
        /// <summary>
        /// Название параметра.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значене параметра.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Имя параметра в соответствии с моделью.
        /// </summary>
        public ParamName ParamName { get; set; }

        /// <summary>
        /// Корректность параметра.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="name">Название параметра.</param>
        /// <param name="paramName">Имя параметра в соответствии с моделью.</param>
        public PenBodyParameterVM(string name, ParamName paramName)
        {
            Name = name;
            ParamName = paramName;
            IsValid = true;
        }
    }
}
