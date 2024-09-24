using System;
using System.Drawing;

public class Line : Point
{
    private Point endPoint;

    public Line(int x1, int y1, int x2, int y2, Color newColor) : base(x1, y1, newColor)
    {
        endPoint = new Point(x2, y2, newColor);
    }

    // Draws the line on the graphics context g
    public override void Draw(Color drawColor, Graphics g)
    {
        Pen pen = new Pen(drawColor);
        g.DrawLine(pen, base.X, base.Y, endPoint.X, endPoint.Y);
    }

    // Draws the line on the graphics context g with a specified thickness
    public override void Draw(Color drawColor, Graphics g, int thickness)
    {
        Pen pen = new Pen(drawColor, thickness);
        g.DrawLine(pen, base.X, base.Y, endPoint.X, endPoint.Y);
    }

    // Returns a formatted string for a text file
    public override string ToString()
    {
        return base.FormatString("l", 5) + base.FormatString(base.X, 5) + base.FormatString(base.Y, 5)
             + base.FormatString(base.Color.R, 5) + base.FormatString(base.Color.G, 5) + base.FormatString(base.Color.B, 5)
             + base.FormatString(endPoint.X, 5) + base.FormatString(endPoint.Y, 5);
    }
}
