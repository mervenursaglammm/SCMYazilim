using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public class Result<T> where T : class
    {
        public List<string> ErrorMessages { get; set; }
        public T result { get; set; }

        public Result()
        {
            ErrorMessages = new List<string>();
        }
    }
}
