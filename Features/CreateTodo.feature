Feature: Create a ToDo Item

Verify that the API allows creating a new todo item

@tag1
Scenario: Add a new todo item
	Given the ToDo API is available
	When I send a POST request to create a todo item with title "Create Test"
	Then the response status should be 201
	And the response should contain a todo item with title "Create Test"
