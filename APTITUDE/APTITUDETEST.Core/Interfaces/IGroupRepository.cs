using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.Interfaces
{
    public interface IGroupRepository
    {
        public Task<JsonResult> GetActiveGroups();
    }
}
