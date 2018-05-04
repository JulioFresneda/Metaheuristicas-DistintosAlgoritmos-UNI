using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace MH
{
    public class QAP
    {
        static Random r = new Random(2);
        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades = new List<List<int>>();
        private List<List<int>> distanciasLocalizaciones = new List<List<int>>();
        private int coste { get; set; }


        public QAP( QAP copia )
        {
            tamProblema = copia.tamProblema;
            localizacionesEnUnidades.AddRange(copia.localizacionesEnUnidades);
            flujosUnidades.AddRange(copia.flujosUnidades);
            distanciasLocalizaciones.AddRange(copia.distanciasLocalizaciones);
            coste = copia.coste;
        }

        public QAP(int t, List<int> lu, List<List<int>> fu, List<List<int>> dl)
        {
            tamProblema = t;
            localizacionesEnUnidades.AddRange(lu);
            flujosUnidades.AddRange(fu);
            distanciasLocalizaciones.AddRange(dl);
            coste = CalcularCoste();
        }
       
        public QAP( string ruta )
        {
            Lector lector = new Lector(ruta);
            tamProblema = lector.GetTam();

            flujosUnidades = ListToMatrix(lector.GetFlujos(),tamProblema);
            distanciasLocalizaciones = ListToMatrix(lector.GetDistancias(),tamProblema);
       

            List<int> numerosordenados = new List<int>();
            for( int i=0; i<tamProblema; i++ ) numerosordenados.Add(i);

            

            while( numerosordenados.Count != 0 )
            {
                int rindex = r.Next(0,numerosordenados.Count);
                localizacionesEnUnidades.Add(numerosordenados[rindex]);
                numerosordenados.RemoveAt(rindex);
            }


            coste = CalcularCoste();

          

        }

        public void SetQAP(QAP copia)
        {
            tamProblema = copia.tamProblema;

            localizacionesEnUnidades.Clear();
            localizacionesEnUnidades.AddRange(copia.localizacionesEnUnidades);

            flujosUnidades.Clear();
            flujosUnidades.AddRange(copia.flujosUnidades);

            distanciasLocalizaciones.Clear();
            distanciasLocalizaciones.AddRange(copia.distanciasLocalizaciones);

            coste = copia.coste;
        }

        public List<List<int>> GetFlujosUnidades()
        {
            return flujosUnidades;
        }

        public List<List<int>> GetDistanciasLocalizaciones()
        {
            return distanciasLocalizaciones;
        }

        public List<int> GetLocalizacionesEnUnidades()
        {
            return localizacionesEnUnidades;
        }

        public void SetLocalizacionesEnUnidades( List<int> lu )
        {
            if( lu.Count == localizacionesEnUnidades.Count ) localizacionesEnUnidades = lu;
     
        }

        public int GetCoste()
        {
            return coste;
        }

        public int CalcularCoste()
        {

            int coste = 0;

            for( int i=0; i<tamProblema; i++ )
            {
                for( int j=0; j<tamProblema; j++ )
                {

                    if( i != j )
                    {
                        coste = coste + flujosUnidades[i][j] * distanciasLocalizaciones[localizacionesEnUnidades[i]][localizacionesEnUnidades[j]];
                    }
                }
            }

            return coste;
        }




        private List<List<int>> ListToMatrix( List<int> a, int ncol )
        {
            List<List<int>> matrix = new List<List<int>>();
            List<int> fila = new List<int>();
   
            for( int i=0; i<a.Count; i++ )
            {
                
                fila.Add(a[i]);
                
                if( (i+1)%ncol==0 && i != 0 )
                {
                    matrix.Add( new List<int>(fila) );

                    fila.Clear();

                }
            }
          
            return matrix;
        }


        public void PrintMatrix( List<List<int>> l )
        {
           
            foreach( List<int> fila in l )
            {
              //  Console.Write(fila.Count);
             
                foreach( int col in fila )
                {
                    Console.Write(col + " " );
                }
                Console.WriteLine("");
               
            }
        }


        public bool IsEquals( QAP qap )
        {
            bool iguales = true;

            for( int i=0; i<localizacionesEnUnidades.Capacity; i++ )
            {
                if (localizacionesEnUnidades[i] != qap.GetLocalizacionesEnUnidades()[i]) iguales = false;
            }

            return iguales;
        }





        









        public int GetTamProblema() => tamProblema;


    }
}