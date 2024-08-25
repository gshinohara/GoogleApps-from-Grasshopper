using Eto.Drawing;
using Eto.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goograsshopper.Components.Initializers
{
    partial class GoogleAuthorize_Attributes
    {
        private class InputForm : Form
        {
            private Task<UserCredential> m_task;

            public bool IsDriveScope { get; private set; }

            public bool IsSheetsScope { get; private set; }

            public InputForm()
            {
                AutoSize = true;
                Resizable = false;
                Topmost = true;

                CheckBox checkBox_Drive = new CheckBox
                {
                    Text = "Google Drive",
                    Checked = IsDriveScope,
                };
                checkBox_Drive.CheckedChanged += (sender, e) => IsDriveScope = (bool)(sender as CheckBox).Checked;

                CheckBox checkBox_Sheets = new CheckBox
                {
                    Text = "Google SpreadSheets",
                    Checked = IsSheetsScope,
                };
                checkBox_Sheets.CheckedChanged += (sender, e) => IsSheetsScope = (bool)(sender as CheckBox).Checked;

                Button button_SetCredential = new Button
                {
                    Text = "Browse",
                };
                button_SetCredential.Click += Button_SetCredential_Click;

                Button button_Cancel = new Button
                {
                    Text = "Cancel",
                };
                button_Cancel.Click += (sender, e) => Close();

                DynamicLayout layout = new DynamicLayout
                {
                    Padding = 30,
                    Spacing = new Size(0, 20),
                };

                layout.AddSeparateRow(checkBox_Drive);
                layout.AddSeparateRow(checkBox_Sheets);
                layout.AddSeparateRow(spacing: new Size(10,0),controls:new Control[] { button_SetCredential, button_Cancel });

                Content = layout;
            }

            private void Button_SetCredential_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Select your client secret file";
                    openFileDialog.Filters.Add(new FileFilter("Client Secret", ".json"));
                    openFileDialog.MultiSelect = false;

                    if (openFileDialog.ShowDialog(Rhino.UI.RhinoEtoApp.MainWindow) == DialogResult.Ok)
                    {
                        GoogleClientSecrets clientSecrets = GoogleClientSecrets.FromFile(openFileDialog.Filenames.FirstOrDefault());

                        List<string> scopes = new List<string>();
                        if (IsDriveScope)
                            scopes.Add(SheetsService.Scope.Spreadsheets);
                        if (IsDriveScope)
                            scopes.Add(DriveService.Scope.Drive);

                        m_task = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets.Secrets, scopes, "user", CancellationToken.None);
                    }
                }

                Close();
            }

            public UserCredential GetCredential()
            {
                if (m_task is Task<UserCredential> task && task.IsCompleted)
                    return task.Result;

                return null;
            }

            protected override void OnClosed(EventArgs e)
            {
                base.OnClosed(e);

                Instances.ActiveCanvas.Refresh();
            }
        }
    }
}
