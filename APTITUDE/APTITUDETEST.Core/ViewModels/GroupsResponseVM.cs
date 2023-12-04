namespace AptitudeTest.Core.ViewModels
{
    public class GroupsResponseVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NumberOfStudentsInGroup { get; set; }
        public bool IsDefault { get; set; }
        public List<GroupedCollegeVM> CollegesUnderGroup { get; set; }
    }

    public class GroupedCollegeVM
    {
        public string? Name { get; set; }
        public int NumberOfStudentsInCollege { get; set; }
    }
}
