using AptitudeTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Application.Services
{
    public class ScreenCaptureService :IScreenCaptureService
    {
        #region Properties
        private readonly IScreenCaptureRepository _screenCaptureRepository;
        #endregion

        #region Constructor
        public ScreenCaptureService(IScreenCaptureRepository screenCaptureRepository)
        {
            _screenCaptureRepository = screenCaptureRepository;
        }
        #endregion

        #region Methods

        #endregion 

    }
}
