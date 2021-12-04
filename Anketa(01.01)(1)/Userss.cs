using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anketa_01._01__1_
{
    public partial class users
    {
        public bool TooOld { get => (DateTime.Now - dr).Days >= 365; }
    }
}
