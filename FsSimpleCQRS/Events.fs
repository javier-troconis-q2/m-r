namespace SimpleCQRS.Events
open System

type Event = obj

type EventMetadata =
    {
        Version : int
    }

type InventoryItemDeactivated =
    {
        Id: Guid
    }
    

type InventoryItemCreated =
    {
        Id: Guid
        Name: string
    }

type InventoryItemRenamed =
    {
        Id: Guid
        NewName: string
    }

type ItemsCheckedInToInventory =
    {
        Id: Guid
        Count: int
    }

type ItemsRemovedFromInventory =
    {
        Id: Guid
        Count: int
    }
