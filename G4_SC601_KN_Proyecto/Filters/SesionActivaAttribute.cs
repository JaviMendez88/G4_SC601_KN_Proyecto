using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Filters
{
  
        public class SesionActivaAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var session = filterContext.HttpContext.Session;

                if (session["IdUsuario"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "Login"
                        })
                    );
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
