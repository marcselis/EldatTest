
using System.Collections.Generic;

namespace Autohmation.Models
{
    public class Lamp
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Transferral> Actions { get; } = new List<Transferral>();
    }
}
