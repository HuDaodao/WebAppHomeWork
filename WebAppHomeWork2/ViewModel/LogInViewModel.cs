using System.ComponentModel;

namespace WebAppHomeWork.ViewModel
{
    public class LogInViewModel
    {
        [DisplayName("用户名")]
        public string UserName { get; set; }
        [DisplayName("密码")]
        public string PassWord { get; set; }
       public string ErrorMsg { get; set; }
    }
}