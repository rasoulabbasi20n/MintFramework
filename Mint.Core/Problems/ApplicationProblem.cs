using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mint.Core.Problems
{
    public class ApplicationProblem : ProblemBase
    {
        public int? Status { get; set; }
        
        public ApplicationProblem(string message) : base(message)
        {
        }
    }
}
