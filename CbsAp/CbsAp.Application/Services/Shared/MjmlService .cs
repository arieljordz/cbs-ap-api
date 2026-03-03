using CbsAp.Application.Abstractions.Services.Shared;
using Mjml.Net;

namespace CbsAp.Application.Services.Shared
{
    public class MjmlService : IMjmlService
    {
        private readonly IMjmlRenderer _mjmlRenderer;

        public MjmlService()
        {
            _mjmlRenderer = new MjmlRenderer();
        }
        public string ConvertMjmlToHtml(string mjmlTemplate)
        {
            var result = _mjmlRenderer.Render(mjmlTemplate);

            if (!result.Errors.Any())
                return result.Html;

           throw new Exception("MJML conversion failed: " + string.Join(", ", result.Errors));
        }
    }
}