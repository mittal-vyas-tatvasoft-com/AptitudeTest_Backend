using AptitudeTest.Core.Interfaces;

namespace AptitudeTest.Application.Services
{
    public class ReportsService : IReportsService
    {
        #region Properties
        private readonly IReportsRepository _repository;
        #endregion

        #region Constructor
        public ReportsService(IReportsRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        #endregion
    }
}
