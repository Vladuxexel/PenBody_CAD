using GalaSoft.MvvmLight;
using PenBody_Cad;
using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PenBody_CadUI.ViewModels
{
    public class PenBodyParametersListVM : 
        ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Регулярное выражения для числа типа double.
        /// </summary>
        private const string _doubleRegex = "^[0-9]*[.,]?[0-9]+$";

        /// <summary>
        /// Регулярное выражения для числа типа int.
        /// </summary>
        private const string _intRegex = "^[0-9]+$";

        /// <summary>
        /// Флаг наличия рёбер у корпуса ручки.
        /// </summary>
        private bool _isRibbed;

        /// <summary>
        /// Поле с моделью.
        /// </summary>
        private PenBodyParametersList _penBodyParametersList;

        /// <summary>
        /// Свойство для вывода ошибки.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Свойство флага наличия рёбер у корпуса ручки.
        /// </summary>
        public bool IsRibbed 
        {
            get => _isRibbed;
            set
            {
                _isRibbed = value;
                PenBodyParametersList.IsRibbed = _isRibbed;
            }
        }

        /// <summary>
        /// Свойство списка параметров.
        /// </summary>
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
                GetProperty(nameof(MainLength)).Value =
                    DotToComma(value);
                RaisePropertyChanged(string.Empty);
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
                GetProperty(nameof(RubberLength)).Value =
                    DotToComma(value);
                RaisePropertyChanged(string.Empty);
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
                GetProperty(nameof(MainDiameter)).Value =
                    DotToComma(value);
                RaisePropertyChanged(string.Empty);
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
                GetProperty(nameof(InnerDiameter)).Value =
                    DotToComma(value);
                RaisePropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// Свойство количества ребёр ручки.
        /// </summary>
        public string EdgesNumber
        {
            get => GetProperty(nameof(EdgesNumber)).Value;
            set
            {
                GetProperty(nameof(EdgesNumber)).Value =
                    DotToComma(value);
                RaisePropertyChanged(nameof(EdgesNumber));
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
                GetProperty(nameof(RubberDiameter)).Value =
                    DotToComma(value);
                RaisePropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public PenBodyParametersListVM()
        {
            SetToDefault();
            IsRibbed = PenBodyParametersList.IsRibbed;
        }

        /// <summary>
        /// Список параметров.
        /// </summary>
        private List<PenBodyParameterVM> _propertyMap = 
            new List<PenBodyParameterVM>()
        {
            new PenBodyParameterVM(
                nameof(MainLength), ParamName.MainLength),
            new PenBodyParameterVM(
                nameof(RubberLength), ParamName.RubberLength),
            new PenBodyParameterVM(
                nameof(MainDiameter), ParamName.MainDiameter),
            new PenBodyParameterVM(
                nameof(RubberDiameter), ParamName.RubberDiameter),
            new PenBodyParameterVM(
                nameof(InnerDiameter), ParamName.InnerDiameter),
            new PenBodyParameterVM(
                nameof(EdgesNumber), ParamName.EdgesNumber)
        };

        /// <summary>
        /// Реализация интерфейса IDataErrorInfo.
        /// </summary>
        /// <param name="propertyName">
        /// Имя валидируемого свойства.
        /// </param>
        /// <returns>Текст ошибки.</returns>
        public string this[string propertyName]
        {
            get
            {
                string error = null;

                var property = GetProperty(propertyName);

                if (property.Value.Trim() == "")
                {
                    error = "Необходимо ввести число";
                }
                else if (!Regex.IsMatch(property.Value, _doubleRegex)
                    || !Regex.IsMatch(property.Value, _intRegex))
                {
                    if (property.ParamName == ParamName.EdgesNumber)
                    {
                        error = "Значение параметра " +
                            "должно представляться целым числом";
                    }
                    else
                    {
                        error = "Значение параметра " +
                            "должно представляться дробным числом";
                    }
                }
                else
                {
                    try
                    {
                        _penBodyParametersList[property.ParamName] =
                            double.Parse(property.Value);
                    }
                    catch (Exception e)
                    {
                        error = e.Message;
                    }
                }
                
                property.IsValid = error == null;

                return error;
            }

        }

        /// <summary>
        /// Проверка модели на корректность.
        /// </summary>
        /// <returns>Корректна ли модель.</returns>
        public bool IsValid()
        {
            foreach(var item in _propertyMap)
            {
                if (!item.IsValid)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получение свойства из списка.
        /// </summary>
        /// <param name="name">Имя свойства.</param>
        /// <returns>Элемент списка параметров.</returns>
        private PenBodyParameterVM GetProperty(string name) => 
            _propertyMap.Find((property) => property.Name == name);

        /// <summary>
        /// Метод установки значений по умолчанию.
        /// </summary>
        public void SetToDefault()
        {
            PenBodyParametersList = new PenBodyParametersList();
            foreach(var item in _propertyMap)
            {
                item.Value = 
                    PenBodyParametersList[item.ParamName].ToString();
                RaisePropertyChanged(item.Name);
            }
        }

        /// <summary>
        /// Получение корректной модели.
        /// </summary>
        /// <returns>Экземпляр класса модели.</returns>
        public PenBodyParametersList GetValidModel() =>
            PenBodyParametersList;

        /// <summary>
        /// Метод для замены точки на запятую.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Строка с выполненной заменой.</returns>
        private string DotToComma(string str) =>
            str.Replace('.', ',').Trim();
    }
}
