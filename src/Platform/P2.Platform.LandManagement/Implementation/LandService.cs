namespace P2.Platform.LandManagement.Implementation
{
    using P2.Platform.LandManagement.Database;
    using System.Linq;

    public class LandService : ILandService
    {
        private readonly OracleContext context;

        public LandService(OracleContext context)
        {
            this.context = context;

            var files = this.context.Headers.Take(10).ToList();
        }
    }
}
