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
                var result = db.usuario.Include(u => u.id_usuario == idUsuarioSession).FirstOrDefault();
                var dto = new PerfilModel
                {
                    Nombre = result.nombre,
                    Apellido1 = result.apellido_1,
                    UsuarioLogin = result.usuario1,
                    Email = result.email,
                    RolUsuario = result.id_rol
                };

               
                return View(dto);
            }

        }


        #endregion


        #region Editar Perfil
        //trae los datos del usuario de la session
        [HttpGet]
        public ActionResult Edituser()
        {

            using (var db = new SC604Proyecto_DBEntities())
            {
                //variables de session que hay id_usuario, nombre y rol
                var idUsuarioSession = int.Parse(Session["IdUsuario"].ToString());
                // Buscamos el usuario específico por su ID y traemos la información de su rol
                var result = db.usuario.Include(u => u.id_usuario == idUsuarioSession).FirstOrDefault();
                var dto = new PerfilModel
                {
                    Nombre = result.nombre,
                    Apellido1 = result.apellido_1,
                    UsuarioLogin = result.usuario1,
                    Email = result.email,
                    RolUsuario = result.id_rol
                };

                // Llenamos el ViewBag con la lista de roles para el DropDownList
                var roles = db.rol
                    .Select(r => new
                    {
                        r.id_rol,
                        r.rol1
                    }).ToList();

                // Los nombres de string deben coincidir exactamente con las propiedades del select anónimo: "id_rol" y "rol1"
                ViewBag.TiposRoles = new SelectList(roles, "id_rol", "rol1", dto.RolUsuario);
                return View(dto);
            }

        }
        #endregion

        [HttpPost]
        public ActionResult EditUser(PerfilModel model)
        {
          
            using (var db = new SC604Proyecto_DBEntities())
            {
                var idUsuarioSession = int.Parse(Session["IdUsuario"].ToString());
                var result = db.usuario.Where(u => u.id_usuario == idUsuarioSession).FirstOrDefault();
                if (result != null)
                {
                    model.Nombre = result.nombre;
                    model.Apellido1 = result.apellido_1;
                    model.UsuarioLogin = result.usuario1;
                    model.Email = result.email;

                    if (model.RolUsuario == 1) {
                        ViewBag.TiposRoles = new SelectList(
                        db.rol.ToList(),
                        "id_rol",
                        "rol1"
                    );
                    }
                    else
                    {
                        ViewBag.MensajeError = "Este usuario no tiene permisos para cambiar el rol.";
                    }
                }

                db.SaveChanges();
            }

            //Actualizamos la información de la sesión(las variables de session) con los nuevos datos del perfil
            //variables de session que hay id_usuario, nombre y rol

            Session["Nombre"] = model.Nombre;
            //Session["CorreoElectronico"] = model.CorreoElectronico;
            return RedirectToAction("UserDetail", "Home");

        }


       



    }
}
