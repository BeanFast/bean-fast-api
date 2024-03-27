using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public class KidsBMIContrants
    {
        public class Index
        {
            public double Average { get; set; }
            public double MinusTwoSD { get; set; }
            public double PlusTwoSD { get; set; }
        }
        public class KidsBMI
        {
            public Index Height { get; set; }
            public Index Weight { get; set; }
        }
        public static Dictionary<double, KidsBMI> FemaleBmis = new Dictionary<double, KidsBMI>()
         {
            {6, new KidsBMI{ Height = new Index{MinusTwoSD =1.061, Average = 1.16, PlusTwoSD=1.258}, Weight = new Index{ MinusTwoSD = 15.9, Average = 20.5, PlusTwoSD=27.1} } },
            {6.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.087, Average = 1.189, PlusTwoSD=1.291}, Weight = new Index{ MinusTwoSD = 16.8, Average = 21.7, PlusTwoSD=28.9} } },
            {7, new KidsBMI{ Height = new Index{MinusTwoSD =1.112, Average = 1.217, PlusTwoSD=1.323}, Weight = new Index{ MinusTwoSD = 17.7, Average = 22.9, PlusTwoSD=30.7} } },
            {7.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.136, Average = 1.245, PlusTwoSD=1.355}, Weight = new Index{ MinusTwoSD = 18.6, Average = 24.1, PlusTwoSD=32.6} } },
            {8, new KidsBMI{ Height = new Index{MinusTwoSD =1.16, Average = 1.273, PlusTwoSD=1.386}, Weight = new Index{ MinusTwoSD = 19.5, Average = 25.4, PlusTwoSD=34.7} } },
            {8.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.183, Average = 1.299, PlusTwoSD=1.416}, Weight = new Index{ MinusTwoSD = 20.4, Average = 26.7, PlusTwoSD=34.7} } },
            {9, new KidsBMI{ Height = new Index{MinusTwoSD =1.205, Average = 1.326, PlusTwoSD=1.446}, Weight = new Index{ MinusTwoSD = 21.3, Average = 28.1, PlusTwoSD=39.4} } },
            {9.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.228, Average = 1.352, PlusTwoSD=1.476}, Weight = new Index{ MinusTwoSD = 22.2, Average = 29.6, PlusTwoSD=42.1} } },
            {10, new KidsBMI{ Height = new Index{MinusTwoSD =1.25, Average = 1.378, PlusTwoSD=1.505}, Weight = new Index{ MinusTwoSD = 23.2, Average = 31.2, PlusTwoSD=45} } },
        };
        public static Dictionary<double, KidsBMI> MaleBmis = new Dictionary<double, KidsBMI>()
        {
            {6, new KidsBMI{ Height = new Index{MinusTwoSD =1.049, Average = 1.151, PlusTwoSD=1.254}, Weight = new Index{ MinusTwoSD = 15.3, Average = 20.2, PlusTwoSD=27.8} } },
            {6.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.074, Average = 1.18, PlusTwoSD=1.286}, Weight = new Index{ MinusTwoSD = 16, Average = 21.2, PlusTwoSD=29.6} } },
            {7, new KidsBMI{ Height = new Index{MinusTwoSD =1.099, Average = 1.208, PlusTwoSD=1.317}, Weight = new Index{ MinusTwoSD = 16.8, Average = 22.4, PlusTwoSD=31.4} } },
            {7.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.124, Average = 1.237, PlusTwoSD=1.349}, Weight = new Index{ MinusTwoSD = 17.6, Average = 23.6, PlusTwoSD=33.5} } },
            {8, new KidsBMI{ Height = new Index{MinusTwoSD =1.15, Average = 1.266, PlusTwoSD=1.382}, Weight = new Index{ MinusTwoSD = 18.6, Average = 25, PlusTwoSD=35.8} } },
            {8.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.176, Average = 1.295, PlusTwoSD=1.414}, Weight = new Index{ MinusTwoSD = 19.6, Average = 26.6, PlusTwoSD=38.3} } },
            {9, new KidsBMI{ Height = new Index{MinusTwoSD =1.203, Average = 1.325, PlusTwoSD=1.447}, Weight = new Index{ MinusTwoSD = 20.8, Average = 28.2, PlusTwoSD=41} } },
            {9.5, new KidsBMI{ Height = new Index{MinusTwoSD =1.23, Average = 1.355, PlusTwoSD=1.481}, Weight = new Index{ MinusTwoSD = 20, Average = 30, PlusTwoSD=43.8} } },
            {10, new KidsBMI{ Height = new Index{MinusTwoSD =1.258, Average = 1.386, PlusTwoSD=1.514}, Weight = new Index{ MinusTwoSD = 23.3, Average = 31.9, PlusTwoSD=46.9} } },
        };
    }

}
