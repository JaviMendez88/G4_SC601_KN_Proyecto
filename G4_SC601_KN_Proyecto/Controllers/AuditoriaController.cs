using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G4_SC601_KN_Proyecto.EntityFramework;
using System.Data.Entity;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class AuditoriaController : Controller
    {
        // GET: Auditoria
        public ActionResult AuditoriaCambios()
        {
            // Validar que un administrador esté en sesión (ajustar seguridad según corresponda)
            if (Session["Rol"] == null || (int)Session["Rol"] != 1)
                return RedirectToAction("Login", "Home");

            using (var db = new SC604Proyecto_DBEntities())
            {
                // Obtenemos los registros y los ordenamos por fecha descendente (los más recientes primero)
                // Usamos Include para traer los datos del usuario que hizo el cambio
                var listaAuditorias = db.auditoria
                                        .Include(a => a.usuario)
                                        .OrderByDescending(a => a.date)
                                        .Take(100) // Recomendable limitar para tableros grandes, puedes hacerlo paginado después
                                        .ToList();

                return View(listaAuditorias);
            }
        }
    }
}
