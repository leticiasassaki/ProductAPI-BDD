Feature: Product

Scenario: Add new Product
	Given I input name "Water"
	And I input id 1
	When I request to create product
	Then Validate product "Water" exists
	And Exclude the product with id 1

Scenario: Update Product
	Given The product "Water" with id 1 already exists
	When I request to update product name to "Juice"
	Then Validate product "Juice" exists
	And Exclude the product with id 1

Scenario: Read All Product
	Given The product "Water" with id 1 already exists
	When I request to get all products
	Then Validate product "Water" exists
	And Exclude the product with id 1