using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.IO;

namespace CherwellTest.Controllers
{
    public class ImageController : ApiController
    {
        [System.Web.Http.HttpGet]
        [Route("api/image")]
        public HttpResponseMessage ConvertHTMLToImage()
        {
            string filePath = HostingEnvironment.MapPath("~/test.jpg");

            var result = new HttpResponseMessage(HttpStatusCode.OK);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                Image image = Image.FromStream(fileStream);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.Jpeg);

                    result.Content = new ByteArrayContent(memoryStream.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                }
            }
            return result;
        }
    }
}
