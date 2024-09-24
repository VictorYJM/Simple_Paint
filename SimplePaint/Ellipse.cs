using System;
using System.Drawing;

public class Ellipse : Point
{
    private int radiusH, radiusV;

    public int RadiusH
    {
        get { return radiusH; }
        set
        {
            if (value < 0)
                throw new Exception("Negative ellipse radius is invalid!");

            radiusH = value;
        }
    }

    public int RadiusV
    {
        get { return radiusV; }
        set
        {
            if (value < 0)
                throw new Exception("Negative ellipse radius is invalid!");

            radiusV = value;
        }
    }

    public Ellipse(int centerX, int centerY, int newRadiusH, int newRadiusV, Color newColor) : base(centerX, centerY, newColor)
    {
        radiusH = newRadiusH;
        radiusV = newRadiusV;
    }

    // Draws an ellipse on the graphics context g
    public override void Draw(Color drawColor, Graphics g)
    {
        Pen pen = new Pen(drawColor);
        g.DrawEllipse(pen, base.X - radiusH, base.Y - radiusV, 2 * radiusH, 2 * radiusV);
    }

    // Draws an ellipse on the graphics context g with a specified thickness
    public override void Draw(Color drawColor, Graphics g, int thickness)
    {
        Pen pen = new Pen(drawColor, thickness);
        g.DrawEllipse(pen, base.X - radiusH, base.Y - radiusV, 2 * radiusH, 2 * radiusV);
    }

    // Returns a formatted string for a text file
    public override string ToString()
    {
        return base.FormatString("e", 5) + base.FormatString(base.X, 5) + base.FormatString(base.Y, 5)
             + base.FormatString(base.Color.R, 5) + base.FormatString(base.Color.G, 5) + base.FormatString(base.Color.B, 5)
             + base.FormatString(radiusH, 5) + base.FormatString(radiusV, 5);
    }
}
