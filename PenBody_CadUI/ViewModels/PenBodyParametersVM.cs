using System.ComponentModel;

namespace PenBody_CadUI.ViewModels
{
    /// <summary>
    /// Класс для валидации параметров модели.
    /// </summary>
    public class PenBodyParametersVM : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Длина основной части.
        /// </summary>
        private double _mainLength;

        /// <summary>
        /// Длина части для резинки.
        /// </summary>
        private double _rubberLength;

        /// <summary>
        /// Диаметр ручки.
        /// </summary>
        private double _mainDiameter;

        /// <summary>
        /// Внутренний диаметр.
        /// </summary>
        private double _innerDiameter;

        /// <summary>
        /// Диаметр части для резинки.
        /// </summary>
        private double _rubberDiameter;

        /// <summary>
        /// Свойство для вывода ошибки.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Свойство длины основной части ручки.
        /// </summary>
        public double MainLength
        {
            get => _mainLength;
            set
            {
                _mainLength = value;
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public double RubberLength
        {
            get => _rubberLength;
            set
            {
                _rubberLength = value;
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство диаметра ручки.
        /// </summary>
        public double MainDiameter
        {
            get => _mainDiameter;
            set
            {
                _mainDiameter = value;
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство внутреннего диаметра ручки.
        /// </summary>
        public double InnerDiameter
        {
            get => _innerDiameter;
            set
            {
                _innerDiameter = value;
                UpdateAll();
            }
        }

        /// <summary>
        /// Свойство длины части для резинки.
        /// </summary>
        public double RubberDiameter
        {
            get => _rubberDiameter;
            set
            {
                _rubberDiameter = value;
                UpdateAll();
            }
        }

        /// <summary>
        /// Реализация интерфейса IDataErrorInfo.
        /// </summary>
        /// <param name="propertyName">Имя валидируемого свойства.</param>
        /// <returns></returns>
        public string this[string propertyName]
        {
            get
            {
                string error = null;

                switch (propertyName)
                {
                    case "MainLength":
                        if (MainLength < 20)
                        {
                            error = "Длина основной части ручки должна быть не меньше 20 мм";
                        }
                        else if (MainLength > 70)
                        {
                            error = "Длина основной части ручки не должна превышать 70 мм";
                        }
                        else if (MainLength < 2 * RubberLength)
                        {
                            error = "Длина основной части ручки должна быть минимум в 2 раза больше части для резинки";
                        }
                        break;
                    case "RubberLength":
                        if (RubberLength < 15)
                        {
                            error = "Длина части для резинки должна быть не меньше 15 мм";
                        }
                        else if (RubberLength > 35)
                        {
                            error = "Длина части для резинки не должна превышать 35 мм";
                        }
                        else if (RubberLength > 0.5 * MainLength)
                        {
                            error = "Длина части для резинки не должна превышать половину длины основной части";
                        }
                        break;
                    case "MainDiameter":
                        if (MainDiameter < 10)
                        {
                            error = "Диаметр ручки должен быть не меньше 10 мм";
                        }
                        else if (MainDiameter > 20)
                        {
                            error = "Диаметр ручки не должен превышать 20 мм";
                        }
                        else if (MainDiameter < RubberDiameter)
                        {
                            error = "Диаметр ручки не должен быть меньше диаметра части для резинки";
                        }
                        else if (MainDiameter < InnerDiameter)
                        {
                            error = "Диаметр ручки не должен быть меньше внутреннего диаметра";
                        }
                        break;
                    case "InnerDiameter":
                        if (InnerDiameter < 2)
                        {
                            error = "Внутренний диаметр ручки должен быть не меньше 2 мм";
                        }
                        else if (InnerDiameter > 10)
                        {
                            error = "Внутренний диаметр ручки не должен превышать 10 мм";
                        }
                        else if (InnerDiameter > MainDiameter)
                        {
                            error = "Внутренний диаметр не должен быть больше диаметра самой ручки";
                        }
                        else if (InnerDiameter > RubberDiameter)
                        {
                            error = "Внутренний диаметр не должен быть больше диаметра части для резинки";
                        }
                        break;
                    case "RubberDiameter":
                        if (RubberDiameter < 7)
                        {
                            error = "Диаметр части для резинки должен быть не меньше 7 мм";
                        }
                        else if (RubberDiameter > 18)
                        {
                            error = "Диаметр части для резинки не должен превышать 18 мм";
                        }
                        else if (RubberDiameter > MainDiameter)
                        {
                            error = "Диаметр части для резинки не должен быть больше диаметра самой ручки";
                        }
                        else if (MainDiameter - RubberDiameter < 2)
                        {
                            error = "Диаметр части для резинки должен быть минимум на 2 мм меньше диаметра основной части";
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
    }
}
