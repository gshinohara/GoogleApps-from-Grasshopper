using Goograsshopper.Components.Initializers;
using Grasshopper.Kernel;

namespace Goograsshopper.Components.Abstracts
{
    public interface IAbstractAccessors : IGH_Component
    {
        GoogleAuthorize Parent { get; set; }

        void ClearParent();
    }
}
