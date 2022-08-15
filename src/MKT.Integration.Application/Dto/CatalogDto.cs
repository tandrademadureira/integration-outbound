using System;

namespace MKT.Integration.Application.Dto
{
    public class CatalogDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Mark { get; set; }
        public bool Approved { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset DataRequisition { get; set; }
        public bool Authenticated { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Roles { get; set; }
        public string CorrelationId { get; set; }
        
        public DefaultRequestHeaders DefaultRequestHeaders { get; set; }
    }

    public class DefaultRequestHeaders
    { 
    }
}
