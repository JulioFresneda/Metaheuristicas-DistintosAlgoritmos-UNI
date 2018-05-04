using MH;
using System;
using System.Collections.Generic;
using System.Text;

namespace MHP2
{
    class AM : AG
    {
        public AM(string ruta, Random rand, bool _pmx = false, int _ciclosMeme = 10, float _porcentajeMeme = 1, bool _mejorPorcentajeMeme = false, int num_Cromosomas = 50, float prob_Cruce = 0.7F, float prob_Mutacion = 0.001F, int num_Iteraciones = 500000, int _mllfo = 50000)
        {
            r = rand;
            numCromosomas = num_Cromosomas;

            if (prob_Cruce <= 1f && prob_Cruce >= 0f) probCruce = prob_Cruce;
            else probCruce = 0.7f;

            if (prob_Mutacion <= 1f && prob_Mutacion >= 0f) probMutacion = prob_Mutacion;
            else probMutacion = 0.001f;

            numIteraciones = num_Iteraciones;

            for (int i = 0; i < numCromosomas; i++) poblacion.Add(new QAP(ruta));


            pmx = _pmx;
            estacionario = false;
            memetico = true;

            numCruces = (probCruce * numCromosomas);
            numGenes = poblacion[0].GetLocalizacionesEnUnidades().Count;
            numMutaciones = (probMutacion * numCromosomas * numGenes);

            ciclosMeme = _ciclosMeme;
            porcentajeMeme = _porcentajeMeme;
            mejorPorcentajeMeme = _mejorPorcentajeMeme;

            maxLlamadasFuncionObjetivo = _mllfo;


            Evolucionar();
        }
    }
}
