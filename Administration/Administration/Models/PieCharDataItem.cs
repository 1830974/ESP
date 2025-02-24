using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Models
{
    public class PieChartDataItem
    {
        public string Title { get; set; }
        public double Value { get; set; }

        public PieChartDataItem() 
        {
            Title = "";
            Value = 0.00d;
        }

        public PieChartDataItem(string title, double value) 
        { 
            Title = title;
            Value = value;
        }
    }
}
