using System.ServiceModel;
using System.ServiceModel.Web;

namespace CarisbrookeOpenFileService.Services
{
    [ServiceContract]
    interface IOpenFileService
    {
        [WebInvoke(UriTemplate = "/openfile", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped), CorsEnabled]
        string OpenFile(string value);

        [WebInvoke(UriTemplate = "/openfolder", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped), CorsEnabled]
        string OpenFolder(string value);
    }
}
