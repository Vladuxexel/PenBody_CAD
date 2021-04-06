using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;

namespace PenBody_Cad
{
    /// <summary>
    /// Класс, представляющий параметр.
    /// </summary>
    public class PenBodyParameter
    {
        /// <summary>
        /// Значение параметра.
        /// </summary>
        private double _value;

        /// <summary>
        /// Минимальная граница параметра.
        /// </summary>
        private double _min;

        /// <summary>
        /// Максимальная граница параметра.
        /// </summary>
        private double _max;

        /// <summary>
        /// Има параметра.
        /// </summary>
        public ParamName Name { get; set; }

        /// <summary>
        /// Свойство значения параметра.
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                var compareResMax = Comparer<double>.Default.Compare(value, _max);
                if (compareResMax > 0)
                {
                    throw new ArgumentException($"Значение параметра не должно превышать {_max}");
                }

                var compareResMin = Comparer<double>.Default.Compare(value, _min);
                if(compareResMin < 0)
                {
                    throw new ArgumentException($"Значение параметра не должно быть меньше {_min}");
                }

                _value = value;
            }
        }

        /// <summary>
        /// Свойство минимальной границы параметра.
        /// </summary>
        public double Min
        {
            get => _min;
            set
            {
                var compareResult = Comparer<double>.Default.Compare(value, _max);
                if (compareResult > 0)
                {
                    throw new ArgumentException("Минимальное значение не может быть больше максимального");
                }

                _min = value;
            }
        }

        /// <summary>
        /// Свойство максимальной границы параметра.
        /// </summary>
        public double Max
        {
            get => _max;
            set
            {
                var compareResult = Comparer<double>.Default.Compare(value, _min);
                if (compareResult < 0)
                {
                    throw new ArgumentException("Максимальное значение не может быть меньше минимального");
                }

                _max = value;
            }
        }

        /// <summary>
        /// Конструктор класса параметра.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="max">Максимальная граница параметра.</param>
        /// <param name="min">Минимальная граница параметра.</param>
        /// <param name="value">Значение параметра.</param>
        public PenBodyParameter(ParamName paramName, double max, double min, double value)
        {
            Name = paramName;
            Max = max;
            Min = min;
            Value = value;
        }
    }
}
