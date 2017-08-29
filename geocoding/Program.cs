using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
namespace geocoding
{
    class longlat
    {
        public double longitude;
        public double latitude;
    }
    class geocode
    {
        public long postal;
        public double longitude;
        public double latitude;
        public string address;
    }
    class Program
    {
        static int mode = -1;
        float dist = 0;
        static geocode[] geocodes;
        public double distance(longlat a,longlat b)
        {
            return Math.Sqrt(Math.Pow(a.longitude - b.longitude,2)
                           + Math.Pow(a.latitude - b.latitude,2));
        }
        static public double distance(double long1, double lat1, double long2, double lat2)
        {
            return Math.Sqrt(Math.Pow(long1 - long2, 2)
                           + Math.Pow(lat1 - lat2, 2));
        }
        static public geocode getgeocode(double longi, double lat)
        {
            geocode code = null;
            double dist = 100000;
            for (int i = 0; i < geocodes.Length; i++)
            {
                double d = distance(geocodes[i].longitude, geocodes[i].latitude, longi, lat);
                if (d < dist)
                {
                    dist = d;
                    code = geocodes[i];
                }
            }
            return code;
        }
        static public geocode getgeocode(long postal)
        {
            geocode code = null;
            for (int i = 0; i < geocodes.Length; i++)
            {
                if (postal == geocodes[i].postal)
                    return geocodes[i];
            }
            return code;
        }
        static public void printgeocode(geocode g = null)
        {
            if (g == null)
                return;
            System.Console.WriteLine(g.postal);
            System.Console.WriteLine(g.address);
            System.Console.WriteLine(g.longitude);
            System.Console.WriteLine(g.latitude);
        }
        static void Main(string[] args)
        {
            //string[] args = { "geocoding.exe", "744107" };
            //string[] args = { "geocoding.exe", "23.8754", "83.635"};
            switch (args.Length)
            {
                case 0:
                    {
                        System.Console.WriteLine("Please Insert an input!");
                        System.Console.WriteLine("For Example,");
                        System.Console.WriteLine("geocoding.exe 507318");
                        System.Console.WriteLine("geocoding.exe 23.8754 83.635");
                        return;
                    }
                case 1:
                    mode = 0;
                    break;
                case 2:
                    mode = 1;
                    break;
                default:
                    return;
            }

            //string[] lines = System.IO.File.ReadAllLines(@"G:\27.geocoder\IN.txt");
            string[] lines = System.IO.File.ReadAllLines(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/IN.txt");//@"G:\27.geocoder\IN.txt");
            geocodes=new geocode[lines.Length];

            int i = 0;
            foreach (string line in lines)
            {
                string[] items = Regex.Split(line, "\t");
                //
                geocodes[i] = new geocode();
                geocodes[i].postal = long.Parse(items[1]);
                geocodes[i].longitude = Double.Parse(items[10]);
                geocodes[i].latitude = Double.Parse(items[9]);
                string addr = "";
                for (int j = 2; j < 8; j++)
                {
                    addr += items[j];
                }
                geocodes[i].address = addr;
                i++;
            }
            geocode result = null;
            if (mode == 0)//postal-->longitude,latitude
            {
                result = getgeocode(long.Parse(args[0]));
                
            }
            else
            {
                result = getgeocode(Double.Parse(args[0]), Double.Parse(args[1]));
            }
            printgeocode(result);
        }
    }
}
