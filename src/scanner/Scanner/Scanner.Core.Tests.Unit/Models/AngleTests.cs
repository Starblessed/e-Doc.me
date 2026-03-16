using OpenCvSharp;

using Scanner.Core.Models;

namespace Scanner.Core.Tests.Unit.Models
{
    public class AngleTests
    {
        [Fact]
        public void AngleFromPoints_WhenVectorsAreParallel_ShouldReturnZeroDegrees()
        {
            // Arrange
            Point a = new Point(0, 1);
            Point b = new Point(0, 0);
            Point c = new Point(0, 2);

            double expectedValue = 0;
            double tolerance = 0.0001;

            // Act
            Angle actualValue = Angle.FromPoints(a, b, c);

            // Assert
            Assert.Equal(expectedValue, actualValue, tolerance);

        }
        [Fact]
        public void AngleFromPoints_WhenVectorsAreOpposite_ShouldReturn180Degrees()
        {
            // Arrange
            Point a = new Point(0, -1);
            Point b = new Point(0, 0);
            Point c = new Point(0, 1);

            double expectedValue = 180.0;
            double tolerance = 0.0001;

            // Act
            Angle actualValue = Angle.FromPoints(a, b, c);

            // Assert
            Assert.Equal(expectedValue, actualValue, tolerance);

        }
        [Fact]
        public void AngleFromPoints_WhenVectorsAreOrthogonal_ShouldReturn90Degrees()
        {
            // Arrange
            Point a = new Point(1, 0);
            Point b = new Point(0, 0);
            Point c = new Point(0, 1);

            double expectedValue = 90.0;
            double tolerance = 0.0001;

            // Act
            Angle actualValue = Angle.FromPoints(a, b, c);

            // Assert
            Assert.Equal(expectedValue, actualValue, tolerance);

        }
        [Fact]
        public void AngleFromPoints_WhenVectorHasNormZero_ShouldFail()
        {
            // Arrange
            Point a = new Point(0, 0);
            Point b = new Point(0, 0);
            Point c = new Point(0, 1);

            // Act
            Action action = () => Angle.FromPoints(a, b, c);

            // Assert
            Assert.Throws<DivideByZeroException>(() => action());

        }
    }
}
