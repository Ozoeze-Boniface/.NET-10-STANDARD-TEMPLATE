namespace KeyRails.BankingApi.Domain.Constants
{
    public class PermissionConstants
    {
        // Mandate Creation & Management
        public const string CreateMandate = "INITIATE_MANDATE_CREATE";
        public const string ActivateMandate = "ACTIVATE_MANDATE";
        public const string DeactivateMandate = "DEACTIVATE_MANDATE";
        public const string ApproveMandate = "APPROVE_MANDATE_LIFECYCLE";
        public const string ViewReport = "VIEW_REPORTS";
        public const string DownloadReport = "DOWNLOAD_REPORTS";
        public const string LiquidateMandate = "CAN_LIQUIDATE_MANDATE";
        public const string ManageStatus = "CAN_MANAGE_MANDATE_STATUS";

        // User
        public const string ViewUserReport = "VIEW_USER_REPORTS";
        public const string ViewAuditLogs = "VIEW_AUDIT_LOGS";
        public const string CreateUser = "CREATE_USER";
        public const string EditUser = "EDIT_USER";
        public const string AssignPermissions = "ASSIGN_PERMISSIONS";
        public const string ActivateDeactivateUser = "SET_USER_STATUS";

    }
}