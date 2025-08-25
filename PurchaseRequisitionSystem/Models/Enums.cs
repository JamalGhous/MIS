namespace PurchaseRequisitionSystem.Models
{
    public enum RequestType
    {
        Clinical,
        NonClinical
    }

    public enum ExpenseType
    {
        CAPEX,
        OPEX
    }

    public enum OpexCategory
    {
        AMC,
        Service,
        SpareParts,
        Renewals,
        Other
    }

    public enum UnitOfMeasure
    {
        Each,
        Kilogram,
        Liter,
        Meter,
        Box,
        Pack,
        Set
    }

    public enum ApprovalStatus
    {
        Initiated,
        PendingHODInitiator,
        PendingHODDepartment,
        PendingFinance,
        PendingWarehouse,
        PendingSCM,
        PendingDirector,
        PendingCFO,
        PendingCEO,
        Approved,
        Rejected,
        Delegated,
        Clarification
    }

    public enum UserRole
    {
        Initiator,
        HOD,
        Finance,
        Warehouse,
        SCM,
        Director,
        CFO,
        CEO,
        Admin
    }

    public enum FacilityCode
    {
        DXB, // Dubai
        SHJ, // Sharjah
        AJM, // Ajman
        AKA  // Akoya Clinic
    }
}