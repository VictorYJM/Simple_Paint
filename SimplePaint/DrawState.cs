public enum DrawState
{
    None,
    WaitingForPoint,
    WaitingForLineStart,
    WaitingForLineEnd,
    WaitingForCircleCenter,
    WaitingForCircleRadius,
    WaitingForEllipseCenter,
    WaitingForEllipseRadiusH,
    WaitingForEllipseRadiusV,
    WaitingForTopLeftDiagonal,
    WaitingForBottomRightDiagonal,
    WaitingForPolylineStart,
    WaitingToContinuePolyline
}