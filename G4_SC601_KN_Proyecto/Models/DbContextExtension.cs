

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
                .Where(e => e.Entity.GetType().Name != "auditoria" &&
                       (e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted));

            if (!entidadesModificadas.Any())
                return;

            if (HttpContext.Current?.Session?["IdUsuario"] == null)
                return;

            int userIdSession = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            string nombreSession = HttpContext.Current.Session["Nombre"]?.ToString() ?? "Anonimo";

            foreach (var entry in entidadesModificadas)
            {
                var nuevaAuditoria = new auditoria
                {
                    NombreTabla = entry.Entity.GetType().Name,
                    accion = entry.State.ToString(),

                    // No Null. BD no lo permite. Si no hay valores, se guarda un string vacío.
                    OldValues = entry.State == EntityState.Modified
                        ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                        : string.Empty,

                    NewValues = entry.State != EntityState.Deleted
                        ? JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                        : string.Empty,

                    id_usuario = userIdSession,
                    NombreUser = nombreSession,
                    date = DateTime.Now,
                };

                this.auditoria.Add(nuevaAuditoria);
            }
        }
    }
}
