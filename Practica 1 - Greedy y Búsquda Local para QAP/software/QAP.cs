using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace MH
{
    public class QAP
    {

        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades, distanciasLocalizaciones;

        private TimeSpan tiempoEjecucion;
       
        public QAP( string ruta )
        {
            Lector lector = new Lector(ruta);
            tamProblema = lector.GetTam();

            flujosUnidades = ListToMatrix(lector.GetFlujos(),tamProblema);
            distanciasLocalizaciones = ListToMatrix(lector.GetDistancias(),tamProblema);
       

            List<int> numerosordenados = new List<int>();
            for( int i=0; i<tamProblema; i++ ) numerosordenados.Add(i);

            Random r = new Random(3);

            while( numerosordenados.Count != 0 )
            {
                int rindex = r.Next(0,numerosordenados.Count);
                localizacionesEnUnidades.Add(numerosordenados[rindex]);
                numerosordenados.RemoveAt(rindex);
            }  

          

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





        public void ResolverGreedy()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<int> potencialFlujoUnidades = new List<int>();
            List<int> potencialDistanciaLocalizaciones = new List<int>();

            ResolverPotenciales(potencialFlujoUnidades,potencialDistanciaLocalizaciones);

            int maxpfu = potencialFlujoUnidades[0];
            int minpdl = potencialDistanciaLocalizaciones[0];

            int indexmaxpfu = 0;
            int indexminpdl = 0;

            for( int i=0; i<tamProblema; i++ )
            {

                maxpfu = -1;
                minpdl = -1;
                
                for( int j=0; j<potencialFlujoUnidades.Count; j++ )
                {
                    if( potencialFlujoUnidades[j] > maxpfu ){
                        maxpfu = potencialFlujoUnidades[j];
                        indexmaxpfu = j;
                        
                    }
                }

                for( int j=0; j<potencialDistanciaLocalizaciones.Count; j++ )
                {
                    if( (minpdl == -1 || potencialDistanciaLocalizaciones[j] < minpdl) && potencialDistanciaLocalizaciones[j]>-1 ){
                        minpdl = potencialDistanciaLocalizaciones[j];
                        indexminpdl = j;
                    }
                }

            
                potencialFlujoUnidades[indexmaxpfu] = -1;
                potencialDistanciaLocalizaciones[indexminpdl] = -1;

                localizacionesEnUnidades[indexmaxpfu] = indexminpdl;

                

            }

            sw.Stop();
       
            tiempoEjecucion = sw.Elapsed;
      

        }


        private void ResolverPotenciales( List<int> pf, List<int> pd )
        {

            for( int j=0; j<tamProblema; j++ )
                {
                    pf.Add(0);
                    pd.Add(0);
                }

            for( int i=0; i<tamProblema; i++ )
            {
                for( int j=0; j<tamProblema; j++ )
                {
                    pf[i] = pf[i] + flujosUnidades[i][j];
                    pd[i] = pd[i] + distanciasLocalizaciones[i][j];
                }
            }
        }



















        private void IntercambiarLocalizaciones( int i, int j, List<int> lista = null )
        {
            if( lista == null )
            {
                int temp = localizacionesEnUnidades[i];
                int temp2 = localizacionesEnUnidades[j];

                localizacionesEnUnidades[j] = temp;
                localizacionesEnUnidades[i] = temp2;
            }
            else
            {
                int temp = lista[i];
                int temp2 = lista[j];

                lista[j] = temp;
                lista[i] = temp2;
            }
        }


        private int CosteMovimiento( int r, int s )
        {
            int coste = 0;

            for( int k=0; k<tamProblema; k++ )
            {
                if( k != r && k != s )
                {
 
                    coste = coste + flujosUnidades[r][k]*(distanciasLocalizaciones[localizacionesEnUnidades[s]][localizacionesEnUnidades[k]]-distanciasLocalizaciones[localizacionesEnUnidades[r]][localizacionesEnUnidades[k]]);                    
                    coste = coste + flujosUnidades[s][k]*(distanciasLocalizaciones[localizacionesEnUnidades[r]][localizacionesEnUnidades[k]]-distanciasLocalizaciones[localizacionesEnUnidades[s]][localizacionesEnUnidades[k]]);
                    coste = coste + flujosUnidades[k][r]*(distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[s]]-distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[r]]);
                    coste = coste + flujosUnidades[k][s]*(distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[r]]-distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[s]]);
   
                }
            }

            return coste;

        }






        public void ResolverBL()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool hayMejora = true;
            bool improveFlag;
            List<bool> dlb = new List<bool>();
            for( int i=0; i<tamProblema; i++ ){ dlb.Add(false);}

            while( hayMejora )
            {
                hayMejora = false;

                for( int i=0; i<tamProblema; i++ )
                {
                    
                    if( !dlb[i] )
                    {
                        improveFlag = false;
                        for( int j=0; j<tamProblema; j++ )
                        {
                            if( i != j ){
                                if( CosteMovimiento(i,j)<0 )
                                {

                                    IntercambiarLocalizaciones(i,j);
                                    dlb[i] = false;
                                    dlb[j] = false;
                                    improveFlag = true;
                                    hayMejora = true;
                                }
                            }
                        }

                        if( improveFlag == false ) dlb[i] = true;
                    }
                }
            }

            sw.Stop();
       
            tiempoEjecucion = sw.Elapsed;
        }



        public TimeSpan GetTiempoEjecucion()
        {
            return tiempoEjecucion;
        }

       
    }
}