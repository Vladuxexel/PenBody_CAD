using GalaSoft.MvvmLight;
using PenBody_Cad;
using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PenBody_CadUI.ViewModels
{
    public class PenBodyParametersListVM : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Регулярное выражения для числа типа double.
        /// </summary>
        private const string _doubleRegex = "^[0-9]*[.,]?[0-9]+$";

        /// <summary>
        /// Поле с моделью.
        /// </summary>
        private PenBodyParametersList _penBodyParametersList;

        /// <summary>
        /// Свойство для вывода ошибки.
        /// </summary>
        public string Error { get; private set; }

        public PenBodyParametersList PenBodyParametersList
        {
            get => _penBodyParametersList;
            set
            {
                _penBodyParametersList = value;
                RaisePropertyChanged(nameof(PenBodyParametersList));
            }
        }

        /// <summary>
        /// Свойство длины основной части ручки.
        /// </summary>
        public string MainLength
        {
            get => GetProperty(nameof(MainLength)).Value;
            set
            {
                GetProperty(nameof(MainLength)).Value = DotToComma(value);
                //UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public string RubberLength
        {
            get => GetProperty(nameof(RubberLength)).Value;
            set
            {
                GetProperty(nameof(RubberLength)).Value = DotToComma(value);
                //UpdateAll();
            }
        }

        /// <summary>
        /// Свойство диаметра ручки.
        /// </summary>
        public string MainDiameter
        {
            get => GetProperty(nameof(MainDiameter)).Value;
            set
            {
                GetProperty(nameof(MainDiameter)).Value = DotToComma(value);
                //UpdateAll();
            }
        }

        /// <summary>
        /// Свойство внутреннего диаметра ручки.
        /// </summary>
        public string InnerDiameter
        {
            get => GetProperty(nameof(InnerDiameter)).Value;
            set
            {
                GetProperty(nameof(InnerDiameter)).Value = DotToComma(value);
                //UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public string RubberDiameter
        {
            get => GetProperty(nameof(RubberDiameter)).Value;
            set
            {
                GetProperty(nameof(RubberDiameter)).Value = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public PenBodyParametersListVM()
        {
            SetToDefault();
        }

        private List<PenBodyParameterVM> _propertyMap = new List<PenBodyParameterVM>()
        {
            new PenBodyParameterVM("MainLength", "40", ParamName.MainLength),
            new PenBodyParameterVM("RubberLength", "20", ParamName.RubberLength),
            new PenBodyParameterVM("MainDiameter", "15", ParamName.MainDiameter),
            new PenBodyParameterVM("RubberDiameter", "10", ParamName.RubberDiameter),
            new PenBodyParameterVM("InnerDiameter", "5", ParamName.InnerDiameter)
        };

        /// <summary>
        /// Реализация интерфейса IDataErrorInfo.
        /// </summary>
        /// <param name="propertyName">Имя валидируемого свойства.</param>
        /// <returns>Текст ошибки.</returns>
        public string this[string propertyName]
        {
            get
            {
                string error = null;

                var property = GetProperty(propertyName);

                if (property.Value.Trim() == "")
                {
                    return "Необходимо ввести число";
                }
                else if (!Regex.IsMatch(property.Value, _doubleRegex))
                {
                    return "Неверный формат числа";
                }

                try
                {
                    _penBodyParametersList[property.ParamName] = double.Parse(property.Value);
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
                Error = error;

                return error;
            }

        }

        private PenBodyParameterVM GetProperty(string name) => 
            _propertyMap.Find((property) => property.Name == name);

        /// <summary>
        /// Метод установки значений по умолчанию.
        /// </summary>
        public void SetToDefault()
        {
            PenBodyParametersList = new PenBodyParametersList();
            MainLength = PenBodyParametersList[ParamName.MainLength].ToString();
            RubberLength = PenBodyParametersList[ParamName.RubberLength].ToString();
            MainDiameter = PenBodyParametersList[ParamName.MainDiameter].ToString();
            RubberDiameter = PenBodyParametersList[ParamName.RubberDiameter].ToString();
            InnerDiameter = PenBodyParametersList[ParamName.InnerDiameter].ToString();
        }

        public PenBodyParametersList GetValidModel() => PenBodyParametersList;

        /// <summary>
        /// Метод обновления всех свойств (Для поддержания актуального состояния валидации).
        /// </summary>
        public void UpdateAll()
        {
            RaisePropertyChanged(nameof(MainLength));
            RaisePropertyChanged(nameof(RubberLength));
            RaisePropertyChanged(nameof(MainDiameter));
            RaisePropertyChanged(nameof(InnerDiameter));
            RaisePropertyChanged(nameof(RubberDiameter));
        }

        /// <summary>
        /// Метод для замены точки на запятую.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Строка с выполненной заменой.</returns>
        private string DotToComma(string str) => str.Replace('.', ',').Trim();
    }
}
