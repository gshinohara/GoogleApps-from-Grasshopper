using Google.Apis.Drive.v3.Data;
using Goograsshopper.Components.Abstracts;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
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

                    File file = GetService().Files.Get(id).Execute();

                    DA.SetData("File", file);

                    break;

                case ReadType.FilePath:
                    //TODO
                    break;

                case ReadType.URL:
                    //TODO
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
                    Message = "File Path";
                    Params.RegisterInputParam(m_Input_FilePath);
                    break;
                case ReadType.ID:
                    Message = "ID";
                    Params.RegisterInputParam(m_Input_ID);
                    break;
                case ReadType.URL:
                    Message = "URL";
                    Params.RegisterInputParam(m_Input_URL);
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