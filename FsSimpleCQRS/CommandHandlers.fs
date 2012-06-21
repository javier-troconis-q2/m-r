namespace SimpleCQRS
open SimpleCQRS.Commands
open SimpleCQRS.Events
open InventoryItem


type InventoryCommandHandlers (eventStore: IEventStore) =
    let load id = eventStore.GetEventsForAggregate id |> replayWith applyOnInventoryItem
    let save = eventStore.SaveEvents

    // load aggregate, execute f on it, then save
    let applyOn id version f =
        load id |>                  
        f |>
        save id version

    member x.Handle (c: CreateInventoryItem) =
        create c.InventoryItemId c.Name |> 
        save c.InventoryItemId -1
    
    member x.Handle (c: DeactivateInventoryItem) = 
        deactivate |> 
        applyOn c.InventoryItemId c.OriginalVersion
    
    member x.Handle (c: RemoveItemsFromInventory) =
        remove c.Count |> 
        applyOn c.InventoryItemId c.OriginalVersion
    
    member x.Handle (c: CheckInItemsToInventory) =
        checkIn c.Count |> 
        applyOn c.InventoryItemId c.OriginalVersion
    
    member x.Handle (c: RenameInventoryItem) =
        rename c.NewName |> 
        applyOn c.InventoryItemId c.OriginalVersion
