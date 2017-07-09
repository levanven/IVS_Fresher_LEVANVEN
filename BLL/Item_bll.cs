using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using DAL;
namespace BLL
{
    public class Item_bll : Interface_Item
    {
        public db_item item_bll;
        public Item_bll()
        {
            item_bll = new db_item();
        }
        public List<DisplayItem_model> GetAll()
        {
            return item_bll.GetAll();
        }
        public int Search(ListItem_model model, out List<DisplayItem_model> lstmodel, out int total, int _page)
        {
            return item_bll.Search(model, out lstmodel, out total, _page);
        }
        public int Insert(Item_model model, out List<string> lstMsg)
        {
            return item_bll.Insert(model, out lstMsg);
        }
        public int Update(Item_model model, out List<string> lstMsg)
        {
            return item_bll.Update(model, out lstMsg);
        }
        public int Delete(List<int> lstID, out List<string> lstMsg)
        {
            return item_bll.Delete(lstID, out lstMsg);
        }
        public int GetDetail(int id, out Item_model model)
        {
            return item_bll.GetDetail(id, out model);
        }
        public int GetCategory(bool flag, out List<GetCatetory_model> lstcategory)
        {
            return item_bll.GetCategory(flag, out lstcategory);
        }
        public int GetMeasure(bool flag, out List<GetMeasure_model> lstmeasure)
        {
            return item_bll.GetMeasure(flag, out lstmeasure);
        }
    }
}
