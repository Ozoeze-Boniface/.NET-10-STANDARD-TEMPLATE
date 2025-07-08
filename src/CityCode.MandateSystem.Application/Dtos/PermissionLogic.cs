namespace CityCode.MandateSystem.Application.Dtos
{
    public enum PermissionLogic
    {
        And,  // User must have ALL permissions
        Or // User must have AT LEAST ONE permission
    }
}