using ClosedXML.Excel;
using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    [SesionActiva]
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


        // Región "Importar Excel" -- relacionada al AdminDashboard para alimentar SQL mediante imports con Excel
        // Pendiente de desarrollar - RF05

        #region Importar Excel - In Progress RF05

        [HttpPost]
    public ActionResult ImportarExcel(HttpPostedFileBase archivoExcel)
    {
        if (archivoExcel == null || archivoExcel.ContentLength == 0)
        {
            TempData["Error"] = "Debe seleccionar un archivo Excel válido.";
            return RedirectToAction("AdminDashboard");
        }

        try
        {
            using (var workbook = new XLWorkbook(archivoExcel.InputStream))
            using (var db = new SC604Proyecto_DBEntities())
            {
                var hoja = workbook.Worksheet(1);
                var filas = hoja.RowsUsed().Skip(1);

                foreach (var row in filas)
                {
                    // Read On de columnas en excel
                    string familyName = row.Cell(38).GetString().Trim();
                    string parentCode = row.Cell(3).GetString().Trim();
                    string materialDesc = row.Cell(18).GetString().Trim();
                    string codigoLoteExcel = row.Cell(39).GetString().Trim();

                        // Fallback por seguridad
                        if (string.IsNullOrEmpty(codigoLoteExcel))
                        {
                            codigoLoteExcel = "IMPORT-EXCEL";
                        }


                        int qty = (int)Math.Max(0, row.Cell(25).GetDouble());


                        decimal labor = row.Cell(30).GetValue<decimal>();
                        decimal indOvh = row.Cell(31).GetValue<decimal>();
                        decimal scrap = row.Cell(32).GetValue<decimal>();
                        decimal materialPrice = row.Cell(34).GetValue<decimal>();


                        // Validación mínima
                        if (string.IsNullOrEmpty(materialDesc))
                        continue;

                    // Tabla Family
                    family family = null;
                    if (!string.IsNullOrEmpty(familyName))
                    {
                        family = db.family.FirstOrDefault(f => f.title == familyName);
                        if (family == null)
                        {
                            family = new family
                            {
                                title = familyName
                            };
                            db.family.Add(family);
                            db.SaveChanges();
                        }
                    }

                    // ====== PARENT ======
                    parent parent = null;
                    if (!string.IsNullOrEmpty(parentCode))
                    {
                        parent = db.parent.FirstOrDefault(p => p.codigo == parentCode);
                        if (parent == null)
                        {
                            parent = new parent
                            {
                                codigo = parentCode
                            };
                            db.parent.Add(parent);
                            db.SaveChanges();
                        }
                    }

                    // Material
                    decimal totalVariance = labor + indOvh + scrap + materialPrice;

                    var material = db.material.FirstOrDefault(m =>
                        m.descriptionM == materialDesc &&
                        m.id_parent == parent.id_parent);

                    if (material == null)
                    {
                        material = new material
                        {
                            id_family = family?.id_family ?? 1,
                            id_parent = parent.id_parent,
                            descriptionM = materialDesc,
                            count_Order = 0,
                            Qty = qty,
                            labor = (int)labor,
                            Ind_OVH = indOvh,
                            Scrap_Allowance = (int)scrap,
                            materialP = materialPrice,
                            total_Variance = totalVariance
                        };
                        db.material.Add(material);
                        db.SaveChanges();
                    }
                    else
                    {
                        material.Qty += qty;
                        material.labor = (int)labor;
                        material.Ind_OVH = indOvh;
                        material.Scrap_Allowance = (int)scrap;
                        material.materialP = materialPrice;
                        material.total_Variance = totalVariance;
                        db.SaveChanges();
                    }


                        // ====== LOTE (desde Excel) ======
                        var lote = db.lote.FirstOrDefault(l =>
                            l.id_material == material.id_material &&
                            l.codigo_lote == codigoLoteExcel);

                        if (lote == null)
                        {
                            lote = new lote
                            {
                                id_material = material.id_material,
                                codigo_lote = codigoLoteExcel,
                                fecha_ingreso = DateTime.Today,
                                fecha_vencimiento = DateTime.Today.AddYears(2)
                            };
                            db.lote.Add(lote);
                            db.SaveChanges();
                        }


                        // Stock

                        var ubicacionDefault = db.ubicacion.FirstOrDefault();

                        if (ubicacionDefault == null)
                            throw new Exception("No existen ubicaciones registradas.");

                        int idUbicacionDefault = ubicacionDefault.id_ubicacion;


                        var stock = db.stock.FirstOrDefault(s =>
                        s.id_material == material.id_material &&
                        s.id_lote == lote.id_lote &&
                        s.id_ubicacion == idUbicacionDefault);

                    if (stock == null)
                    {
                        stock = new stock
                        {
                            id_material = material.id_material,
                            id_lote = lote.id_lote,
                            id_ubicacion = idUbicacionDefault,
                            cantidad = qty
                        };
                        db.stock.Add(stock);
                    }
                    else
                    {
                        stock.cantidad += qty;
                    }

                        // Tabla de movimientos y ajuste de inventario
                        db.movimiento_inventario.Add(new movimiento_inventario
                    {
                        id_material = material.id_material,
                        id_lote = lote.id_lote,
                        id_ubicacion = idUbicacionDefault,
                        tipo = "AJUSTE",
                        cantidad = qty,
                        fecha = DateTime.Now,
                        id_usuario = Convert.ToInt32(Session["IdUsuario"])
                    });

                    db.SaveChanges();
                }
            }

            TempData["Success"] = "El archivo Excel fue importado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al importar el archivo: " + ex.Message;
        }

            return RedirectToAction("AdminDashboard", "Home");
        }

        #endregion Importar Excel - In Progress RF05



    }
}