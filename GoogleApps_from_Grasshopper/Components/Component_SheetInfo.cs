using Google.Apis.Sheets.v4.Data;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Goograsshopper.Components
{
    public class Component_SheetInfo : GH_Component, IGH_VariableParameterComponent
    {
        public Component_SheetInfo()
          : base("Sheet Info", "Info",
              "",
              Settings.Category, Settings.SubCat_SpreadSheets)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Sheet(), "Sheet", "S", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(default, default, default, default);
            pManager.AddGenericParameter(default, default, default, default);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Sheet sheet = null;
            DA.GetData("Sheet", ref sheet);


            if (Params.Output.Any(p => p.Name == "Name"))
                DA.SetData("Name", sheet.Properties.Title);
            if (Params.Output.Any(p => p.Name == "Hidden"))
                DA.SetData("Hidden", sheet.Properties.Hidden);
            if (Params.Output.Any(p => p.Name == "ID"))
                DA.SetData("ID", sheet.Properties.SheetId);
            if (Params.Output.Any(p => p.Name == "Index"))
                DA.SetData("Index", sheet.Properties.Index);
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Output && index == Params.Output.Count && index < m_AdditionalParams.Count;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Output && index < Params.Output.Count && index > 0;
        }

        private List<IGH_Param> m_AdditionalParams = new List<IGH_Param>
        {
            new Param_String
            {
                Name = "Name",
                NickName = "N",
                Access = GH_ParamAccess.item,
            },
            new Param_Boolean
            {
                Name = "Hidden",
                NickName = "H",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "ID",
                NickName = "ID",
                Access = GH_ParamAccess.item,
            },
            new Param_Integer
            {
                Name = "Index",
                NickName = "i",
                Access = GH_ParamAccess.item,
            }
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

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("235034E1-A8BD-482E-8DD3-C2CDAD5711BE");
    }
}