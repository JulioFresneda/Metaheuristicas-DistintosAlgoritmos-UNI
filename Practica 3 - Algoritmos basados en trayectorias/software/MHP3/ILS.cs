using MH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHP3
{
    class ILS
    {

        private QAP bestSolution;
        Random rand = new Random();



        public ILS(string ruta)
        {
            bestSolution = new QAP(ruta);

            EjecutarILS();

        }




        private void EjecutarILS()
        {
            QAP qap = new QAP(bestSolution);

            BusquedaLocal bl = new BusquedaLocal(qap);
            qap = bl.ResolverBL();
            QAP best = new QAP(qap);

            for (int i = 0; i < 24; i++)
            {
                Mutar(qap);
                bl = new BusquedaLocal(qap);
                qap = bl.ResolverBL();

                if (qap.GetCoste() < best.GetCoste()) best = new QAP(qap);
                else qap = new QAP(best);


            }


            bestSolution = new QAP(best);
        }

        private void Mutar(QAP qap)
        {
            int tamSubcadena = qap.GetTamProblema() / 4;
            List<int> localizacionesEnUnidades = qap.GetLocalizacionesEnUnidades();

            int inicio = rand.Next(0, qap.GetTamProblema() - tamSubcadena+1);
            int fin = inicio + tamSubcadena;

            List<int> subcadena = new List<int>();

            for( int i=inicio; i<fin; i++ )
            {
                subcadena.Add(localizacionesEnUnidades[i]);
            }

            int index;

            while( subcadena.Count != 0 )
            {
                index = rand.Next(0, subcadena.Count);
                localizacionesEnUnidades[inicio + subcadena.Count-1] = subcadena[index];
                subcadena.RemoveAt(index);
            }

            qap.SetLocalizacionesEnUnidades(localizacionesEnUnidades);


        }

        public QAP GetQAP()
        {
            return bestSolution;
        }



        
    }
}


