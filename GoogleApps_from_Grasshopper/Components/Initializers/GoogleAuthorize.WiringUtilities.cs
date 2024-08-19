using System.Drawing;

namespace Goograsshopper.Components.Initializers
{
    internal static class GetObjectOnCanvas_WireUtil
    {
        public static void DrawCustomWire(this GoogleAuthorize_Attributes srcAtt, PointF targetPt, Graphics graphics, KnownColor color = KnownColor.MediumSeaGreen)
        {
            Pen pen = new Pen(Color.FromKnownColor(color), 3);
            graphics.DrawBezier(pen, srcAtt.Grip, new PointF(srcAtt.Grip.X, srcAtt.Grip.Y + 50), new PointF(targetPt.X, targetPt.Y - 50), targetPt);
            pen.Dispose();
        }
    }
}