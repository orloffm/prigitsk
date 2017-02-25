using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlovMikhail.GitTools.Structure
{
  public  interface IBranchingStrategy
    {
      /// <summary>Orders branch names by priority according to the branching strategy.</summary>
      IEnumerable<string> OrderBranchNames(IEnumerable<string> branches);
    }
}
