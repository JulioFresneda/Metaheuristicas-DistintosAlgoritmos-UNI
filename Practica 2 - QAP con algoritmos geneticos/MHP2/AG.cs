using MH;
using System;
using System.Collections.Generic;
using System.Text;

namespace MHP2
{
    class AG
    {
        public int numCromosomas { get; }
        public int numGenes { get; }
        public float probCruce { get; }
        public float probMutacion { get; }
        public int numIteraciones { get; }

        public bool pmx { get; }
        public bool estacionario { get; }

        public float numCruces { get; }
        public float numMutaciones { get; }

        private List<QAP> poblacion = new List<QAP>();
        private List<QAP> poblacionP = new List<QAP>();
        private List<QAP> poblacionI = new List<QAP>();
        private List<QAP> poblacionH = new List<QAP>();


        QAP mejorPob;

        public AG( string ruta, bool _pmx = false, bool _estacionario = false, int num_Cromosomas = 50, float prob_Cruce = 0.7f, float prob_Mutacion = 0.001f, int num_Iteraciones = 50000 )
        {
            numCromosomas = num_Cromosomas;

            if (prob_Cruce <= 1f && prob_Cruce >= 0f) probCruce = prob_Cruce;
            else probCruce = 0.7f;

            if (prob_Mutacion <= 1f && prob_Mutacion >= 0f) probMutacion = prob_Mutacion;
            else probMutacion = 0.001f;

            numIteraciones = num_Iteraciones;

            for( int i=0; i<numCromosomas; i++ ) poblacion.Add( new QAP(ruta) );


            pmx = _pmx;
            estacionario = _estacionario;

            numCruces = (probCruce * numCromosomas);
            numGenes = poblacion[0].GetLocalizacionesEnUnidades().Count;
            numMutaciones = (probMutacion * numCromosomas * numGenes);
            





            Evolucionar();
        }




        private void Evolucionar()
        {
            for( int i=0; i<numIteraciones; i++ )
            {
                Console.WriteLine("Iteracion " + i);
                QAP mejor = poblacion[0];
                for (int j = 0; j < poblacion.Count; j++)
                {
                    if (poblacion[j].GetCoste() < mejor.GetCoste()) mejor = poblacion[j];
                }

                mejorPob = new QAP(mejor);



                if (estacionario)
                {
                    SeleccionEstacionario();
                    CruceEstacionario();
                    MutacionEstacionario();
                    ReemplazamientoEstacionario();
                }
                else
                {
                    SeleccionGeneracional();
                    CruceGeneracional();
                    MutacionGeneracional();
                    ReemplazamientoGeneracional();
                }

                      

                Console.WriteLine("Mejor solucion encontrada: " + mejor.GetCoste());

                if (i == 20000)
                {
                    foreach (QAP c in poblacion) Console.Write(c.GetCoste() + " ");
                    Console.ReadKey();
                }
        
            }
        }


        private void SeleccionEstacionario()
        {
            Random r = new Random();

            for (int i = 0; i < 2; i++)
            {
                int r1 = r.Next(0, numCromosomas);
                int r2 = r.Next(0, numCromosomas);

                while (r2 == r1) r2 = r.Next(0, numCromosomas);

                poblacionP.Add(TorneoBinario(r1, r2));



            }
        }





        private void SeleccionGeneracional()
        {
            Random r = new Random();

            for ( int i=0; i<numCromosomas; i++ )
            {
                int r1 = r.Next(0, numCromosomas);
                int r2 = r.Next(0, numCromosomas);

                while( r2 == r1 ) r2 = r.Next(0, numCromosomas);

                poblacionP.Add(TorneoBinario(r1, r2));



            }
        }

        private void CruceEstacionario()
        {
            Tuple<QAP, QAP> cruzados;
            Console.WriteLine("Antes del cruce " + poblacionP[0].GetCoste() + " " + poblacionP[1].GetCoste());
            if (pmx) cruzados = CruzarPMX(0, 1);
            else cruzados = CruzarPosicion(0, 1);
            poblacionI.Add(cruzados.Item1);
            poblacionI.Add(cruzados.Item2);

            Console.WriteLine("Después del cruce " + poblacionI[0].GetCoste() + " " + poblacionI[1].GetCoste());

        }

