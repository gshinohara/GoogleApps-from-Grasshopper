using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace Goograsshopper.Components.Abstracts
{
    public abstract class SpreadSheetsAccessors : AbstractAccessors<SheetsService>
    {
        public SpreadSheetsAccessors(string name, string nickname, string description, string category, string subcategory) : base(name, nickname, description, category, subcategory)
        {
        }

        protected override SheetsService GetService()
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = Parent.Credential });
            return service;
        }
    }
}
