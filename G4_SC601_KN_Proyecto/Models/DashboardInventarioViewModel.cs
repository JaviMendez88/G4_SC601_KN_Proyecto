using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G4_SC601_KN_Proyecto.Models
{
    public class DashboardInventarioViewModel
    {
        public List<StockViewModel> StockActual { get; set; }
        public List<StockViewModel> ProximosVencimientos { get; set; }

        public int TotalMateriales { get; set; }
        public int TotalLotes { get; set; }
        public int TotalStock { get; set; }
    }


}
}