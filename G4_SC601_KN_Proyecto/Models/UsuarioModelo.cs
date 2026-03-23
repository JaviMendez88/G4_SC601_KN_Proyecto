using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace G4_SC601_KN_Proyecto.Models
{
    public class UsuarioModelo
    {
        public int IdUsuario { get; set; }

        [StringLength(40)]
        public string Nombre { get; set; }


        [StringLength(30)]
        public string Apellido1 { get; set; }


        [StringLength(50)]
        public string UsuarioLogin { get; set; }


        [StringLength(100)]
        public string Email { get; set; }

        public string Contrasena { get; set; }
        public string ConfirmPassword { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
    }
}