using AptitudeTest.Common.Data;
using AptitudeTest.Core.Interfaces;
using APTITUDETEST.Common.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Data.Data
{
    public class ScreenCaptureRepository:IScreenCaptureRepository
    {
        #region Properies

        private readonly AppDbContext _appDbContext;

        #endregion

        #region Constructor
        public ScreenCaptureRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Methods



        #endregion
    }
}
