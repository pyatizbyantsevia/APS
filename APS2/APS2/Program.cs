using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace APS2
{
    internal class Program
    {
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new RtosForm());
            
        }
    }
}
