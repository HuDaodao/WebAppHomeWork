using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebAppHomeWork.ViewModel
{
    public class JsonTreeModel : BaseViewModel

    {
        public int id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public bool children { get; set; }
    }
}