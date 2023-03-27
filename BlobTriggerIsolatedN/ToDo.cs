using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobTriggerIsolatedN
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; }
    }
}
