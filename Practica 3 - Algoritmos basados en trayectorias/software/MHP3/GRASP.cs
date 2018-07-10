using MH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHP3
{
    class GRASP
    {
        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades = new List<List<int>>();
        private List<List<int>> distanciasLocalizaciones = new List<List<int>>();
        Random rand = new Random();

        private List<Tuple<int,int>> LCu = new List<Tuple<int,int>>();
        private List<Tuple<int, int>> LCl = new List<Tuple<int, int>>();

        private List<int> LRCu = new List<int>();
        private List<int> LRCl = new List<int>();

        List<int> potencialFlujoUnidades = new List<int>();
        List<int> potencialDistanciaLocalizaciones = new List<int>();

        double d_min_LRCu, d_max_LRCu;
        double d_min_LRCl, d_max_LRCl;

        List<int> Costes = new List<int>();

        List<Tuple<int, int>> S = new List<Tuple<int, int>>();

        QAP solucion;
        QAP bestSolution;

        public GRASP(string ruta)
        {

            

            for (int x = 0; x < 25; x++)
            {
                potencialDistanciaLocalizaciones.Clear();
                potencialFlujoUnidades.Clear();
                LCl.Clear();
                LCu.Clear();
                LRCl.Clear();
                LRCu.Clear();
                Costes.Clear();
                S.Clear();

            
                QAP qap = new QAP(ruta);
                tamProblema = qap.GetTamProblema();
                
                flujosUnidades = qap.GetFlujosUnidades();
                distanciasLocalizaciones = qap.GetDistanciasLocalizaciones();
                ResolverPotenciales(potencialFlujoUnidades, potencialDistanciaLocalizaciones);


                for (int i = 0; i < tamProblema; i++)
                {
                    LCu.Add(item: new Tuple<int, int>(i, potencialFlujoUnidades[i]));
                    LCl.Add(item: new Tuple<int, int>(i, potencialDistanciaLocalizaciones[i]));

                }

                for (int i = 0; i < tamProblema; i++)
                {
                    localizacionesEnUnidades.Add(-1);
                }

                Etapa1();
                Etapa2();

                localizacionesEnUnidades.Clear();
                for (int i = 0; i < tamProblema; i++) localizacionesEnUnidades.Add(-1);
                foreach (Tuple<int, int> s in S)
                {
                    localizacionesEnUnidades[s.Item1] = s.Item2;
                }

                bool error = false;
                if (localizacionesEnUnidades.Contains(-1)) error = true;
                OptimizacionBL();

                if (x == 0) bestSolution = new QAP(solucion);
                else
                {
                    if (bestSolution.CalcularCoste() > solucion.CalcularCoste()) bestSolution.SetQAP(solucion);
                }
            }


        }


        private void OptimizacionBL()
        {
            QAP qap = new QAP(tamProblema, localizacionesEnUnidades, flujosUnidades, distanciasLocalizaciones);
            BusquedaLocal bl = new BusquedaLocal(qap);
            solucion = bl.ResolverBL();
        }

        public QAP GetQAP()
        {
            return bestSolution;
        }





        private void Etapa1()
        {
            CalcularMinMax();
            double umbralu = (d_max_LRCu - 0.3 * (d_max_LRCu - d_min_LRCu));
            double umbrall = (d_min_LRCl + 0.3 * (d_max_LRCl - d_min_LRCl));
            for ( int i=0; i<LCl.Count; i++ )
            {
                if (LCu[i].Item2 >= umbralu ) LRCu.Add(LCu[i].Item1);
                if (LCl[i].Item2 <= umbrall ) LRCl.Add(LCl[i].Item1);
            }

            int u1 = rand.Next(0, LRCu.Count);
            int u2 = rand.Next(0, LRCu.Count);


            if( LRCu.Count > 1 ) while ( u1 == u2 ) u2 = rand.Next(0, LRCu.Count);

            int l1 = rand.Next(0, LRCl.Count);
            int l2 = rand.Next(0, LRCl.Count);
            if( LRCu.Count > 1 ) while (l1 == l2) l2 = rand.Next(0, LRCl.Count);

            S.Add(new Tuple<int, int>(LRCu[u1], LRCl[l1]));
            if( LRCu.Count > 1 && LRCl.Count > 1 ) S.Add(new Tuple<int, int>(LRCu[u2], LRCl[l2]));
            else
            {
                u2 = rand.Next(0, LCu.Count);
                while (LRCu[u1] == LCu[u2].Item1) u2 = rand.Next(0, LCu.Count);

                l2 = rand.Next(0, LCl.Count);
                while (LRCl[l1] == LCl[l2].Item1) l2 = rand.Next(0, LCl.Count);

                S.Add(new Tuple<int, int>(LCu[u2].Item1, LCl[l2].Item1));
            }


        }


        private void Etapa2()
        {
            List<Tuple<int, int>> LC = new List<Tuple<int, int>>();
            List<Tuple<int, int>> LRC = new List<Tuple<int, int>>();


            List<int> notu = new List<int>();
            List<int> notl = new List<int>();
            

            while( S.Count != tamProblema )
            {
                LC.Clear();
                for (int i = 0; i < tamProblema; i++)
                {
                    for (int j = 0; j < tamProblema; j++)
                    {
                        if( !notu.Contains(i) && !notl.Contains(j) && i != S[0].Item1 && j != S[0].Item2 && i != S[1].Item1 && j != S[1].Item2) LC.Add(new Tuple<int, int>(i, j));
                    }
                }

                Costes.Clear();
                LRC.Clear();
                int costemin = 0;
                int costemax = 0;
                for( int i=0; i<LC.Count; i++ )
                {
                    int coste = 0;
                    for( int j = 0; j<S.Count; j++ )
                    {
                        coste = coste + flujosUnidades[LC[i].Item1][S[j].Item1] * distanciasLocalizaciones[LC[i].Item2][S[j].Item2];
                    }
                    Costes.Add(coste);
                    if (i == 0) costemin = costemax = coste;
                    else
                    {
                        if (costemin > coste) costemin = coste;
                        if (costemax < coste) costemax = coste;
                    }

                }

                
                for( int i=0; i<LC.Count; i++ )
                {
                    if (Costes[i] <= costemin + 0.3 * (costemax - costemin)) LRC.Add(LC[i]);
                }

                int index = rand.Next(0, LRC.Count);
                S.Add(LRC[index]);
                notu.Add(LRC[index].Item1);
                notl.Add(LRC[index].Item2);
              
               
                    

            }
        }








        private void CalcularMinMax()
        {

            d_min_LRCu = LCu[0].Item2;
            d_max_LRCu = 0;

            d_min_LRCl = LCl[0].Item2;
            d_max_LRCl = 0;


            for ( int i=0; i<LCu.Count; i++ )
            {
                if (LCl[i].Item2 > d_max_LRCl) d_max_LRCl = LCl[i].Item2;
                if (LCl[i].Item2 < d_min_LRCl) d_min_LRCl = LCl[i].Item2;

                if (LCu[i].Item2 > d_max_LRCu) d_max_LRCu = LCu[i].Item2;
                if (LCu[i].Item2 < d_min_LRCu) d_min_LRCu = LCu[i].Item2;
            }
            
            

            
        }





        

        private void ResolverPotenciales(List<int> pf, List<int> pd)
        {

            for (int j = 0; j < tamProblema; j++)
            {
                pf.Add(0);
                pd.Add(0);
            }

            for (int i = 0; i < tamProblema; i++)
            {
                for (int j = 0; j < tamProblema; j++)
                {
                    pf[i] = pf[i] + flujosUnidades[i][j] + flujosUnidades[j][i];
                    pd[i] = pd[i] + distanciasLocalizaciones[i][j] + distanciasLocalizaciones[j][i];
                }
            }
        }
    }
}
