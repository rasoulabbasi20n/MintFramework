# MintFramework
Mint, because it's cool! 
A framework to facilitate using DDD tactical patterns and CQS. 
We will talk about each of the architectural decisions we made, but for the starters here is a list of features of the Mint.

Features:
- defined backbone data structures of DDD such as Aggregate Root, Entity, Value Object, Repository, and Domain Event.
- defined command bus, event bus, and query bus to use the CQS pattern.
- defined unified Logging service definition to use in framework processes.
- defined how UnitOfWork should commit command and event changes.
- defined application events to enable integration between bounded contexts or microservices
- defined results for application-related processes like command handlers, query handlers, and domain event handlers to encapsulate validation errors or other framework processes.
- defined different results for clients to hide framework processes from clients. 
