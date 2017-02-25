using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlovMikhail.GitTools.Processing
{
  public  class Node
    {
        public Node(string hash)
        {
            Hash = hash;
            Parents = new List<Node>();
            Children = new List<Node>();
            Tags = new List<Tag>();
        }

      public string Hash { get; private set; }
      public List<Node> Parents { get; private set; }
      public List<Node> Children { get; private set; }
      public Branch Branch { get;  set; }
      public List<Tag> Tags { get; private set; }


        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }

    public class Tag : GitRef
    {
    }

    public class Branch : GitRef
    {
    }

    public abstract class GitRef
    {
        
    }
}
