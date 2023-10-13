# MintFramework
Mint, because it's cool! 
A framework to facilitate using DDD tactical patterns and CQS. 
We will talk about each of the architectural decisions we made, but for the starters here is a list of features of the Mint.

## Features:
- defined backbone data structures of DDD such as Aggregate Root, Entity, Value Object, Repository, and Domain Event.
- defined command bus, event bus, and query bus to use the CQS pattern.
- defined unified Logging service definition to use in framework processes.
- defined how UnitOfWork should commit command and event changes.
- defined application events to enable integration between bounded contexts or microservices
- defined results for application-related processes like command handlers, query handlers, and domain event handlers to encapsulate validation errors or other framework processes.
- defined different results for clients to hide framework processes from clients. 

# Architectural and design decisions
Knowing these decisions is the key to using this framework in a healthy way. If you want to use this framework to make your application, you must know what happens behind the scenes.
## Domain Event Handlers 
The domain event handlers are in the same transaction as the origin command handler. If you need to handle domain events in a different transaction, you can raise an ApplicationEvent. All domain event handlers are assumed to be consistent with the origin command handler. This decision has been made to remove the need for multiple transactions for simple alteration of other aggregates. We know the rule of one-aggregate-one-transaction, but there must be a way to do simple alterations in a sync way when there is no need for complex solutions such as the Saga pattern. 
These situations happen when:

- for one use case there are at least two respondent aggregates in the same bounded context (being in the same bounded context is mandatory). 
- the aggregates have no problem with loading and saving changes in one transaction (no performance issues).
- the aggregates must validate the situation, and any exception that occurs must roll back the transaction (one violation results in rolling back all changes).

So for the need for eventual consistency or any other reasons, you have the possibility of raising ApplicationEvents. 
