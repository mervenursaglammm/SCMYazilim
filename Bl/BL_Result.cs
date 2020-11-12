using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public class BL_Result<T> where T : class
    {
        public T Result { get; set; }
        public List<string> Messages { get; set; }
        public BL_Result()
        {
            Messages = new List<string>();
        }
    }
}
