using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DomainClasses
{
    public class verifyUser
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid VerifyCode { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string Explain { get; set; }
    }
}
