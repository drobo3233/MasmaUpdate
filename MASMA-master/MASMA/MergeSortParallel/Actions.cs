using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA.MergeSortParallel
{
   
        public class Actions
        {
            public const string MergeAndCompare = "merge-and-compare";
            public const string Terminate = "terminate";
            public const string SortAndAssignData = "sort-asignData";
        public const string DoneSortAndAssignData = "done-sort-asignData";
        public const string Print = "print";
        public const string Merge = "merge";


    }

        public class Agents
        {
            public static string MasterAgent = "master-agent";
        }
    
}
