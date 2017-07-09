using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MODEL;
using BLL;
using PagedList;
using PagedList.Mvc;
namespace IVS.LeVanVen.Controllers
{
    public class ItemController : Controller
    {
        public Item_bll _itemBLL;

        public ItemController()
        {
            _itemBLL = new Item_bll();
        }
        [HttpGet]
        public ActionResult Index(ListItem_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            int total = new int();
            List<DisplayItem_model> lstModel = new List<DisplayItem_model>();
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            if (!string.IsNullOrEmpty(Session["code"] as string))
            {
                Model.code = Session["code"].ToString();
            }
            if (!string.IsNullOrEmpty(Session["name"] as string))
            {
                Model.name = Session["name"].ToString();
            }
            if (Session["category_id"] as int? != null)
            {
                Model.category_id = (int)Session["category_id"];
            }
            _itemBLL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<DisplayItem_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            _itemBLL.GetCategory(true, out lstcombobox);
            ViewBag.lstcombobox = lstcombobox;
            ViewBag.page = 0;
            if (page != null)
            {
                ViewBag.page = pageNumber - 1;
            }
            return View(new Tuple<ListItem_model, IPagedList<DisplayItem_model>>(Model, list));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(ListItem_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            List<DisplayItem_model> lstModel = new List<DisplayItem_model>();
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            int total = new int();
            _itemBLL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<DisplayItem_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            Session["code"] = Model.code;
            Session["name"] = Model.name;
            Session["category_id"] = Model.category_id;
            TempData["CountResult"] = total.ToString() + " row(s) found!";
            _itemBLL.GetCategory(true, out lstcombobox);
            ViewBag.lstcombobox = lstcombobox;
            return View(new Tuple<ListItem_model, IPagedList<DisplayItem_model>>(Model, list));
        }
        [HttpGet]
        public ActionResult Add()
        {
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            List<GetMeasure_model> _lstcombobox = new List<GetMeasure_model>();
            _itemBLL.GetCategory(true, out lstcombobox);
            _itemBLL.GetMeasure(true, out _lstcombobox);
            ViewBag.Category = lstcombobox;
            ViewBag.Measure = _lstcombobox;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Item_model Model)
        {
            List<string> lstMsg = new List<string>();
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            List<GetMeasure_model> _lstcombobox = new List<GetMeasure_model>();
            _itemBLL.GetCategory(true, out lstcombobox);
            _itemBLL.GetMeasure(true, out _lstcombobox);
            ViewBag.Category = lstcombobox;
            ViewBag.Measure = _lstcombobox;
            int returnCode = _itemBLL.Insert(Model, out lstMsg);
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
            Item_model Model = new Item_model();
            int returnCode = _itemBLL.GetDetail(id, out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new Item_model();
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

            Item_model Model = new Item_model();
            int returnCode = _itemBLL.GetDetail(id, out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new Item_model();
            }

            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            List<GetMeasure_model> _lstcombobox = new List<GetMeasure_model>();
            _itemBLL.GetCategory(true, out lstcombobox);
            _itemBLL.GetMeasure(true, out _lstcombobox);
            ViewBag.Category = lstcombobox;
            ViewBag.Measure = _lstcombobox;

            return View(Model);
        }
        [HttpPost]
        public ActionResult Edit(Item_model Model)
        {
            if (string.IsNullOrEmpty(Model.id.ToString()))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            List<string> lstMsg = new List<string>();
            int returnCode = _itemBLL.Update(Model, out lstMsg);
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            List<GetMeasure_model> _lstcombobox = new List<GetMeasure_model>();
            _itemBLL.GetCategory(true, out lstcombobox);
            _itemBLL.GetMeasure(true, out _lstcombobox);
            ViewBag.Category = lstcombobox;
            ViewBag.Measure = _lstcombobox;
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

            int returnCode = _itemBLL.Delete(id, out lstMsg);
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