        private void CruceGeneracional()
        {
            Tuple<QAP, QAP> cruzados;
            for( int i=0; i<numCruces; i+=2 )
            {
                if( pmx ) cruzados = CruzarPMX(i, i + 1);
                else cruzados = CruzarPosicion(i, i + 1);
                poblacionI.Add(cruzados.Item1);
                poblacionI.Add(cruzados.Item2);
            }
            for( int i=(int)numCruces+1; i<numCromosomas; i++ )
            {
                poblacionI.Add(poblacionP[i]);
            }
            
        }

        private void MutacionEstacionario()
        {
            float max = 1 / probMutacion;
            int g1, g2;

            Random r = new Random();
            int c = r.Next(0, (int)max);
            if( c == 1 )
            {
                g1 = r.Next(0, numGenes);
                g2 = r.Next(0, numGenes);
                while (g2 == g1) g2 = r.Next(0, numGenes);

                Mutar(g1, g2, poblacionI[0]);
            }

            c = r.Next(0, (int)max);
            if (c == 1)
            {
                g1 = r.Next(0, numGenes);
                g2 = r.Next(0, numGenes);
                while (g2 == g1) g2 = r.Next(0, numGenes);

                Mutar(g1, g2, poblacionI[1]);
            }


        }

        private void MutacionGeneracional()
        {
            Random r = new Random();
            int c, g1, g2;
            for( int i=0; i<numMutaciones; i++ )
            {
                c = r.Next(0, numCromosomas);
                g1 = r.Next(0, numGenes);
                g2 = r.Next(0, numGenes);
                while ( g2 == g1 ) g2 = r.Next(0, numGenes);

                Mutar(g1, g2, poblacionI[c]);

            }

            
        }

        private void ReemplazamientoEstacionario()
        {
            poblacionH = poblacionI;

            int it1 = 0;
            int it2 = 0;

            QAP peor = poblacion[0];
            for( int i=0; i<poblacion.Count; i++ )
            {
                if( poblacion[i].GetCoste() > peor.GetCoste() )
                {
                    it1 = i;
                    peor = poblacion[i];
                }
            }


            if (it1 != 0) peor = poblacion[0];
            else peor = poblacion[1];
            for (int i = 0; i < poblacion.Count; i++)
            {
                if (poblacion[i].GetCoste() > peor.GetCoste() && i != it1 )
                {
                    it2 = i;
                    peor = poblacion[i];
                }
            }

            if (poblacion[it1].GetCoste() > poblacionH[0].GetCoste())
            {
                poblacion[it1] = new QAP(poblacionH[0]);
                Console.WriteLine("Sustituido cromosoma " + it1 + " por: " + poblacionH[0].GetCoste());
            }
            if (poblacion[it2].GetCoste() > poblacionH[1].GetCoste())
            {
                poblacion[it2] = new QAP(poblacionH[1]);
                Console.WriteLine("Sustituido cromosoma " + it2 + " por: " + poblacionH[1].GetCoste());
            }

                poblacionH.Clear();
            poblacionP.Clear();

        }

        private void ReemplazamientoGeneracional()
        {
            poblacionH = poblacionI;


            QAP peorH = poblacionH[0];
            int it = 0;


            for (int i = 0; i < poblacion.Count; i++)
            {
                if (poblacionH[i].GetCoste() > peorH.GetCoste())
                {
                    peorH = poblacionH[i];
                    it = i;
                }
            }

            poblacionH[it] = new QAP(mejorPob);
            

            poblacion.Clear();
            poblacion.AddRange(poblacionH);
            poblacionH.Clear();
            poblacionP.Clear();


        }




        private void Mutar(int i, int j, QAP qap)
        {
            {
                List<int> lista = qap.GetLocalizacionesEnUnidades();

                int temp = lista[i];
                int temp2 = lista[j];

                lista[j] = temp;
                lista[i] = temp2;
            }
        }


