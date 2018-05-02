using MHP2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MH
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan tiempoEjecucion;
              string ruta = "C:/Users/Julio/Desktop/MH/MH/MHP2/data/" + Console.ReadLine();


            QAP qap = new QAP(ruta);



            Stopwatch sw = new Stopwatch();


            sw.Start();
            AGG aggpos = new AGG(ruta);
            sw.Stop();

            tiempoEjecucion = sw.Elapsed;


            Console.WriteLine("Solucion AG: " + aggpos.GetBestSolution().CalcularCoste());
            Console.WriteLine("Tiempo ejecución: " + tiempoEjecucion);


            Console.ReadKey();



      
        }
    }
}
