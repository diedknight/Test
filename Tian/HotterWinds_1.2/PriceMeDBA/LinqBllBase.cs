
namespace PriceMe.Bll
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Linq;
    using System.Text;
    using System.Reflection;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Configuration;
    public   class LinqBllBase<T> where T:class,new()
    {
        private string connection =ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
        
        //public LinqBllBase(string connetion)
        //{
        //    connection = connetion;
        //}

        public  void Add(T entity)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                tb.InsertOnSubmit(entity);
                ct.SubmitChanges();
            }
        }

        public void Update(T entity)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                tb.Attach(entity, true);
                ct.SubmitChanges();
            }
        }

        public void Update(List<T> entity)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                tb.AttachAll(entity, true);
                ct.SubmitChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var entity = tb.Where("id=@0", id).Single();
                tb.DeleteOnSubmit(entity);
                ct.SubmitChanges();
            }
        }

        public void Delete(Expression<Func<T, bool>> expr)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                tb.DeleteAllOnSubmit(tb.Where(expr));
                ct.SubmitChanges();
            }
        }

        public void Delete(int[] ids)
        {
            using (var ct = new DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var list = new List<T>();
                foreach (var item in ids.Distinct())
                {
                    list.Add(tb.Where("id=@0", item).SingleOrDefault());
                }
                tb.DeleteAllOnSubmit(list);
                ct.SubmitChanges();
            }
        }

        public T GetSingle(int id)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                return tb.Where("id=@0",id).SingleOrDefault();
            }
        }

        public T Single(Expression<Func<T,bool>> expr)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
               return  ct.GetTable<T>().SingleOrDefault(expr);
            }
        }


        public List<T> Query(string condition, string orderby, int startIndex, int pageSize, ref int pageCount, object[] param)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                IQueryable OrderList = tb;
                if (!string.IsNullOrEmpty(condition))
                    OrderList = OrderList.Where(condition, param);
                if (!string.IsNullOrEmpty(orderby))
                    OrderList = OrderList.OrderBy(orderby);
                pageCount = OrderList.Count();
                return OrderList.Skip(startIndex).Take(pageSize).Cast<T>().ToList();
            }
        }

        public List<T> Query(string condition, string orderby, object[] param)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                IQueryable OrderList = tb;
                if (!string.IsNullOrEmpty(condition))
                    OrderList = OrderList.Where(condition, param);
                if (!string.IsNullOrEmpty(orderby))
                    OrderList = OrderList.OrderBy(orderby);
                return OrderList.Cast<T>().ToList();
            }
        }

        public List<T> Query(Expression<Func<T,bool>> expr){
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                return tb.Where(expr).ToList();
            }
        }

        public List<T> Query<OrderyType>(Expression<Func<T, bool>> where, Expression<Func<T, OrderyType>> orderby, bool desc)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var list = tb.Where(where);
                if (desc)
                    list = list.OrderByDescending(orderby);
                else
                    list = list.OrderBy(orderby);
                return list.ToList();
            }
        }

        public List<T> Query<OrderyType>(Expression<Func<T, bool>> where, Expression<Func<T, OrderyType>> orderby, bool desc, int start_index, int page_size, ref int record_count)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var list = tb.Where(where);
                if (desc)
                    list = list.OrderByDescending(orderby);
                else
                    list = list.OrderBy(orderby);
                record_count = list.Count();
                return list.Skip(start_index).Take(page_size).ToList();
            }
        }

        
        

        public List<T> Query(int[] ids)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var list = new List<T>();
                foreach (var item in ids.Distinct())
                {
                    var md = tb.Where("id=@0", item).SingleOrDefault();
                    if (md != null)
                        list.Add(md);
                }
                return list;
            }
        }
        public List<T> Query(List<int> ids)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                var list = new List<T>();
                foreach (var item in ids.Distinct())
                {
                    var md = tb.Where("TypeID=@0", item).SingleOrDefault();
                    if (md != null)
                        list.Add(md);
                }
                return list;
            }
        }

        public List<U> Query<U>(string sql, object[] parameterValues)
        {
            using (var ct = new System.Data.Linq.DataContext(connection))
            {
                var tb = ct.GetTable<T>();
                return ct.ExecuteQuery<U>(sql, parameterValues).ToList();
            }
        }

        public T CreateModel()
        {
            return new T();
        }

     
    }

   
}
