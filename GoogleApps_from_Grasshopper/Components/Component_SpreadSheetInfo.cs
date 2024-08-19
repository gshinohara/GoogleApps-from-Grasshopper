using Google.Apis.Sheets.v4.Data;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Goograsshopper.Components
{
    public class Component_SpreadSheetInfo : GH_Component, IGH_VariableParameterComponent
    {
        public Component_SpreadSheetInfo()
          : base("SpreadSheet Info", "Info",
              "",
              Settings.Category, Settings.SubCat_SpreadSheets)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_SpreadSheet(), "SpreadSheet", "SS", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(default, default, default, default);
            pManager.AddGenericParameter(default, default, default, default);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Spreadsheet spreadsheet = null;
            DA.GetData("SpreadSheet", ref spreadsheet);

            if (Params.Output.Any(p => p.Name == "Name"))
                DA.SetData("Name", spreadsheet.Properties.Title);
            if (Params.Output.Any(p => p.Name == "Sheets"))
                DA.SetDataList("Sheets", spreadsheet.Sheets);
            if (Params.Output.Any(p => p.Name == "ID"))
                DA.SetData("ID", spreadsheet.SpreadsheetId);
            if (Params.Output.Any(p => p.Name == "URL"))
                DA.SetData("URL", spreadsheet.SpreadsheetUrl);

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
                Access = GH_ParamAccess.item,
            },
            new Param_Sheet
            {
                Name = "Sheets",
                NickName = "S",
                Access = GH_ParamAccess.list,
            },
            new Param_String
            {
                Name = "ID",
                NickName = "ID",
                Access = GH_ParamAccess.item,
            },
            new Param_String
            {
                Name = "URL",
                NickName = "URL",
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

        public override Guid ComponentGuid => new Guid("E98AE319-A793-441A-931A-DEC783366F9E");
    }
}