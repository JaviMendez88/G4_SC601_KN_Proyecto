using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Filters;
using G4_SC601_KN_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    [SesionActiva]
    public class InventarioController : Controller
    {
        private ProyectoDBEntities db = new ProyectoDBEntities();

        public ActionResult Index()
        {
            var listaStock = db.stock.Select(s => new StockViewModel
            {
                Material = s.material.descriptionM,
                Lote = s.lote.codigo_lote,
                Cantidad = s.cantidad,
                FechaVencimiento = s.lote.fecha_vencimiento
            }).ToList();

            DateTime fechaLimite = DateTime.Now.AddMonths(6);

            // Productos que vencen en los próximos 6 meses
            ViewBag.Vencimientos = listaStock
                .Where(s => s.FechaVencimiento <= fechaLimite)
                .OrderBy(s => s.FechaVencimiento)
                .ToList();

            return View(listaStock);
        }

        public ActionResult RegistrarMovimiento()
        {
            // Listas para los dropdowns del formulario
            ViewBag.Materials = db.material.ToList();
            ViewBag.Lotes = db.lote.ToList();
            ViewBag.Ubicaciones = db.ubicacion.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult RegistrarMovimiento(int id_material, int id_lote, int id_ubicacion, string tipo, int cantidad)
        {
            try
            {
                // Registrar el movimiento en el historial
                var nuevoMovimiento = new movimiento_inventario
                {
                    id_material = id_material,
                    id_lote = id_lote,
                    id_ubicacion = id_ubicacion,
                    tipo = tipo,
                    cantidad = cantidad,
                    fecha = DateTime.Now,
                    id_usuario = 3 // Temporal: usuario admin fijo
                };
                db.movimiento_inventario.Add(nuevoMovimiento);

                var stockActual = db.stock.FirstOrDefault(s => s.id_material == id_material && s.id_lote == id_lote && s.id_ubicacion == id_ubicacion);

                if (stockActual != null)
                {
                    if (tipo == "ENTRADA") stockActual.cantidad += cantidad;
                    if (tipo == "SALIDA") stockActual.cantidad -= cantidad;

                    // Evita cantidades negativas por seguridad
                    if (stockActual.cantidad < 0) stockActual.cantidad = 0;
                }
                else if (tipo == "ENTRADA")
                {
                    // Crear registro de stock si no existe y el movimiento es de entrada
                    var nuevoStock = new stock
                    {
                        id_material = id_material,
                        id_lote = id_lote,
                        id_ubicacion = id_ubicacion,
                        cantidad = cantidad
                    };
                    db.stock.Add(nuevoStock);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar: " + ex.Message;

                // Recargar listas para volver a mostrar el formulario
                ViewBag.Materials = db.material.ToList();
                ViewBag.Lotes = db.lote.ToList();
                ViewBag.Ubicaciones = db.ubicacion.ToList();
                return View();
            }
        }
    }
}