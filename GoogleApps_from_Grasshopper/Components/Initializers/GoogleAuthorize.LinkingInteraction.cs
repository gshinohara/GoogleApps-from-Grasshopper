﻿using Goograsshopper.Components.Abstracts;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Goograsshopper.Components.Initializers
{
    internal class GoogleAuthorize_LinkingInteraction : GH_AbstractInteraction
    {
        private enum LinkMode
        {
            Replace,
            Add,
            Remove
        }

        private LinkMode m_mode;

        private PointF m_MouseLocation;

        private GoogleAuthorize_Attributes m_attributes;

        private IAbstractAccessors m_target;

        public GoogleAuthorize_LinkingInteraction(GH_Canvas canvas, GH_CanvasMouseEvent mouseEvent, GoogleAuthorize_Attributes attributes) : base(canvas, mouseEvent)
        {
            m_mode = LinkMode.Replace;
            m_attributes = attributes;
            m_target = null;
            m_MouseLocation = PointF.Empty;
            canvas.CanvasPostPaintObjects += Canvas_CanvasPostPaintObjects;
            Instances.CursorServer.AttachCursor(canvas, "GH_NewWire");
        }

        public override void Destroy()
        {
            m_canvas.CanvasPostPaintObjects -= Canvas_CanvasPostPaintObjects;
            base.Destroy();
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_Document document = m_attributes.Owner.OnPingDocument();
            if (document.FindObject(e.CanvasLocation, 5) is IAbstractAccessors obj && !(obj.Parent is GoogleAuthorize parent && parent != m_attributes.Owner))
                m_target = obj;
            else
                m_target = null;

            m_MouseLocation = e.CanvasLocation;

            sender.Refresh();
            return base.RespondToMouseMove(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GoogleAuthorize component = m_attributes.Owner;

            if (m_target != null)
            {
                switch (m_mode)
                {
                    case LinkMode.Replace:
                        foreach (Guid id in component.ConnectedIds)
                            component.OnPingDocument().FindObject<IAbstractAccessors>(id, true).ClearParent();
                        component.ConnectedIds.Clear();
                        m_target.Parent = component;
                        break;
                    case LinkMode.Add:
                        m_target.Parent = component;
                        break;
                    case LinkMode.Remove:
                        if (component.ConnectedIds.Contains(m_target.InstanceGuid))
                        {
                            m_target.ClearParent();
                            component.ConnectedIds.Remove(m_target.InstanceGuid);
                        }
                        break;
                }
                component.ExpireSolution(true);
            }

            sender.ActiveInteraction = null;
            return base.RespondToMouseUp(sender, e);
        }

        public override GH_ObjectResponse RespondToKeyDown(GH_Canvas sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    m_mode = LinkMode.Add;
                    break;
                case Keys.ControlKey:
                    m_mode = LinkMode.Remove;
                    break;
            }

            sender.Refresh();
            return base.RespondToKeyDown(sender, e);
        }

        public override GH_ObjectResponse RespondToKeyUp(GH_Canvas sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
                m_mode = LinkMode.Replace;

            sender.Refresh();
            return base.RespondToKeyUp(sender, e);
        }

        private void Canvas_CanvasPostPaintObjects(GH_Canvas sender)
        {
            PointF targetPt = m_target != null ? m_target.Attributes.Bounds.Location : m_MouseLocation;
            if (targetPt == Point.Empty)
                return;

            sender.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            KnownColor color = KnownColor.Transparent;
            switch (m_mode)
            {
                case LinkMode.Replace:
                    Instances.CursorServer.AttachCursor(Canvas, "GH_NewWire");
                    m_attributes.DrawCustomWire(targetPt, sender.Graphics); //Default color
                    break;
                case LinkMode.Add:
                    Instances.CursorServer.AttachCursor(Canvas, "GH_AddWire");
                    color = KnownColor.Red;
                    goto default;
                case LinkMode.Remove:
                    Instances.CursorServer.AttachCursor(Canvas, "GH_RemoveWire");
                    color = KnownColor.Green;
                    goto default;
                default:
                    m_attributes.DrawCustomWire(targetPt, sender.Graphics, color);
                    break;
            }
        }
    }
}
