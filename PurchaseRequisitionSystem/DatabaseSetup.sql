-- Purchase Requisition System Database Setup Script
-- Run this script if you need to manually create the database

-- Create the database
CREATE DATABASE PurchaseRequisitionSystemDb;
GO

USE PurchaseRequisitionSystemDb;
GO

-- The Entity Framework will create all tables automatically when the application runs
-- This script is provided for reference only

-- Sample data insertion (optional - the application seeds this automatically)
/*
INSERT INTO FacilityMasters (Code, Name, Description, IsActive, CreatedDate, CreatedBy)
VALUES 
(0, 'Dubai', 'Dubai Facility', 1, GETUTCDATE(), 'System'),
(1, 'Sharjah', 'Sharjah Facility', 1, GETUTCDATE(), 'System'),
(2, 'Ajman', 'Ajman Facility', 1, GETUTCDATE(), 'System'),
(3, 'Akoya Clinic', 'Akoya Clinic Facility', 1, GETUTCDATE(), 'System');
*/

PRINT 'Database setup complete. Run the application to create tables automatically.'