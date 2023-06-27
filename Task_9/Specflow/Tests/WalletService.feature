Feature: Wallet Service Tests

A short summary of the feature

#TC 1,2,15
Scenario: TC_1_2_15_Get balance new not active user without transaction
	Given get not active user id
	When get user balance
	Then get user balance response Status is 'InternalServerError'
	And get user balance response Content is 'not active user'

#TC3
Scenario: TC_3_Get balance not exist user
	Given get not exist user id
	When get user balance
	Then get user balance response Status is 'InternalServerError'
	And get user balance response Content is 'not active user'
	
#TC 4, 5, 7, 8, 10, 12, 13, 
Scenario: T4_5_7_8_10_12_13_Get user balance with one or some transactions and some positive overall balance 
	Given get active user id
	When balance charge <Amount>
	And get user balance
	Then get user balance response Status is 'Ok'
	And user balance should be <ExpectedBalance>
Examples: 
| Amount                | ExpectedBalance |
| 20,30,-50             | 0               |
| 20,1.01,-10,-11       | 0.01            |
| 10,9999989.99, -10,10 | 9999999.99      |
| 100, 9999900          | 10000000        |
| 0.01                  | 0.01            |
| 9999999.99            | 9999999.99      |
| 10000000              | 10000000        |

#TC 6, 9, 11, 14 
Scenario: T6_9_11_14_Get user balance with one or some transactions and some negative overall balance
	Given get active user id
	When balance charge <Amount>
	And get user balance
	Then get user balance response Status is 'Ok'
	And user balance should be <ExpectedBalance>
Examples: 
| Amount                 | ExpectedBalance |
| 1, -1.01               | 1               |
| 10,20,-10,-10000020.01 | 20              |
| -0.01                  | 0               |
| -10000000.01           | 0               |


#TC16
Scenario: T16_Get user balance after revert
	Given get active user id
	And balance charge <Amount>
	And revert transaction with <revertAmount>
	When get user balance
	Then get user balance response Status is 'Ok'
	And user balance should be <ExpectedBalance>
Examples: 
| Amount         | revertAmount | ExpectedBalance |
| 100,-0.01, 100 | -0.01        | 200             |

#TC17
Scenario: T17_Get transaction for active user without any transactions
	Given get active user id
	When get transaction
	Then get transaction response Status is 'Ok'
	And count of transactions should be 0

#TC18,19
Scenario: T18_19_Get transaction for user with one ore some transactions
	Given get active user id
	And balance charge <Amount>
	When get transaction
	Then get transaction response Status is 'Ok'
	And count of transactions should be <expectedCount>
Examples: 
| Amount | expectedCount |
| 100    | 1             |
| 100,2  | 2             |

#TC21
Scenario: T21_Get transaction and check all fields
	Given get active user id
	And balance charge 100
	And get charge transaction id with 100 amount
	When get transaction
	Then check all get transaction response fields

#TC22
Scenario: T22_Make revert and get transaction - count is transaction is two
	Given get active user id
	And balance charge <Amount>
	When revert transaction with <Amount>
	And get transaction
	Then check get transaction response fields after revert
Examples: 
| Amount |
| 100    |

#TC30
Scenario: T30_Get transaction for not active user
	Given get not active user id
	When get transaction
	Then get transaction response Status is 'Ok'
	And count of transactions should be 0

#TC31
Scenario: T31_Get transaction for not exsist user
	Given get not exist user id
	When get transaction
	Then get transaction response Status is 'Ok'
	And count of transactions should be 0

#TC33
Scenario: T33_Revert transaction with wrong id
	When  revert wrong transaction
	Then revert transaction response Status is 'NotFound'
	And revert transaction response Content is: <ExpectedMessage>
Examples: 
| ExpectedMessage                                  |
| The given key was not present in the dictionary. |

#TC34,36,39
Scenario: T34_36_39_Revert transaction with some amount
	Given get active user id
	And balance charge <Amount>
	When revert transaction with <Amount>
	And get user balance
	Then revert transaction response Status is 'Ok'
	And user balance should be <expectedBalance>
Examples: 
| Amount    | expectedBalance |
| 0.01      | 0               |
| 10000000  | 0               |
| 999999.99 | 0               |

