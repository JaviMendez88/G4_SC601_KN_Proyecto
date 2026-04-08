
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace G4_SC601_KN_Proyecto.Models
{
    public class materialModel
    {
        [Required]
        public int idMaterial { get; set; }
        [Required]
        public int idFamily { get; set; }
        [Required]
        public int idParent{ get; set; }
        [Required]
        public string descriptionM { get; set; }
        [Required]
        public int countOrder { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public int labor { get; set; }
        [Required]
        public decimal IndOVH { get; set; }
        [Required]
        public int  Scrap_Allowance { get; set; }
        [Required]
        public decimal materialPrice { get; set; }
        [Required]
        public decimal totalVariance { get; set; }
        




    }
}