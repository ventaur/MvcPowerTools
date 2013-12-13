﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlConventionsSample.Models;
using MvcPowerTools.Controllers;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var f = new Fixture();           
            return View(f.Create<FluentMyModel>());
        }

        [HttpPost]
        public ActionResult Index(FluentMyModel model)
        {
            if (ModelState.IsValid)
            {
                return Content("ok");
            }
            return Content("not ok");

            //return View(model);
        }

        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var fixture = new Fixture();
            var model=fixture.Build<MyModel>().Without(d => d.File).Create<MyModel>();
            return View(model);
        }

        public ActionResult Contact(string bla="lol")
        {
            ViewBag.Message = "Your contact page."+bla;

            return View();
        }
    }
}