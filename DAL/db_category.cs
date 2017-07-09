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
    public class db_category
    {
        #region connection
        public IDbConnection db;
        public db_category()
        {
            db = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        #endregion
        #region GetAll
        public List<DisplayCategory_model> GetAll()
        {

            string sql = "SELECT cate.`id`, cate_parent.`name` as parent_name, cate.`code`, cate.`name`, cate.`description`";
                   sql += " FROM product_category cate LEFT JOIN(SELECT `id`, `name` FROM `product_category`) cate_parent";
                   sql += " ON cate.`parent_id` = cate_parent.id";
                   return db.Query<DisplayCategory_model>(sql).ToList();
        }
        #endregion
        #region GetByID
        public int GetByID(long ID, out Category_model Model)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            Model = new Category_model();
            try
            {
                if (ID != 0)
                {
                    string sql = "SELECT *";
                    sql += " FROM product_category";
                    sql += " WHERE `id` = @id";
                    Model = db.Query<Category_model>(sql, new { id = ID }).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }

            return returnCode;
        }
        #endregion
        #region GetDetail
        public int GetDetail(long ID, out DisplayCategory_model Model)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            Model = new DisplayCategory_model();
            try
            {
                if (ID != 0)
                {
                    string sql = "SELECT cate.`id`, cate_parent.`name` parent_name, cate.`code`, cate.`name`, cate.`description`";
                    sql += " FROM product_category cate LEFT JOIN(SELECT `id`, `name` FROM `product_category`) cate_parent";
                    sql += " ON cate.`parent_id` = cate_parent.id WHERE cate.`id` = @id";
                    Model = db.Query<DisplayCategory_model>(sql, new { id = ID }).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }

            return returnCode;
        }
        #endregion
        #region GetCategoryByID
        public List<CategoryParent_model> GetCategoryByID(long id)
        {
            List<CategoryParent_model> model = new List<CategoryParent_model>();
            try
            {
                string sql = "SELECT DISTINCT id,name";
                sql += "  FROM product_category";
                sql += "  WHERE parent_id = @id";
                model = db.Query<CategoryParent_model>(sql, new { id = id }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
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
        #endregion
        #region GetListCategory
        public int GetListCategory(long ID, out List<ListItem_model> ListCategory)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            ListCategory = new List<ListItem_model>();
            try
            {
                if (ID != 0)
                {
                    string sql = "SELECT `id`,`category_id`,`code`";
                    sql += " FROM product_item";
                    sql += " WHERE `category_id` = @id";
                    ListCategory = db.Query<ListItem_model>(sql, new { id = ID }).ToList();
                }
            }
            catch (Exception)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
            }

            return returnCode;
        }
        #endregion
        #region GetParent
        public List<CategoryParent_model> GetParent()
        {
            List<CategoryParent_model> model = new List<CategoryParent_model>();
            try
            {
                string sql = "SELECT DISTINCT cate_parent.id AS id, cate_parent.name AS name";
                sql += "  FROM product_category cate INNER JOIN (SELECT id, name FROM product_category) cate_parent";
                sql += "  ON cate.parent_id = cate_parent.id";
                model = db.Query<CategoryParent_model>(sql).ToList();


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region GetChildrens_Category
        public int GetChildrens_Category(long ID, out List<Category_model> Childrens_Category)
        {
            int returnCode = (int)Common.ReturnCode.Succeed;
            Childrens_Category = new List<Category_model>();
            try
            {
                if (ID != 0)
                {
                    string sql = "SELECT * FROM `product_category` WHERE `parent_id` = @id";
                    var model = db.Query<Category_model>(sql, new { id = ID }).ToList();
                    Childrens_Category = model;
                    return returnCode;
                }
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;

            }
            return returnCode;
        }
        #endregion
        #region GetListParent
        public List<CategoryParent_model> GetListParent()
        {
            string sql = "SELECT `id`, `parent_id`, `name` FROM `product_category` ORDER BY `id` DESC";
            return db.Query<CategoryParent_model>(sql).ToList();
        }
        #endregion
        #region Insert
        public int Insert(Category_model model, out List<string> lstMsg)
        {
            int result = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();
            try
            {
                if (isError(model, (int)Common.ActionType.Add, out lstMsg))
                {
                    return (int)Common.ReturnCode.UnSuccess;
                }
                var sql = "INSERT INTO `product_category` (`parent_id`, `code`, `name`, `description`) VALUES(@parent_id, @code, @name, @description); ";
                db.Execute(sql, model);
            }
            catch (Exception ex)
            {
                lstMsg.Add("Exception Occurred.");
                result = (int)Common.ReturnCode.UnSuccess;
            }
            return result;
        }
        #endregion
        #region Update
        public int Update(Category_model model, out List<string> lstMsg)
        {
            int result = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();

            try
            {
                if (isError(model, (int)Common.ActionType.Update, out lstMsg))
                {
                    return (int)Common.ReturnCode.UnSuccess;
                }
                string sql = "UPDATE `product_category` SET `parent_id` = @parent_id, `code` = @code, `name` = @name, `description` = @description, `updated_datetime` = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFFFFF") + "' WHERE `id` = @id ";
                db.Execute(sql, model);
            }
            catch (Exception ex)
            {
                lstMsg.Add("Exception Occurred.");
                result = (int)Common.ReturnCode.UnSuccess;
            }

            return result;
        }
        #endregion
        #region Delete
        public int Delete(List<int> lstID, out List<string> lstMsg)
        {
            List<ListItem_model> ListCategory = new List<ListItem_model>();
            List<Category_model> Childrens_Category = new List<Category_model>();

            int returnCode = (int)Common.ReturnCode.Succeed;
            lstMsg = new List<string>();
            try
            {
                db.Open();
                using (var _transaction = db.BeginTransaction())
                {

                    for (int i = 0; i < lstID.Count; i++)
                    {

                        if (string.IsNullOrEmpty(lstID[i].ToString()))
                        {
                            lstMsg.Add("Data has already been deleted by other user!");
                            return (int)Common.ReturnCode.UnSuccess;
                        }
                        if (!string.IsNullOrEmpty(lstID[i].ToString()))
                        {
                            int product_category = GetChildrens_Category(lstID[i], out Childrens_Category);


                            if (product_category == (int)Common.ReturnCode.Succeed && Childrens_Category.Count() > (int)Common.ReturnCode.Succeed)
                            {

                                foreach (var Child in Childrens_Category)
                                {
                                    string sql = "UPDATE `product_category` SET `parent_id`=0  WHERE `id` = @id";
                                    db.Execute(sql, new { id = Child.id });

                                    int result = GetListCategory(Child.id, out ListCategory);
                                    if (result == (int)Common.ReturnCode.Succeed)
                                    {
                                        foreach (var product_item in ListCategory)
                                        {
                                            string strQuer = "UPDATE `product_item` SET `category_id`=0  WHERE `code` = @code AND `id` = @id";
                                            db.Execute(strQuer, new { code = product_item.code, id = product_item.id });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int result = GetListCategory(lstID[i], out ListCategory);
                                if (result == (int)Common.ReturnCode.Succeed)
                                {
                                    foreach (var product_item in ListCategory)
                                    {
                                        string strQuer = "UPDATE `product_item` SET `category_id`=0  WHERE `code` = @code AND `id` = @id";
                                        db.Execute(strQuer, new { code = product_item.code, id = product_item.id });
                                    }
                                }

                            }
                            db.Execute("DELETE FROM `product_category` WHERE `id`=@id", new { id = lstID[i] });
                        }
                    }
                    _transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                returnCode = (int)Common.ReturnCode.UnSuccess;
                lstMsg = new List<string>();
            }

            return returnCode;
        }
        #endregion
        #region Search
        public int Search(SearchCategory_model searchCondition, out List<DisplayCategory_model> lstModel, out int total, int _page)
        {
            _page = _page * 15 - 15;
            int returnCode = (int)Common.ReturnCode.Succeed;
            lstModel = new List<DisplayCategory_model>();
            total = new int();
            try
            {
                string sql = "SELECT cate.`id`, cate_parent.`name` parent_name, cate.`code`, cate.`name`, cate.`description` ";
                sql += "FROM `product_category` cate  ";
                sql += "LEFT JOIN (SELECT `id`, `name` FROM `product_category`) cate_parent ";
                sql += "ON cate.`parent_id` = cate_parent.`id` WHERE TRUE ";

                string _sql = "SELECT COUNT(cate.`id`) ";
                _sql += "FROM `product_category` cate  ";
                _sql += "LEFT JOIN (SELECT `id`, `name` FROM `product_category`) cate_parent ";
                _sql += "ON cate.`parent_id` = cate_parent.`id` WHERE TRUE ";

                if (!string.IsNullOrEmpty(searchCondition.parent_id.ToString()))
                {
                    sql += "AND (cate.`id` = @parent_id OR cate.`id` IN (SELECT `id` FROM tuankhai_freshernet.product_category WHERE `parent_id` = @parent_id)) ";
                    _sql += "AND (cate.`id` = @parent_id OR cate.`id` IN (SELECT `id` FROM tuankhai_freshernet.product_category WHERE `parent_id` = @parent_id)) ";
                }
                if (!string.IsNullOrEmpty(searchCondition.code))
                {
                    sql += "AND cate.`code` LIKE @code ";
                    _sql += "AND cate.`code` LIKE @code ";
                }
                if (!string.IsNullOrEmpty(searchCondition.name))
                {
                    sql += "AND cate.`name` LIKE @name ";
                    _sql += "AND cate.`name` LIKE @name ";
                }
                sql += " ORDER BY cate.`id` ASC LIMIT @page,15";
                lstModel = db.Query<DisplayCategory_model>(sql, new { parent_id = searchCondition.parent_id, code = '%' + searchCondition.code + '%', name = '%' + searchCondition.name + '%', page = _page }).ToList();
                total = db.ExecuteScalar<int>(_sql, new { parent_id = searchCondition.parent_id, code = '%' + searchCondition.code + '%', name = '%' + searchCondition.name + '%', page = _page });
            }
            catch (Exception ex)
            {
                return returnCode = (int)Common.ReturnCode.UnSuccess;
            }
            return returnCode;
        }
        #endregion
        #region isError
        public bool isError(Category_model Model, int action, out List<string> lstMessgae)
        {
            bool flag = false;
            lstMessgae = new List<string>();
            if ((int)Common.ActionType.Add == action)
            {
                string sql = "SELECT * FROM `product_category` WHERE `code` = @code";
                var result = db.Query<Category_model>(sql, new { code = Model.code.Trim() }).ToList();
                if (result.Count != 0)
                {
                    flag = true;
                    lstMessgae.Add("[Code] is duplicate!");
                }
            }
            if ((int)Common.ActionType.Update == action)
            {
                string sql = "SELECT * FROM `product_category` WHERE `code` = @code AND `id` <> @id";
                var result = db.Query<Category_model>(sql, new { code = Model.code.Trim(), id = Model.id }).ToList();
                if (result.Count != 0)
                {
                    flag = true;
                    lstMessgae.Add("[Code] is duplicate!");
                }
            }
            if ((int)Common.ActionType.Delete == action)
            {
                string sql = "SELECT `id` FROM `product_category` WHERE  `id` = @id";
                var hasItem = db.Query<CategoryParent_model>(sql, new { code = Model.code.Trim() }).ToList();
                if (hasItem.Count == 0)
                {
                    flag = true;
                    lstMessgae.Add("[Category] is not found!");
                }
            }
            return flag;
        }
        #endregion

    }
}
