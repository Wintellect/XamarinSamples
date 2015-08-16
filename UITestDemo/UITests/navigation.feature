Feature: Navigation
	Users should be able to go to the detail page

Scenario: Go to detail page
	Given I am in the app
	When I press the detail button
	Then I should be on the detail screen
