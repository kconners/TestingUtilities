using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingUtilities.Models
{
    public class QuickResult
    {
        public Guid idNumber = new Guid();
        public Guid clientIdNumber = new Guid();
        public int status;
        public string testscript { get; set; }
        public string testcase { get; set; }
        public string results { get; set; }
    }
}
