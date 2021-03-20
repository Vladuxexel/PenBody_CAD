using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_CadUI.ViewModels
{
    public class PenVM : ViewModelBase, IDataErrorInfo
    {
        private double _mainLength;
        private double _rubberLength;
        private double _mainDiameter;
        private double _innerDiameter;
        private double _rubberDiameter;

        public string Error { get; private set; }

        public double MainLength
        {
            get => _mainLength;
            set
            {
                _mainLength = value;
                UpdateAll();
            }
        }

        public double RubberLength
        {
            get => _rubberLength;
            set
            {
                _rubberLength = value;
                UpdateAll();
            }
        }

        public double MainDiameter
        {
            get => _mainDiameter;
            set
            {
                _mainDiameter = value;
                UpdateAll();
            }
        }

        public double InnerDiameter
        {
            get => _innerDiameter;
            set
            {
                _innerDiameter = value;
                UpdateAll();
            }
        }

        public double RubberDiameter
        {
            get => _rubberDiameter;
            set
            {
                _rubberDiameter = value;
                UpdateAll();
            }
        }
        
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
                        if (RubberDiameter < 10)
                        {
                            error = "Диаметр части для резинки должен быть не меньше 10 мм";
                        }
                        else if (RubberDiameter > 18)
                        {
                            error = "Диаметр части для резинки не должен превышать 18 мм";
                        }
                        else if (RubberDiameter > MainDiameter)
                        {
                            error = "Диаметр части для резинки не должен быть больше диаметра самой ручки";
                        }
                        break;
                }
                Error = error;

                return error;
            }

        }

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
