Feature: Flyweight factory

Scenario: When building flyweight it is posible with simple registeration
	Given Container builder
	When registering Flyweight circle with shared resources
	And registering Other flyweight square with other shared resources
	Then It is possible to render both shapes

Scenario: When building flyweight it is posible to share objects between flyweight
	Given Container builder
	When registering Flyweight circle with shared resources
	Then It is possible to share some parts of the objects

Scenario: When building flyweight it is share objects are shared between prerequisites of flyweigth
	Given Container builder
	When registering Flyweight circle with shared resources
	Then It is possible to share some parts of the objects