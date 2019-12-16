using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using Cnam.UEGLG101.Journey.Data;
using Microsoft.Owin.Hosting;

namespace Cnam.UEGLG101.Journey.App
{
    class Program
    {
        static void Main(string[] args)
        {
            AreaRepository.Current.Areas = DataReader.GetAllAreas();
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.ReadLine();
            }
        }
    }
}