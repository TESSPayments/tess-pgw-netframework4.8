using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessPgw.Core.Enums
{
    public enum Actions
    {
        AUTHENTICATION,
        REFUND,
        VOID,
        RECURRING,
        RETRY_RECURRING,
        INQUIRY_BY_PAYMENT_ID,
        INQUIRY_BY_ORDER_ID,
        CALLBACK_NOTIFICATION
    }
}
