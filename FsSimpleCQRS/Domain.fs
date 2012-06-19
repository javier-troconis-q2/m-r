namespace SimpleCQRS
open System
open SimpleCQRS.Events

module InventoryItem =
    type State = 
        {
            Id: Guid
            Activated: bool
        }

    let fire o =
        [o :> Event]

    let rename newName s =
        if String.IsNullOrEmpty(newName) then raise (ArgumentException "newName")
        fire {InventoryItemRenamed.Id= s.Id; NewName = newName}

    let remove count s =
        if count <= 0 then raise (InvalidOperationException "cant remove negative count from inventory")
        fire {ItemsRemovedFromInventory.Id = s.Id; Count = count } 

    let checkIn count s =
        if count <= 0 then raise (InvalidOperationException "must have a count greater than 0 to add to inventory")
        fire {ItemsCheckedInToInventory.Id= s.Id; Count = count } 
    
    let deactivate s =
        if not s.Activated then raise (InvalidOperationException "already deactivated")
        fire {InventoryItemDeactivated.Id = s.Id}

    let create id name =
        fire {InventoryItemCreated.Id = id; Name = name}

    let applyOnInventoryItem s (e: Event) =
        match e with
        | :? InventoryItemCreated as e -> {Id = e.Id; Activated = true } 
        | :? InventoryItemDeactivated as e -> {s with Activated = false; }
        | _ -> s

    let load application =
        let empty = { Id = Guid.Empty; Activated = false} 
        Seq.fold application empty
