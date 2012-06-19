namespace SimpleCQRS.Events
open System

type Event = obj

type IEventPublisher =
    abstract member Publish : Event -> unit

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
