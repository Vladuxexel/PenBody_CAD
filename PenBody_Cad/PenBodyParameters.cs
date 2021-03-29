using System;

namespace PenBody_Cad
{
    /// <summary>
    /// Класс параметров корпуса пишущей ручки.
    /// </summary>
    public class PenBodyParameters
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
        /// Свойство длины основной части ручки.
        /// </summary>
        public double MainLength
        {
            get => _mainLength;
            set
            {
                if (value < 20)
                {
                    throw new ArgumentException("Длина основной части ручки должна быть не меньше 20 мм");
                }

                if (value > 70)
                {
                    throw new ArgumentException("Длина основной части ручки не должна превышать 70 мм");
                }

                if (value < 2 * RubberLength)
                {
                    throw new ArgumentException(
                        "Длина основной части ручки должна быть минимум в 2 раза больше части для резинки");
                }

                _mainLength = value;
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
                if (value < 15)
                {
                    throw new ArgumentException("Длина части для резинки должна быть не меньше 15 мм");
                }

                if (value > 35)
                {
                    throw new ArgumentException("Длина части для резинки не должна превышать 35 мм");
                }

                if (value > 0.5 * MainLength)
                {
                    throw new ArgumentException("Длина части для резинки не должна превышать половину длины основной части");
                }

                _rubberLength = value;
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
                if (value < 10)
                {
                    throw new ArgumentException("Диаметр ручки должен быть не меньше 10 мм");
                }

                if (value > 20)
                {
                    throw new ArgumentException("Диаметр ручки не должен превышать 20 мм");
                }

                _mainDiameter = value;
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
                if (value < 2)
                {
                    throw new ArgumentException("Внутренний диаметр ручки должен быть не меньше 2 мм");
                }

                if (value > 10)
                {
                    throw new ArgumentException("Внутренний диаметр ручки не должен превышать 10 мм");
                }

                if (value > MainDiameter)
                {
                    throw new ArgumentException("Внутренний диаметр не должен быть больше диаметра самой ручки");
                }

                if (value > RubberDiameter)
                {
                    throw new ArgumentException("Внутренний диаметр не должен быть больше диаметра части для резинки");
                }

                if (RubberDiameter - value < 2)
                {
                    throw new ArgumentException("Внутренний диаметр должен быть минимум на 2 мм меньше диаметра части для резинки");
                }

                _innerDiameter = value;
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
                if (value < 7)
                {
                    throw new ArgumentException("Диаметр части для резинки должен быть не меньше 7 мм");
                }

                if (value > 18)
                {
                    throw new ArgumentException("Диаметр части для резинки не должен превышать 18 мм");
                }

                if (value > MainDiameter)
                {
                    throw new ArgumentException("Диаметр части для резинки не должен быть больше диаметра самой ручки");
                }

                if (MainDiameter - value < 2)
                {
                    throw new ArgumentException("Диаметр части для резинки должен быть минимум на 2 мм меньше диаметра основной части");
                }

                _rubberDiameter = value;
            }
        }
    }
}
