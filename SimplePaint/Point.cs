using System;
using System.Drawing;

public class Point : IComparable<Point>
{
    private int x, y;
    private Color color;

    public Point(int cX, int cY, Color whichColor)
    {
        x = cX;
        y = cY;
        color = whichColor;
    }

    public int X
    {
        get { return x; }
        set
        {
            if (value < 0)
                throw new Exception("Negative X coordinate is invalid!");

            x = value;
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            if (value < 0)
                throw new Exception("Negative Y coordinate is invalid!");

            y = value;
        }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    // Draws the point on the graphics context g
    public virtual void Draw(Color color, Graphics g)
    {
        Pen pen = new Pen(color);
        g.DrawLine(pen, x, y, x, y);
    }

    // Draws the point on the graphics context g with a specified thickness
    public virtual void Draw(Color color, Graphics g, int thickness)
    {
        Pen pen = new Pen(color, thickness);
        g.DrawLine(pen, x, y, x, y);
    }

    // Compares two Point objects
    public int CompareTo(Point other)
    {
        int differenceX = X - other.X;

        if (differenceX == 0) { return Y - other.Y; }

        return differenceX;
    }

    // Formats an integer value and returns it
    public string FormatString(int value, int numberOfPositions)
    {
        string str = value + "";
        while (str.Length < numberOfPositions)
            str = "0" + str;

        return str.Substring(0, numberOfPositions);
    }

    // Formats a string value and returns it
    public string FormatString(string value, int numberOfPositions)
    {
        string str = value + "";
        while (str.Length < numberOfPositions)
            str += " ";

        return str.Substring(0, numberOfPositions);
    }

    // Returns a formatted string for a text file
    public override string ToString()
    {
        return FormatString("p", 5) + FormatString(X, 5) + FormatString(Y, 5)
             + FormatString(Color.R, 5) + FormatString(Color.G, 5) + FormatString(Color.B, 5);
    }
}
