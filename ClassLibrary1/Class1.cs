using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public static class Func
    {
        public static double AgeAVG(DateTime[] mas)
        {
            int i = 0;
            double sum = 0;


            foreach (DateTime c in mas)
            {
                sum += ((DateTime.Now - c).TotalDays) / 365;
            }

            sum = Math.Round(sum / mas.Length, 2);
            return sum;
        }
        public static List<string> strname(List<string> name, string str)
        {
            name = name.Where(x => x.Contains(str)).ToList();
            return name;
        }
    }
}
