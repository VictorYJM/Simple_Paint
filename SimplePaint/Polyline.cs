using System;
using System.Collections.Generic;
using System.Drawing;

public class Polyline : Point
{
    // List to store all the points/vertices of the polyline
    List<Point> pointsList = new List<Point>();

    public List<Point> PointsList => pointsList;

    // Starts the polyline
    public Polyline(int cX, int cY, Color whichColor) : base(cX, cY, whichColor)
    {
        Point newPoint = new Point(cX, cY, whichColor);
        AddPoint(newPoint);
    }

    // Adds a point/vertex to the polyline
    public void AddPoint(Point point)
    {
        pointsList.Add(point);
    }

    // Draws the polyline on the graphics context g
    public override void Draw(Color color, Graphics g)
    {
        int cordX = 0;
        int cordY = 0;
        bool firstAccess = true;
        Pen pen = new Pen(color);

        foreach (var point in pointsList)
        {
            if (!firstAccess)
                g.DrawLine(pen, point.X, point.Y, cordX, cordY);

            else
                firstAccess = false;

            cordX = point.X;
            cordY = point.Y;
        }
    }

    // Draws the polyline on the graphics context g with a specified thickness
    public override void Draw(Color color, Graphics g, int thickness)
    {
        int cordX = 0;
        int cordY = 0;
        bool firstAccess = true;
        Pen pen = new Pen(color, thickness);

        foreach (var point in pointsList)
        {
            if (!firstAccess)
                g.DrawLine(pen, point.X, point.Y, cordX, cordY);

            else
                firstAccess = false;

            cordX = point.X;
            cordY = point.Y;
        }
    }

    // Returns a formatted string for a text file
    // traversing through the list of points/vertices
    public override string ToString()
    {
        string result = "";

        foreach (Point point in pointsList)
            result += FormatString("poli", 5) + FormatString(point.X, 5) + FormatString(point.Y, 5)
             + FormatString(Color.R, 5) + FormatString(Color.G, 5) + FormatString(Color.B, 5) + "\n";

        // The returned string will have an empty line
        // to indicate the end of the polyline
        return result;
    }
}
