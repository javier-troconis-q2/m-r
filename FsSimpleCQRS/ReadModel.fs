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

type IReadModelFacade =
    abstract member GetInventoryItems : unit -> IEnumerable<InventoryItemListDto>
    abstract member GetInventoryItemDetails : Guid -> InventoryItemDetailsDto


type DataBase() =
    static let list = new List<InventoryItemListDto>()
    static let details = new Dictionary<Guid, InventoryItemDetailsDto>()
    static member List = list
    static member Details = details

type ReadModelFacade() =
    interface IReadModelFacade with
        member x.GetInventoryItems() = DataBase.List :> IEnumerable<InventoryItemListDto>
        member x.GetInventoryItemDetails id = DataBase.Details.[id]

type InventoryListView() =
    member x.Handle (e: InventoryItemCreated) =
        DataBase.List.Add({ Id = e.Id; Name = e.Name })
    
    member x.Handle (e: InventoryItemRenamed) =
        let item = DataBase.List.Find(fun x -> x.Id = e.Id)
        item.Name <- e.NewName
    
    member x.Handle (e: InventoryItemDeactivated) =
        DataBase.List.RemoveAll (fun x -> x.Id = e.Id) |> ignore


type InventoryItemDetailView() =
    let find id = DataBase.Details.[id]
    member x.Handle (e: InventoryItemCreated) =
        DataBase.Details.Add(e.Id, {Id = e.Id; Name = e.Name; CurrentCount = 0; Version= 0})
    
    member x.Handle (e: InventoryItemRenamed) =
        let item = find e.Id
        item.Name <- e.NewName
    
    member x.Handle (e: ItemsRemovedFromInventory) =
            let item = find e.Id
            item.CurrentCount <- item.CurrentCount - e.Count
    
    member x.Handle (e: ItemsCheckedInToInventory) =
            let item = find e.Id
            item.CurrentCount <- item.CurrentCount + e.Count
    
    member x.Handle (e: InventoryItemDeactivated) =
            DataBase.Details.Remove(e.Id) |> ignore
