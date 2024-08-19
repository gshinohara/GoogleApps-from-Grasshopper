using Goograsshopper.Kernel.Types;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Goograsshopper.Kernel.Parameters
{
    public class Param_Sheet : GH_PersistentParam<GH_Sheet>
    {
        public Param_Sheet() : base("Sheet", "Sheet", "", null, null)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public override Guid ComponentGuid => new Guid("CF31732F-995A-4CAC-9AD4-B883C2863911");

        protected override GH_GetterResult Prompt_Singular(ref GH_Sheet value)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Sheet> values)
        {
            throw new NotImplementedException();
        }
    }
}
