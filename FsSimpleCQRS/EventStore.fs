namespace SimpleCQRS
open System
open SimpleCQRS.Events

type IEventStore<'TEvent> =
    abstract member GetEventsForAggregate : Guid -> 'TEvent seq
    abstract member SaveEvents : Guid -> int -> 'TEvent seq -> unit
