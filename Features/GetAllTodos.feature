Feature: Get All Todo Items

Verify that the API returns all exisiting todo items

@tag1
Scenario: Retrieve all todo items 
	Given the ToDo API is available
	When I send a GET request for all todo items
	Then the response status should be 200
	And the response should contain a list of todo items
