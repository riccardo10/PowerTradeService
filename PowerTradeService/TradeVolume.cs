using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace PowerTradeService
{
    [DelimitedRecord(",")]
    internal class TradeVolume
    {
        [FieldCaption("LocalTime")]
        public string? LocalTime { get; set; }

        [FieldCaption("Volume")]
        public double? Volume { get; set; }
    }
}
