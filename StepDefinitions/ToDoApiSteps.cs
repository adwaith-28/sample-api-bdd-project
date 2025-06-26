using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace ToDoApi.BDDTests.StepDefinitions
{
    [Binding]
    public class ToDoApiSteps
    {
        private readonly ScenarioContext _context;
        private RestClient _client;
        private RestResponse _response;
        private string _baseUrl = "https://todo-api-yourname.azurewebsites.net/todos";
        private JObject _createdItem;
        private int _createdItemId;

        public ToDoApiSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"the ToDo API is available")]
        public void GivenTheToDoAPIIsAvailable()
        {
            _client = new RestClient(_baseUrl);
        }

        [When(@"I send a GET request for all todo items")]
        public async Task WhenISendAGETRequestForAllTodoItems()
        {
            var request = new RestRequest("", Method.Get);
            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"the response should contain a list of todo items")]
        public void ThenTheResponseShouldContainAListOfTodoItems()
        {
            var items = JsonConvert.DeserializeObject<List<object>>(_response.Content);
            Assert.IsTrue(items.Count >= 0);
        }

        [When(@"I send a POST request to create a todo item with title ""(.*)""")]
        public async Task WhenISendAPOSTRequestToCreateATodoItemWithTitle(string title)
        {
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(new { title = title, isComplete = false });

            _response = await _client.ExecuteAsync(request);
            _createdItem = JObject.Parse(_response.Content);
            _createdItemId = (int)_createdItem["id"];
            _context["CreatedItemId"] = _createdItemId;
        }

        [Then(@"the response should contain a todo item with title ""(.*)""")]
        public void ThenTheResponseShouldContainATodoItemWithTitle(string title)
        {
            Assert.IsNotNull(_createdItem);
            Assert.AreEqual(title, _createdItem["title"]?.ToString());
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expected)
        {
            Assert.AreEqual(expected, (int)_response.StatusCode);
        }

        [Given(@"a todo item with title ""(.*)"" is created")]
        public async Task GivenATodoItemWithTitleIsCreated(string title)
        {
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(new { title = title, isComplete = false });
            var response = await _client.ExecuteAsync(request);

            var item = JObject.Parse(response.Content);
            _createdItemId = (int)item["id"];
            _context["CreatedItemId"] = _createdItemId;
        }

        [When(@"I send a GET request for that todo item")]
        public async Task WhenISendAGETRequestForThatTodoItem()
        {
            var request = new RestRequest($"{_context["CreatedItemId"]}", Method.Get);
            _response = await _client.ExecuteAsync(request);
            _createdItem = JObject.Parse(_response.Content);
        }

        [When(@"I send a PUT request to update the item with title ""(.*)""")]
        public async Task WhenISendAPUTRequestToUpdateTheItemWithTitle(string newTitle)
        {
            var request = new RestRequest($"{_context["CreatedItemId"]}", Method.Put);
            request.AddJsonBody(new { id = _context["CreatedItemId"], title = newTitle, isComplete = true });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"a GET request to the item should return title ""(.*)""")]
        public async Task ThenAGETRequestToTheItemShouldReturnTitle(string expectedTitle)
        {
            var request = new RestRequest($"{_context["CreatedItemId"]}", Method.Get);
            var getResponse = await _client.ExecuteAsync(request);

            var item = JObject.Parse(getResponse.Content);
            Assert.AreEqual(expectedTitle, item["title"]?.ToString());
        }

        [When(@"I send a DELETE request for that todo item")]
        public async Task WhenISendADELETERequestForThatTodoItem()
        {
            var request = new RestRequest($"{_context["CreatedItemId"]}", Method.Delete);
            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"a GET request to the item should return status 404")]
        public async Task ThenAGETRequestToTheItemShouldReturnStatus404()
        {
            var request = new RestRequest($"{_context["CreatedItemId"]}", Method.Get);
            var getResponse = await _client.ExecuteAsync(request);
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
