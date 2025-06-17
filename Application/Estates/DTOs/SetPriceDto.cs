using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.DTOs
{
    public class SetPriceDto
    {
        public Guid EstatesId { get; set; }
        public long PricePerMeter { get; set; }
        public long Price{ get; set; }
    }
}
