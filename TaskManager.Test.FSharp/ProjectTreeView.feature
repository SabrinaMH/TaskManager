Feature: ProjectTreeView

Scenario: Show open tasks for a project
	Given I have a project
	And the project has 2 open tasks
	When I view the project tree view
	Then the project node shows 2 open tasks