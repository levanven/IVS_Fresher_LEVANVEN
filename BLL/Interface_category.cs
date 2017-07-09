using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
namespace BLL
{
    interface Interface_category
    {
        List<DisplayCategory_model> GetAll();
        List<CategoryParent_model> GetListParent();
        int GetByID(long ID, out Category_model model);
        int GetDetail(long ID, out DisplayCategory_model model);
        int Insert(Category_model model, out List<string> lstMsg);
        int Update(Category_model model, out List<string> lstMsg);
        int Delete(List<int> id, out List<string> lstMsg);
        List<CategoryParent_model> GetParent();
        List<CategoryParent_model> GetCategoryByID(long id);
        int GetCategory(bool hasEmpty, out List<GetCatetory_model> lstCombobox);
        int Search(SearchCategory_model model, out List<DisplayCategory_model> data, out int page, int pagenumber);
    }
}
