using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace MH
{
    public class MedirTiempos
    {

        List<TimeSpan> tiemposGreedy = new List<TimeSpan>();
        List<List<int>> solucionesGreedy = new List<List<int>>();
        List<int> costesGreedy = new List<int>();

        List<TimeSpan> tiemposBL = new List<TimeSpan>();
        List<List<int>> solucionesBL = new List<List<int>>();
        List<int> costesBL = new List<int>();


        List<int> mejoresSoluciones = new List<int>();

        private float desviacionGreedy;
        private float desviacionBL;

        private TimeSpan mediaGreedy, mediaBL;

        List<float> desviacionesGreedy, desviacionesBL;

        public MedirTiempos(QAP qap)
        { 

            mejoresSoluciones = new List<int>(){6156,6194,3796,64,6922,2520135,7763962,5166,90998,115534,152002,149036,21052466,1185996137,498896643,44759294,240516,8133398,48816,273038};
            

            qap.ResolverBL();
            tiemposBL.Add(qap.GetTiempoEjecucion());
            solucionesBL.Add(qap.GetLocalizacionesEnUnidades());
            costesBL.Add(qap.CalcularCoste());
            

           
            qap.ResolverGreedy();
            tiemposGreedy.Add(qap.GetTiempoEjecucion());
            solucionesGreedy.Add(qap.GetLocalizacionesEnUnidades());
            costesGreedy.Add(qap.CalcularCoste());

            if( solucionesBL.Count == mejoresSoluciones.Count ) CalcularDesviaciones();
            CalcularTiempos();


        }


        private void CalcularDesviaciones()
        {
            desviacionGreedy = 0;
            desviacionBL = 0;

            desviacionesBL = new List<float>();
            desviacionesGreedy = new List<float>();

            for( int i=0; i<costesGreedy.Count; i++ )
            {
                desviacionesGreedy.Add(100*(((float)costesGreedy[i]-(float)mejoresSoluciones[i])/(float)mejoresSoluciones[i]));
                desviacionGreedy+= 100*(((float)costesGreedy[i]-(float)mejoresSoluciones[i])/(float)mejoresSoluciones[i]);
                
                desviacionesBL.Add(100*(((float)costesBL[i]-(float)mejoresSoluciones[i])/(float)mejoresSoluciones[i]));
                desviacionBL+= 100*(((float)costesBL[i]-(float)mejoresSoluciones[i])/(float)mejoresSoluciones[i]);
            }

            desviacionGreedy = desviacionGreedy/(float)costesGreedy.Count;
            desviacionBL = desviacionBL/(float)costesBL.Count;
        }


        private void CalcularTiempos()
        {
            mediaGreedy = tiemposGreedy[0];
            mediaBL = tiemposBL[0];

            for( int i=1; i<tiemposBL.Count; i++ )
            {
                mediaGreedy+=tiemposGreedy[i];
                mediaBL+=tiemposBL[i];
            }

       
        }

        public List<float> GetDesviacionesGreedy()
        {
            return desviacionesGreedy;
        }
        
        public List<float> GetDesviacionesBL()
        {
            return desviacionesBL;
        }

        public TimeSpan GetMediaEjecucionGreedy()
        {
            return mediaGreedy;
        }

        public TimeSpan GetMediaEjecucionBL()
        {
            return mediaBL;
        }

        public float GetDesviacionGreedy()
        {
            return desviacionGreedy;
        }

        public float GetDesviacionBL()
        {
            return desviacionBL;
        }

        public List<TimeSpan> GetTiemposGreedy()
        {
            return tiemposGreedy;
        }

        public List<TimeSpan> GetTiemposBL()
        {
            return tiemposBL;
        }


        public List<int> GetCostesGreedy()
        {
            return costesGreedy;
        }

        public List<int> GetCostesBL()
        {
            return costesBL;
        }

        public List<List<int>> GetSolucionesGreedy()
        {
            return solucionesGreedy;
        }

        public List<List<int>> GetSolucionesBL()
        {
            return solucionesBL;
        }


    }
}