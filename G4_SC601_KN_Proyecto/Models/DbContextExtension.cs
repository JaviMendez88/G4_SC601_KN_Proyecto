
/*

using System;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Newtonsoft.Json;

namespace G4_SC601_KN_Proyecto.EntityFramework 
{
    public partial class SC604Proyecto_DBEntities
    {
        public override int SaveChanges()
        {
            AuditarCambios();
            return base.SaveChanges();
        }

        private void AuditarCambios()
        {
            var entidadesModificadas = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            if (!entidadesModificadas.Any()) return;

            int? userId = null;
            if (HttpContext.Current != null && HttpContext.Current.Session["IdUsuario"] != null)
            {
                userId = (int)HttpContext.Current.Session["IdUsuario"];
            }

            if (userId == null) return;

            foreach (var entry in entidadesModificadas)
            {
                if (entry.Entity.GetType().Name.ToLower().Contains("auditoria")) continue;

                var nuevaAuditoria = new auditoria
                {
                    tabla_afectada = entry.Entity.GetType().Name.Split('_')[0],
                    accion = entry.State.ToString(),
                    fecha = DateTime.Now,
                    id_usuario = userId.Value,
                    detalles = GenerarDetallesJSON(entry)
                };

                this.auditoria.Add(nuevaAuditoria); 
            }
        }

        private string GenerarDetallesJSON(DbEntityEntry entry)
        {
            try
            {
                var detalles = new
                {
                    entidad = entry.Entity.GetType().Name,
                    cambios = entry.CurrentValues.PropertyNames
                        .Where(prop => entry.State == EntityState.Added || 
                                      (entry.State == EntityState.Modified && 
                                       !Equals(entry.OriginalValues[prop], entry.CurrentValues[prop])) ||
                                      entry.State == EntityState.Deleted)
                        .Select(prop => new
                        {
                            propiedad = prop,
                            valorAnterior = entry.State != EntityState.Added ? entry.OriginalValues[prop] : null,
                            valorNuevo = entry.State != EntityState.Deleted ? entry.CurrentValues[prop] : null
                        })
                        .ToList()
                };

                return JsonConvert.SerializeObject(detalles, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }
    }
}
*/