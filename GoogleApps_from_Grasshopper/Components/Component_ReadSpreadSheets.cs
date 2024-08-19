using Google.Apis.Sheets.v4.Data;
using Goograsshopper.Components.Abstracts;
using Goograsshopper.Kernel.Parameters;
using Grasshopper.Kernel;
using System;

namespace Goograsshopper.Components
{
    public class Component_ReadSpreadSheets : SpreadSheetAccessors
    {
        public Component_ReadSpreadSheets()
          : base("Read SpreadSheets", "Read",
              "",
              Settings.Category, Settings.SubCat_SpreadSheets)
        {
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
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("870FB2D4-D00F-4CD9-BC2B-1CC875876829");
    }
}