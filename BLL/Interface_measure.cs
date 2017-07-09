using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;

namespace BLL
{
    interface Interface_measure
    {
        List<Measure_model> GetAll();
        int Search(Measure_model searchCondition, out List<Measure_model> lstModel, out int total, int _page);
        int Insert(Measure_model Model, out List<string> lstMsg);
        int Update(Measure_model Model, out List<string> lstMsg);
        int Delete(List<int> id, out List<string> lstMsg);
        int GetDetail(int id, out Measure_model Model);
    }
}
