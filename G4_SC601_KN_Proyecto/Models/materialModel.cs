
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G4_SC601_KN_Proyecto.Models
{
    public class materialModel
    {
        public int idMaterial { get; set; }
        public int idFamily { get; set; }
        public int idParent{ get; set; }
        public string descriptionM { get; set; }
        public int countOrder { get; set; }
        public int Qty { get; set; }
        public int labor { get; set; }
        public decimal IndOVH { get; set; }
        public int  Scrap_Allowance { get; set; }
        public decimal materialPrice { get; set; }
        public decimal totalVariance { get; set; }
     



    }
}