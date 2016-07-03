Feature: SaveNote
	As a user, I want to be able to 
	view a note that I've appended
	to a task.

Scenario: View note appended to task
	Given I have a task
	And the task has a note
	When I select the task
	Then I can see the note
