Feature: LoginLogout
	I can Login to BLR Admin
	And I can Logout from BLR Admin

@admin
Scenario Outline: BLR Admin Portal: Login and Logout
	Given I go to BLR Admin portal
	Then I provide my <username> and <password> and <staysigned> and press Sign in button
	And I verify I see <myusername> and I am logged <logged>
	Then I click logout link and verify I am logged out
Examples: 
	| username | password   | staysigned | myusername			  | logged |
	| spider   | itmagnet03 | false      | SPIDER PROJECT MANAGER | true   |

@ignore
Scenario Outline: BLR Client Portal: Login and Logout 
	Given I go to BLR Client portal
	Then I provide my <username> and <password> and <staysigned> and press Sign in button
	And I verify I see <myusername> and I am logged <logged>
	Then I click logout link and verify I am logged out
Examples: 
	| username | password | staysigned | myusername    | logged |
	| client   | face11   | false      | Client Adidas | true   |

@ignore
Scenario Outline: BLR Supplier Portal: Login and Logout 
	Given I go to BLR Supplier portal
	Then I provide my <username> and <password> and <staysigned> and press Sign in button
	And I verify I see <myusername> and I am logged <logged>
	Then I click logout link and verify I am logged out
Examples: 
	| username | password   | staysigned | myusername     | logged |
	| supplier | itmagnet03 | false      | Supplier Sakib | true   |