namespace SimpleCQRS
open System
open SimpleCQRS.Events

type IEventStore =
    abstract member GetEventsForAggregate : Guid -> Event seq
    abstract member SaveEvents : Guid -> int -> Event seq -> unit
