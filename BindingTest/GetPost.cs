using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace BindingTest
{
    public static class GetPost
    {
        private static HttpClient _client = new HttpClient();

        [FunctionName("GetPost")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, [DummyBinding]TraceWriter log)
        {
            var rawPost = await _client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1");
            var post = JsonConvert.DeserializeObject<Post>(rawPost);
            return new OkObjectResult(post.Body);
        }
    }

    public class asd : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            AssemblyLoadContext.Default.Resolving += Default_Resolving;
        }

        private Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            var path = new FileInfo(GetType().Assembly.Location).Directory.FullName;

            return Assembly.LoadFile(Path.Combine(path, "Newtonsoft.Json.dll"));
        }
    }

    [Binding]
    public class DummyBindingAttribute : Attribute { }

    public class Post
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
