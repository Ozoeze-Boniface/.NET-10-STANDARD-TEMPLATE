using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Constants
{
    public class PermissionConstants
    {
        // Mandate Creation & Management
        public const string CreateMandate = "CreateMandate";
        public const string EditMandate = "EditMandate";
        public const string ViewMandate = "ViewMandate";
        public const string DeleteMandate = "DeleteMandate";

        // Mandate Approval Workflow
        public const string SubmitMandateForApproval = "SubmitMandateForApproval";
        public const string ApproveMandate = "ApproveMandate";
        public const string RejectMandate = "RejectMandate";
        public const string ReassignMandate = "ReassignMandate";

        // Mandate Execution
        public const string ExecuteMandate = "ExecuteMandate";
        public const string CancelMandate = "CancelMandate";

        // Mandate Audit & Logs
        public const string ViewMandateAuditTrail = "ViewMandateAuditTrail";
        public const string ViewMandateExecutionLog = "ViewMandateExecutionLog";

        // Mandate Role Management
        public const string AssignMandateSignatory = "AssignMandateSignatory";
        public const string ConfigureMandateRules = "ConfigureMandateRules";

        // Admin / System Level
        public const string ManageMandatePermissions = "ManageMandatePermissions";
        public const string OverrideMandateLimits = "OverrideMandateLimits";
        public const string CreateUser = "CreateUser";
    }
}