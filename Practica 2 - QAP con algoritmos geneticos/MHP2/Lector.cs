using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;


namespace MH
{
    public class Lector
    {
        private int tamProblema;
        private List<int> flujos = new List<int>();
        private List<int> distancias = new List<int>();


        public Lector()
        {
            tamProblema = 0;
        }

        public Lector(string ruta)
        {
            tamProblema = 0;
            System.IO.StreamReader archivo = new System.IO.StreamReader(ruta);

            string datos = archivo.ReadToEnd();
            string nombre = "";

            int i = 0;
  
 

            while( datos[i] != '1' && datos[i] != '2' && datos[i] != '3' && datos[i] != '4' && datos[i] != '5' && datos[i] != '6' && datos[i] != '7' && datos[i] != '8' && datos[i] != '9' && datos[i] != '0' )
            {
                i++;
            }

            while( datos[i] == '1' || datos[i] == '2' || datos[i] == '3' || datos[i] == '4' || datos[i] == '5' || datos[i] == '6' || datos[i] == '7' || datos[i] == '8' || datos[i] == '9' || datos[i] == '0' )
            {
                nombre+=datos[i];
                i++;
            }

            
            tamProblema = Int32.Parse(nombre);
            nombre = "";

            while( datos[i] != '1' && datos[i] != '2' && datos[i] != '3' && datos[i] != '4' && datos[i] != '5' && datos[i] != '6' && datos[i] != '7' && datos[i] != '8' && datos[i] != '9' && datos[i] != '0' )
            {
                i++;
            }


            for( int k = 0; k<tamProblema*tamProblema; k++ )
            {
                 while( datos[i] != '1' && datos[i] != '2' && datos[i] != '3' && datos[i] != '4' && datos[i] != '5' && datos[i] != '6' && datos[i] != '7' && datos[i] != '8' && datos[i] != '9' && datos[i] != '0' )
                {
                    i++;
                }

                while( datos[i] == '1' || datos[i] == '2' || datos[i] == '3' || datos[i] == '4' || datos[i] == '5' || datos[i] == '6' || datos[i] == '7' || datos[i] == '8' || datos[i] == '9' || datos[i] == '0' )
                {
                    nombre+=datos[i];
                    i++;
                }

            
                flujos.Add(Int32.Parse(nombre));
                nombre = "";
            }

            while( datos[i] != '1' && datos[i] != '2' && datos[i] != '3' && datos[i] != '4' && datos[i] != '5' && datos[i] != '6' && datos[i] != '7' && datos[i] != '8' && datos[i] != '9' && datos[i] != '0' )
            {
                i++;
            }

         

            for( int k = 0; k<tamProblema*tamProblema; k++ )
            {
                 while( datos[i] != '1' && datos[i] != '2' && datos[i] != '3' && datos[i] != '4' && datos[i] != '5' && datos[i] != '6' && datos[i] != '7' && datos[i] != '8' && datos[i] != '9' && datos[i] != '0' )
                {
                    i++;
                }

                while( datos[i] == '1' || datos[i] == '2' || datos[i] == '3' || datos[i] == '4' || datos[i] == '5' || datos[i] == '6' || datos[i] == '7' || datos[i] == '8' || datos[i] == '9' || datos[i] == '0' )
                {
                    nombre+=datos[i];
                    i++;
                }


                distancias.Add(Int32.Parse(nombre));
                nombre = "";
            }



        }


        public void Imprimir()
        {
            int i = 0;
            Console.WriteLine(tamProblema);
            foreach( int f in flujos ){
                i++;
                Console.Write(f + " ");
                if( i == tamProblema ){
                    Console.WriteLine("");
                    i = 0;
                } 
            }
 
            i = 0;
            Console.WriteLine("-----------------");
            foreach( int d in distancias ){
                i++;
                Console.Write(d + " ");
                if( i == tamProblema ){
                    Console.WriteLine("");
                    i = 0;
                }
            }
        }



        public List<int> GetDistancias()
        {
            return distancias;
        }

        public List<int> GetFlujos()
        {
            return flujos;
        }

        public int GetTam()
        {
            return tamProblema;
        }


    }
}