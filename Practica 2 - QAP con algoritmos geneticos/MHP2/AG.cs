using MH;
using System;
using System.Collections.Generic;
using System.Text;

namespace MHP2
{
    class AG
    {
        int mutados;
        protected Random r;
        protected int numCromosomas { get; set; }
        protected int numGenes { get; set; }
        protected float probCruce { get; set; }
        protected float probMutacion { get; set; }
        protected int numIteraciones { get; set; }
        protected int llamadasFuncionObjetivo { get; set; }
        protected int maxLlamadasFuncionObjetivo { get; set; }

        protected bool pmx { get; set; }
        protected bool estacionario { get; set; }

        protected bool memetico { get; set; }
        protected int ciclosMeme { get; set; }
        protected float porcentajeMeme { get; set; }
        protected bool mejorPorcentajeMeme { get; set; }


        protected float numCruces { get; set; }
        protected float numMutaciones { get; set; }



        protected List<QAP> poblacion = new List<QAP>();
        protected List<QAP> poblacionP = new List<QAP>();
        protected List<QAP> poblacionI = new List<QAP>();
        protected List<QAP> poblacionH = new List<QAP>();


        QAP mejorPob;
        BusquedaLocal bl = new BusquedaLocal();




        protected void Evolucionar()
        {
            mutados = 0;
            llamadasFuncionObjetivo = 0;
            

            for( int i=0;i<numIteraciones && llamadasFuncionObjetivo < maxLlamadasFuncionObjetivo; i++ )
            {


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
                    if (memetico && i % ciclosMeme == 0) AplicarMemetico();
                    SeleccionGeneracional();
                    CruceGeneracional();
                    MutacionGeneracional();
                    ReemplazamientoGeneracional();
                }

                
                

                /*if (i % 100 == 0)
                {
                    Console.WriteLine("Iteracion " + i);
                    Console.WriteLine("Mejor sol " + GetBestSolution().GetCoste());
                    Console.WriteLine();

                    Console.WriteLine();
              
                }*/


        
            }
            

        }


        private void AplicarMemetico()
        {
            int llam;
            if ( mejorPorcentajeMeme )
            {
                
                List<QAP> copia = new List<QAP>();
                copia.AddRange(poblacion);
                List<int> mejores = new List<int>();
                QAP mejor = copia[0];
                int indicemejor = 0;
                for( int i=0; i<porcentajeMeme*numCromosomas; i++ )
                {
                    for( int j=0; j<copia.Count; j++ )
                    {
                        if (mejor.GetCoste() < copia[i].GetCoste())
                        {
                            mejor = copia[i];
                            indicemejor = i;
                        }

                    }
                    mejores.Add(indicemejor);
                    copia.Remove(mejor);
                }

                for (int i = 0; i < mejores.Count; i++)
                {


                    bl.SetQAP(poblacion[mejores[i]]);
                    llam = llamadasFuncionObjetivo;
                    poblacion[mejores[i]].SetQAP(bl.ResolverBL(ref llam));
                    llamadasFuncionObjetivo = llam;

                }
            }
            else
            {

                for (int i = 0; i < porcentajeMeme * numCromosomas; i++)
                {


                    bl.SetQAP(poblacion[i]);
                    llam = llamadasFuncionObjetivo;
                    poblacion[i].SetQAP(bl.ResolverBL(ref llam));
                    llamadasFuncionObjetivo = llam;

                }

            }

        }

        private void SeleccionEstacionario()
        {
            

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
            
            if (pmx)
            {
                cruzados = CruzarPMX(0, 1);
                
            }
            else cruzados = CruzarPosicion(0, 1);
            poblacionI.Add(new QAP(cruzados.Item1));
            poblacionI.Add(new QAP(cruzados.Item2));

        }

        private void CruceGeneracional()
        {
            Tuple<QAP, QAP> cruzados;
            for( int i=0; i<numCruces; i+=2 )
            {
                if( pmx ) cruzados = CruzarPMX(i, i + 1);
                else cruzados = CruzarPosicion(i, i + 1);
                poblacionI.Add(new QAP(cruzados.Item1));
                poblacionI.Add(new QAP(cruzados.Item2));
            }
            for( int i=(int)numCruces+1; i<numCromosomas; i++ )
            {
                poblacionI.Add(new QAP(poblacionP[i]));
            }
            
        }

