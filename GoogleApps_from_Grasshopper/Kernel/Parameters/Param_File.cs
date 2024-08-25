using Goograsshopper.Kernel.Types;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Goograsshopper.Kernel.Parameters
{
    public class Param_File : GH_PersistentParam<GH_File>
    {
        public Param_File() : base("File", "File", "", null, null)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        public override Guid ComponentGuid => new Guid("EAD2C3DB-A013-4A92-8FEC-26E35C1F031C");

        protected override GH_GetterResult Prompt_Singular(ref GH_File value)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_File> values)
        {
            throw new NotImplementedException();
        }
    }
}
