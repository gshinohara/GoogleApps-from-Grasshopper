using Google.Apis.Services;
using Goograsshopper.Components.Initializers;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using System;
using System.Linq;

namespace Goograsshopper.Components.Abstracts
{
    public abstract class AbstractAccessors<TService> : GH_Component
        where TService : BaseClientService
    {
        private Guid m_ConnectedId;

        internal GoogleAuthorize Parent
        {
            get => OnPingDocument().FindObject<GoogleAuthorize>(m_ConnectedId, true);
            set
            {
                m_ConnectedId = value.InstanceGuid;
                value.ConnectedIds.Add(InstanceGuid);
            }
        }

        public AbstractAccessors(string name, string nickname, string description, string category, string subcategory) : base(name, nickname, description, category, subcategory)
        {
        }

        public void ClearParent()
        {
            m_ConnectedId = Guid.Empty;
        }

        protected abstract TService GetService();

        public override void AddedToDocument(GH_Document document)
        {
            base.AddedToDocument(document);

            GoogleAuthorize googleAuthorize = OnPingDocument().Objects
                .Where(obj => obj is GoogleAuthorize)
                .OrderBy(obj => GH_GraphicsUtil.Distance(Attributes.Pivot, (obj.Attributes as GoogleAuthorize_Attributes).Grip))
                .FirstOrDefault() as GoogleAuthorize;

            if (googleAuthorize != null && googleAuthorize.Credential != null)
                Parent = googleAuthorize;
        }
    }
}