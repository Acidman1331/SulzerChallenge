using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulzerAirlines.Application.PriceCalculation;

public class TimeFactorRulesOptions
{
    public List<TimeFactorRule> Rules { get; set; } = new();
    public decimal DefaultFactor { get; set; }
}

public class TimeFactorRule
{
    public int StartHour { get; set; }
    public int EndHour { get; set; }
    public decimal Factor { get; set; }
}