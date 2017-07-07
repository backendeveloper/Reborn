using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reborn.Web.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class BaseController : Controller
    {
    }
}