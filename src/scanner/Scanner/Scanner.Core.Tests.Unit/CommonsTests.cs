using System;
using System.Collections.Generic;
using System.Text;
using Scanner;
namespace Scanner.Core.Tests.Unit
{
    public class CommonsTests
    {

        [Fact]
        public void ArgMax_WhenArrayIsNull_ShouldFail()
        {
            // Arrange
            double[]? sut = null;

            // Act
            Action action = () => Core.Commons.ArgMax(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }

        [Fact]
        public void ArgMax_WhenArrayIsEmpty_ShouldFail()
        {
            // Arrange
            double[] sut = Array.Empty<double>();

            // Act
            Action action = () => Core.Commons.ArgMax(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }

        [Fact]
        public void ArgMax_WhenFirstValueIsMax_ShouldReturnZero()
        {
            // Arrange
            int expectedValue = 0;
            double[] sut = { 3, 2, 1 };

            // Act
            int actualValue = Core.Commons.ArgMax(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ArgMax_WhenLastValueIsMax_ShouldReturnLengthMinusOne()
        {
            // Arrange
            double[] sut = { 1, 2, 3 };
            int expectedValue = sut.Length - 1;

            // Act
            int actualValue = Core.Commons.ArgMax(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);

        }

        [Fact]
        public void ArgMin_WhenArrayIsNull_ShouldFail()
        {
            // Arrange
            double[]? sut = null;

            // Act
            Action action = () => Core.Commons.ArgMin(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }

        [Fact]
        public void ArgMin_WhenArrayIsEmpty_ShouldFail()
        {
            // Arrange
            double[] sut = Array.Empty<double>();

            // Act
            Action action = () => Core.Commons.ArgMin(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }

        [Fact]
        public void ArgMin_WhenFirstValueIsMin_ShouldReturnZero()
        {
            // Arrange
            int expectedValue = 0;
            double[] sut = { 1, 2, 3 };

            // Act
            int actualValue = Core.Commons.ArgMin(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);

        }

        [Fact]
        public void ArgMin_WhenLastValueIsMin_ShouldReturnLengthMinusOne()
        {
            // Arrange
            double[] sut = { 3, 2, 1 };
            int expectedValue = sut.Length - 1;

            // Act
            int actualValue = Core.Commons.ArgMin(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
        

    }
}
