Feature: ToDo API
  Testing basic CRUD operations on the ToDo API hosted on Azure

  Scenario: Add a new ToDo item
    Given the ToDo API is available
    When I send a POST request to create a todo item with title "Test Task"
    Then the response status should be 201
    And the response should contain a todo item with title "Test Task"

  Scenario: Get all ToDo items
    Given the ToDo API is available
    When I send a GET request for all todo items
    Then the response status should be 200
    And the response should contain a list of todo items
