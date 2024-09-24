using System;
using System.Drawing;

public class Circle : Point
{
    int radius;

    public int Radius
    {
        get { return radius; }
        set
        {
            if (value < 0)
                throw new Exception("Negative radius is invalid!");

            radius = value;
        }
    }

    public Circle(int centerX, int centerY, int newRadius, Color newColor) : base(centerX, centerY, newColor)
    {
        radius = newRadius;
    }

    // Draws the circle on the graphics context g
    public override void Draw(Color drawColor, Graphics g)
    {
        Pen pen = new Pen(drawColor);
        g.DrawEllipse(pen, base.X - radius, base.Y - radius, 2 * radius, 2 * radius);
    }

    // Draws the circle on the graphics context g with a specified thickness
    public override void Draw(Color drawColor, Graphics g, int thickness)
    {
        Pen pen = new Pen(drawColor, thickness);
        g.DrawEllipse(pen, base.X - radius, base.Y - radius, 2 * radius, 2 * radius);
    }

    // Returns a formatted string for a text file
    public override string ToString()
    {
        return base.FormatString("c", 5) + base.FormatString(base.X, 5) + base.FormatString(base.Y, 5)
             + base.FormatString(base.Color.R, 5) + base.FormatString(base.Color.G, 5) + base.FormatString(base.Color.B, 5)
             + base.FormatString(radius, 5);
    }
}
