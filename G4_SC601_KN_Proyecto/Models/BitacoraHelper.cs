using System;
using G4_SC601_KN_Proyecto.EntityFramework;

namespace G4_SC601_KN_Proyecto.Models
{
    public static class BitacoraHelper
    {
        public static void GuardarError(Exception ex, string contexto)
        {
            try
            {
                using (var db = new ProyectoDBEntities())
                {
                    var error = new bitacora_errores
                    {
                        mensaje_error = ex.Message,
                        stack_trace = ex.StackTrace,
                        contexto = contexto,
                        fecha_hora = DateTime.Now,
                        usuario = "Sistema"
                    };

                    db.bitacora_errores.Add(error);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Si la bitácora falla, no bloqueamos al usuario
            }
        }
    }
}