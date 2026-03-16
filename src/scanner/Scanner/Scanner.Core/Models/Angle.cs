using OpenCvSharp;

namespace Scanner.Core.Models
{
    public readonly struct Angle
    {
        double Degrees { get; }

        private Angle(double degrees)
        {
            Degrees = degrees;
        }

        public static Angle FromPoints(Point a, Point b, Point c)
        {
            Point ba = new Point(a.X - b.X, a.Y - b.Y);
            Point bc = new Point(c.X - b.X, c.Y - b.Y);

            int dotProduct = ba.X * bc.X + ba.Y * bc.Y;

            double normBA = Math.Sqrt(ba.X * ba.X + ba.Y * ba.Y);
            double normBC = Math.Sqrt(bc.X * bc.X + bc.Y * bc.Y);

            double normProduct = normBA * normBC;

            if (normProduct == 0) { throw new DivideByZeroException("The product of norms can not be zero."); }

            double cosine = Math.Clamp(dotProduct / normProduct, -1.0, 1.0);
            double angle = 180 * (Math.Acos(cosine) / Math.PI);

            return new Angle(angle);
        }

        public static implicit operator double(Angle angle)
        {
            return angle.Degrees;
        }
    }

    public interface IPolygonAngles : IEnumerable<Angle> { int VertexCount { get; } }
    public readonly record struct QuadrilateralAngles
        (
        Angle TopLeft,
        Angle TopRight,
        Angle BottomRight,
        Angle BottomLeft
        ) : IPolygonAngles
    {
        public int VertexCount => 4;

        public IEnumerator<Angle> GetEnumerator()
        {
            yield return TopLeft;
            yield return TopRight;
            yield return BottomRight;
            yield return BottomLeft;
            
        }

        // Generic interface enumerator
        IEnumerator<Angle> IEnumerable<Angle>.GetEnumerator() => GetEnumerator();

        // Non-generic interface enumerator
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public static QuadrilateralAngles FromQuadrilateral(Quadrilateral quad)
        {
            Angle tl = Angle.FromPoints(quad.BottomLeft, quad.TopLeft, quad.TopRight);
            Angle tr = Angle.FromPoints(quad.TopLeft, quad.TopRight, quad.BottomRight);
            Angle br = Angle.FromPoints(quad.TopRight, quad.BottomRight, quad.BottomLeft);
            Angle bl = Angle.FromPoints(quad.BottomRight, quad.BottomLeft, quad.TopLeft);
            return new QuadrilateralAngles(tl, tr, br, bl);
        }

        
    }
}
