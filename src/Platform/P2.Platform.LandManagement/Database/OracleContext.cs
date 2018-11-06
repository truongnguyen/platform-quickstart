namespace P2.Platform.LandManagement.Database
{
    using Oracle.ManagedDataAccess.Client;
    using Oracle.ManagedDataAccess.EntityFramework;
    using System.Data.Entity;

    [DbConfigurationType(typeof(ModelConfiguration))]
    public class OracleContext : DbContext
    {
        public OracleContext(string nameOrConnectionString) : 
            base(new OracleConnection(nameOrConnectionString), true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("USER_INFO", "R50DEV");
            modelBuilder.Entity<Header>().ToTable("V_PROC_MIDCAP_CHECK_HDR", "R52DEV");
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Header> Headers { get; set; }
    }

    public class ModelConfiguration : DbConfiguration
    {
        public ModelConfiguration()
        {
            SetProviderServices("Oracle.ManagedDataAccess.Client", EFOracleProviderServices.Instance);
        }
    }
}
