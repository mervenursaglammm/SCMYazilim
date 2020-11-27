using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Remainder
    {
       public int Id { get; set; }
        public int adminId { get; set; }
        public float remainder { get; set; }
        public DateTime paymentDate { get; set; }
    }
}
