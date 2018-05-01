using MH;
using System;
using System.Collections.Generic;
using System.Text;

namespace MHP2
{
    class AGE : AG
    {
        public AGE(string ruta, bool _pmx = false, int num_Cromosomas = 50, float prob_Mutacion = 0.001F, int num_Iteraciones = 50000)
        {
            numCromosomas = num_Cromosomas;

     

            if (prob_Mutacion <= 1f && prob_Mutacion >= 0f) probMutacion = prob_Mutacion;
            else probMutacion = 0.001f;

            numIteraciones = num_Iteraciones;

            for (int i = 0; i < numCromosomas; i++) poblacion.Add(new QAP(ruta));


            pmx = _pmx;
            estacionario = true;
            memetico = false;

            numGenes = poblacion[0].GetLocalizacionesEnUnidades().Count;



            Evolucionar();
        }
    }
}
