using Scanner.Core.Geometry;
using OpenCvSharp;


namespace Scanner.Core.Tests.Unit.Geometry
{
    public class ContourUtilsTests
    {
        [Fact]
        public void ReorderContour_WhenContourIsEmpty_ShouldFail()
        {
            // Arrange
            Point[] sut = Array.Empty<Point>();

            // Act
            Action action = () => ContourUtils.ReorderContour(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }
        [Fact]
        public void ReorderContour_WhenContourIsNull_ShouldFail()
        {
            // Arrange
            Point[]? sut = null;

            // Act
            Action action = () => ContourUtils.ReorderContour(sut);

            // Assert
            Assert.Throws<ArgumentException>(() => action());

        }

        [Fact]
        public void ReorderContour_WhenContourIsOrdered_ShouldReturnSameContour()
        {
            // Arrange
            Point[] sut = {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1)
            };

            Point[] expectedValue = sut;

            // Act
            Point[] actualValue = ContourUtils.ReorderContour(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
        [Fact]
        public void ReorderContour_WhenContourIsDisordered_ShouldReturnOrdered()
        {
            // Arrange
            Point[] sut = {
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 1),
                new Point(0, 0)
            };

            Point[] expectedValue = {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1)
            };


            // Act
            Point[] actualValue = ContourUtils.ReorderContour(sut);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
