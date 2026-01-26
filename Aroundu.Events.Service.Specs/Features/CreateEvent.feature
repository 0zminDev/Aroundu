Feature: Create Event
    As a user
    I want to create local events
    So that I can meet people around me

@tag1
Scenario: Successfully creating a valid event
    Given I am an authenticated user
    When I create an event with name "Arvato Hackathon"
    Then The event creation should be successful
    And The event PublicKey should be returned