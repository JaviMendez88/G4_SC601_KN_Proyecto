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

                if (user != null)
                {
                    model.Nombre = user.nombre;
                    model.Apellido1 = user.apellido_1;
                    model.UsuarioLogin = user.usuario1; // Nota: en EF se llama 'usuario1'
                    model.Email = user.email;
                    model.Rol = user.rol != null ? user.rol.rol1 : "Sin Rol"; 
                }
            }

            return View(model);
        }
        #endregion


        #region edit
        [HttpGet]
        public ActionResult EditUser()
        {
            // Validar que la sesión exista
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["IdUsuario"];
            UsuarioModelo model = new UsuarioModelo();

            using (var db = new SC604Proyecto_DBEntities())
            {
                var user = db.usuario.FirstOrDefault(u => u.id_usuario == userId);
                if (user != null)
                {
                    model.Nombre = user.nombre;
                    model.Apellido1 = user.apellido_1;
                    model.UsuarioLogin = user.usuario1; 
                    model.Email = user.email;

                    // Llenamos el IdRol en el modelo para que la vista sepa con qué rol iniciar
                    if (user.id_rol != null) 
                    {
                        model.IdRol = (int)user.id_rol; 
                    }
                }

                // Llenamos el ViewBag con la lista de roles para el DropDownList
                ViewBag.Roles = new SelectList(db.rol.ToList(), "id_rol", "rol1");
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult EditUser(UsuarioModelo usuario)
        {
            // Validar que la sesión exista
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = (int)Session["IdUsuario"];

            // Aquí actualizamos la información del usuario en la base de datos
            using (var db = new SC604Proyecto_DBEntities())
            {
                var userUpdate = db.usuario.FirstOrDefault(u => u.id_usuario == userId);

                if (userUpdate != null) 
                {
                    userUpdate.nombre = usuario.Nombre;
                    userUpdate.apellido_1 = usuario.Apellido1;
                    userUpdate.usuario1 = usuario.UsuarioLogin; 
                    userUpdate.email = usuario.Email;

                    // Validar que solo un administrador (asumiendo que IdRol 1 es Admin) puede cambiar de rol
                    if (userUpdate.id_rol == 1 && usuario.IdRol > 0)
                    {
                        userUpdate.id_rol = usuario.IdRol; 
                    }

                    db.SaveChanges();
                    return RedirectToAction("UserDetail");
                }

                // Si la actualización falla y retornamos a la vista, tenemos que recargar el ViewBag
                ViewBag.Roles = new SelectList(db.rol.ToList(), "id_rol", "rol1", usuario.IdRol);
            }

            return View(usuario);
        }

        #endregion




    }
}
