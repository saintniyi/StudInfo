using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudInfoModel
{
    public class Role
    {
        public string? Id { get; set; }


        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
