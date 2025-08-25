# Purchase Requisition System

A comprehensive .NET Core 8.0 MVC web application for digitizing the Purchase Requisition (PR) process with approval hierarchy, role-based access control, delegation, and dashboards.

## Features

### Authentication & Access Control
- Windows Authentication (Active Directory) integration
- Role-Based Access Control (RBAC)
- Support for multiple user roles: Initiator, HOD, Finance, SCM, Warehouse, Directors, CFO, CEO, Admin

### Core Functionality
- **Purchase Requisition Creation**: Complete workflow for creating PRs with all required fields
- **Approval Hierarchy**: Configurable approval routing with default AD-based and Hierarchy Master routing
- **Delegation Support**: Users can delegate approval responsibilities to other users
- **Multi-level Approval**: 8-level approval process from HOD to CEO
- **CAPEX/OPEX Support**: Different workflows for capital and operational expenses

### Key Components

#### Models
- `PurchaseRequisition`: Core PR entity with all required fields
- `ApplicationUser`: Extended Identity user with role and department information
- `FacilityMaster`: Manages facilities (Dubai, Sharjah, Ajman, Akoya Clinic)
- `HierarchyMaster`: Configurable approval hierarchies
- `ApprovalHistory`: Tracks all approval actions
- `DelegationHistory`: Manages delegation records
- `PRAttachment`: File attachment support

#### Services
- `IPRService & PRService`: Core PR business logic
- `IApprovalService & ApprovalService`: Approval workflow management
- Auto-generation of PR numbers
- File upload and management

#### Controllers
- `HomeController`: Dashboard and main navigation
- `PurchaseRequisitionController`: PR CRUD operations and approval
- `AdminController`: System administration features

### Workflow Process

1. **Initiation**: User creates PR selecting facility, request type, expense type
2. **Form Completion**: 
   - OPEX: Category (AMC, Service, Spare Parts, Renewals, Other)
   - CAPEX: Skip OPEX categories
   - Cost estimation, item details, specifications, attachments
3. **Approval Routing**:
   - Check for Hierarchy Master configuration
   - Fall back to default AD-based routing if no hierarchy exists
   - Route through: HOD → Finance → Warehouse → SCM → Director → CFO → CEO
4. **Actions**: Approve, Reject, Delegate, Request Clarification
5. **Completion**: Final approval or rejection with full audit trail

### Dashboard Features
- **My Requests**: Track all initiated PRs with status
- **Pending Approvals**: Items awaiting user's approval
- **Statistics**: Visual summary of PR status (In Process, Approved, Rejected)
- **Timeline View**: Complete approval history visualization

### Administrative Features
- Facility Master management
- Hierarchy Master configuration
- User management with role assignment
- System reports and analytics

### Technology Stack
- **.NET Core 8.0**: Web framework
- **Entity Framework Core**: Data access with SQL Server
- **ASP.NET Core Identity**: Authentication and authorization
- **Bootstrap 5**: Responsive UI framework
- **Font Awesome**: Icons
- **jQuery**: Client-side interactions

### Database Schema
The system uses Entity Framework Code First approach with the following key entities:
- ApplicationUsers (extends IdentityUser)
- PurchaseRequisitions
- FacilityMasters
- HierarchyMasters
- ApprovalHistories
- DelegationHistories
- PRAttachments

### Security Features
- Windows Authentication integration
- Role-based access control
- Secure file upload handling
- Anti-forgery token protection
- Authorization filters on controllers

### File Structure
```
PurchaseRequisitionSystem/
├── Controllers/
│   ├── HomeController.cs
│   ├── PurchaseRequisitionController.cs
│   └── AdminController.cs
├── Models/
│   ├── Enums.cs
│   ├── ApplicationUser.cs
│   ├── PurchaseRequisition.cs
│   └── [Other models...]
├── ViewModels/
│   ├── CreatePRViewModel.cs
│   └── DashboardViewModel.cs
├── Services/
│   ├── IPRService.cs, PRService.cs
│   └── IApprovalService.cs, ApprovalService.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Views/
│   ├── Home/
│   ├── PurchaseRequisition/
│   ├── Admin/
│   └── Shared/
└── wwwroot/
    └── uploads/
```

### Setup Instructions

1. **Prerequisites**:
   - .NET 8.0 SDK
   - SQL Server (LocalDB for development)
   - Visual Studio 2022 or VS Code

2. **Installation**:
   ```bash
   git clone [repository-url]
   cd PurchaseRequisitionSystem
   dotnet restore
   dotnet build
   ```

3. **Database Setup**:
   - Update connection string in `appsettings.json`
   - The application will create the database and seed initial data on first run
   - Default admin user: admin@mis.local / Admin@123

4. **Run Application**:
   ```bash
   dotnet run
   ```

### Configuration

#### Connection String
Update `appsettings.json` with your SQL Server connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PurchaseRequisitionSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Default Facilities
The system comes pre-configured with:
- DXB - Dubai
- SHJ - Sharjah
- AJM - Ajman
- AKA - Akoya Clinic

### Extensibility

The system is designed to be easily extensible:
- Add new approval levels by extending the ApprovalStatus enum
- Configure custom approval hierarchies via Hierarchy Master
- Add new facilities through admin interface
- Customize user roles and permissions

### Support

For technical support and customization requests, refer to the system administrator or development team.

---

**Note**: This system implements enterprise-grade features for purchase requisition management with full audit trails, approval hierarchies, and role-based security suitable for healthcare and corporate environments.