        private void MutacionEstacionario()
        {
            float max = 1 / (probMutacion * 2 * numGenes);
            int g1, g2;

            
            int c = r.Next(1, (int)max);
            if( c == 1 )
            {
                
                g1 = r.Next(0, numGenes);
                g2 = r.Next(0, numGenes);
                while (g2 == g1) g2 = r.Next(0, numGenes);

                Mutar(g1, g2, poblacionI[0]);
            
            }

            c = r.Next(1, (int)max);
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

            for( int i=0; i<poblacion.Count; i++ )
            {
                if( poblacion[i].GetCoste() > poblacion[it1].GetCoste() )
                {
                    it1 = i;
                }
            }

            if (it1 == 0) it2 = 1;
            for (int i = 0; i < poblacion.Count; i++)
            {
                if (poblacion[i].GetCoste() > poblacion[it2].GetCoste() && i != it1 )
                {
                    it2 = i;
                }
            }

            if( poblacionH[0].GetCoste() < poblacion[it1].GetCoste() && poblacionH[1].GetCoste() < poblacion[it2].GetCoste() )
            {
              
                poblacion[it1] = new QAP(poblacionH[0]);
                poblacion[it2] = new QAP(poblacionH[1]);
            }
            else if (poblacionH[1].GetCoste() < poblacion[it1].GetCoste() && poblacionH[0].GetCoste() < poblacion[it2].GetCoste())
            {
               
                poblacion[it1] = new QAP(poblacionH[1]);
                poblacion[it2] = new QAP(poblacionH[0]);
            }
            else
            {
                if (poblacionH[0].GetCoste() < poblacion[it1].GetCoste())
                {
                   
                    poblacion[it1] = new QAP(poblacionH[0]);
                }
                else if (poblacionH[0].GetCoste() < poblacion[it2].GetCoste())
                {
                   
                    poblacion[it2] = new QAP(poblacionH[0]);
                }
                else if (poblacionH[1].GetCoste() < poblacion[it1].GetCoste())
                {
                  
                    poblacion[it1] = new QAP(poblacionH[1]);
                }
                else if (poblacionH[1].GetCoste() < poblacion[it2].GetCoste())
                {
                  
                    poblacion[it2] = new QAP(poblacionH[1]);
                }
            }

            poblacionI.Clear();
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

                qap.CalcularCoste();
                llamadasFuncionObjetivo++;
                mutados++;
            }
        }


        private Tuple<QAP,QAP> CruzarPMX( int pos1, int pos2 )
        {
            QAP c1 = poblacionP[pos1];
            QAP c2 = poblacionP[pos2];




            List<int> genesc1 = c1.GetLocalizacionesEnUnidades();
            List<int> genesc2 = c2.GetLocalizacionesEnUnidades();

            
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
            llamadasFuncionObjetivo += 2;
            QAP uno = new QAP(c1.GetTamProblema(), genesH1, c1.GetFlujosUnidades(), c1.GetDistanciasLocalizaciones());
            QAP dos = new QAP(c2.GetTamProblema(), genesH2, c2.GetFlujosUnidades(), c2.GetDistanciasLocalizaciones());


            return new Tuple<QAP, QAP>(uno,dos);



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




            llamadasFuncionObjetivo += 2;
            QAP uno = new QAP(c1.GetTamProblema(), nuevosgenesc1, c1.GetFlujosUnidades(), c1.GetDistanciasLocalizaciones());
            QAP dos = new QAP(c2.GetTamProblema(), nuevosgenesc2, c2.GetFlujosUnidades(), c2.GetDistanciasLocalizaciones());



            return new Tuple<QAP, QAP>(uno, dos);

            
        }





        private QAP TorneoBinario( int r1, int r2 )
        {
            int coste1 = poblacion[r1].GetCoste();
            int coste2 = poblacion[r2].GetCoste();
            if ( coste1 <= coste2 ) return new QAP(poblacion[r1]);
            else return new QAP(poblacion[r2]);
        }

        private void Shuffle<T>( List<T> list )
        {
      
            int n = list.Count;
            
            while (n > 1)
            {
                int k = (r.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            
  
        }


        public QAP GetBestSolution()
        {

            int it = 0;
            for (int i = 0; i < poblacion.Count; i++)
            {
                if (poblacion[i].GetCoste() < poblacion[it].GetCoste()) { 

                    it = i;
                }
            }

            return new QAP(poblacion[it]);
        }
        

        

    }

}
