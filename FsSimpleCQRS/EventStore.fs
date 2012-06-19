namespace SimpleCQRS
open System
open SimpleCQRS.Events

type IEventStore =
    abstract member GetEventsForAggregate : Guid -> Event list
    abstract member SaveEvents : Guid -> int -> Event list -> unit


type EventStore (publisher : IEventPublisher) =
    let (|NextVersion|_|) eventDescriptor =
        Some(eventDescriptor, eventDescriptor |> List.rev |> List.head |> fst)
    let mutable events = new Map<Guid,(int * Event) list>(Seq.empty)
    
    member this.GetEventsForAggregate aggregateId =
        events.Item aggregateId |> List.map snd
    
    member this.SaveEvents id expectedVersion newEvents =
        let eventsWithIds = 
            newEvents |>
            List.mapi (fun i e -> (i+expectedVersion, e) )
        match events.TryFind id with
        | None ->
            events <- events |> Map.add id eventsWithIds
        | Some(NextVersion(eventDescriptors, expectedVersion)) ->
                events <- events |> 
                    Map.remove id |> 
                    Map.add id (eventDescriptors @ eventsWithIds) 
        | _ -> raise (Exception "Concurrency problem")
        newEvents |> Seq.iter publisher.Publish