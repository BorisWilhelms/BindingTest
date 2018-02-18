using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BindingTest
{
    public static class GetPost
    {
        private static HttpClient _client = new HttpClient();

        [FunctionName("GetPost")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            var rawPost = await _client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1");
            var post = JsonConvert.DeserializeObject<Post>(rawPost);
            return new OkObjectResult(post.Body);
        }
    }

    public class Post
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
