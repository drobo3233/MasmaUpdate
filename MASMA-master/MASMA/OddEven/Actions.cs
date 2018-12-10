namespace MASMA.OddEven
{
    public class Actions
    {
        public const string Start = "start";
        public const string Done = "done";
        public const string Restart = "restart";
        public const string IsSorted = "isSorted";

        public const string AssignAndSort = "assign-and-sort";
        public const string EvenPhase = "start-even-phase";
        public const string OddPhase = "start-odd-phase";
        public const string MergeAndSplit = "merge-split";

        public const string RefreshData = "refresh-data";
        public const string PrintData = "print";
        public const string DoneAssignAndSort = "done-sort-asignData";
        public const string DonePrint = "done-print";
    }

    public class Agents
    {
        public static string MasterAgent = "master-agent";
    }
}
