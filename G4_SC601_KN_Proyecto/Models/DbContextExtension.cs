

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
            try
            {
                AuditarCambios();
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in entityErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Entidad: {entityErrors.Entry.Entity.GetType().Name} | " +
                            $"Propiedad: {error.PropertyName} | " +
                            $"Error: {error.ErrorMessage}"
                        );
                    }
                }

                throw;
            }
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
                    id_usuario = userIdSession,

                    NombreTabla = entry.Entity.GetType().Name.Length > 100
                        ? entry.Entity.GetType().Name.Substring(0, 100)
                        : entry.Entity.GetType().Name,

                    accion = entry.State.ToString(),

                    OldValues = entry.State == EntityState.Modified
                        ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                        : string.Empty,

                    NewValues = entry.State != EntityState.Deleted
                        ? JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                        : string.Empty,

                    NombreUser = nombreSession.Length > 50
                        ? nombreSession.Substring(0, 50)
                        : nombreSession,

                    date = DateTime.Now
                };

                this.auditoria.Add(nuevaAuditoria);
            }

        }
    }
}
