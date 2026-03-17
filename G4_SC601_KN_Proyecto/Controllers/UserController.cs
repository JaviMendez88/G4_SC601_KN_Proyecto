using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Models;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class UserController : Controller
    {


        #region details
        [HttpGet]
        public ActionResult UserDetail()
        {
            // Validar que la sesión exista
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            UsuarioModelo model = new UsuarioModelo();
            int userId = (int)Session["IdUsuario"];

            using (var db = new SC604Proyecto_DBEntities())
            {
                // Buscamos el usuario específico por su ID y traemos la información de su rol
                var user = db.usuario.Include(u => u.rol).FirstOrDefault(u => u.id_usuario == userId);

                if (userInfo != null)
                {
                    model.Nombre = userInfo.nombre;
                    model.Apellido1 = userInfo.apellido_1;
                    model.UsuarioLogin = userInfo.usuario1; // Nota: en EF se llama 'usuario1'
                    model.Email = userInfo.email;
                    model.Rol = userInfo.rol != null ? userInfo.rol.rol1 : "Sin Rol"; 
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UserDetail(UsuarioModelo usuario)
        {
            // Aquí puedes manejar la lógica si guardas o actualizas detalles
            return View(usuario);
        }
        #endregion


        #region edit
        [HttpGet]
        public ActionResult EditUser()
        {
            return View();
        }


        [HttpPost]
        public ActionResult EditUser(UsuarioModelo usuario)
        {
            return View();
        }

        #endregion


        #region delete
        [HttpPost]
        public ActionResult DeleteUser() {
            return View();
        }

        #endregion

    }
}
