namespace Ice_Cream_Parlour_Eproject.Helpers
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Staff = "Staff";
        public const string User = "User";

        public const string AdminOrManager = Admin + "," + Manager;
        public const string AllStaff = Admin + "," + Manager + "," + Staff;
    }
}