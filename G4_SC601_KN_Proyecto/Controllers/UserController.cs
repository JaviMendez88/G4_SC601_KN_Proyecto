using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Filters;
using G4_SC601_KN_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace G4_SC601_KN_Proyecto.Controllers
{
    [SesionActiva]
    public class UserController : Controller
       
    {
        #region perfil
        //trae los datos del usuario de la session
        [HttpGet]
        public ActionResult UserDetail()
        {

            using (var db = new SC604Proyecto_DBEntities())
            {
                //variables de session que hay id_usuario, nombre y rol
                var idUsuarioSession = int.Parse(Session["IdUsuario"].ToString());
                // Buscamos el usuario específico por su ID y traemos la información de su rol
                var result = db.usuario.Where(u => u.id_usuario == idUsuarioSession).FirstOrDefault();
                var dto = new PerfilModel
                {
                    Nombre = result.nombre,
                    Apellido1 = result.apellido_1,
                    UsuarioLogin = result.usuario1,
                    Email = result.email

                };
                return View(dto);
            }
        }

        [HttpPost]
        public ActionResult UserDetail(PerfilModel model)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var idUsuarioSession = int.Parse(Session["IdUsuario"].ToString());
                var result = db.usuario.Where(u => u.id_usuario == idUsuarioSession).FirstOrDefault();
                


                if (result != null)
                {
                    result.nombre = model.Nombre;
                    result.apellido_1 = model.Apellido1;
                    result.usuario1 = model.UsuarioLogin;
                    result.email = model.Email;
                    
                    db.SaveChanges();
                    
                    ViewBag.Mesaje = "Perfil actualizado correctamente";
                }
                Session["Nombre"] = model.Nombre;
         
         
                return RedirectToAction("UserDetail", "User");
            }
        }


        #endregion

        #region CRUD usuarios
        [HttpGet]
        public ActionResult ConsultarUsuarios()
        {
            

            using (var db = new SC604Proyecto_DBEntities())
            {
                var result = db.usuario.ToList();
                return View(result);
            }
        }
        #endregion
    }
}
