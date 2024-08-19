using Google.Apis.Sheets.v4.Data;
using Grasshopper.Kernel.Types;
using System;

namespace Goograsshopper.Kernel.Types
{
    public class GH_Sheet : GH_Goo<Sheet>
    {
        public GH_Sheet() : base()
        {
        }

        public GH_Sheet(Sheet sheet) : base(sheet)
        {
        }

        public GH_Sheet(GH_Sheet sheet) : base(sheet)
        {
        }

        public override string TypeName => "Sheet";

        public override string TypeDescription => "Sheet of Google SpreadSheet";

        public override bool IsValid => throw new NotImplementedException();

        public override string ToString()
        {
            return "Sheet";
        }

        public override IGH_Goo Duplicate()
        {
            return new GH_Sheet(this);
        }

        public override bool CastFrom(object source)
        {
            switch (source)
            {
                case Sheet sheet:
                    Value = sheet;
                    return true;
                default:
                    return false;
            }
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (Value == null)
                return false;

            if (typeof(Q).IsAssignableFrom(typeof(Sheet)))
            {
                target = (Q)(object)Value;
                return true;
            }

            return false;
        }
    }
}
