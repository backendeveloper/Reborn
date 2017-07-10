using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reborn.Web.Api.Utils.Exception;
using Reborn.Web.Api.V1.Models;

namespace Reborn.Web.Api.V1.Controllers
{
    [ErrorActionFilter]
    [ValidationActionFilter]
    public class BaseV1Controller : Controller
    {
    }
}