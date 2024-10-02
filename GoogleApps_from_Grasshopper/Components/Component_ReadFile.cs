using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Goograsshopper.Components.Abstracts;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Goograsshopper.Components
{
    public class Component_ReadFile : DriveAccessors
    {
        private enum ReadType
        {
            FilePath,
            ID,
            URL,
        }

        private readonly Param_FilePath m_Input_FilePath = new Param_FilePath
        {
            Name = "File Path",
            NickName = "P",
            Description = "",
            Access = GH_ParamAccess.item,
        };

        private readonly Param_FilePath m_Input_FilePath_Root = new Param_FilePath
        {
            Name = "Root",
            NickName = "R",
            Description = "",
            Access = GH_ParamAccess.item,
        };

        private readonly Param_String m_Input_ID = new Param_String
        {
            Name = "File ID",
            NickName = "ID",
            Description = "",
            Access = GH_ParamAccess.item,
        };

        private readonly Param_String m_Input_URL = new Param_String
        {
            Name = "URL",
            NickName = "URL",
            Description = "",
            Access = GH_ParamAccess.item,
        };

        public Component_ReadFile()
          : base("Read File", "Read",
              "",
              Settings.Category, Settings.SubCat_Drive)
        {
            ValuesChanged();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_File(), "File", "F", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            switch ((ReadType)GetValue("ReadType", (int)ReadType.ID))
            {
                case ReadType.ID:
                    string id = default;
                    DA.GetData("File ID", ref id);

                    FilesResource.GetRequest file_get = GetService().Files.Get(id);
                    file_get.SupportsAllDrives = true;
                    file_get.Fields = "id, name, webViewLink, parents, kind, mimeType";

                    File file = file_get.Execute();

                    DA.SetData("File", file);

                    break;

                case ReadType.FilePath:
                    string filePath = default;
                    string root = default;
                    DA.GetData("Root", ref root);
                    DA.GetData("File Path", ref filePath);

                    DrivesResource.ListRequest driveListRequest = GetService().Drives.List();
                    driveListRequest.PageSize = 100;

                    List<string> pathElements = filePath.Split(System.IO.Path.PathSeparator).ToList();
                    foreach (Drive drive in driveListRequest.Execute().Drives)
                    {
                        if (System.IO.Path.GetFullPath(filePath).Contains(System.IO.Path.Combine(root, drive.Name)))
                        {
                            foreach (string element in System.IO.Path.Combine(root, drive.Name).Split(System.IO.Path.PathSeparator))
                            {
                                if (pathElements[0] == element)
                                    pathElements.Remove(element);
                            }
                            break;
                        }
                    }                 

                    FilesResource.ListRequest fileListRequest = GetService().Files.List();
                    fileListRequest.IncludeItemsFromAllDrives = true;
                    fileListRequest.PageSize = 100;
                    fileListRequest.SupportsAllDrives = true;
                    fileListRequest.Fields = "nextPageToken, files(name, parents)";
                    fileListRequest.Q = $"(name = '{pathElements.LastOrDefault()}') and (trashed = false)";
                  
                    File file_suggested = null;
                    foreach (File file_candidates in fileListRequest.Execute().Files)
                    {
                        bool isbreak = false;

                        string parent_folder_id = file_candidates.Parents.FirstOrDefault();

                        foreach (string element in pathElements.Reverse<string>())
                        {
                            FilesResource.GetRequest parent_folder_get = GetService().Files.Get(parent_folder_id);
                            parent_folder_get.SupportsAllDrives = true;
                            parent_folder_get.Fields = "parents, name";
                            File parent_folder = parent_folder_get.Execute();

                            if (parent_folder.Parents.FirstOrDefault() != null)
                            {
                                if (parent_folder.Name == element)
                                    parent_folder_id = parent_folder.Parents.FirstOrDefault();
                                else
                                    break;
                            }
                               else
                                isbreak = true;
                        }

                        if (isbreak)
                        {
                            file_suggested = file_candidates;
                            break;
                        }
                    }

                    DA.SetData("File", file_suggested);

                    break;

                case ReadType.URL:
                    //TODO
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, @"Sorry for no implemantations, it don't work with the read type.");
                    break;
            }
        }

        protected override void ValuesChanged()
        {
            foreach (IGH_Param p in new List<IGH_Param>(Params.Input))
                Params.UnregisterParameter(p);

            switch ((ReadType)GetValue("ReadType", (int)ReadType.ID))
            {
                case ReadType.FilePath:
                    if (m_Input_FilePath_Root.PersistentData.IsEmpty)
                        m_Input_FilePath_Root.SetPersistentData(@"G:\Shared drives\");

                    Params.RegisterInputParam(m_Input_FilePath_Root);
                    Params.RegisterInputParam(m_Input_FilePath);

                    Message = "File Path";

                    break;

                case ReadType.ID:
                    Params.RegisterInputParam(m_Input_ID);

                    Message = "ID";

                    break;

                case ReadType.URL:
                    Params.RegisterInputParam(m_Input_URL);

                    Message = "URL";

                    break;
            }

            ExpireSolution(true);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            ReadType readType = (ReadType)GetValue("ReadType", (int)ReadType.ID);

            EventHandler onClick = (sender, e) =>
            {
                SetValue("ReadType", (int)(sender as ToolStripMenuItem).Tag);
            };

            ToolStripMenuItem item_FilePath = new ToolStripMenuItem("Read file path", null, onClick)
            {
                Checked = readType == ReadType.FilePath,
                Tag = ReadType.FilePath,
            };

            ToolStripMenuItem item_ID = new ToolStripMenuItem("Read ID", null, onClick)
            {
                Checked = readType == ReadType.ID,
                Tag = ReadType.ID,
            };

            ToolStripMenuItem item_URL = new ToolStripMenuItem("Read URL", null, onClick)
            {
                Checked = readType == ReadType.URL,
                Tag = ReadType.URL,
            };

            menu.Items.Add(item_FilePath);
            menu.Items.Add(item_ID);
            menu.Items.Add(item_URL);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("35A8ACA8-CEF5-40C0-91CE-D727932A6F39");
    }
}