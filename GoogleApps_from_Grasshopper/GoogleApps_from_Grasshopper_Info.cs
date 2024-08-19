using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Goograsshopper
{
    public class GoogleApps_from_Grasshopper_Info : GH_AssemblyInfo
    {
        public override string Name => "Goograsshopper";

        public override Bitmap Icon => null;

        public override string Description => "Helper components of Google API";

        public override Guid Id => new Guid("225826b3-27b7-4fa9-a96e-fa1b5832c1dc");

        public override string AuthorName => "Gaku Shinohara";

        public override string AuthorContact => "https://github.com/gshinohara";
    }

    public static class Settings
    {
        public static string Category => "Google";

        public static string SubCat_App => "App";

        public static string SubCat_SpreadSheets => "SpreadSheets";
    }
}