        private Tuple<QAP,QAP> CruzarPMX( int pos1, int pos2 )
        {
            QAP c1 = poblacionP[pos1];
            QAP c2 = poblacionP[pos2];

            List<int> genesc1 = c1.GetLocalizacionesEnUnidades();
            List<int> genesc2 = c2.GetLocalizacionesEnUnidades();

            Random r = new Random();
            int corteDcha = r.Next(1, genesc1.Count);
            int corteIzq = r.Next(0, corteDcha);

            List<int> genesH1 = new List<int>();
            List<int> genesH2 = new List<int>();

            List<int> cadenaCentralH1 = new List<int>();
            List<int> cadenaCentralH2 = new List<int>();

            for ( int i = 0; i<genesc1.Count; i++ )
            {
                if( i < corteIzq || i > corteDcha )
                {
                    genesH1.Add(-1);
                    genesH2.Add(-1);
                }
                else
                {
                    genesH1.Add(genesc2[i]);
                    genesH2.Add(genesc1[i]);

                    cadenaCentralH1.Add(genesc2[i]);
                    cadenaCentralH2.Add(genesc1[i]);
                }
            }

            for( int i=0; i<genesc1.Count; i++ )
            {
                if (i < corteIzq || i > corteDcha)
                {
                    genesH1[i] = genesc1[i];
                    while (cadenaCentralH1.Contains(genesH1[i]))
                    {
                        genesH1[i] = (cadenaCentralH2[cadenaCentralH1.IndexOf(genesH1[i])]);
                    }

                    genesH2[i] = genesc2[i];
                    while (cadenaCentralH2.Contains(genesH2[i]))
                    {
                        genesH2[i] = (cadenaCentralH1[cadenaCentralH2.IndexOf(genesH2[i])]);
                    }
                    
                }
                    
            }


            return new Tuple<QAP, QAP>(new QAP(c1.GetTamProblema(), genesH1, c1.GetFlujosUnidades(), c1.GetDistanciasLocalizaciones(), c1.GetTiempoEjecucion()), new QAP(c2.GetTamProblema(), genesH2, c2.GetFlujosUnidades(), c2.GetDistanciasLocalizaciones(), c2.GetTiempoEjecucion()));



        }



        private Tuple<QAP,QAP> CruzarPosicion( int pos1, int pos2 )
        {
            QAP c1 = poblacionP[pos1];
            QAP c2 = poblacionP[pos2];

            List<int> genesc1 = c1.GetLocalizacionesEnUnidades();
            List<int> genesc2 = c2.GetLocalizacionesEnUnidades();



            List<int> genesComunes = new List<int>();
            List<int> restos = new List<int>();

            for( int i=0; i<c1.GetTamProblema(); i++ )
            {
                if (genesc1[i] == genesc2[i]) genesComunes.Add(genesc1[i]);
                else
                {
                    genesComunes.Add(-1);
                    restos.Add(genesc1[i]);
                }
            }

            List<int> nuevosgenesc1 = new List<int>(genesComunes);
            List<int> nuevosgenesc2 = new List<int>(genesComunes);


            Shuffle<int>(restos);
            for( int i=0, j=0; i<nuevosgenesc1.Count; i++ )
            {
                if( nuevosgenesc1[i] == -1 )
                {
                    nuevosgenesc1[i] = restos[j];
                    j++;
                }
            }

            Shuffle<int>(restos);
            for (int i = 0, j = 0; i < nuevosgenesc2.Count; i++)
            {
                if (nuevosgenesc2[i] == -1)
                {
                    nuevosgenesc2[i] = restos[j];
                    j++;
                }
            }

   


            return new Tuple<QAP, QAP>(new QAP(c1.GetTamProblema(), nuevosgenesc1, c1.GetFlujosUnidades(), c1.GetDistanciasLocalizaciones(), c1.GetTiempoEjecucion()), new QAP(c2.GetTamProblema(), nuevosgenesc2, c2.GetFlujosUnidades(), c2.GetDistanciasLocalizaciones(), c2.GetTiempoEjecucion()));

            
        }





        private QAP TorneoBinario( int r1, int r2 )
        {
            int coste1 = poblacion[r1].CalcularCoste();
            int coste2 = poblacion[r2].CalcularCoste();
            if ( coste1 <= coste2 ) return new QAP(poblacion[r1]);
            else return new QAP(poblacion[r2]);
        }

        private void Shuffle<T>( List<T> list )
        {
      
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            
  
        }


        public QAP GetBestSolution()
        {
            QAP mejorPi = poblacion[0];
            int it = 0;
            for (int i = 0; i < poblacion.Count; i++)
            {
                if (poblacion[i].GetCoste() < mejorPi.GetCoste())
                {
                    mejorPi = poblacionH[i];
                    it = i;
                }
            }

            return new QAP(poblacion[it]);
        }
        

        

    }

}
