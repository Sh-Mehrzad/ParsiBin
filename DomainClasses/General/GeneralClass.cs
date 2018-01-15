using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses.General
{
    //Interface
    /// <summary>
    /// {
    ///     void GeneralClass();
    ///     int method(int a);
    /// }
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GeneralClass<T>
    {
        public GeneralClass()
        {
            if (CreatedOn == null || !CreatedOn.HasValue)
            {
                CreatedOn = DateTime.Now;
            }
            this.IsEnabled = true;
        }

        public virtual T ID { get; set; }

        [DisplayName("قابل نمایش باشد")]
        public bool IsEnabled { get; set; }

        [DisplayName("حذف")]
        public bool IsDeleted { get; set; }

        [DisplayName("تاریخ ثبت")]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdatedOn { get; set; }
    }
}
