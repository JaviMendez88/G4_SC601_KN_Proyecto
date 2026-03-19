using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;


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


        #region Index User

        public ActionResult IndexUser()
        {
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Login");
            }
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


        #region AccountRecovery

        // Se crea un step de Account Recovery en un form para después trasladar al user a una pantalla adicional
        // de Reset Password, esto para evitar uso de correo de terceros para resetear su
        // contraseña sin que el usuario se entere

        [HttpPost]
        public ActionResult AccountRecovery(UsuarioModelo modelo)
        {
            using (var context = new SC604Proyecto_DBEntities())
            {
                var usuarioDb = context.usuario
                    .Where(u => u.email == modelo.Email)
                    .FirstOrDefault();

                if (usuarioDb != null)
                {
                    string passwordTemporal = GenerarPasswordTemporal();

                    usuarioDb.contrasena = passwordTemporal;
                    usuarioDb.intentos_fallidos = 0;
                    usuarioDb.bloqueado = false;

                    context.SaveChanges();

                    EnviarCorreoPasswordTemporal(modelo.Email, passwordTemporal);
                }

                ViewBag.Message = "Se ha enviado una contraseña temporal.";
                return View();
            }
        }

        #endregion


        #region ResetPassword

        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            return View(new UsuarioModelo { IdUsuario = id });
        }

        [HttpPost]
        public ActionResult ResetPassword(UsuarioModelo modelo)
        {
            if (modelo.Contrasena != modelo.ConfirmPassword)
            {
                ViewBag.Message = "Las contraseñas no coinciden";
                return View(modelo);
            }

            using (var context = new SC604Proyecto_DBEntities())
            {
                var usuarioDb = context.usuario
                    .Where(u => u.id_usuario == modelo.IdUsuario)
                    .FirstOrDefault();

                if (usuarioDb == null)
                {
                    return RedirectToAction("Login");
                }

                usuarioDb.contrasena = modelo.Contrasena;
                usuarioDb.intentos_fallidos = 0;
                usuarioDb.bloqueado = false;

                context.SaveChanges();
            }

            return RedirectToAction("Login");
        }

        #endregion


        #region TempPassword

        private string GenerarPasswordTemporal(int longitud = 8)
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@$!";
            Random random = new Random();
            return new string(Enumerable.Repeat(caracteres, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        #region EnviarCorreo
        private void EnviarCorreoPasswordTemporal(string destinatario, string passwordTemporal)
        {
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("proyectoufidelitas@gmail.com");
            correo.To.Add(destinatario);
            correo.Subject = "Recuperación de contraseña";
            correo.Body = $@"
Hola,

Se ha solicitado una recuperación de contraseña.

Tu contraseña temporal es:
{passwordTemporal}

Por favor inicia sesión y cámbiala a la brevedad.

Saludos.";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(
                "proyectoufidelitas@gmail.com",
                "rwelrkfhnsifkqzf"
            );
            smtp.EnableSsl = true;

            smtp.Send(correo);
        }

        #endregion

    }
}