using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace HelloApi.Functions {
    public class HelloWorldFunction {
        [Function("HelloWorld")]
        async public Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")]HttpRequestData request){
            var response = HttpResponseData.CreateResponse(request);
            await response.WriteStringAsync("Hello User! Thanks for invoking me!");
            return response;

        }
    }
}
