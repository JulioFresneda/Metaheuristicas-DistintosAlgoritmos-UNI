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

            

            Console.WriteLine("Introduzca semilla: ");
            int semilla = Console.ReadKey().KeyChar - 48;
            Console.WriteLine("Semilla: " + semilla);



            Random rand = new Random(semilla);

            

            Console.WriteLine("Introduzca el nombre de la base de datos a cargar (debe estar en el mismo directorio que el ejecutable): ");
            string ruta = "./"+ Console.ReadLine();

            Console.WriteLine("Se ejecutarán todos los algoritmos para estos datos");


            AGG agg = new AGG(ruta,rand);
            Console.WriteLine("Coste AGG-Pos: " + agg.GetBestSolution().GetCoste());


            agg = new AGG(ruta, rand,true);
            Console.WriteLine("Coste AGG-Pmx: " + agg.GetBestSolution().GetCoste());


            AGE age = new AGE(ruta, rand);
            Console.WriteLine("Coste AGE-Pos: " + age.GetBestSolution().GetCoste());


            age = new AGE(ruta, rand,true);
            Console.WriteLine("Coste AGE-Pmx: " + age.GetBestSolution().GetCoste());


            AM am = new AM(ruta, rand,num_Cromosomas:10);
            Console.WriteLine("Coste AM-10,1: " + am.GetBestSolution().GetCoste());

            am = new AM(ruta, rand,num_Cromosomas: 10,_porcentajeMeme:0.1f);
            Console.WriteLine("Coste AM-10,0.1: " + am.GetBestSolution().GetCoste());

            am = new AM(ruta, rand, num_Cromosomas: 10, _porcentajeMeme: 0.1f, _mejorPorcentajeMeme:true);
            Console.WriteLine("Coste AM-10,0.1mej: " + am.GetBestSolution().GetCoste());


        }
    }
}
