using Common.Utilities;
using Entities.Common;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace Data
{
    public class ApplicationDbContext(DbContextOptions options, ILogger<ApplicationDbContext> _logger)
        : IdentityDbContext<
          User, Role, Guid,
          IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
          IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            //modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Data"));
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();
            _logger.LogInformation("Model creating completed with custom conventions and configurations.");

        }

        public override int SaveChanges()
        {
            _cleanString();
            _logger.LogInformation("SaveChanges called (sync).");

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            _logger.LogInformation("SaveChanges called (sync, acceptAllChangesOnSuccess={Accept})", acceptAllChangesOnSuccess);

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            _logger.LogInformation("SaveChangesAsync called (acceptAllChangesOnSuccess={Accept})", acceptAllChangesOnSuccess);

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            _logger.LogInformation("SaveChangesAsync called.");

            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        _logger.LogDebug("Sanitized string property '{Property}' on entity '{EntityType}'.", propName, item.Entity.GetType().Name);
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }


        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) {

                _logger.LogWarning("Attempted to begin a new transaction while another is active.");

                return null; }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            _logger.LogInformation("Transaction started with ID: {TransactionId}", _currentTransaction.TransactionId);

            return _currentTransaction;
        }

        public void RollbackTransaction()
        {
            try
            {
                _logger.LogWarning("Rolling back transaction ID: {TransactionId}", _currentTransaction?.TransactionId);

                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                    _logger.LogInformation("Transaction rolled back and disposed.");

                }
            }
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
                _logger.LogInformation("Transaction committed successfully. ID: {TransactionId}", transaction.TransactionId);

            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error committing transaction ID: {TransactionId}", transaction.TransactionId);

                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                    _logger.LogInformation("Transaction disposed after commit.");

                }
            }
        }
    }
}
