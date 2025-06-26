Feature: Update a ToDo Item

Verify that the API allows updating an exisiting todo item

@tag1
Scenario: Update a todo item
	Given the ToDo API is available
	And a todo item with title "Old Title" is created 
	When I send a PUT request to update the item with title "Updated Title"
	Then the response status should be 204
	And a GET request to the item should return title "Updated Title"
