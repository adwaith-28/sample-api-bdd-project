Feature: Get a specific ToDo Item

Verify retrieval of a todo item by ID

@tag1
Scenario: Retrieve a todo item by ID
	Given the ToDo API is available
	And a todo item with title "Get Test" is created
	When I send a GET request for that todo item
	Then the response status should be 200
