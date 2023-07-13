using System;
using System.Drawing;
using System.Linq;
using static System.Math;
using Point = System.Windows.Point;

namespace AgWillow;

/// <summary>
/// Methods for finding circle intersections
/// </summary>
/// <remarks>Adapted from http://csharphelper.com/blog/2014/09/determine-where-two-circles-intersect-in-c/</remarks>
public static class CircleIntersections
{
    /// <summary>
    /// Gets the intersections of two circles
    /// </summary>
    /// <param name="center1">The first circle's center</param>
    /// <param name="center2">The second circle's center</param>
    /// <param name="radius1">The first circle's radius</param>
    /// <param name="radius2">The second circle's radius. If omitted, assumed to equal the first circle's radius</param>
    /// <returns>An array of intersection points. May have zero, one, or two values</returns>
    public static Point[] GetCircleIntersections(Point center1, Point center2, double radius1, double? radius2 = null)
    {
        (double r1, double r2) = (radius1, radius2 ?? radius1);
        (double x1, double y1, double x2, double y2) = (center1.X, center1.Y, center2.X, center2.Y);
        // d = distance from center1 to center2
        double d = Sqrt(Pow(x1 - x2, 2) + Pow(y1 - y2, 2));
        // Return an empty array if there are no intersections
        if (!(Abs(r1 - r2) <= d && d <= r1 + r2)) { return Array.Empty<Point>(); }

        // Intersections i1 and possibly i2 exist
        double dsq = d * d;
        (double r1sq, double r2sq) = (r1 * r1, r2 * r2);
        double r1sq_r2sq = r1sq - r2sq;
        double a = r1sq_r2sq / (2 * dsq);
        double c = Sqrt(2 * (r1sq + r2sq) / dsq - (r1sq_r2sq * r1sq_r2sq) / (dsq * dsq) - 1);

        double fx = (x1 + x2) / 2 + a * (x2 - x1);
        double gx = c * (y2 - y1) / 2;

        double fy = (y1 + y2) / 2 + a * (y2 - y1);
        double gy = c * (x1 - x2) / 2;

        var i1 = new Point(fx + gx, fy + gy);
        var i2 = new Point(fx - gx, fy - gy);

        return i1 == i2 ? new[] { i1 } : new[] { i1, i2 };
    }

    /// <summary>
    /// Gets the intersections of two circles
    /// </summary>
    /// <param name="center1">The first circle's center</param>
    /// <param name="center2">The second circle's center</param>
    /// <param name="radius1">The first circle's radius</param>
    /// <param name="radius2">The second circle's radius. If omitted, assumed to equal the first circle's radius</param>
    /// <returns>An array of intersection points. May have zero, one, or two values</returns>
    public static PointF[] GetCircleIntersections(PointF center1, PointF center2, double radius1, double? radius2 = null)
    {
        var c1 = new Point(center1.X, center1.Y);
        var c2 = new Point(center2.X, center2.Y);
        Point[] points = GetCircleIntersections(c1, c2, radius1, radius2);

        return points.Select(p => new PointF((float)p.X, (float)p.Y)).ToArray();
    }
}