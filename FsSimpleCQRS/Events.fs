namespace SimpleCQRS.Events
open System

type Event = 
    interface
    end

type EventMetadata =
    {
        Version : int
    }

type InventoryItemDeactivated =
    {
        Id: Guid
    }
    interface Event
    

type InventoryItemCreated =
    {
        Id: Guid
        Name: string
    }
    interface Event

type InventoryItemRenamed =
    {
        Id: Guid
        NewName: string
    }
    interface Event

type ItemsCheckedInToInventory =
    {
        Id: Guid
        Count: int
    }
    interface Event

type ItemsRemovedFromInventory =
    {
        Id: Guid
        Count: int
    }
    interface Event
