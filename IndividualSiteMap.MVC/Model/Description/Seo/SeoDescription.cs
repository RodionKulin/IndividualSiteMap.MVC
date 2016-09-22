using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualSiteMap.MVC
{
    public class SeoDescription
    {
        //свойства
        public UpdateFrequency UpdateFrequency { get; set; }
        public Priority Priority { get; set; }



        //инициализация
        public SeoDescription()
        {
        }
        public SeoDescription(UpdateFrequency updateFrequency, Priority priority)
        {
            UpdateFrequency = updateFrequency;
            Priority = priority;
        }

    }
}
