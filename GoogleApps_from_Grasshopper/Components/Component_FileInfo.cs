using Google.Apis.Drive.v3.Data;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Goograsshopper.Components
{
    public class Component_FileInfo : GH_Component, IGH_VariableParameterComponent
    {
        public Component_FileInfo()
          : base("File Info", "Info",
              "",
              Settings.Category, Settings.SubCat_Drive)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_File(), "File", "F", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(default, default, default, default);
            pManager.AddGenericParameter(default, default, default, default);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            File file = null;
            DA.GetData("File", ref file);

            if (Params.Output.Any(p => p.Name == "Name"))
                DA.SetData("Name", file.Name);
            if (Params.Output.Any(p => p.Name == "Type"))
                DA.SetData("Type", file.MimeType);
            if (Params.Output.Any(p => p.Name == "Folder ID"))
                DA.SetData("Folder ID", file.Parents.FirstOrDefault());
            if (Params.Output.Any(p => p.Name == "ID"))
                DA.SetData("ID", file.Id);
            if (Params.Output.Any(p => p.Name == "URL"))
                DA.SetData("URL", file.WebViewLink);
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Output && index == Params.Output.Count && index < m_AdditionalParams.Count;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Output && index < Params.Output.Count && index > 1;
        }

        private List<IGH_Param> m_AdditionalParams = new List<IGH_Param>
        {
            new Param_String
            {
                Name = "Name",
                NickName = "N",
                Description = "Name of the file or folder.",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "Type",
                NickName = "T",
                Description = "Represented with MIME types. Please check out details in the document (https://developers.google.com/drive/api/guides/mime-types).",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "Folder ID",
                NickName = "FID",
                Description = "ID of a folder containing the file.",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "ID",
                NickName = "ID",
                Description = "ID of the file.",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "URL",
                NickName = "URL",
                Description = "Link to the file.",
                Access = GH_ParamAccess.item,
            },
        };

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output)
                return m_AdditionalParams[index];
            return null;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            for (int i = 0; i < Params.Output.Count; i++)
                Params.Output[i] = m_AdditionalParams[i];
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);

            EventHandler onClick = (sender, e) =>
            {
                foreach (IGH_Param p in m_AdditionalParams)
                {
                    if (!Params.Output.Contains(p))
                        Params.RegisterOutputParam(p);
                }

                ExpireSolution(true);
            };

            ToolStripMenuItem item_Expand = new ToolStripMenuItem("Expand all Params", null, onClick);

            menu.Items.Add(item_Expand);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("9C369B5D-84E6-4454-AAA6-ECAA08FB6686");
    }
}