using Google.Apis.Sheets.v4.Data;
using Grasshopper.Kernel.Types;
using System;

namespace Goograsshopper.Kernel.Types
{
    public class GH_SpreadSheet : GH_Goo<Spreadsheet>
    {
        public GH_SpreadSheet() : base()
        {
        }

        public GH_SpreadSheet(Spreadsheet spreadsheet) : base(spreadsheet)
        {
        }

        public GH_SpreadSheet(GH_SpreadSheet spreadsheet) : base(spreadsheet)
        {
        }

        public override string TypeName => "SpreadSheet";

        public override string TypeDescription => "Google SpreadSheet";

        public override bool IsValid => throw new NotImplementedException();

        public override string ToString()
        {
            return "SpreadSheet";
        }

        public override IGH_Goo Duplicate()
        {
            return new GH_SpreadSheet(this);
        }

        public override bool CastFrom(object source)
        {
            switch (source)
            {
                case Spreadsheet spreadsheet:
                    Value = spreadsheet;
                    return true;
                default:
                    return false;
            }
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (Value == null)
                return false;

            if (typeof(Q).IsAssignableFrom(typeof(Spreadsheet)))
            {
                target = (Q)(object)Value;
                return true;
            }

            return false;
        }
    }
}
