using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Filters;
using G4_SC601_KN_Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    [SesionActiva]

    public class MaterialController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        #region ConsultMaterial

        [HttpGet]
        public ActionResult ConsultMaterial()
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var materials = db.material.Include("family").Include("parent").AsNoTracking().ToList();
                return View(materials);
            }

        }
        #endregion

        #region create Material
        [HttpGet]
        public ActionResult CreateMaterial()
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var families = db.family.
                    Select(f => new
                    {
                        f.id_family,
                        f.title
                    }).ToList();
                ViewBag.Families = new SelectList(families, "id_family", "title");
                var parents = db.parent.Select(p => new
                {
                    p.id_parent,
                    p.codigo
                }).ToList();
                ViewBag.Parents = new SelectList(parents, "id_parent", "codigo");
                return View();
            }

        }


        [HttpPost]
        public ActionResult CreateMaterial(materialModel model)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {

                // Validar que el id_family existe
                if (model.idFamily <= 0 || !db.family.Any(f => f.id_family == model.idFamily))
                {
                    ViewBag.Families = new SelectList(db.family.ToList(), "id_family", "title");
                    ViewBag.Parents = new SelectList(db.parent.ToList(), "id_parent", "codigo");
                    ViewBag.Message = "Debe seleccionar una familia válida";
                    return View(model);
                }

                // Validar que el id_parent existe
                if (model.idParent <= 0 || !db.parent.Any(p => p.id_parent == model.idParent))
                {
                    ViewBag.Families = new SelectList(db.family.ToList(), "id_family", "title");
                    ViewBag.Parents = new SelectList(db.parent.ToList(), "id_parent", "codigo");
                    ViewBag.Message = "Debe seleccionar un padre válido";
                    return View(model);
                }

                // Calcular totalVariance en el servidor
                decimal totalVariance = model.labor + model.IndOVH + model.Scrap_Allowance + model.materialPrice;

                var newMaterial = db.material.Add(new material
                {
                    id_family = model.idFamily,
                    id_parent = model.idParent,
                    descriptionM = model.descriptionM,
                    count_Order = model.countOrder,
                    Qty = model.Qty,
                    labor = model.labor,
                    Ind_OVH = model.IndOVH,
                    Scrap_Allowance = model.Scrap_Allowance,
                    materialP = model.materialPrice,
                    total_Variance = totalVariance
                });
                db.SaveChanges();
                ViewBag.Message = "Material creado exitosamente";
            }
            return RedirectToAction("ConsultMaterial", "Material");

        }


        #endregion

        #region Update Material
        [HttpGet]
        public ActionResult EditMaterial(int id)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var material = db.material.Where(m => m.id_material == id).FirstOrDefault();
                if (material == null)
                {
                    return RedirectToAction("ConsultMaterial");
                }

                // Cargar dropdowns
                var families = db.family.Select(f => new { f.id_family, f.title }).ToList();
                ViewBag.Families = new SelectList(families, "id_family", "title", material.id_family);

                var parents = db.parent.Select(p => new { p.id_parent, p.codigo }).ToList();
                ViewBag.Parents = new SelectList(parents, "id_parent", "codigo", material.id_parent);

                // Convertir entidad a modelo
                var model = new materialModel
                {
                    idMaterial = material.id_material,
                    idFamily = material.id_family,
                    idParent = material.id_parent,
                    descriptionM = material.descriptionM,
                    countOrder = material.count_Order,
                    Qty = material.Qty,
                    labor = material.labor,
                    IndOVH = material.Ind_OVH,
                    Scrap_Allowance = material.Scrap_Allowance,
                    materialPrice = material.materialP,
                    totalVariance = material.total_Variance
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditMaterial(materialModel model)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                // Validar que el material existe
                var datos = db.material.Where(m => m.id_material == model.idMaterial).FirstOrDefault();
                if (datos == null)
                {
                    ViewBag.Material = "Material no encontrado";
                    return View(model);
                }

                // Validar que el id_family existe
                if (model.idFamily <= 0 || !db.family.Any(f => f.id_family == model.idFamily))
                {
                    ViewBag.Material = "Debe seleccionar una familia válida";
                    return View(model);
                }

                // Validar que el id_parent existe
                if (model.idParent <= 0 || !db.parent.Any(p => p.id_parent == model.idParent))
                {
                    ViewBag.Material = "Debe seleccionar un padre válido";
                    return View(model);
                }

                // Actualizar datos
                datos.id_family = model.idFamily;
                datos.id_parent = model.idParent;
                datos.descriptionM = model.descriptionM;
                datos.count_Order = model.countOrder;
                datos.Qty = model.Qty;
                datos.labor = model.labor;
                datos.Ind_OVH = model.IndOVH;
                datos.Scrap_Allowance = model.Scrap_Allowance;
                datos.materialP = model.materialPrice;

                // Calcular totalVariance
                decimal totalVariance = model.labor + model.IndOVH + model.Scrap_Allowance + model.materialPrice;
                datos.total_Variance = totalVariance;

                var result = db.SaveChanges();

                if (result <= 0)
                {
                    ViewBag.Material = "Error al actualizar el material";
                    return View(model);
                }
                return RedirectToAction("ConsultMaterial", "Material");
            }
        }

        #endregion

        #region Delete Material
        [HttpGet]
        public ActionResult DeleteMaterial(int id)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var material = db.material.Where(m => m.id_material == id).FirstOrDefault();
                if (material != null)
                {
                    db.material.Remove(material);
                    db.SaveChanges();
                }
                return RedirectToAction("ConsultMaterial", "Material");
            }
        }

        #endregion


        #region InsertFamily

        [HttpGet]
        public ActionResult CreateFamily()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateFamily(family table)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var newFamily = db.family.Add(new family
                {
                    id_family = table.id_family,
                    title = table.title
                });
                db.SaveChanges();
            }
            return RedirectToAction("ConsultMaterial", "Material");
        }
        #endregion

        #region DeleteFamily
        [HttpGet]
        public ActionResult DeleteFamily(int id)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var family = db.family.Where(f => f.id_family == id).FirstOrDefault();
                if (family != null)
                {
                    db.family.Remove(family);
                    db.SaveChanges();
                }
                return RedirectToAction("ConsultMaterial", "Material");
            }
        }

        #endregion

        #region Insert Parent
        [HttpGet]
        public ActionResult CreateParent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateParent(parent table)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var newParent = db.parent.Add(new parent
                {
                    id_parent = table.id_parent,
                    codigo = table.codigo
                });
                db.SaveChanges();
            }
            return RedirectToAction("ConsultMaterial", "Material");
        }
        #endregion

        #region delete parent

        [HttpGet]
        public ActionResult DeleteParent(int id)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var parent = db.parent.Where(p => p.id_parent == id).FirstOrDefault();
                if (parent != null)
                {
                    db.parent.Remove(parent);
                    db.SaveChanges();
                }
                return RedirectToAction("ConsultMaterial", "Material");
            }
        }


        #endregion

    }
}
    

