using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using DAL;
namespace BLL
{
   public class Measure_bll :Interface_measure
    {
        public db_measure  measure_bll;
        public Measure_bll()
        {
            measure_bll = new db_measure();
        }
        public List<Measure_model> GetAll()
        {
            return measure_bll.GetAll();
        }
        public int Search(Measure_model model, out List<Measure_model> lstmodel, out int total, int _page)
        {
            return measure_bll.Search(model, out lstmodel, out total, _page);
        }
        public int Insert(Measure_model model, out List<string> lstMsg)
        {
            return measure_bll.Insert(model, out lstMsg);
        }
        public int Update(Measure_model model, out List<string> lstMsg)
        {
            return measure_bll.Update(model, out lstMsg);
        }
        public int Delete(List<int> id, out List<string> lstMsg)
        {
            return measure_bll.Delete(id, out lstMsg);
        }
        public int GetDetail(int id, out Measure_model model)
        {
            return measure_bll.GetDetail(id, out model);
        }
    }
}
