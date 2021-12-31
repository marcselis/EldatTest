using System.Collections.Generic;
using System.Linq;
using Autohmation.Domain;
using Autohmation.Models;
using Lamp = Autohmation.Models.Lamp;

namespace Autohmation.Mappers
{
    public class LampMapper : IMapper<Domain.Lamp,Lamp>
    {
        public Lamp Map(Domain.Lamp source)
        {
            Transferral action;
            switch (source.State)
            {
                case LampState.On:
                    action = new Transferral { Description = "Turn off", Verb="POST", Link=$"lamps/{source.Name}/turnoff"};
                    break;
                default:
                    action = new Transferral {Description = "Turn on", Verb="POST", Link = $"lamps/{source.Name}/turnon"};
                    break;
            }
            return new Lamp {Name=source.Name,Description=source.Description, Actions = { action}};
        }

        public IEnumerable<Lamp> Map(IEnumerable<Domain.Lamp> source)
        {
            return source.Select(Map);
        }
    }
}
