using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class HomeController : Controller
    {
        #region Index Público

        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region Log In

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsuarioModelo modelo)
        {
            using (var context = new SC604Proyecto_DBEntities())
            {
                var usuarioDb = context.usuario
                    .Where(u => u.email == modelo.Email)
                    .FirstOrDefault();

                // Usuario no existe
                if (usuarioDb == null)
                {
                    ViewBag.Message = "Usuario o contraseña incorrectos";
                    return View(modelo);
                }

                // Usuario bloqueado
                if (usuarioDb.bloqueado)
                {
                    ViewBag.Message = "Usuario bloqueado por múltiples intentos fallidos";
                    return View(modelo);
                }

                // Contraseña incorrecta
                if (usuarioDb.contrasena != modelo.Contrasena)
                {
                    usuarioDb.intentos_fallidos++;

                    if (usuarioDb.intentos_fallidos >= 3)
                    {
                        usuarioDb.bloqueado = true;
                    }

                    context.SaveChanges();

                    ViewBag.Message = "Usuario o contraseña incorrectos";
                    return View(modelo);
                }

                // Login correcto
                usuarioDb.intentos_fallidos = 0;
                context.SaveChanges();

                // Cerrar Sesión
                Session["IdUsuario"] = usuarioDb.id_usuario;
                Session["Nombre"] = usuarioDb.nombre;
                Session["Rol"] = usuarioDb.id_rol;

                return RedirectToAction("IndexUser");
            }
        }

        #endregion


        #region Logout

        public ActionResult Logout()
        {
            Session.Clear();

            Session.Abandon();

            return RedirectToAction("Login", "Home");
        }

        #endregion


        #region Sign Up

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(UsuarioModelo modelo)
        {
            if (modelo.Contrasena != modelo.ConfirmPassword)
            {
                ViewBag.Message = "Las contraseñas no coinciden";
                return View(modelo);
            }

            using (var context = new SC604Proyecto_DBEntities())
            {
                var existe = context.usuario
                    .Where(u => u.email == modelo.Email)
                    .FirstOrDefault();

                if (existe != null)
                {
                    ViewBag.Message = "El correo ya está registrado";
                    return View(modelo);
                }

                var nuevoUsuario = new usuario
                {
                    nombre = modelo.Nombre,
                    apellido_1 = modelo.Apellido1,
                    usuario1 = modelo.UsuarioLogin,
                    email = modelo.Email,
                    contrasena = modelo.Contrasena,
                    id_rol = 2,
                    intentos_fallidos = 0,
                    bloqueado = false
                };

                context.usuario.Add(nuevoUsuario);
                context.SaveChanges();
            }

            return RedirectToAction("Login");
        }

        #endregion


        #region AccountRecovery

        public ActionResult AccountRecovery()
        {
            return View();
        }

        #endregion

    }
}