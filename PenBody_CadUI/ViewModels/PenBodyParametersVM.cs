using PenBody_Cad;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PenBody_CadUI.ViewModels
{
    /// <summary>
    /// Класс для валидации параметров модели.
    /// </summary>
    public class PenBodyParametersVM : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Длина основной части ручки по умолчанию.
        /// </summary>
        private const string MAIN_LENGTH = "40";

        /// <summary>
        /// Длина части для резинки по умолчанию.
        /// </summary>
        private const string RUBBER_LENGTH = "20";

        /// <summary>
        /// Диаметр основной ручки по умолчанию.
        /// </summary>
        private const string MAIN_DIAMETER = "15";

        /// <summary>
        /// Внутренний диаметр ручки по умолчанию.
        /// </summary>
        private const string INNER_DIAMETER = "5";

        /// <summary>
        /// Диаметр части для резинки по умолчанию.
        /// </summary>
        private const string RUBBER_DIAMETER = "10";

        /// <summary>
        /// Регулярное выражения для числа типа double.
        /// </summary>
        private const string _doubleRegex = "^[0-9]*[.,]?[0-9]+$";

        /// <summary>
        /// Поле с моделью.
        /// </summary>
        private PenBodyParameters _penBodyParameters;

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
        public PenBodyParametersVM()
        {
            _penBodyParameters = new PenBodyParameters
            {
                MainLength = double.Parse(MAIN_LENGTH),
                RubberLength = double.Parse(RUBBER_LENGTH),
                MainDiameter = double.Parse(MAIN_DIAMETER),
                RubberDiameter = double.Parse(RUBBER_DIAMETER),
                InnerDiameter = double.Parse(INNER_DIAMETER)
            };
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
                    case "MainLength":
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
                                _penBodyParameters.MainLength = double.Parse(MainLength);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "RubberLength":
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
                                _penBodyParameters.RubberLength = double.Parse(RubberLength);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "MainDiameter":
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
                                _penBodyParameters.MainDiameter = double.Parse(MainDiameter);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "InnerDiameter":
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
                                _penBodyParameters.InnerDiameter = double.Parse(InnerDiameter);
                            }
                            catch (Exception e)
                            {
                                error = e.Message;
                            }
                        }
                        break;
                    case "RubberDiameter":
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
                                _penBodyParameters.RubberDiameter = double.Parse(RubberDiameter);
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
        /// Метод установки значений по умолчанию.
        /// </summary>
        public void SetToDefault()
        {
            MainLength = MAIN_LENGTH;
            RubberLength = RUBBER_LENGTH;
            MainDiameter = MAIN_DIAMETER;
            InnerDiameter = INNER_DIAMETER;
            RubberDiameter = RUBBER_DIAMETER;
        }

        /// <summary>
        /// Получить валидную модель.
        /// </summary>
        /// <returns>Валидная модель.</returns>
        public PenBodyParameters GetValidModel() => _penBodyParameters;

        /// <summary>
        /// Метод для замены точки на запятую.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Строка с выполненной заменой.</returns>
        private string DotToComma(string str) => str.Replace('.', ',').Trim();
    }
}
