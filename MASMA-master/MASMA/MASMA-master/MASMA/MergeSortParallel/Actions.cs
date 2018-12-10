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
        public const string Terminate2 = "terminate2";
        public const string SortAndAssignData = "sort-asignData";
        public const string DoneSortAndAssignData = "done-sort-asignData";
        public const string Print = "print";
        public const string Merge = "merge";
        public const string Split = "split";
        public const string OwnSplit = "split-own";
        public const string DonePhase1 = "done-phase1";
        public const string Assign = "asign";
        public const string AssignData = "asignData";
        public const string DoneAsignData="done-asignData";


    }

    public class Agents
    {
        public static string MasterAgent = "master-agent";
    }

}
