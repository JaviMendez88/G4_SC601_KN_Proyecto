using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G4_SC601_KN_Proyecto.EntityFramework;
using G4_SC601_KN_Proyecto.Models;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class MaterialController : Controller
    {
        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsultMaterial()
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var materials = db.material.Include("family").Include("parent").ToList();
                return View(materials);
            }

        }

        #region create Material
        [HttpGet]
        public ActionResult CreateMaterial()
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var families = db.family.ToList();
                ViewBag.Families = new SelectList(families, "id_family", "title");
                var parents = db.material.ToList();
                ViewBag.Parents = new SelectList(parents, "id_Parent", "codigo");
                return View();
            }

        }


        [HttpPost]
        public ActionResult CreateMaterial(materialModel model)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                //validaciones 
                int mat = db.material.Count(m => m.descriptionM == model.descriptionM);
                if (mat >= 0)
                {
                    ViewBag.ErrorMessage = "El material ya existe";

                }

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
                    total_Variance = model.totalVariance
                });
                db.SaveChanges();
                ViewBag.SuccessMessage = "Material creado exitosamente";
            }
            return RedirectToAction("ConsultMaterial", "Material");

        }


        #endregion

        #region Update Material

        public ActionResult EditMaterial(int id)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var result = db.material.Where(m => m.id_material == 0).FirstOrDefault();
                return View(result);
            }
        }

        [HttpPost]
        public ActionResult EditMaterial(material model)
        {
            using (var db = new SC604Proyecto_DBEntities())
            {
                var datos = db.material.Where(m => m.id_material == model.id_material).FirstOrDefault();
                datos.id_family = model.id_family;
                datos.id_parent = model.id_parent;
                datos.descriptionM = model.descriptionM;
                datos.count_Order = model.count_Order;
                datos.Qty = model.Qty;
                datos.labor = model.labor;
                datos.Ind_OVH = model.Ind_OVH;
                datos.Scrap_Allowance = model.Scrap_Allowance;
                datos.materialP = model.materialP;
                datos.total_Variance = model.total_Variance;
                var result = db.SaveChanges();

                if (result <= 0)
                {
                    ViewBag.Material = "Error al actualizar el material";
                    return View();
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
    }
}
