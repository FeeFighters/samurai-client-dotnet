using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samurai
{
    public enum TransactionType
    {
        Purchase,
        Authorize,
        Capture,
        Void,
        Credit
    }
}
