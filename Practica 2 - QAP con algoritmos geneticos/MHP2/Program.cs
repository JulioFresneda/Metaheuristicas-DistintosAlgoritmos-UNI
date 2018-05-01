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
            AG agg = new AG(ruta,true);
            Console.WriteLine("Solucion AGG: " + agg.GetBestSolution().CalcularCoste());


            Console.ReadKey();



      
        }
    }
}
