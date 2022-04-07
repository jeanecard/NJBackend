using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NJBudgetWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly ILogger _logger;

        private GroupController()
        {
            //Dummy for DI.
        }

        public GroupController(IGroupService gService, ILogger<GroupController> logger)
        {
            _groupService = gService;
            _logger = logger;
        }

        /// <summary>
        /// First violent version. We rely on webapi middleware for error.
        /// </summary>
        /// <param name="idAppartenance"></param>
        /// <returns></returns>
        // GET api/<GroupController>/5

        
        [HttpGet("ByIdAppartenance/{idAppartenance}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsAsync(Guid idAppartenance)
        {
            if(idAppartenance == Guid.Empty)
            {
                return BadRequest("Guid can not be empty.");
            }
            try
            {
                using var getTask = _groupService.GetGroupsAsync(idAppartenance);
                await getTask;
                if (getTask.IsCompletedSuccessfully)
                {
                    return Ok(getTask.Result);
                }
                else
                {
                    _logger.LogError("Could not procees task");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }

        /// <summary>
        /// First violent version. We rely on webapi middleware for error.
        /// month == 0  => current month
        /// </summary>
        /// <param name="idAppartenance"></param>
        /// <returns></returns>
        // GET api/<GroupController>/5
        [HttpGet("ById/{idGroup}")]
        public async Task<ActionResult<Compte>> GetCurrentCompteAsync(Guid idGroup)
        {
            if(idGroup == Guid.Empty)
            {
                return null;
            }
            try
            {
                var now = DateTime.Now;
                using var getTask = _groupService.GetCompteAsync(idGroup, (byte)now.Month, (ushort)now.Year);
                await getTask;
                if (getTask.IsCompletedSuccessfully)
                {
                    return Ok(getTask.Result);
                }
                else
                {
                    _logger.LogError("Could not procees task");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Bennie, bennie, bennie !!!!!!");
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
