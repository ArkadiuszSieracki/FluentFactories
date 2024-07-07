Feature: Sut Chain Example is a small enricher, it is adding its own world to sentence and execute next enricher


Scenario: Building manually Hello World enricher
    Given Hallo world enrichers manual assemblies
	When Executing enrich method
	Then the result should be "Hello World!"

Scenario: Building automatically Hello World enricher
    Given Hallo world enricher with Autofac
	When Executing enrich method
	Then the result should be "Hello World!"