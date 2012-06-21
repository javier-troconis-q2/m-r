namespace SimpleCQRS.Commands
open System

type Command =
    interface
    end

type ICommandSender =
    abstract member Send : Command -> unit

type DeactivateInventoryItem =
    {
        InventoryItemId: Guid
        OriginalVersion: int
    }
    interface Command

type CreateInventoryItem =
    {
        InventoryItemId: Guid
        Name: string
    }
    interface Command

type RenameInventoryItem =
    {
        InventoryItemId: Guid
        NewName: string
        OriginalVersion: int
    }
    interface Command

type CheckInItemsToInventory =
    {
        InventoryItemId: Guid
        Count: int
        OriginalVersion: int
    }
    interface Command

type RemoveItemsFromInventory =
    {
        InventoryItemId: Guid
        Count: int
        OriginalVersion: int
    }
    interface Command
