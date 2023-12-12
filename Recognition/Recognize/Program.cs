using OpenCvSharp;

string targetFile = "c:\\users\\qinx\\Pictures\\Screenshots\\Screenshot 2023-12-12 144358.png";
// Load the image
using Mat src = new Mat(targetFile, ImreadModes.Color);
using Mat gray = new Mat(), edges = new Mat();

// Convert it to grayscale
Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
Cv2.Canny(gray, edges, 50, 200);
using (new Window("edges", edges))
{
    Cv2.WaitKey();
}
// Use the Hough transform to detect lines in the image
LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Cv2.PI / 4, 50, 250, 10);
List<LineSegmentPoint> filteredLines = new List<LineSegmentPoint>();

Console.WriteLine($"Detected {lines.Length} lines");

foreach (LineSegmentPoint line in lines)
{
    // Calculate the slope of the line
    double slope = (double)(line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);

    // Check if the line is vertical (slope is infinity)
    if (double.IsInfinity(slope))
    {
        filteredLines.Add(line);
    }
    // Check if the line is horizontal (slope is 0)
    else if (Math.Abs(slope) < 0.1) // Use a small threshold to account for slight inaccuracies
    {
        filteredLines.Add(line);
    }
}

// Sort the filtered lines by Y coordinate (top to bottom)
filteredLines.Sort((a, b) => a.P1.Y.CompareTo(b.P1.Y));

var top  = filteredLines[0];
var bottom  = filteredLines[filteredLines.Count - 1];

// Sort the filtered lines by X coordinate (left to right)
filteredLines.Sort((a, b) => a.P1.X.CompareTo(b.P1.X));
var left = filteredLines[0];
var right = filteredLines[filteredLines.Count - 1];

List<LineSegmentPoint> outterLines = new List<LineSegmentPoint>() { top, bottom, left, right };

Console.WriteLine($"Filtered to {outterLines.Count} lines");
// Draw the lines on the image
foreach (LineSegmentPoint line in outterLines)
{
    Cv2.Line(src, line.P1, line.P2, Scalar.Red, 1, LineTypes.AntiAlias);
}

// Display the image
using (new Window("lines", src))
{
    Cv2.WaitKey();
}