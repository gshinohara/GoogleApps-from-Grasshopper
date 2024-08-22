using Google.Apis.Sheets.v4.Data;
using Goograsshopper.Components.Abstracts;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Goograsshopper.Components
{
    public class Component_ReadSpreadSheets : SpreadSheetAccessors
    {
        private readonly Param_String m_AdditionalInput = new Param_String
        {
            Name = "Range",
            NickName = "R",
            Description = @"The range representing positions, in A1 notation, of cells you will read.",
            Access = GH_ParamAccess.item,
        };

        private readonly Param_GenericObject m_AdditionalOutput = new Param_GenericObject
        {
            Name = "Cells",
            NickName = "C",
            Description = "The values of cells you read.",
            Access = GH_ParamAccess.list,
        };

        private readonly Param_Integer m_AdditionalOutput_RowCount = new Param_Integer
        {
            Name = "Rows of Range",
            NickName = "R",
            Description = "The number of rows lying on the range.",
            Access = GH_ParamAccess.item,
        };

        public Component_ReadSpreadSheets()
          : base("Read SpreadSheets", "Read",
              "",
              Settings.Category, Settings.SubCat_SpreadSheets)
        {
            ValuesChanged();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("SheetId", "ID", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_SpreadSheet(), "SpreadSheet", "SS", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string id = default;
            DA.GetData(0, ref id);

            Spreadsheet ss = GetSheetsService().Spreadsheets.Get(id).Execute();

            DA.SetData(0, ss);

            if (Params.Input.Contains(m_AdditionalInput) && Params.Output.Contains(m_AdditionalOutput))
            {
                string range = default;
                DA.GetData(m_AdditionalInput.Name, ref range);

                ValueRange valueRange = GetSheetsService().Spreadsheets.Values.Get(id, range).Execute();

                DA.SetDataList(m_AdditionalOutput.Name, valueRange.Values.SelectMany(i => i));
                DA.SetData(m_AdditionalOutput_RowCount.Name, valueRange.Values.Count);
            }
        }

        protected override void ValuesChanged()
        {
            if (GetValue("IsReaCells", false)) 
            {
                Message = "Read Cells";

                if (!Params.Input.Contains(m_AdditionalInput) && !Params.Output.Contains(m_AdditionalOutput) && !Params.Output.Contains(m_AdditionalOutput_RowCount))
                {
                    Params.RegisterInputParam(m_AdditionalInput);
                    Params.RegisterOutputParam(m_AdditionalOutput);
                    Params.RegisterOutputParam(m_AdditionalOutput_RowCount);
                }
            }
            else
            {
                Message = "Ignore Cells";

                if (Params.Input.Contains(m_AdditionalInput) && Params.Output.Contains(m_AdditionalOutput) && Params.Output.Contains(m_AdditionalOutput_RowCount))
                {
                    Params.UnregisterInputParameter(m_AdditionalInput, true);
                    Params.UnregisterOutputParameter(m_AdditionalOutput, true);
                    Params.UnregisterOutputParameter(m_AdditionalOutput_RowCount, true);
                }
            }

            ExpireSolution(true);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            EventHandler onClick = (sender, e) =>
            {
                SetValue("IsReaCells", !GetValue("IsReaCells", false));
            };

            Menu_AppendItem(menu, "Read cells mode", onClick, true, GetValue("IsReaCells", false));
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("870FB2D4-D00F-4CD9-BC2B-1CC875876829");
    }
}