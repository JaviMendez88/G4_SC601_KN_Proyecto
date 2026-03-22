

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
            int? userIdSession = 0;
            string nombreSession = "Anonimo";

            if (HttpContext.Current?.Session != null)
            {
                if (HttpContext.Current.Session["IdUsuario"] != null)
                {
                    userIdSession = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                }

                if (HttpContext.Current.Session["Nombre"] != null)
                {
                    nombreSession = HttpContext.Current.Session["Nombre"].ToString();
                }



            }
            foreach (var entry in entidadesModificadas)
            {


                var nuevaAuditoria = new auditoria
                {
                    NombreTabla = entry.Entity.GetType().BaseType.Name,
                    accion = entry.State.ToString(),
                    OldValues = entry.State == EntityState.Modified
                    ? JsonConvert.SerializeObject(entry.OriginalValues.ToObject())
                        : null,
                    NewValues = entry.State != EntityState.Deleted
                    ? JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                        : null,
                    id_usuario = userIdSession ?? 0,
                    NombreUser = nombreSession,
                    date = DateTime.Now,
                };

                this.auditoria.Add(nuevaAuditoria);
            }
        }

    }
}
