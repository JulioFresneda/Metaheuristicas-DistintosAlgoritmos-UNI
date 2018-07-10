using MH;
using MHP3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHP3
{
    class BMB
    {
        QAP solucion;
        public BMB( int numMultiarranque, string ruta )
        {
            QAP qap = new QAP(ruta);
            BusquedaLocal bl = new BusquedaLocal(qap);
            QAP best = bl.ResolverBL();
            
            for( int i = 1; i<numMultiarranque; i++ )
            {
                qap = new QAP(ruta);
                bl = new BusquedaLocal(qap);
                qap = bl.ResolverBL();
                if (qap.GetCoste() < best.GetCoste()) best.SetQAP(qap);
            
            }

            solucion = best;
        }


        public QAP GetQAP()
        {
            return solucion;
        }
    }
}
