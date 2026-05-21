using System.Reflection;
using System.Text.Json;
using Bogus;
using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.System;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Entities.ActivityLog;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using CbsAp.Application.DTOs.ActivityLog;

namespace CbsAp.Infrastracture.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserLogInfo> UserLogInfos { get; set; }

        public DbSet<PasswordResetAudit> PasswordResetAudits { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionGroup> PermissionGroups { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<RolePermissionGroup> RolePermissionGroups { get; set; }

        public DbSet<ControlElement> ControlElements { get; set; }

        public DbSet<Notice> Notices { get; set; }

        public DbSet<Dimension> Dimensions { get; set; }

        public DbSet<GoodReceipt> GoodsReceipts { get; set; }

        public DbSet<TaxCode> TaxCodes { get; set; }

        public DbSet<RoleEntity> RoleEntities { get; set; }

        public DbSet<EntityProfile> EntityProfiles { get; set; }

        public DbSet<EntityMatchingConfig> EntityMatchingConfigs { get; set; }

        public DbSet<SupplierInfo> SupplierInfos { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<InvRoutingFlow> InvRoutingFlows { get; set; }

        public DbSet<InvRoutingFlowLevels> Levels { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvAllocLine> InvoicesAllocLines { get; set; }

        public DbSet<InvAllocLineFreeField> InvAllocLineFreeFields { get; set; }

        public DbSet<InvAllocLineDimension> invAllocLineDimensions { get; set; }

        public DbSet<InvoiceComment> InvoiceComments { get; set; }

        public DbSet<InvoiceAttachnment> InvoiceAttachnments { get; set; }

        public DbSet<InvoiceActivityLog> InvoiceActivityLogs { get; set; }

        public DbSet<InvoiceArchive> InvoiceArchives { get; set; }

        public DbSet<InvAllocLineArchive> InvoiceAllocationLinesArchive { get; set; }

        public DbSet<InvAllocLineFreeFieldArchive> InvAllocLineFreeFieldsArchive { get; set; }

        public DbSet<InvAllocLineDimensionArchive> InvAllocLineDimensionsArchive { get; set; }

        public DbSet<InvoiceCommentArchive> InvoiceCommentsArchive { get; set; }

        public DbSet<InvoiceAttachnmentArchive> InvoiceAttachnmentsArchive { get; set; }

        public DbSet<InvoiceActivityLogArchive> InvoiceActivityLogsArchive { get; set; }

        public DbSet<SystemVariable> SystemVariables { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

        public DbSet<PurchaseOrderMatchTracking> PurchaseOrderMatchTrackings { get; set; }

        public DbSet<PurchaseOrderMatchTrackingArchive> PurchaseOrderMatchTrackingsArchive { get; set; }
        public DbSet<Keyword> Keywords { get; set; }


        public DbSet<InvInfoRoutingLevel> InvInfoRoutingsLevels { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region Audit Log Insert here
        public string AuditModule { get; set; }
        public string AuditUser { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();
            var dateNow = DateTime.UtcNow;
            var activityList = new List<ActivityLogDto>();
            foreach (var entry in entries)
            { 
                var tableName = entry.Metadata.GetTableName();
                if (entry.State == EntityState.Modified)
                {
                    foreach (var prop in entry.OriginalValues.Properties)
                    {
                        var activity = new ActivityLogDto();
                        var propertyEntry = entry.Property(prop.Name);
                        if (propertyEntry.IsModified)
                        {
                            activity.Activity = "UPDATE";
                            activity.ActivityDate = dateNow;
                            activity.ActionBy = AuditUser;
                            activity.Module =  AuditModule;
                            activity.OldValue = entry.OriginalValues[prop.Name] != null ? entry.OriginalValues[prop.Name].ToString() : string.Empty;
                            activity.NewValue = entry.CurrentValues[prop.Name] != null ? entry.CurrentValues[prop.Name].ToString() : string.Empty;
                            activity.ColumnName = propertyEntry.Metadata.Name;
                            activity.TableName = tableName;
                            activity.metaDataNew = entry.State == EntityState.Added || entry.State == EntityState.Modified
                                                    ? JsonSerializer.Serialize(entry.CurrentValues.Properties
                                                        .ToDictionary(p => p.Name, p => entry.CurrentValues[p]))
                                                    : null;
                            activity.metaDataOld = entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                                                    ? JsonSerializer.Serialize(entry.OriginalValues.Properties
                                                        .Where(p => entry.Property(p.Name).IsModified || entry.State == EntityState.Deleted)
                                                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p]))
                                                    : null;
                            activityList.Add(activity);
                        }
                    }
                }

                if (entry.State == EntityState.Deleted)
                {
                    var activity = new ActivityLogDto();
                    foreach (var prop in entry.OriginalValues.Properties)
                    {
                        activity.Activity = "DELETE";
                        activity.OldValue = string.Empty;
                        activity.NewValue = string.Empty;
                        activity.ActivityDate = dateNow;
                        activity.ActionBy = AuditUser;
                        activity.Module = AuditModule;
                        activity.TableName = tableName;
                        activity.OldValue = entry.CurrentValues[prop.Name] != null ? entry.CurrentValues[prop.Name].ToString() : string.Empty;
                        activity.metaDataOld = entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                                                    ? JsonSerializer.Serialize(entry.OriginalValues.Properties
                                                        .Where(p => entry.Property(p.Name).IsModified || entry.State == EntityState.Deleted)
                                                        .ToDictionary(p => p.Name, p => entry.OriginalValues[p]))
                                                    : null;
                        activity.TableName = tableName;
                    }
                    activityList.Add(activity);
                }

                if (entry.State == EntityState.Added)
                {
                    var pkValues = entry.Metadata.FindPrimaryKey().Properties
                    .ToDictionary(p => p.Name, p => entry.Property(p.Name).CurrentValue);
                    var activity = new ActivityLogDto();
                    foreach (var prop in entry.CurrentValues.Properties)
                    {
                        activity.Activity = "INSERT";
                        activity.OldValue = string.Empty;
                        activity.NewValue = string.Empty;
                        activity.TableName = tableName;
                        activity.ActivityDate = dateNow;
                        activity.ActionBy = AuditUser;
                        activity.Module = AuditModule;
                        activity.metaDataNew = entry.State == EntityState.Added || entry.State == EntityState.Modified
                                                ? JsonSerializer.Serialize(entry.CurrentValues.Properties
                                                    .ToDictionary(p => p.Name, p => entry.CurrentValues[p]))
                                                : null;
                        activity.metaDataOld = entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                                                ? JsonSerializer.Serialize(entry.OriginalValues.Properties
                                                    .Where(p => entry.Property(p.Name).IsModified || entry.State == EntityState.Deleted)
                                                    .ToDictionary(p => p.Name, p => entry.OriginalValues[p]))
                                                : null;
                    }

                    var exists = activityList.Any(p => p.TableName == tableName && p.Activity == "INSERT");
                    if(!exists)
                        activityList.Add(activity);
                }

            }

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entry in entries)
            {
                var pkValues = entry.Metadata.FindPrimaryKey().Properties
                    .ToDictionary(p => p.Name, p => entry.Property(p.Name).CurrentValue);

                var fkValues = entry.Metadata.GetForeignKeys()
                    .SelectMany(fk => fk.Properties)
                    .ToDictionary(p => p.Name, p => entry.Property(p.Name).CurrentValue);

                var invoiceId = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name.Equals("InvoiceID", StringComparison.OrdinalIgnoreCase))
                    ?.CurrentValue;

                if (invoiceId != null)
                {
                    activityList.ForEach(f => f.InvoiceID = invoiceId == null ? 0 : Convert.ToInt32(invoiceId));
                    break;
                }
            }

            string tableFilePath = Path.Combine("actTable", "cbsap.table.json");
            string columnFilePath = Path.Combine("actTable", "cbsap.column.json");

            // Read JSON file content
            string tableJson = File.ReadAllText(tableFilePath);
            string columnJson = File.ReadAllText(columnFilePath);

            // Deserialize into List<string>
            List<string> tableList = JsonSerializer.Deserialize<List<string>>(tableJson);
            List<string> columnList = JsonSerializer.Deserialize<List<string>>(columnJson);

            var _filteredByTable = activityList.Select(s => new ActivityLog
            {
                InvoiceID = s.InvoiceID,
                Activity = s.Activity,
                ActionBy = s.ActionBy,
                Module = s.Module,
                OldValue = s.OldValue,
                NewValue = s.NewValue,
                TableName = s.TableName,
                ColumnName = s.ColumnName,
                metaDataNew = s.metaDataNew,
                metaDataOld = s.metaDataOld,
                MetaData = s.MetaData,
                ActivityDate = s.ActivityDate
            }).Where(w => !tableList.Contains(w.TableName)).ToList();

            var _filteredByColumn = _filteredByTable.Where(w => !columnList.Contains(w.ColumnName)).ToList();

            
            ActivityLogs.AddRange(_filteredByColumn);
            await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
        #endregion
    }
}