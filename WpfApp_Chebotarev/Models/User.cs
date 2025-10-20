using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp_Chebotarev
{
    public partial class User
    {
        [Key]
        public int id { get; set; }
        public string password { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string role { get; set; }
        public string username { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsFirstLogin { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? FailedLoginAttempts { get; set; }

    }
}

