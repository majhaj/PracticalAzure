using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace BlobTriggerIsolatedN
{
    public record ToDoDto(string Description);

    public class ToDoFunctions
    {
        private readonly ToDoStore _toDoStore;
        private ILogger<ToDoFunctions> _logger;

        public ToDoFunctions(ILoggerFactory loggerFactory, ToDoStore toDoStore)
        {
            _logger = loggerFactory.CreateLogger<ToDoFunctions>();
            _toDoStore = toDoStore;
        }

        [Function("GetTodos")]
        [OpenApiOperation(operationId: "getToDos", Summary = "Get all todos", Description = "Gets all todos (completed and not completed)")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ToDo>), Summary ="successful operation", Description ="successful operation")]
        public async Task<HttpResponseData> GetToDos([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "todos")]
            HttpRequestData request)
        {
            _logger.LogInformation("Getting todos");
            var response = request.CreateResponse(HttpStatusCode.OK);

            var allToDos = _toDoStore.GetToDos();
            await response.WriteAsJsonAsync(allToDos);
            
            return response;
        }

        [Function("CreateToDo")]
        [OpenApiOperation(operationId: "createToDos", Summary = "Creates a new todo", Description = "Creates a new todo from a description")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ToDoDto), Required = true, Description = "Description of a new todo")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Created, Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> CreateToDo([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "todos")]
            HttpRequestData request)
        {
            _logger.LogInformation("Creating todo");
            var response = request.CreateResponse(HttpStatusCode.Created);

            var dto = await request.ReadFromJsonAsync<ToDoDto>();

            _toDoStore.AddToDo(dto.Description);

            return response;
        }

        [Function("MarkToDoCompleted")]
        [OpenApiOperation(operationId: "markToDoCompleted", Summary = "Marks todo as completed", Description = "Marks todo as completed based on id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary ="Todo id", Description = "Todo id to complete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent, Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> MarkToDoCompleted([HttpTrigger(AuthorizationLevel.Anonymous, "PATCH", Route = "todos/{id}/completed")]
            HttpRequestData request, Guid id)
        {
            _logger.LogInformation("Marking todo as completed");
            var response = request.CreateResponse(HttpStatusCode.NoContent);

            _toDoStore.MarkCompleted(id);

            return response;
        }


    }
}
