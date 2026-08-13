namespace Goke.Core.Authorization
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class AuthorizeAttribute : Attribute
    {
        public string? Roles { get; set; }

        public string? Policies { get; set; }

        // Format:
        // "Permission=Weather.Read"
        // "Tenant=BankA"
        // "EmployeeId"   // presence only
        public string? Claims { get; set; }

        public AuthorizeAttribute() { }

        public AuthorizeAttribute(string roles)
        {
            Roles = roles;
        }
    }

}
