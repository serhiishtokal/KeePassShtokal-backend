using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassShtokal.Contracts.Response
{
    public class ErrorModel
    {
        public string FieldName { get; set; }

        public string Message { get; set; }
    }
}
