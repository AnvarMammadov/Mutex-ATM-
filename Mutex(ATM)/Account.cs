using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Fullname { get; set; }
        public long CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}