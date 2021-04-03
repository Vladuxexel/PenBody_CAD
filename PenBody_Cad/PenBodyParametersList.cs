using System;
using System.Collections.Generic;

namespace PenBody_Cad
{
    public class PenBodyParametersList
    {
        private List<PenBodyParameter> _parameters = new List<PenBodyParameter>
        {
            new PenBodyParameter(ParamName.MainLength, 70, 20, 40),
            new PenBodyParameter(ParamName.RubberLength, 35, 15, 10),
            new PenBodyParameter(ParamName.MainDiameter, 20, 10, 15),
            new PenBodyParameter(ParamName.RubberDiameter, 18, 7, 10),
            new PenBodyParameter(ParamName.InnerDiameter, 10, 2, 5)
        };

        public double this[ParamName paramName]
        {
            get => GetParam(paramName).Value;
            set
            {
                switch (paramName)
                {
                    case ParamName.MainLength:
                        if (value < GetParam(ParamName.RubberLength).Value * 2)
                        {
                            throw new ArgumentException("Длина основной части ручки должна быть " +
                                "минимум в 2 раза больше части для резинки");
                        }
                        break;
                    case ParamName.RubberLength:
                        if (value > GetParam(ParamName.MainLength).Value * 0.5)
                        {
                            throw new ArgumentException("Длина части для резинки не должна превышать " +
                                "половину длины основной части");
                        }
                        break;
                    case ParamName.MainDiameter:
                        break;
                    case ParamName.RubberDiameter:
                        if (value > GetParam(ParamName.MainDiameter).Value)
                        {
                            throw new ArgumentException("Диаметр части для резинки не " +
                                "должен быть больше диаметра самой ручки");
                        }
                        if (GetParam(ParamName.MainDiameter).Value - value < 2)
                        {
                            throw new ArgumentException("Диаметр части для резинки должен быть " +
                                "минимум на 2 мм меньше диаметра основной части");
                        }
                        break;
                    case ParamName.InnerDiameter:
                        if (GetParam(ParamName.RubberDiameter).Value - value < 2)
                        {
                            throw new ArgumentException("Внутренний диаметр должен быть " +
                                "минимум на 2 мм меньше диаметра части для резинки");
                        }
                        break;
                }

                GetParam(paramName).Value = value;
            }
        }

        private PenBodyParameter GetParam(ParamName paramName) =>
            _parameters.Find((parameter) => parameter.Name.Equals(paramName));
    }
}
