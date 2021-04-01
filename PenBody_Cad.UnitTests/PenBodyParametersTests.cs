using NUnit.Framework;
using System;

namespace PenBody_Cad.UnitTests
{
    [TestFixture]
    public class PenBodyParametersTests
    {
        [Test]
        public void MainLength_GoodValue()
        {
            //Setup
            var penBodyParameters = new PenBodyParameters();
            const double sourseMainLength = 30;
            const double expectedMainLength = sourseMainLength;

            //Act
            penBodyParameters.MainLength = sourseMainLength;
            var actualMainLength = penBodyParameters.MainLength;

            //Assert
            Assert.AreEqual(expectedMainLength, actualMainLength);
        }

        [TestCase(15)]
        [TestCase(90)]
        [TestCase(30)]
        public void MainLength_BadValue(double badMainLength)
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainLength = 40, RubberLength = 20 };

            //Assert
            Assert.Throws<ArgumentException>
            (
                //Act
                () => penBodyParameters.MainLength = badMainLength
            );
        }

        [Test]
        public void RubberLength_GoodValue()
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainLength = 40 };
            const double sourseRubberLength = 20;
            const double expectedRubberLength = sourseRubberLength;

            //Act
            penBodyParameters.RubberLength = sourseRubberLength;
            var actualRubberLength = penBodyParameters.RubberLength;

            //Assert
            Assert.AreEqual(expectedRubberLength, actualRubberLength);
        }

        [TestCase(10)]
        [TestCase(50)]
        [TestCase(25)]
        public void RubberLength_BadValue(double badRubberLength)
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainLength = 40 };

            //Assert
            Assert.Throws<ArgumentException>
            (
                //Act
                () => penBodyParameters.RubberLength = badRubberLength
            );
        }

        [Test]
        public void MainDiameter_GoodValue()
        {
            //Setup
            var penBodyParameters = new PenBodyParameters();
            const double sourseMainDiameter = 20;
            const double expectedMainDiameter = sourseMainDiameter;

            //Act
            penBodyParameters.MainDiameter = sourseMainDiameter;
            var actualMainDiameter = penBodyParameters.MainDiameter;

            //Assert
            Assert.AreEqual(expectedMainDiameter, actualMainDiameter);
        }

        [TestCase(5)]
        [TestCase(50)]
        public void MainDiameter_BadValue(double badMainDiameter)
        {
            //Setup
            var penBodyParameters = new PenBodyParameters();

            //Assert
            Assert.Throws<ArgumentException>
            (
                //Act
                () => penBodyParameters.MainDiameter = badMainDiameter
            );
        }

        [Test]
        public void RubberDiameter_GoodValue()
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainDiameter = 17 };
            const double sourseRubberDiameter = 15;
            const double expectedRubberDiameter = sourseRubberDiameter;

            //Act
            penBodyParameters.RubberDiameter = sourseRubberDiameter;
            var actualMainDiameter = penBodyParameters.RubberDiameter;

            //Assert
            Assert.AreEqual(expectedRubberDiameter, actualMainDiameter);
        }

        [TestCase(6)]
        [TestCase(20)]
        [TestCase(18)]
        [TestCase(17)]
        public void RubberDiameter_BadValue(double badRubberDiameter)
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainDiameter = 17 };

            //Assert
            Assert.Throws<ArgumentException>
            (
                //Act
                () => penBodyParameters.RubberDiameter = badRubberDiameter
            );
        }

        [Test]
        public void InnerDiameter_GoodValue()
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainDiameter = 15, RubberDiameter = 12 };
            const double sourseInnerDiameter = 5;
            const double expectedInnerDiameter = sourseInnerDiameter;

            //Act
            penBodyParameters.InnerDiameter = sourseInnerDiameter;
            var actualInnerDiameter = penBodyParameters.InnerDiameter;

            //Assert
            Assert.AreEqual(expectedInnerDiameter, actualInnerDiameter);
        }

        [TestCase(1)]
        [TestCase(20)]
        [TestCase(10)]
        public void InnerDiameter_BadValue(double badInnerDiameter)
        {
            //Setup
            var penBodyParameters = new PenBodyParameters() { MainDiameter = 15, RubberDiameter = 10 };

            //Assert
            Assert.Throws<ArgumentException>
            (
                //Act
                () => penBodyParameters.InnerDiameter = badInnerDiameter
            );
        }
    }
}
