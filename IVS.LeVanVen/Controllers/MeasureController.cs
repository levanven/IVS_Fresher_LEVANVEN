using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using PagedList;
using PagedList.Mvc;


namespace IVS.LeVanVen.Controllers
{
    public class MeasureController : Controller
    {
        public Measure_bll _measureBLL;
        public MeasureController()
        {
            _measureBLL = new Measure_bll();
        }

        [HttpGet]
        public ActionResult Index(Measure_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            List<Measure_model> lstModel = new List<Measure_model>();
            int total = new int();
            if (!string.IsNullOrEmpty(Session["m_code"] as string))
            {
                Model.code = Session["m_code"].ToString();
            }
            if (!string.IsNullOrEmpty(Session["m_name"] as string))
            {
                Model.name = Session["m_name"].ToString();
            }
            _measureBLL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<Measure_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            ViewBag.page = 0;
            if (page != null)
            {
                ViewBag.page = pageNumber - 1;
            }
            return View(new Tuple<Measure_model, IPagedList<Measure_model>>(Model, list));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(Measure_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            List<Measure_model> lstModel = new List<Measure_model>();
            int total = new int();
            _measureBLL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<Measure_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            Session["m_code"] = Model.code;
            Session["m_name"] = Model.name;
            TempData["CountResult"] = total.ToString() + " row(s) found!";
            return View(new Tuple<Measure_model, IPagedList<Measure_model>>(Model, list));
        }

        [HttpGet]
        public ActionResult Add()
        {
            Measure_model Model = new Measure_model();

            return View(Model);
        }

        [HttpPost]
        public ActionResult Add(Measure_model Model)
        {
            List<string> lstMsg = new List<string>();
            int returnCode = _measureBLL.Insert(Model, out lstMsg);
            if (ModelState.IsValid)
            {
                if (!((int)Common.ReturnCode.Succeed == returnCode))
                {
                    if (lstMsg != null)
                    {
                        for (int i = 0; i < lstMsg.Count(); i++)
                        {
                            ModelState.AddModelError(string.Empty, lstMsg[i]);
                        }
                    }
                    return View(Model);
                }
                TempData["Success"] = "Inserted Successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            Measure_model Model = new Measure_model();
            int returnCode = _measureBLL.GetDetail(id, out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new Measure_model();
            }

            return View(Model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }

            Measure_model Model = new Measure_model();
            int returnCode = _measureBLL.GetDetail(id, out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new Measure_model();
            }

            return View(Model);
        }
        [HttpPost]
        public ActionResult Edit(Measure_model Model)
        {
            if (string.IsNullOrEmpty(Model.id.ToString()))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            List<string> lstMsg = new List<string>();
            int returnCode = _measureBLL.Update(Model, out lstMsg);
            if (ModelState.IsValid)
            {
                if (!((int)Common.ReturnCode.Succeed == returnCode))
                {
                    if (lstMsg != null)
                    {
                        for (int i = 0; i < lstMsg.Count(); i++)
                        {
                            ModelState.AddModelError(string.Empty, lstMsg[i]);
                        }
                    }
                    return View(Model);
                }
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            List<string> lstMsg = new List<string>();

            int returnCode = _measureBLL.Delete(id, out lstMsg);
            if (((int)Common.ReturnCode.Succeed == returnCode))
            {
                return Json(new { Message = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}