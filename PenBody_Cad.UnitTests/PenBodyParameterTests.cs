using NUnit.Framework;
using PenBody_Cad.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_Cad.UnitTests
{
    [TestFixture]
    public class PenBodyParameterTests
    {
        [TestCase(ParamName.MainLength, 30, 40, 20,
            TestName = "Минимум больше максимума")]
        [TestCase(ParamName.MainLength, -20, 10, 20,
            TestName = "Максимум меньше минимума")]
        [TestCase(ParamName.MainLength, 30, 10, 5,
            TestName = "Значение меньше минимума")]
        [TestCase(ParamName.MainLength, 30, 40, 50,
            TestName = "Значение больше максимума")]
        public void ParameterConstructor_BadValue(ParamName name, double max, double min, double value)
        {
            //Act
            Assert.Throws<ArgumentException>(
                () =>
                {
                    var penBodyParameter = new PenBodyParameter(name, max, min, value);
                });
        }

        [Test(Description = "Позитивный тест конструктора класса параметра")]
        public void ParameterConstructor_GoodValue()
        {
            //Setup
            var name = ParamName.MainLength;
            var max = 30;
            var min = 10;
            var value = 20;
            var expectedName = name;
            var expectedMax = max;
            var expectedMin = min;
            var expectedValue = value;

            //Act
            var penBodyParameter = new PenBodyParameter(name, max, min, value);
            var actualName = penBodyParameter.Name;
            var actualMax = penBodyParameter.Max;
            var actualMin = penBodyParameter.Min;
            var actualValue = penBodyParameter.Value;

            //Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedMin, actualMin);
            Assert.AreEqual(expectedMax, actualMax);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCase(50, Description = "Значение параметра превышает максимально допустимое")]
        [TestCase(5, Description = "Значение параметра меньше минимально допустимого")]
        public void Parameter_Value_GoodValue(double badValue)
        {
            //Setup
            var name = ParamName.MainLength;
            var max = 30;
            var min = 10;
            var value = 20;
            var penBodyParameter = new PenBodyParameter(name, max, min, value);

            //Assert
            Assert.Throws<ArgumentException>(
                () =>
                {
                    //Act
                    penBodyParameter.Value = badValue;
                });
        }
    }
}
