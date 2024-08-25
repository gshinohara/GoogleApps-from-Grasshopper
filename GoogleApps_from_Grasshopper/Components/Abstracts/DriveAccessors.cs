using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace Goograsshopper.Components.Abstracts
{
    public abstract class DriveAccessors : AbstractAccessors<DriveService>
    {
        public DriveAccessors(string name, string nickname, string description, string category, string subcategory) : base(name, nickname, description, category, subcategory)
        {
        }

        protected override DriveService GetService()
        {
            DriveService service = new DriveService(new BaseClientService.Initializer() { HttpClientInitializer = Parent.Credential });
            return service;
        }
    }
}
