using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_Cad
{
    public class PenBodyParameter
    {
        private double _value;
        private double _min;
        private double _max;

        public ParamName Name { get; set; }

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

        public PenBodyParameter(ParamName paramName, double max, double min, double value)
        {
            Name = paramName;
            Max = max;
            Min = min;
            Value = value;
        }
    }
}
