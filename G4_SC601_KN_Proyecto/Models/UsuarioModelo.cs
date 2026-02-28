using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G4_SC601_KN_Proyecto.Models
{
    public class UsuarioModelo
    {
        public int ConsecutiveNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}