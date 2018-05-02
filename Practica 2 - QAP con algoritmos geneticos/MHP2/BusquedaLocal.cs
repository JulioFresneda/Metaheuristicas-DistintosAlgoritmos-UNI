using System;
using System.Collections.Generic;
using System.Text;
using MH;

namespace MHP2
{
    class BusquedaLocal
    {

        private int tamProblema;
        private List<int> localizacionesEnUnidades = new List<int>();
        private List<List<int>> flujosUnidades = new List<List<int>>();
        private List<List<int>> distanciasLocalizaciones = new List<List<int>>();

        

        public BusquedaLocal()
        {
            tamProblema = 0;
        }

        public BusquedaLocal( QAP qap )
        {
            tamProblema = qap.GetTamProblema();
            localizacionesEnUnidades.AddRange(qap.GetLocalizacionesEnUnidades());
            flujosUnidades.AddRange(qap.GetFlujosUnidades());
            distanciasLocalizaciones.AddRange(qap.GetDistanciasLocalizaciones());
        }

        public void SetQAP(QAP qap)
        {
            tamProblema = qap.GetTamProblema();

            localizacionesEnUnidades.Clear();
            localizacionesEnUnidades.AddRange(qap.GetLocalizacionesEnUnidades());

            flujosUnidades.Clear();
            flujosUnidades.AddRange(qap.GetFlujosUnidades());

            distanciasLocalizaciones.Clear();
            distanciasLocalizaciones.AddRange(qap.GetDistanciasLocalizaciones());
        }

        public QAP ResolverBL()
        {
            
            

            bool hayMejora = true;
            bool improveFlag;
            List<bool> dlb = new List<bool>();
            for (int i = 0; i < tamProblema; i++) { dlb.Add(false); }

            while (hayMejora)
            {
                hayMejora = false;

                for (int i = 0; i < tamProblema; i++)
                {

                    if (!dlb[i])
                    {
                        improveFlag = false;
                        for (int j = 0; j < tamProblema; j++)
                        {
                            if (i != j)
                            {
                                if (CosteMovimiento(i, j) < 0)
                                {

                                    IntercambiarLocalizaciones(i, j);
                                    dlb[i] = false;
                                    dlb[j] = false;
                                    improveFlag = true;
                                    hayMejora = true;
                                }
                            }
                        }

                        if (improveFlag == false) dlb[i] = true;
                    }
                }
            }

            

            return new QAP(tamProblema, localizacionesEnUnidades, flujosUnidades, distanciasLocalizaciones);
        }


        private int CosteMovimiento(int r, int s)
        {
            int coste = 0;

            for (int k = 0; k < tamProblema; k++)
            {
                if (k != r && k != s)
                {

                    coste = coste + flujosUnidades[r][k] * (distanciasLocalizaciones[localizacionesEnUnidades[s]][localizacionesEnUnidades[k]] - distanciasLocalizaciones[localizacionesEnUnidades[r]][localizacionesEnUnidades[k]]);
                    coste = coste + flujosUnidades[s][k] * (distanciasLocalizaciones[localizacionesEnUnidades[r]][localizacionesEnUnidades[k]] - distanciasLocalizaciones[localizacionesEnUnidades[s]][localizacionesEnUnidades[k]]);
                    coste = coste + flujosUnidades[k][r] * (distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[s]] - distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[r]]);
                    coste = coste + flujosUnidades[k][s] * (distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[r]] - distanciasLocalizaciones[localizacionesEnUnidades[k]][localizacionesEnUnidades[s]]);

                }
            }

            return coste;

        }



        private void IntercambiarLocalizaciones(int i, int j, List<int> lista = null)
        {
            if (lista == null)
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
    }
}
