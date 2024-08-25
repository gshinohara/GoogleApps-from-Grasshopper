using Google.Apis.Drive.v3.Data;
using Grasshopper.Kernel.Types;
using System;

namespace Goograsshopper.Kernel.Types
{
    public class GH_File : GH_Goo<File>
    {
        public GH_File() : base()
        {
        }

        public GH_File(File file) : base(file)
        {
        }

        public GH_File(GH_File file) : base(file)
        {
        }

        public override string TypeName => "File or Folder";

        public override string TypeDescription => "File of Folder of Google Drive";

        public override bool IsValid => throw new NotImplementedException();

        public override string ToString()
        {
            return "File";
        }

        public override IGH_Goo Duplicate()
        {
            return new GH_File(this);
        }

        public override bool CastFrom(object source)
        {
            switch (source)
            {
                case File file:
                    Value = file;
                    return true;
                default:
                    return false;
            }
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (Value == null)
                return false;

            if (typeof(Q).IsAssignableFrom(typeof(File)))
            {
                target = (Q)(object)Value;
                return true;
            }

            return false;
        }
    }
}
