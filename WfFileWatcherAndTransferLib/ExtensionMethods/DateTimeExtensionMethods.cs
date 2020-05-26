using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfFileWatcherAndTransferLib
{
    public static class DateTimeExtensionMethods
    {
        public static string GetYearMonthDay(this DateTime dt)
        {
            return ($"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}");

        }
    }
}
