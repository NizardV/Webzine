using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Business.Contracts.Dto;

namespace Webzine.Business.Contracts.Services
{
    /// <summary>
    /// Defines a contract for retrieving dashboard statistics.
    /// </summary>
    /// <remarks>Implementations of this interface provide methods to access aggregated data or metrics for
    /// display on a dashboard. This interface is intended to be used by components that require up-to-date dashboard
    /// information, such as UI controllers or reporting services.</remarks>
    public interface IDashboardService
    {
        /// <summary>
        /// Retrieves aggregated statistics for display on the dashboard.
        /// </summary>
        /// <returns>A <see cref="DashboardDto"/> containing the current dashboard statistics. The returned object includes
        /// summary data relevant to the dashboard view.</returns>
        DashboardDto GetDashboardStats();
    }
}
