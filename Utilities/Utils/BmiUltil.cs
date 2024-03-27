using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Utils
{
    public class BmiUltil
    {
        public static string GetBMIStatus(double currentBMI, bool gender, DateTime birthDay)
        {
            double age = TimeUtil.GetAgeRoundedToHalfYear(birthDay);
            KidsBMIContrants.KidsBMI bmi = null;
            if (!gender)
            {
                bmi = KidsBMIContrants.FemaleBmis[age];
            }
            else
            {
                bmi = KidsBMIContrants.MaleBmis[age];
            }
            
            return "123";
            
        }

    }
}