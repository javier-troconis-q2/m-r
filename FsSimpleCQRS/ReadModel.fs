namespace SimpleCQRS

open SimpleCQRS.Events
open System
open System.Collections.Generic

type InventoryItemListDto = 
    {
        Id: Guid
        mutable Name: string 
    }

type InventoryItemDetailsDto =
    {
        Id: Guid
        mutable Name: string
        mutable CurrentCount: int
        mutable Version: int
    }

type IDatabase =
    abstract member Details : Dictionary<Guid, InventoryItemDetailsDto>
    abstract member List : List<InventoryItemListDto>


type InventoryListView(database: IDatabase) =
    member x.Handle (e: InventoryItemCreated) =
        database.List.Add({ Id = e.Id; Name = e.Name })
    
    member x.Handle (e: InventoryItemRenamed) =
        let item = database.List.Find(fun x -> x.Id = e.Id)
        item.Name <- e.NewName
    
    member x.Handle (e: InventoryItemDeactivated) =
        database.List.RemoveAll (fun x -> x.Id = e.Id) |> ignore


type InventoryItemDetailView(database: IDatabase) =
    let find id = database.Details.[id]
    
    member x.Handle (e: InventoryItemCreated, m : EventMetadata ) =
        database.Details.Add(e.Id, {Id = e.Id; Name = e.Name; CurrentCount = 0; Version= m.Version})
    
    member x.Handle (e: InventoryItemRenamed, m : EventMetadata ) =
        let item = find e.Id
        item.Name <- e.NewName
        item.Version <- m.Version
    
    member x.Handle (e: ItemsRemovedFromInventory, m : EventMetadata ) =
        let item = find e.Id
        item.CurrentCount <- item.CurrentCount - e.Count
        item.Version <- m.Version

    member x.Handle (e: ItemsCheckedInToInventory, m : EventMetadata ) =
        let item = find e.Id
        item.CurrentCount <- item.CurrentCount + e.Count
        item.Version <- m.Version
    
    member x.Handle (e: InventoryItemDeactivated) =
        database.Details.Remove(e.Id) |> ignore
