using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.General
{
    public abstract class GeneralClassIntID
    {
        public GeneralClassIntID()
        {
            if (CreatedOn == null || !CreatedOn.HasValue)
            {
                CreatedOn = DateTime.Now;
            }
            this.IsEnabled = true;
        }


        public int ID { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}
