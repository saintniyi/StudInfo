using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoModel.ViewModels
{
    public class ReportVM
    {
        
        public string Name { get; set; } = string.Empty;
        public Score Score { get; set; } = null!;
        public Gpa Gpa  { get; set; } = null!;
        public Cgpa Cgpa { get; set; } = null!;

        public List<Score> Scores { get; set; }
        public List<string> DistinctGroupCode { get; set; }
        public List<Gpa> Gpas { get; set; }
        public List<Cgpa> Cgpas { get; set; }

    }
}
