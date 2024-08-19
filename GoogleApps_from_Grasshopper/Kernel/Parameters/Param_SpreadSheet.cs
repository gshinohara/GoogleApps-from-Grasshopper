using Goograsshopper.Kernel.Types;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Goograsshopper.Kernel.Parameters
{
    public class Param_SpreadSheet : GH_PersistentParam<GH_SpreadSheet>
    {
        public Param_SpreadSheet() : base("SpreadSheet", "SpreadSheet", "", null, null)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public override Guid ComponentGuid => new Guid("859D1602-9D69-4275-AA7C-76210387D332");

        protected override GH_GetterResult Prompt_Singular(ref GH_SpreadSheet value)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_SpreadSheet> values)
        {
            throw new NotImplementedException();
        }
    }
}
