using MH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHP3
{
    class ES
    {
        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades = new List<List<int>>();
        private List<List<int>> distanciasLocalizaciones = new List<List<int>>();
        private int coste;
        Random rand = new Random();

        public ES()
        {
            tamProblema = 0;
            coste = 0;
        }

        public ES(QAP qap)
        {
            tamProblema = qap.GetTamProblema();

            localizacionesEnUnidades.AddRange(qap.GetLocalizacionesEnUnidades());
            flujosUnidades.AddRange(qap.GetFlujosUnidades());
            distanciasLocalizaciones.AddRange(qap.GetDistanciasLocalizaciones());
            coste = qap.GetCoste();

            ResolverES();

        }

        public QAP GetQAP()
        {
            QAP qap = new QAP(tamProblema, localizacionesEnUnidades, flujosUnidades, distanciasLocalizaciones);
            return qap;
        }





        private void ResolverES()
        {
            
            double To = CalcularTo();
            double T = To;
            double Tf = 0.001;

            List<int> BestSolution = new List<int>();
            
            List<int> s = new List<int>();
            List<int> s_prima = new List<int>();

            s.AddRange(localizacionesEnUnidades);
            BestSolution.AddRange(s);
            int costeBS = CalcularCoste(BestSolution);
            int costeS = costeBS;
            int numEnfriamientos = 50000 / ((tamProblema) ^ 2);
            int dif_f = 0;

            do
            {
                for (int i = 1; i < numEnfriamientos; i++)
                {
             
                    s_prima = Neighborhood_Op(s, ref dif_f);
             
                    double prob = U();
                    double cauchy = Math.Exp(-dif_f / ( T));
                    if (dif_f < 0 || prob <= cauchy)
                    {
                        s.Clear();
                        s.AddRange(s_prima);
                        costeS = costeS + dif_f;
                        if (costeS < costeBS)
                        {
                            BestSolution.Clear();
                            BestSolution.AddRange(s);
                            costeBS = costeS;
                
                        }
                    }
                }


                T = g(T, To, numEnfriamientos);

             
            } while (T > Tf);





            localizacionesEnUnidades = BestSolution;
            coste = costeBS;
        }



        private double g( double T, double To, int M, double Tf = 0.001)
        {
            double beta = (To - Tf) / (M * To * Tf);
            double Tret =  T / (1 + beta * T);
            return T*0.9;
        }

        private double CalcularTo( double fi = 0.3, double eta = 0.3f )
        {
            return (eta * coste) / -Math.Log(fi);
            
        }


        private List<int> Neighborhood_Op( List<int> s, ref int coste)
        {
            List<int> n = new List<int>();
            n.AddRange(s);

            
            int i = rand.Next(0, n.Count);
            int j = rand.Next(0, n.Count);
            while (j == i) j = rand.Next(0, n.Count);

            IntercambiarLocalizaciones(i, j, n);

            coste = CosteMovimiento(i, j, s);

            return n;
        }

        private double U()
        {
     
            double f = rand.NextDouble();
            return f;
        }









        private void IntercambiarLocalizaciones(int i, int j, List<int> lista)
        {
 
            {
                int temp = lista[i];
                int temp2 = lista[j];

                lista[j] = temp;
                lista[i] = temp2;
            }
        }


        private int CosteMovimiento(int r, int s, List<int> solucion)
        {
            int coste = 0;

            for (int k = 0; k < tamProblema; k++)
            {
                if (k != r && k != s)
                {

                    coste = coste + flujosUnidades[r][k] * (distanciasLocalizaciones[solucion[s]][solucion[k]] - distanciasLocalizaciones[solucion[r]][solucion[k]]);
                    coste = coste + flujosUnidades[s][k] * (distanciasLocalizaciones[solucion[r]][solucion[k]] - distanciasLocalizaciones[solucion[s]][solucion[k]]);
                    coste = coste + flujosUnidades[k][r] * (distanciasLocalizaciones[solucion[k]][solucion[s]] - distanciasLocalizaciones[solucion[k]][solucion[r]]);
                    coste = coste + flujosUnidades[k][s] * (distanciasLocalizaciones[solucion[k]][solucion[r]] - distanciasLocalizaciones[solucion[k]][solucion[s]]);

                }
            }

            return coste;

        }















        private int CalcularCoste(List<int> s)
        {

            int coste = 0;

            for (int i = 0; i < tamProblema; i++)
            {
                for (int j = 0; j < tamProblema; j++)
                {

                    if (i != j)
                    {
                        coste = coste + flujosUnidades[i][j] * distanciasLocalizaciones[s[i]][s[j]];
                    }
                }
            }

            return coste;
        }



    }
}
