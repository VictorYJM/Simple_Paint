using System;
using System.Drawing;

public class Rectangle : Point
{
    int cidX, cidY;

    public int CidX
    {
        get { return cidX; }
        set
        {
            if (value < 0)
                throw new Exception("Negative coordinate is invalid!");

            cidX = value;
        }
    }

    public int CidY
    {
        get { return cidY; }
        set
        {
            if (value < 0)
                throw new Exception("Negative coordinate is invalid!");

            cidY = value;
        }
    }

    public Rectangle(int cseX, int cseY, int cidX, int cidY, Color whichColor) : base(cseX, cseY, whichColor)
    {
        CidX = cidX;
        CidY = cidY;
    }

    // Draws the rectangle on the graphics context g
    public override void Draw(Color drawColor, Graphics g)
    {
        int width = Math.Abs(cidX - base.X);
        int height = Math.Abs(cidY - base.Y);
        Pen pen = new Pen(drawColor);

        g.DrawRectangle(pen, base.X, base.Y, width, height);
    }

    // Draws the rectangle on the graphics context g with a specified thickness
    public override void Draw(Color drawColor, Graphics g, int thickness)
    {
        int width = Math.Abs(cidX - base.X);
        int height = Math.Abs(cidY - base.Y);
        Pen pen = new Pen(drawColor, thickness);

        g.DrawRectangle(pen, base.X, base.Y, width, height);
    }

    // Returns a formatted string for a text file
    public override string ToString()
    {
        return base.FormatString("r", 5) + base.FormatString(base.X, 5) + base.FormatString(base.Y, 5)
             + base.FormatString(base.Color.R, 5) + base.FormatString(base.Color.G, 5) + base.FormatString(base.Color.B, 5)
             + base.FormatString(cidX, 5) + base.FormatString(cidY, 5);
    }
}
