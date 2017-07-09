using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;

namespace BLL
{
    interface Interface_Item
    {
        List<DisplayItem_model> GetAll();
        int Search(ListItem_model model, out List<DisplayItem_model> lstmodel, out int total, int _page);
        int Insert(Item_model model, out List<string> lstMsg);
        int Update(Item_model model, out List<string> lstMsg);
        int Delete(List<int> lstID, out List<string> lstMsg);
        int GetDetail(int id, out Item_model model);
        int GetCategory(bool flag, out List<GetCatetory_model> lstcategory);
        int GetMeasure(bool flag, out List<GetMeasure_model> lstmeasure);
    }
}
