using NUnit.Framework;
using PenBody_Cad.Enums;
using System;

namespace PenBody_Cad.UnitTests
{
    [TestFixture]
    public class PenBodyParametersListTests
    {
        [TestCase(ParamName.MainLength, 30,
            TestName = "Длина основной части превышает" +
            " длину части для резинки меньше, чем в 2 раза")]
        [TestCase(ParamName.RubberLength, 25,
            TestName = "Длина части для резинки превышает" +
            " половину длины основной части")]
        [TestCase(ParamName.RubberDiameter, 16,
            TestName = "Диаметр части для резинки" +
            " больше диаметра ручки")]
        [TestCase(ParamName.RubberDiameter, 15,
            TestName = "Диаметр части для резинки меньше" +
            " диаметра ручки меньше, чем на 2 мм")]
        [TestCase(ParamName.InnerDiameter, 16,
            TestName = "Внутренний диаметр меньше" +
            " диаметра части для резинки меньше, чем на 2 мм")]
        public void Indexer_Set_BadValue
            (ParamName name, double badValue)
        {
            //Setup
            var penBodyParametersList = new PenBodyParametersList();

            //Asset
            Assert.Throws<ArgumentException>(
                () =>
                {
                    //Act
                    penBodyParametersList[name] = badValue;
                });
        }

        [TestCase(ParamName.MainLength, 60,
            TestName = "Позитивный тест " +
            "установки длины основной части")]
        [TestCase(ParamName.RubberLength, 15,
            TestName = "Позитивный тест " +
            "установки длины части для резинки")]
        [TestCase(ParamName.MainDiameter, 18,
            TestName = "Позитивный тест" +
            " установки диаметра ручки")]
        [TestCase(ParamName.RubberDiameter, 13,
            TestName = "Позитивный тест установки" +
            " диаметра части для резинки")]
        [TestCase(ParamName.InnerDiameter, 7,
            TestName = "Позитивный тест установки " +
            "внутреннего диаметра")]
        public void Indexer_Set_GoodValue
            (ParamName name, double goodValue)
        {
            //Setup
            var penBodyParameter = 
                new PenBodyParametersList();
            var expectedValue = goodValue;

            //Act
            penBodyParameter[name] = goodValue;
            var actualValue = penBodyParameter[name];

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
