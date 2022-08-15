using Shared.Domain.SeedWork;
using Shared.Util.Result;
using System;

namespace MKT.Integration.Domain.AggregateModels.Products
{
    public class Product : Entity, IAggregateRoot
    {
        private Product()
        {

        }
            

        public Result UpdateCatalog()
        {
            Approved = true;
            Amount = 10; 
            UpdatedAt = DateTimeOffset.Now;
            DataRequisition = DateTimeOffset.Now;

            return Result.Ok();
        }

        public string Description { get; private set; }
        public string Mark { get; private set; }
        public bool Approved { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset? DataRequisition { get; set; }
    }
}

