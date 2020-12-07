using Microsoft.AspNetCore.Mvc;
using Response = Vrnz2.Challenge.ServiceContracts.Responses.Management.Customers.PingController;

namespace Vrnz2.Challenge.Payments.WebApi.Controllers
{
    [Route("")]
    public class PingController
        : Controller
    {
        #region Methods

        /// <summary>
        /// Service Heart Beat end point
        /// </summary>
        /// <returns>DateTime.UtcNow + Service Name</returns>
        [HttpGet("ping")]
        [ProducesResponseType(typeof(Response.Ping), 200)]
        public JsonResult Ping()
            => Json(new Response.Ping());

        #endregion
    }
}
