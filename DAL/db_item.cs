using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using Dapper;

namespace DAL
{
   public class db_item
    {
        #region connection
        public IDbConnection db;
        public db_item()
        {
            db = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        #endregion
        #region GetAll
        public List<DisplayItem_model> GetAll()
        {
            string sql = "SELECT `id`, `code`, `name`, `specification`, `description`, `dangerous` FROM `product_item` WHERE TRUE";
            return db.Query<DisplayItem_model>(sql).ToList();
        }
        #endregion
        #region GetDetail
        public int GetDetail(int id, out Item_model Model)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            Model = new Item_model();
            try
            {
                if (id != 0)
                {
                    string sql = "SELECT p.`id`, p.`category_id`, c.`name` AS `category_name`, p.`code`, p.`name`, p.`specification`, p.`description`, p.`dangerous`, p.`discontinued_datetime`, ";
                    sql += "p.`inventory_measure_id`, m1.`name` AS `inventory_measure_name`, p.`inventory_expired`, p.`inventory_standard_cost`, p.`inventory_list_price`, p.`manufacture_day`, ";
                    sql += "p.`manufacture_make`, p.`manufacture_tool`, p.`manufacture_finished_goods`, p.`manufacture_size`, p.`manufacture_size_measure_id`, m2.`name` AS `manufacture_size_measure_name`, ";
                    sql += "p.`manufacture_weight`, p.`manufacture_weight_measure_id`, m3.`name` AS `manufacture_weight_measure_name`, p.`manufacture_style`, p.`manufacture_class`, ";
                    sql += "`manufacture_color` FROM `product_item` AS p ";
                    sql += "LEFT OUTER JOIN `product_measure` AS m1 ON ( p.`inventory_measure_id` = m1.`id`) ";
                    sql += "LEFT OUTER JOIN `product_measure` AS m2 ON ( p.`manufacture_size_measure_id` = m2.`id`) ";
                    sql += "LEFT OUTER JOIN `product_measure` AS m3 ON ( p.`manufacture_weight_measure_id` = m3.`id`) ";
                    sql += "LEFT OUTER JOIN `product_category` AS c ON ( p.`category_id` = c.`id`) ";
                    sql += "WHERE p.`id` = @id ";
                    Model = db.Query<Item_model>(sql, new { id = id }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                return (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region GetCategory
        public int GetCategory(bool hasEmpty, out List<GetCatetory_model> lstCombobox)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstCombobox = new List<GetCatetory_model>();
            try
            {

                string sql = "SELECT `id`, `name` FROM `product_category`";
                lstCombobox = db.Query<GetCatetory_model>(sql).ToList();
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        public int GetMeasure(bool hasEmpty, out List<GetMeasure_model> lstCombobox)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstCombobox = new List<GetMeasure_model>();
            try
            {

                string sql = "SELECT `id`, `name` FROM `product_measure`";
                lstCombobox = db.Query<GetMeasure_model>(sql).ToList();
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region Insert
        public int Insert(Item_model Model, out List<string> lstMsg)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();
            try
            {
                if (isError(Model, (int)Common.ActionType.Add, out lstMsg))
                {
                    return (int)Common.ReturnCode.UnSuccess;
                }
                string sql = "INSERT INTO `product_item`";
                sql += "(`category_id`, `code`, `name`, `specification`, `description`, `dangerous`, `discontinued_datetime`, ";
                sql += "`inventory_measure_id`, `inventory_expired`, `inventory_standard_cost`, `inventory_list_price`, `manufacture_day`, ";
                sql += "`manufacture_make`, `manufacture_tool`, `manufacture_finished_goods`, `manufacture_size`, `manufacture_size_measure_id`, ";
                sql += "`manufacture_weight`, `manufacture_weight_measure_id`, `manufacture_style`, `manufacture_class`, ";
                sql += "`manufacture_color`) VALUES (@category_id, @code, @name, @specification, @description, @dangerous, @discontinued_datetime, ";
                sql += "@inventory_measure_id, @inventory_expired, @inventory_standard_cost, @inventory_list_price, @manufacture_day, ";
                sql += "@manufacture_make, @manufacture_tool, @manufacture_finished_goods, @manufacture_size, @manufacture_size_measure_id, ";
                sql += "@manufacture_weight, @manufacture_weight_measure_id, @manufacture_style, @manufacture_class, @manufacture_color)";
                db.Execute(sql, new
                {
                    category_id = Model.category_id,
                    code = Model.code,
                    name = Model.name,
                    specification = Model.specification,
                    description = Model.description,
                    dangerous = Model.dangerous,
                    discontinued_datetime = Model.discontinued_datetime,
                    inventory_measure_id = Model.inventory_measure_id,
                    inventory_expired = Model.inventory_expired,
                    inventory_standard_cost = Model.inventory_standard_cost,
                    inventory_list_price = Model.inventory_list_price,
                    manufacture_day = Model.manufacture_day,
                    manufacture_make = Model.manufacture_make,
                    manufacture_tool = Model.manufacture_tool,
                    manufacture_finished_goods = Model.manufacture_finished_goods,
                    manufacture_size = Model.manufacture_size,
                    manufacture_size_measure_id = Model.manufacture_size_measure_id,
                    manufacture_weight = Model.manufacture_weight,
                    manufacture_weight_measure_id = Model.manufacture_weight_measure_id,
                    manufacture_style = Model.manufacture_style,
                    manufacture_class = Model.manufacture_class,
                    manufacture_color = Model.manufacture_color
                });
            }
            catch (Exception ex)
            {
                return (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region Update
        public int Update(Item_model Model, out List<string> lstMsg)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();
            try
            {
                if (isError(Model, (int)Common.ActionType.Update, out lstMsg))
                {
                    returnCode = (int)Common.ReturnCode.UnSuccess;
                }
                string sql = "UPDATE `product_item` SET ";
                sql += "`category_id` = @category_id, `code` = @code, `name` = @name, `specification` = @specification, ";
                sql += "`description` = @description, `dangerous` = @dangerous, `discontinued_datetime` = @discontinued_datetime, ";
                sql += "`inventory_measure_id` = @inventory_measure_id, `inventory_expired` = @inventory_expired, `inventory_standard_cost` = inventory_standard_cost, ";
                sql += "`inventory_list_price` = inventory_list_price,`manufacture_make` = @manufacture_make, `manufacture_tool` = @manufacture_tool, ";
                sql += "`manufacture_finished_goods` = @manufacture_finished_goods, `manufacture_size` = @manufacture_size, `manufacture_size_measure_id` = @manufacture_size_measure_id, ";
                sql += "`manufacture_weight` = @manufacture_weight, `manufacture_weight_measure_id` = @manufacture_weight_measure_id, `manufacture_style` = @manufacture_style, ";
                sql += "`manufacture_class` = @manufacture_class, `manufacture_color` = @manufacture_color ";
                sql += "WHERE `id` = @id";
                db.Execute(sql, new
                {
                    category_id = Model.category_id,
                    id = Model.id,
                    code = Model.code,
                    name = Model.name,
                    specification = Model.specification,
                    description = Model.description,
                    dangerous = Model.dangerous,
                    discontinued_datetime = Model.discontinued_datetime,
                    inventory_measure_id = Model.inventory_measure_id,
                    inventory_expired = Model.inventory_expired,
                    inventory_standard_cost = Model.inventory_standard_cost,
                    inventory_list_price = Model.inventory_list_price,
                    manufacture_day = Model.manufacture_day,
                    manufacture_make = Model.manufacture_make,
                    manufacture_tool = Model.manufacture_tool,
                    manufacture_finished_goods = Model.manufacture_finished_goods,
                    manufacture_size = Model.manufacture_size,
                    manufacture_size_measure_id = Model.manufacture_size_measure_id,
                    manufacture_weight = Model.manufacture_weight,
                    manufacture_weight_measure_id = Model.manufacture_weight_measure_id,
                    manufacture_style = Model.manufacture_style,
                    manufacture_class = Model.manufacture_class,
                    manufacture_color = Model.manufacture_color

                });
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region Delete
        public int Delete(List<int> lstID, out List<string> lstMsg)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();
            try
            {
                db.Open();
                using (var _transaction = db.BeginTransaction())
                {
                    for (int i = 0; i < lstID.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(lstID[i].ToString()))
                        {
                            string sql = "DELETE FROM `product_item` WHERE `id` = @id";
                            db.Execute(sql, new { id = lstID[i] });
                        }
                        else
                        {
                            lstMsg.Add("Item has ID " + lstID[i].ToString() + " has been delete ");
                        }
                    }
                    _transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region Search
        public int Search(ListItem_model searchCondition, out List<DisplayItem_model> lstModel, out int total, int _page)
        {
            _page = _page * 15 - 15;
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstModel = new List<DisplayItem_model>();
            total = new int();
            try
            {
                string sql = "SELECT item.`id`, item.`code`, item.`name`, cate.`name` AS category_name, item.`specification`, item.`description` ";
                sql += "FROM `product_item` AS item ";
                sql += "JOIN (SELECT c.`id`, cpp.`name` AS category_parent_name, c.`name` ";
                sql += "FROM `product_category` AS c ";
                sql += "LEFT JOIN (SELECT cp.`name`, cp.`id` FROM `product_category` cp) cpp on cpp.`id` = c.`parent_id` ";
                sql += ") cate ON item.`category_id` = cate.`id` WHERE TRUE ";

                string _sql = "SELECT count(item.`id`) ";
                _sql += "FROM `product_item` AS item ";
                _sql += "JOIN (SELECT c.`id`, cpp.`name` AS category_parent_name, c.`name` ";
                _sql += "FROM `product_category` AS c ";
                _sql += "LEFT JOIN (SELECT cp.`name`, cp.`id` FROM `product_category` cp) cpp on cpp.`id` = c.`parent_id` ";
                _sql += ") cate ON item.`category_id` = cate.`id` WHERE TRUE ";

                if (!string.IsNullOrEmpty(searchCondition.category_id.ToString()))
                {
                    sql += "AND (item.`category_id` = @category_id OR item.`category_id` IN (SELECT cate.`id` FROM `product_category` cate WHERE cate.`parent_id` = @category_id)) ";
                    _sql += "AND (item.`category_id` = @category_id OR item.`category_id` IN (SELECT cate.`id` FROM `product_category` cate WHERE cate.`parent_id` = @category_id)) ";
                }
                if (!string.IsNullOrEmpty(searchCondition.code))
                {
                    sql += "AND item.`code` LIKE @code ";
                    _sql += "AND item.`code` LIKE @code ";
                }
                if (!string.IsNullOrEmpty(searchCondition.name))
                {
                    sql += "AND item.`name` LIKE @name ";
                    _sql += "AND item.`name` LIKE @name ";
                }
                sql += " LIMIT @page,15";
                lstModel = db.Query<DisplayItem_model>(sql, new { category_id = searchCondition.category_id, code = '%' + searchCondition.code + '%', name = '%' + searchCondition.name + '%', page = _page }).ToList();
                total = db.ExecuteScalar<int>(_sql, new { category_id = searchCondition.category_id, code = '%' + searchCondition.code + '%', name = '%' + searchCondition.name + '%', page = _page });
            }
            catch (Exception ex)
            {
                return returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region isError
        public bool isError(Item_model Model, int action, out List<string> listError)
        {
            bool flag = false;
            listError = new List<string>();
            if ((int)Common.ActionType.Add == action)
            {
                var result = db.Query<Item_model>("SELECT * FROM `product_item` WHERE `code` = @code", new { code = Model.code.Trim() }).ToList();
                if (result.Count != 0)
                {
                    flag = true;
                    listError.Add("[Code] is duplicate!");
                }
            }
            if ((int)Common.ActionType.Update == action)
            {
                var result = db.Query<Item_model>("SELECT * FROM `product_item` WHERE `code` = @code AND `id` <> @id", new { code = Model.code.Trim(), id = Model.id }).ToList();
                if (result.Count != 0)
                {
                    flag = true;
                    listError.Add("[Code] is duplicate!");
                }
            }
            return flag;
        }
        #endregion
    }
}
