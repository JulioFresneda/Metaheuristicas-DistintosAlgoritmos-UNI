using MHP2;
using System;
using System.Collections;
using System.Collections.Generic;


namespace MH
{
    class Program
    {
        static void Main(string[] args)
        {

            string ruta = "C:/Users/Julio/Desktop/MH/MH/MHP2/data/" + Console.ReadLine();


            QAP qap = new QAP(ruta);
            AM am = new AM(ruta, false, 10, 0.1f, false);
            Console.WriteLine("Solucion AGG: " + am.GetBestSolution().CalcularCoste());


            Console.ReadKey();



      
        }
    }
}
