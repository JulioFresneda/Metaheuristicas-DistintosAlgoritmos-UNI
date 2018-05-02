using MH;
using System;
using System.Collections.Generic;
using System.Text;

namespace MHP2
{
    class Greedy
    {
        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades = new List<List<int>>();
        private List<List<int>> distanciasLocalizaciones = new List<List<int>>();


        public Greedy(QAP qap)
        {
            tamProblema = qap.GetTamProblema();
            localizacionesEnUnidades.AddRange(qap.GetLocalizacionesEnUnidades());
            flujosUnidades.AddRange(qap.GetFlujosUnidades());
            distanciasLocalizaciones.AddRange(qap.GetDistanciasLocalizaciones());
        }


        public QAP ResolverGreedy()
        {


            List<int> potencialFlujoUnidades = new List<int>();
            List<int> potencialDistanciaLocalizaciones = new List<int>();

            ResolverPotenciales(potencialFlujoUnidades, potencialDistanciaLocalizaciones);

            int maxpfu = potencialFlujoUnidades[0];
            int minpdl = potencialDistanciaLocalizaciones[0];

            int indexmaxpfu = 0;
            int indexminpdl = 0;

            for (int i = 0; i < tamProblema; i++)
            {

                maxpfu = -1;
                minpdl = -1;

                for (int j = 0; j < potencialFlujoUnidades.Count; j++)
                {
                    if (potencialFlujoUnidades[j] > maxpfu)
                    {
                        maxpfu = potencialFlujoUnidades[j];
                        indexmaxpfu = j;

                    }
                }

                for (int j = 0; j < potencialDistanciaLocalizaciones.Count; j++)
                {
                    if ((minpdl == -1 || potencialDistanciaLocalizaciones[j] < minpdl) && potencialDistanciaLocalizaciones[j] > -1)
                    {
                        minpdl = potencialDistanciaLocalizaciones[j];
                        indexminpdl = j;
                    }
                }


                potencialFlujoUnidades[indexmaxpfu] = -1;
                potencialDistanciaLocalizaciones[indexminpdl] = -1;

                localizacionesEnUnidades[indexmaxpfu] = indexminpdl;



            }


            return new QAP(tamProblema, localizacionesEnUnidades, flujosUnidades, distanciasLocalizaciones);


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
                    pf[i] = pf[i] + flujosUnidades[i][j];
                    pd[i] = pd[i] + distanciasLocalizaciones[i][j];
                }
            }
        }
    }
}
