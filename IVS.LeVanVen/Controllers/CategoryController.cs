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
    public class CategoryController : Controller
    {
        public Category_bll _categoryBL;
        public CategoryController()
        {
            _categoryBL = new Category_bll();
        }
        public ActionResult Index(SearchCategory_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            int total = new int();
            List<DisplayCategory_model> lstModel = new List<DisplayCategory_model>();
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            if (!string.IsNullOrEmpty(Session["code"] as string))
            {
                Model.code = Session["code"].ToString();
            }
            if (!string.IsNullOrEmpty(Session["name"] as string))
            {
                Model.name = Session["name"].ToString();
            }
            if (Session["parent_id"] as int? != null)
            {
                Model.parent_id = (int)Session["parent_id"];
            }
            _categoryBL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<DisplayCategory_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            _categoryBL.GetCategory(true, out lstcombobox);
            ViewBag.lstcombobox = lstcombobox;
            ViewBag.page = 0;
            if (page != null)
            {
                ViewBag.page = pageNumber - 1;
            }
            return View(new Tuple<SearchCategory_model, IPagedList<DisplayCategory_model>>(Model, list));
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(SearchCategory_model Model, int? page)
        {
            var pageNumber = page ?? 1;
            List<DisplayCategory_model> lstModel = new List<DisplayCategory_model>();
            List<GetCatetory_model> lstcombobox = new List<GetCatetory_model>();
            int total = new int();
            _categoryBL.Search(Model, out lstModel, out total, pageNumber);
            var list = new StaticPagedList<DisplayCategory_model>(lstModel, pageNumber, 15, total);
            ViewBag.ListSearch = lstModel.OrderByDescending(x => x.id);
            Session["code"] = Model.code;
            Session["name"] = Model.name;
            Session["parent_id"] = Model.parent_id;
            TempData["CountResult"] = total.ToString() + " row(s) found!";
            _categoryBL.GetCategory(true, out lstcombobox);
            ViewBag.lstcombobox = lstcombobox;
            return View(new Tuple<SearchCategory_model, IPagedList<DisplayCategory_model>>(Model, list));
        }
        #region ADD
        [HttpGet]
        public ActionResult Add()
        {
            Category_model Model = new Category_model();
            ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
            return View(Model);
        }

        [HttpPost]
        public ActionResult Add(Category_model Model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
                return View(Model);
            }
            List<string> lstMsg = new List<string>();
            int returnCode = _categoryBL.Insert(Model, out lstMsg);
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
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
        #endregion
        #region View & Edit
        [HttpGet]
        public ActionResult View(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }

            DisplayCategory_model Model = new DisplayCategory_model();
            int returnCode = _categoryBL.GetDetail(long.Parse(id), out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new DisplayCategory_model();
            }

            return View(Model);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }

            Category_model Model = new Category_model();
            int returnCode = _categoryBL.GetByID(long.Parse(id), out Model);
            if (Model == null)
            {
                TempData["Error"] = "Data has already been deleted by other user!";
                return RedirectToAction("Index");
            }
            if (!((int)Common.ReturnCode.Succeed == returnCode))
            {
                Model = new Category_model();
            }
            ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
            return View(Model);
        }
        [HttpPost]
        public ActionResult Edit(Category_model Model)
        {
            if (ModelState.IsValid)
            {
                List<string> lstMsg = new List<string>();
                int returnCode = _categoryBL.Update(Model, out lstMsg);

                if (!((int)Common.ReturnCode.Succeed == returnCode))
                {
                    if (lstMsg != null)
                    {
                        for (int i = 0; i < lstMsg.Count(); i++)
                        {
                            ModelState.AddModelError(string.Empty, lstMsg[i]);
                        }
                    }
                    ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
                    return View(Model);
                }
                TempData["Success"] = "Updated Successfully!";
                return RedirectToAction("View", new { @id = Model.id });
            }
            ViewBag.Parent = new SelectList(_categoryBL.GetListParent(), "id", "name");
            return View(Model);
        }
        #endregion
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            List<string> lstMsg = new List<string>();

            int returnCode = _categoryBL.Delete(id, out lstMsg);
            if (((int)Common.ReturnCode.Succeed == returnCode))
            {
                return Json(new { Message = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetParent()
        {
            var model = _categoryBL.GetParent();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategory(long id)
        {
            var model = _categoryBL.GetCategoryByID(id);
            CategoryParent_model item = new CategoryParent_model();
            item.id = 0;
            item.name = " ";
            model.Add(item);
            return Json(model.OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
        }
    }
}