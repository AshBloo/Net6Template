using AdminLTE.Data;
using Coravel.Pro.Features.Metrics;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminLTE.Coravel.Metric
{
    public class UserCountMetric : IMetricCard
    {
        private ApplicationDbContext _context;

        // Inject dbContext from service provider.
        public UserCountMetric(ApplicationDbContext context) =>
            this._context = context;

        // Fetch and assign values in this method.  
        public async Task ExecuteAsync()
        {
            int count = await this._context.Users.CountAsync();
            this.Value = $"{count} Users";
            this.Title = "User Count";
            this.IsPositiveMetric = true;
            this.IncludeArrow = true;
        }

        // These are implemented from IMetric Card.
        public string Value { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool IsPositiveMetric { get; set; }
        public bool IncludeArrow { get; set; }
    }
}
