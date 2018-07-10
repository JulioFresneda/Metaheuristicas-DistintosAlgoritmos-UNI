using MH;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHP3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Introduzca ruta del archivo.");
            string ruta = Console.ReadLine();

            QAP qap = new QAP(ruta);
            sw.Start();
            ES es = new ES(qap);
            sw.Stop();

            Console.WriteLine("ES: " + es.GetQAP().GetCoste() + " - tiempo: " + sw.Elapsed);

            sw.Restart();
            BMB bmb = new BMB(25,ruta);
            sw.Stop();

            Console.WriteLine("BMB: " + bmb.GetQAP().GetCoste() + " - tiempo: " + sw.Elapsed);

            sw.Restart();
            GRASP g = new GRASP(ruta);
            sw.Stop();

            Console.WriteLine("GRASP: " + g.GetQAP().GetCoste() + " - tiempo: " + sw.Elapsed);

            sw.Restart();
            ILS ils = new ILS(ruta);
            sw.Stop();

            Console.WriteLine("ILS: " + ils.GetQAP().GetCoste() + " - tiempo: " + sw.Elapsed);

            sw.Restart();
            ILS_ES ie = new ILS_ES(ruta);
            sw.Stop();

            Console.WriteLine("ILS-ES: " + ie.GetQAP().GetCoste() + " - tiempo: " + sw.Elapsed);

            Console.WriteLine();
            Console.WriteLine("Listo ;)");
            Console.ReadKey();

        }




    }
}
