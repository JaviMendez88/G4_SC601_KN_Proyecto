using System;

namespace G4_SC601_KN_Proyecto.Models
{
    public class StockViewModel
    {
        public string Material { get; set; }
        public string Lote { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}