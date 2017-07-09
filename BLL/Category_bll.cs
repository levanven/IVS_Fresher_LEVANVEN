using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using DAL;
namespace BLL
{
    public class Category_bll : Interface_category
    {
        public db_category category_bll;
        public Category_bll()
        {
            category_bll = new db_category();
        }
        public int Delete(List<int>id, out List<string> lstMsg)
        {
            return category_bll.Delete(id, out lstMsg);
        }

        public List<DisplayCategory_model> GetAll()
        {
           return category_bll.GetAll();
        }

        public int GetByID(long ID, out Category_model model)
        {
            return category_bll.GetByID(ID, out model);
        }

        public List<CategoryParent_model> GetCategoryByID(long id)
        {
            return category_bll.GetCategoryByID(id);
        }

        public int GetCategory(bool flag, out List<GetCatetory_model> lstcategory)
        {
            return category_bll.GetCategory(flag, out lstcategory);
        }
        public int GetDetail(long ID, out DisplayCategory_model model)
        {
            return category_bll.GetDetail(ID, out model);
        }

        public List<CategoryParent_model> GetListParent()
        {
            return category_bll.GetListParent();
        }

        public List<CategoryParent_model> GetParent()
        {
            return category_bll.GetParent();
        }

        public int Insert(Category_model model, out List<string> lstMsg)
        {
            return category_bll.Insert(model, out lstMsg);
        }

        public int Search(SearchCategory_model model, out List<DisplayCategory_model> data, out int page, int pagenumber)
        {
            return category_bll.Search(model, out data, out page, pagenumber);
        }

        public int Update(Category_model model, out List<string> lstMsg)
        {
            return category_bll.Update(model, out lstMsg);
        }
    }
}
