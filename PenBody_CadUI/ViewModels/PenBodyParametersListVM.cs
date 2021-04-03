using PenBody_Cad;
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
        /// Длина основной части.
        /// </summary>
        private string _mainLength;

        /// <summary>
        /// Длина части для резинки.
        /// </summary>
        private string _rubberLength;

        /// <summary>
        /// Диаметр ручки.
        /// </summary>
        private string _mainDiameter;

        /// <summary>
        /// Внутренний диаметр.
        /// </summary>
        private string _innerDiameter;

        /// <summary>
        /// Диаметр части для резинки.
        /// </summary>
        private string _rubberDiameter;

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
                OnPropertyChanged(nameof(PenBodyParametersList));
            }
        }

        /// <summary>
        /// Свойство длины основной части ручки.
        /// </summary>
        public string MainLength
        {
            get => _mainLength;
            set
            {
                _mainLength = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public string RubberLength
        {
            get => _rubberLength;
            set
            {
                _rubberLength = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство диаметра ручки.
        /// </summary>
        public string MainDiameter
        {
            get => _mainDiameter;
            set
            {
                _mainDiameter = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство внутреннего диаметра ручки.
        /// </summary>
        public string InnerDiameter
        {
            get => _innerDiameter;
            set
            {
                _innerDiameter = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public string RubberDiameter
        {
            get => _rubberDiameter;
            set
            {
                _rubberDiameter = DotToComma(value);
                UpdateAll();
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public PenBodyParametersListVM()
        {
            _penBodyParametersList = new PenBodyParametersList();
            MainLength = _penBodyParametersList[ParamName.MainLength].ToString();
            RubberLength = _penBodyParametersList[ParamName.RubberLength].ToString();
            MainDiameter = _penBodyParametersList[ParamName.MainDiameter].ToString();
            RubberDiameter = _penBodyParametersList[ParamName.RubberDiameter].ToString();
            InnerDiameter = _penBodyParametersList[ParamName.InnerDiameter].ToString();
        }

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

                switch (propertyName)
                {
                    case nameof(MainLength):
                        //TODO: Duplication
                        if (MainLength.Trim() == "")
                        {
                            error = "Необходимо ввести число";
                        }
                        else if (!Regex.IsMatch(MainLength, _doubleRegex))
                        {
                            error = "Неверный формат числа";
                        }
                        else
                        {
                            try
                            {
                                _penBodyParametersList[ParamName.MainLength] = double.Parse(MainLength);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "RubberLength":
                        //TODO: Duplication
                        if (RubberLength.Trim() == "")
                        {
                            error = "Необходимо ввести число";
                        }
                        else if (!Regex.IsMatch(RubberLength, _doubleRegex))
                        {
                            error = "Неверный формат числа";
                        }
                        else
                        {
                            try
                            {
                                _penBodyParametersList[ParamName.RubberLength] = double.Parse(RubberLength);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "MainDiameter":
                        //TODO: Duplication
                        if (MainDiameter.Trim() == "")
                        {
                            error = "Необходимо ввести число";
                        }
                        else if (!Regex.IsMatch(MainDiameter, _doubleRegex))
                        {
                            error = "Неверный формат числа";
                        }
                        else
                        {
                            try
                            {
                                _penBodyParametersList[ParamName.MainDiameter] = double.Parse(MainDiameter);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "InnerDiameter":
                        //TODO: Duplication
                        if (InnerDiameter.Trim() == "")
                        {
                            error = "Необходимо ввести число";
                        }
                        else if (!Regex.IsMatch(InnerDiameter, _doubleRegex))
                        {
                            error = "Неверный формат числа";
                        }
                        else
                        {
                            try
                            {
                                _penBodyParametersList[ParamName.InnerDiameter] = double.Parse(InnerDiameter);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "RubberDiameter":
                        //TODO: Duplication
                        if (RubberDiameter.Trim() == "")
                        {
                            error = "Необходимо ввести число";
                        }
                        else if (!Regex.IsMatch(RubberDiameter, _doubleRegex))
                        {
                            error = "Неверный формат числа";
                        }
                        else
                        {
                            try
                            {
                                _penBodyParametersList[ParamName.RubberDiameter] = double.Parse(RubberDiameter);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                }
                Error = error;

                return error;
            }

        }

        /// <summary>
        /// Метод установки значений по умолчанию.
        /// </summary>
        public void SetToDefault()
        {
            PenBodyParametersList = new PenBodyParametersList();
        }

        public PenBodyParametersList GetValidModel() => PenBodyParametersList;

        /// <summary>
        /// Метод обновления всех свойств (Для поддержания актуального состояния валидации).
        /// </summary>
        public void UpdateAll()
        {
            OnPropertyChanged(nameof(MainLength));
            OnPropertyChanged(nameof(RubberLength));
            OnPropertyChanged(nameof(MainDiameter));
            OnPropertyChanged(nameof(InnerDiameter));
            OnPropertyChanged(nameof(RubberDiameter));
        }

        /// <summary>
        /// Метод для замены точки на запятую.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Строка с выполненной заменой.</returns>
        private string DotToComma(string str) => str.Replace('.', ',').Trim();
    }
}
