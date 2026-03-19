using G4_SC601_KN_Proyecto.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult ListUsers()
        {
            if (Session["Rol"] == null || (int)Session["Rol"] != 1)
                return RedirectToAction("Login", "Home");

            using (var db = new SC604Proyecto_DBEntities())
            {
                var usuarios = db.usuario.ToList();
                return View(usuarios);
            }
        }

        public ActionResult EditUser(int id)
        {
            if ((int)Session["Rol"] != 1)
                return RedirectToAction("Login", "Home");

            using (var db = new SC604Proyecto_DBEntities())
            {
                var user = db.usuario.Find(id);
                ViewBag.Roles = new SelectList(db.rol.ToList(), "id_rol", "rol1", user.id_rol);
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult EditUser(usuario user)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var userDb = db.usuario.Find(user.id_usuario);

                userDb.nombre = user.nombre;
                userDb.apellido_1 = user.apellido_1;
                userDb.email = user.email;
                userDb.id_rol = user.id_rol;

                db.SaveChanges();
            }

            return RedirectToAction("ListUsers");
        }
    }
}