#TC35,40
Scenario: T35_40_Balance is N and revert transaction with some amount
	Given get active user id
	And balance charge <Amount>
	And revert transaction with <revertAmount>
	When get user balance
	Then revert transaction response Status is 'Ok'
	And user balance should be <ExpectedBalance>
Examples: 
| Amount         | revertAmount | ExpectedBalance |
| 100,-0.01, 100 | -0.01        | 200             |

#TC37
Scenario: T37_Revert transaction with 10kk more amount
	Given get active user id
	And balance charge 20, -10
	And revert transaction with 20
	And balance charge 10000000.01
	When revert transaction with 10000000.01
	And get user balance
	Then revert transaction response Status is 'Ok'
	And user balance should be -10

#TC38
Scenario: T38_Revert of revert
	Given get active user id
	And balance charge 20
	When revert transaction with 20
	And revert of revert transaction with 20
	And get user balance
	Then revert transaction response Status is 'Ok'
	And user balance should be 20

#TC41,42
Scenario: T41_42_CreateFewTransactionAndRevertSomeOfThem
	Given get active user id
	And balance charge <Amount>
	When revert some transaction with <revertAmount>
	And get user balance
	Then revert transaction response Status is 'Ok'
	And user balance should be <expectedBalance>
Examples: 
| Amount              | revertAmount        | expectedBalance |
| 1,1,1               | 1,1                 | 1               |
| 100,100,100,200,300 | 100,100,100,200,300 | 0               |

#TC43
Scenario: T43_Charge balance for not active user
	Given get not active user id
	When balance charge 
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: not active user

#TC44
Scenario: T44_Charge balance for not exist user
	Given get not exist user id
	When balance charge 
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: not active user

#TC46
Scenario: T46_Charge 0 amount
	Given get active user id
	When balance charge 0
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: Amount cannot be '0'

#TC47
Scenario: T47_Balance 0 charge amount more than max sum
	Given get active user id
	When balance charge 10000000.01
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: <ExpectedMessage>
Examples: 
| ExpectedMessage                                                                      |
| After this charge balance could be '10000000.01', maximum user balance is '10000000' |

#TC48
Scenario: T48_Charge amount with precision two
	Given get active user id
	When balance charge 0.001
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: <ExpectedMessage>
Examples: 
| ExpectedMessage                                      |
| Amount value must have precision 2 numbers after dot |

#TC49
Scenario: T49_One transaction with some positive amount
	Given get active user id
	When balance charge 0.01
	And get user balance
	Then charge balance response Status is 'Ok'
	And user balance should be 0.01

#TC45,50,53,56
Scenario: T45_50_53_56_Balance N and charge some amount - result balance is positive
	Given get active user id
	When balance charge <Amount>
	And get user balance
	Then charge balance response Status is 'Ok'
	And user balance is more than 0
Examples: 
| Amount          |
#45
| 100, -50, 0.01  |
#50
| 100, -50, -0.01 |
#53
| 55,30           |
| 55,-30          |
#56
| 100, 10, 100    |
| 100, 10, -100   |

#TC51
Scenario: T51_Balance is some negative amount and charge amount to get max sum
	Given get active user id
	And balance charge 2000,-1000
	And revert transaction with 2000
	When balance charge 10001000
	And get user balance
	Then charge balance response Status is 'Ok'
	And user balance should be 10000000

#TC52
Scenario: T52_Balance is some negative amount and charge amount to get more than max sum
	Given get active user id
	And balance charge 2000,-1000
	And revert transaction with 2000
	When balance charge 10001000.01
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: <ExpectedMessage>
	Examples: 
	| ExpectedMessage                                                                      |
	| After this charge balance could be '10000000.01', maximum user balance is '10000000' |

	#TC54,55
Scenario: T54_55_Charge balance with overall negative result
	Given get active user id
	When balance charge <Amount>
	Then charge balance response Status is 'InternalServerError'
	And charge balance response Content is: <ExpectedMessage>
Examples: 
| Amount    | ExpectedMessage                              |
#54
| 20,30,-70 | User have '50.0', you try to charge '-70.0'. |
#55
| -30       | User have '0', you try to charge '-30.0'.    |