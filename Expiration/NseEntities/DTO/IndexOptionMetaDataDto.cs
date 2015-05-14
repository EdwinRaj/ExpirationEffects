using System;

namespace Nse.Entities.DTO
{
    public class IndexOptionMetaDataDto
    {
        public string DerivativeType { get; set; }
        public string Symbol { get; set; }
        public string ExpiryYear { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
