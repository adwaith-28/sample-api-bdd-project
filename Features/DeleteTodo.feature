Feature: Delete a Todo Item

Verify that the API allows deleting a todo item

@tag1
Scenario: Delete a todo item
	Given the Todo API is available
	And a todo item with title "Delete test" is created
	When I send a DELETE request for that todo item
	Then the response status should be 204
	And a GET request to the item should return status 404
