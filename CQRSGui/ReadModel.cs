using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleCQRS;

namespace CQRSGui
{
    public interface IReadModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }

    public class ReadModelFacade : IReadModelFacade
    {
        private readonly IDatabase database;

        public ReadModelFacade(IDatabase database)
        {
            this.database = database;
        }

        public IEnumerable<InventoryItemListDto> GetInventoryItems()
        {
            return database.List;
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            return database.Details[id];
        }
    }


    public class BullShitDatabase : IDatabase
    {
        private readonly Dictionary<Guid, InventoryItemDetailsDto> details = new Dictionary<Guid, InventoryItemDetailsDto>();
        private readonly List<InventoryItemListDto> list = new List<InventoryItemListDto>();

        public Dictionary<Guid, InventoryItemDetailsDto> Details
        {
            get { return details; }
        }

        public List<InventoryItemListDto> List
        {
            get { return list; }
        }
    }

}
