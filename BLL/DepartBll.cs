using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class DepartBll
    {
        HomeWorkDB db= new HomeWorkDB();

        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAllDeparts()
        {
            return db.Department.ToList();
        }

        /// <summary>
        /// 通过ID获取部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public Department GetDepartmentById(int? id)
        {
            return db.Department.FirstOrDefault(depart => depart.Id == id);
        }

        /// <summary>
        ///获取部门子部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public List<Department> GetDepart(int id)
        {
            return db.Department.Where(depart => depart.ParentId == id).ToList();
        }


        public void SaveDept(Department dept)
        {
            if (dept.Id == 0)
            {
                db.Department.Add(dept);
            }
            else
            {
                Department department = new Department();
                department = db.Department.Where(e => e.Id == dept.Id).FirstOrDefault();
                department.Name = dept.Name;
                department.Note = dept.Note;
                department.ParentId = dept.ParentId;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteDept(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                var sonCount=db.Department.Where(dept => dept.ParentId == id).Count();
                if (sonCount != 0)
                {
                    return false;
                }else
                {
                    var dept=GetDepartmentById(id);
                    db.Department.Remove(dept);
                    db.SaveChanges();
                    return true;
                }
            }
        }

        /// <summary>
        /// 可调整至的部门(排除自己及自己的后辈部门)
        /// </summary>
        /// <param name="id">当前部门ID</param>
        /// <returns></returns>
        public List<Department> GetParentDept(int id)
        {
            List<int> arrs = new List<int>();
            List<int> arr = CanBeArr(arrs, id);
            List<Department> canBeParentList;
            canBeParentList = (from p in db.Department
                               where !arr.Contains(p.Id)
                               select p).ToList();
            return canBeParentList;
        }

        /// <summary>
        /// 将自己及后代部门的ID组成一个List
        /// </summary>
        /// <param name="arrs">id列表</param>
        /// <param name="id">当前ID</param>
        /// <returns></returns>
        public List<int> CanBeArr(List<int> arrs, int id)
        {
            arrs.Add(id);
            var infordept = GetDepartmentById(id);
            var inforSon=db.Department.Where(dept => dept.ParentId == infordept.Id);
            if (inforSon.Count()> 0)
            {
                foreach (var sonDept in inforSon.ToList())
                {
                    CanBeArr(arrs, sonDept.Id);
                }
            }
            return arrs;
        }


    }
}
