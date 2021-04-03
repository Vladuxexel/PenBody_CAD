using System;
using System.Collections.Generic;

namespace PenBody_Cad
{
    public class PenBodyParametersList
    {
        #region Максимальные значения параметров
        /// <summary>
        /// Максимальная длина основной части ручки.
        /// </summary>
        private const double MaxMainLength = 70;

        /// <summary>
        /// Максимальная длина части для резинки.
        /// </summary>
        private const double MaxRubberLength = 30;

        /// <summary>
        /// Максимальный диаметр основной ручки.
        /// </summary>
        private const double MaxMainDiameter = 18;

        /// <summary>
        /// Минимальный диаметр части для резинки.
        /// </summary>
        private const double MaxRubberDiameter = 18;

        /// <summary>
        /// Максимальный внутренний диаметр ручки.
        /// </summary>
        private const double MaxInnerDiameter = 7;
        #endregion

        #region Минимальные значения параметров
        /// <summary>
        /// Минимальная длина основной части ручки.
        /// </summary>
        private const double MinMainLength = 30;

        /// <summary>
        /// Минимальная длина части для резинки.
        /// </summary>
        private const double MinRubberLength = 15;

        /// <summary>
        /// Минимальный диаметр основной ручки.
        /// </summary>
        private const double MinMainDiameter = 10;

        /// <summary>
        /// Минимальный диаметр части для резинки.
        /// </summary>
        private const double MinRubberDiameter = 7;

        /// <summary>
        /// Минимальный внутренний диаметр ручки.
        /// </summary>
        private const double MinInnerDiameter = 2;
        #endregion

        #region Стандартные значения параметров
        /// <summary>
        /// Длина основной части ручки по умолчанию.
        /// </summary>
        private const double DefaultMainLength = 40;

        /// <summary>
        /// Длина части для резинки по умолчанию.
        /// </summary>
        private const double DefaultRubberLength = 20;

        /// <summary>
        /// Диаметр основной ручки по умолчанию.
        /// </summary>
        private const double DefaultMainDiameter = 15;

        /// <summary>
        /// Диаметр части для резинки по умолчанию.
        /// </summary>
        private const double DefaultRubberDiameter = 10;

        /// <summary>
        /// Внутренний диаметр ручки по умолчанию.
        /// </summary>
        private const double DefaultInnerDiameter = 5;
        #endregion

        private List<PenBodyParameter> _parameters = new List<PenBodyParameter>
        {
            new PenBodyParameter(ParamName.MainLength, MaxMainLength, MinMainLength, DefaultMainLength),
            new PenBodyParameter(ParamName.RubberLength, MaxRubberLength, MinRubberLength, DefaultRubberLength),
            new PenBodyParameter(ParamName.MainDiameter, MaxMainDiameter, MinMainDiameter, DefaultMainDiameter),
            new PenBodyParameter(ParamName.RubberDiameter, MaxRubberDiameter, MinRubberDiameter, DefaultRubberDiameter),
            new PenBodyParameter(ParamName.InnerDiameter, MaxInnerDiameter, MinInnerDiameter, DefaultInnerDiameter)
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
