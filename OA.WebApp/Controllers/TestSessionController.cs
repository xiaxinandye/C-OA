using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Controllers
{
    public class TestSessionController : Controller
    {
        // GET: TestSession
        public ActionResult Index()
        {
            //object a = null;
            //return Content(a.ToString());
            int a = 0;
            int c = 2 / a;
            return Content(c.ToString());
        }
    }
}