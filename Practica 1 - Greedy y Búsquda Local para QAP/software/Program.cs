using System;
using System.Collections;
using System.Collections.Generic;


namespace MH
{
    class Program
    {
        static void Main(string[] args)
        {

            string ruta = "./" + args[0];

            Console.WriteLine(ruta);

            QAP qap = new QAP(ruta);

            MedirTiempos mt = new MedirTiempos(qap);

            Console.WriteLine();


            Console.WriteLine("Tiempo Greedy: " + mt.GetTiemposGreedy()[0]);

            Console.WriteLine("Coste Greedy: " + mt.GetCostesGreedy()[0]);

            Console.WriteLine("Solución Greedy: ");
            
            foreach( int i in mt.GetSolucionesGreedy()[0] )
            {
                Console.Write(i+" ");
            }

            Console.WriteLine();


            Console.WriteLine("Tiempo BL: " + mt.GetTiemposBL()[0]);

            Console.WriteLine("Coste BL: " + mt.GetCostesBL()[0]);

            Console.WriteLine("Solución BL: ");
            
            foreach( int i in mt.GetSolucionesBL()[0] )
            {
                Console.Write(i+" ");
            }

            Console.WriteLine();
        




      
        }
    }
}
