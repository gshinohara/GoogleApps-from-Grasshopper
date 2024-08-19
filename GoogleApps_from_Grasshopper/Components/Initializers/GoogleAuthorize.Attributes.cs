using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Goograsshopper.Components.Abstracts;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Goograsshopper.Components.Initializers
{
    internal class GoogleAuthorize_Attributes : GH_Attributes<GoogleAuthorize>
    {
        private Task<UserCredential> m_UserCredentialTask;

        public PointF Grip => new PointF(Bounds.Left + 100, Bounds.Bottom);

        private RectangleF ButtonBounds;

        public GoogleAuthorize_Attributes(GoogleAuthorize owner) : base(owner)
        {
            m_UserCredentialTask = null;
        }

        private void SetCredential()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select your client secret file";
                openFileDialog.Filter = "|*.json";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    GoogleClientSecrets clientSecrets = GoogleClientSecrets.FromFile(openFileDialog.FileName);
                    string[] scopes = new string[] { SheetsService.Scope.Spreadsheets };
                    m_UserCredentialTask = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets.Secrets, scopes, "user", CancellationToken.None);
                }
            }
        }

        protected override void Layout()
        {
            base.Layout();

            RectangleF rect = Bounds;
            rect.Width = 200;
            rect.Height = 100;
            Bounds = rect;

            RectangleF buttonRect = Bounds;
            buttonRect.Y = buttonRect.Bottom - 22;
            buttonRect.Height = 22;
            buttonRect.Inflate(-2, -2);
            ButtonBounds = buttonRect;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            switch (channel)
            {
                case GH_CanvasChannel.First:
                    foreach (Guid id in Owner.ConnectedIds)
                    {
                        if (Owner.OnPingDocument().FindObject(id, true) is SpreadSheetAccessors obj)
                        {
                            RectangleF rect = obj.Attributes.Bounds;
                            rect.Inflate(6, 6);
                            GraphicsPath path = GH_CapsuleRenderEngine.CreateRoundedRectangle(rect, 3);
                            graphics.FillPath(new SolidBrush(Color.Red), path);
                        }
                    }
                    break;

                case GH_CanvasChannel.Objects:
                    GH_CapsuleRenderEngine.RenderInputGrip(graphics, canvas.Viewport.Zoom, Grip, true);

                    GH_Capsule fullCapsule = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Normal);
                    fullCapsule.Render(graphics, GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, Selected, false, false));
                    fullCapsule.Dispose();

                    string text_auth;
                    if (Owner.GetUserCredential() is UserCredential credential)
                    {
                        var service = new BaseClientService.Initializer() { HttpClientInitializer = credential };
                        text_auth = Owner.GetUserCredential().Token.AccessToken;
                    }
                    else
                    {
                        text_auth = "No authorization";
                    }
                    graphics.DrawString(text_auth, GH_FontServer.StandardAdjusted, new SolidBrush(Color.Black), Bounds, GH_TextRenderingConstants.CenterCenter);

                    GH_Capsule buttonCapsule = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Black, "Set a new Client Secret", 2, 0);
                    buttonCapsule.Render(graphics, Selected, false, false);
                    buttonCapsule.Dispose();

                    break;

                case GH_CanvasChannel.Wires:
                    foreach (Guid connectedId in Owner.ConnectedIds)
                    {
                        if (Owner.OnPingDocument().FindObject(connectedId, true) is IGH_DocumentObject connectedObj)
                            this.DrawCustomWire(connectedObj.Attributes.Bounds.Location, graphics); //Default color
                    }
                    break;
            }
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && GH_GraphicsUtil.Distance(e.CanvasLocation, Grip) <= 10)
            {
                sender.ActiveInteraction = new GoogleAuthorize_LinkingInteraction(sender, e, this);
                return GH_ObjectResponse.Handled;
            }
            else if (e.Button == MouseButtons.Left && ButtonBounds.Contains(e.CanvasLocation))
            {
                SetCredential();
                sender.Refresh();
                return GH_ObjectResponse.Handled;
            }

            return base.RespondToMouseDown(sender, e);
        }
    }
}