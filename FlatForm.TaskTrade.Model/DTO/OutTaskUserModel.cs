using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class OutTaskUserModel
    {
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string UserMobile { get; set; }
        public bool IsActived { get; set; }
        public string UserPwd { get; set; }
        public string Token { get; set; }
        public string TokenKey { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int LoginTimes { get; set; }
        public int UserType { get; set; }
        public int CompanyID { get; set; }
        public string ImagePath { get; set; }
        public string UserVersion { get; set; }
        public int CRMID { get; set; }
        public int Crmdepid { get; set; }
        public int ID { get; set; }
        public string TGuid { get; set; }
        public DateTime CreatedTime { get; set; }
        public int IOrder { get; set; }
    }
}
