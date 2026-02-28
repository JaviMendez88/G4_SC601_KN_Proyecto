using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class HomeController : Controller
    {
        #region Index
        public ActionResult Index()
        {
            return View();
        }


        #endregion Index

        #region Log In
        public ActionResult Login()
        {
            return View();
        }


        #endregion Log In

        #region Sign Up

        public ActionResult Signup()
        {
            return View();
        }

        #endregion Sign Up

    }
}