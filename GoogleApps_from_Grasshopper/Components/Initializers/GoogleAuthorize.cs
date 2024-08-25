using Google.Apis.Auth.OAuth2;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Goograsshopper.Components.Initializers
{
    public class GoogleAuthorize : GH_DocumentObject
    {
        private HashSet<Guid> m_ids;

        internal HashSet<Guid> ConnectedIds => m_ids;

        public UserCredential Credential => (Attributes as GoogleAuthorize_Attributes).Credential;

        public GoogleAuthorize() : base("Authorization", "Auth", "", Settings.Category, Settings.SubCat_App)
        {
            m_ids = new HashSet<Guid>();
        }

        public override Guid ComponentGuid => new Guid("3A0B491B-53FD-46BA-9765-A722C950D138");

        public override void CreateAttributes()
        {
            Attributes = new GoogleAuthorize_Attributes(this);
        }
    }
}
