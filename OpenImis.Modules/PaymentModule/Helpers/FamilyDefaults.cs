using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Helpers
{
    public class FamilyDefaults
    {
        public int Adults { get; set; }
        public int Children { get; set; }
        public int OtherAdults { get; internal set; }
        public int OtherChildren { get; internal set; }
    }
}
