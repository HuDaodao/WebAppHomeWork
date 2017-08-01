using System.Collections.Generic;
using System.Linq;
//using System.Web.Security;
using System.Xml.Schema;
using DAL;
using Model;
using System.Security.Cryptography;
using System.Text;
using System;

namespace BLL
{
    public class EmployeeBll
    {
        HomeWorkDB db = new HomeWorkDB();

        /// <summary>
        /// 总数据条数
        /// </summary>
        /// <returns></returns>
        public int CountEmployees(string searchMsg)
        {
            return db.Employee.Count(emp => emp.EmployeeName.Contains(searchMsg));
        }

        /// <summary>
        /// 分页获取员工列表
        /// </summary>
        /// <param name="searchMsg">员工姓名</param>
        /// <param name="pageStar">开始条数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="total">总共员工条数</param>
        /// <returns></returns>
        public List<Employee> GetPageEmployees(string searchMsg,int pageStar,int pageSize,out int total)
        {
            IQueryable<Employee> empList;
            if (string.IsNullOrEmpty(searchMsg))
            {
                empList = db.Employee;
            }
            else
            {
                empList = db.Employee.Where(emp => emp.EmployeeName.Contains(searchMsg));
            }
            total = db.Employee.Count();
            return empList.OrderByDescending(emp => emp.Id)
                                   .Skip(pageStar)
                                   .Take(pageSize)
                                   .ToList();
        }

        /// <summary>
        /// 通过ID获取一个员工
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public Employee GetEmployeeById(int empId)
        {
            return db.Employee.FirstOrDefault(emp => emp.Id == empId);
        }

        /// <summary>
        /// 检查姓名是否存在
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="id">员工id</param>
        /// <returns></returns>
        public bool CheckName(string name, int id)
        {
            var emp = db.Employee.FirstOrDefault(emo => emo.EmployeeName == name);
            if (emp != null)
            {
                return emp.Id == id;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 保存一个员工
        /// </summary>
        /// <param name="emp"></param>
        public void SaveEmployee(Employee emp)
        {
            string baseEncrypt = Base64(emp.PassWord);
            emp.PassWord = baseEncrypt;
            if (emp.Id == 0)
            {
                db.Employee.Add(emp);
            }
            else
            {
                Employee employee = new Employee();
                employee = db.Employee.Where(e => e.Id == emp.Id).FirstOrDefault();
                employee.EmployeeName = emp.EmployeeName;
                employee.Age = emp.Age;
                employee.PassWord = emp.PassWord;
                employee.Email = emp.Email;
                employee.DepartmentId = emp.DepartmentId;
            }
            
            db.SaveChanges();
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(int id)
        {
            Employee employee = GetEmployeeById(id);
            if (employee.Id != 0)
            {
                db.Employee.Remove(employee);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 修改是否为管理员
        /// </summary>
        /// <param name="id"></param>
        public void IsAdminChange(int id)
        {
            Employee emp= GetEmployeeById(id);
            emp.IsAdmin = !emp.IsAdmin;
            db.SaveChanges();
        }

        /// <summary>
        /// 登录验证（用户身份）
        /// </summary>
        /// <param name="empLog">登录的用户</param>
        /// <returns></returns>
        public UserStatus IsValidEmp(Employee empLog)
        {
            if (string.IsNullOrEmpty(empLog.EmployeeName) || string.IsNullOrEmpty(empLog.PassWord))
            {
                return UserStatus.NonAuthenticatedUser;
            }
            else
            {
                HomeWorkDB db = new HomeWorkDB();
                int empCount = db.Employee.Count();
                if (empCount == 0)
                {
                    db.Employee.Add(new Employee
                    {
                        EmployeeName="admin",
                        //PassWord="admin",
                        PassWord = Base64("admin"),
                        Age =23,
                        IsAdmin = true
                    });
                    db.SaveChanges();
                }
                string base64Pass = Base64(empLog.PassWord);
                Employee employee = db.Employee.Where(emp => emp.EmployeeName == empLog.EmployeeName && emp.PassWord == base64Pass).FirstOrDefault();
                if (employee != null)
                {
                    if (employee.IsAdmin)
                    {
                        return UserStatus.AuthenticatedAdmin;
                    }
                    else
                    {
                        return UserStatus.AuthenticatedUser;
                    }
                }
                else
                {
                    return UserStatus.NonAuthenticatedUser;
                }
            }
        }

        
        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="value">待解密字段</param>
        /// <returns></returns>
        public static string UnBase64( string value)
        {
            var btArray = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(btArray);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <returns></returns>
        public static string Base64( string value)
        {
            var btArray = Encoding.UTF8.GetBytes(value);
            var result= Convert.ToBase64String(btArray, 0, btArray.Length);
            return result;
        }
    }
}