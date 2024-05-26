using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFoody.Application.UseCases.Promotion.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class SelectUserPromotionDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ApplyType { get; set; }
        public decimal AmountRate { get; set; }
        public decimal AmountValue { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public decimal MaximumApplyValue { get; set; }

        public int UsageLimit { get; set; }
        public int NumberOfUsed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
        
        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int TotalPages { get; set; }
    }

}
