Feature: User Service Tests

A short summary of the feature

#TC 1-7
Scenario Outline: T1-7 New valid user should be register
	When register valid user with <condition>
	Then register new user response Status is 'Ok'
	
	Examples:
| condition                  |
| emptyFields                |
| nullFields                 |
| length1SymbolFields        |
| length100MoreSymbolsFields |
| upperCaseFields            |
| digitFields                |
| specialCharactersFields    |

#TC 8
Scenario: T8 New registred valid users should be return autoincremented user ID
	When register 3 valid users
	Then user id should be incremented

#TC 9
Scenario: T9 Register new user after deleting of last user - new user id is incremented 
	When get not active user id
	And delete user
	And register valid user
	Then delete user response Status is 'Ok'
	And register new user response Status is 'Ok'
	And new user id after deleting should be incremented

#TC 10
Scenario: T10 Get not exist user status  - status code is not found
	Given get not exist user id
	When get user status
	Then get user status response Status is 'NotFound'
	And get user status response Content is 'Specified argument was out of the range of valid values. (Parameter 'cannot find user with this id')'

#TC 11
Scenario: T11 Default user status should be false
Given get not active user id
When get user status
Then user isActive status should be 'false'

#TC 12
Scenario: T12 Check changed true user status
Given get not active user id
When set <isActive> user status
And get user status
Then get user status response Status is 'Ok'
And user isActive status should be 'true'
Examples:
| isActive |
| true     |

#TC 13
Scenario: T13 Check changed false user status
Given get not active user id
When set <isActive> user status
And get user status
Then get user status response Status is 'Ok'
And user isActive status should be 'false'
Examples:
| isActive     |
| true - false |

#TC 14
Scenario: T14 Set user status for not exist user
Given get not exist user id
When set <isActive> user status
Then set user status response Status is 'NotFound'
And set user status response Content is 'Specified argument was out of the range of valid values. (Parameter 'cannot find user with this id')'
Examples:
| isActive |
| true     |

#TC15, 17, 19
Scenario: T15-17-19 Set user status - user status is true
Given get not active user id
When set <isActive> user status
Then set user status response Status is 'Ok'
And user isActive status should be 'true'
Examples:
| isActive            |
| true                |
| true - false - true |
| true - true         |

#TC16, 18
Scenario: T16-18 Set user status - user status is false
Given get not active user id
When set <isActive> user status
Then set user status response Status is 'Ok'
And user isActive status should be 'false'
Examples:
| isActive     |
| true - false |
| false        |

#TC 20
Scenario: T20 Delete not active user
Given get not active user id
When delete user
Then delete user response Status is 'Ok'

#TC 21
Scenario: T21 Delete not exsist user
Given get not exist user id
When delete user
Then delete user response Status is 'InternalServerError'
And delete user response Content is 'Sequence contains no elements'
