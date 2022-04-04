using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NJBudgetWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyntheseController : ControllerBase
    {
        private readonly ISyntheseService _synthService;
        private readonly ILogger _logger;

        private SyntheseController()
        {
            //Dummy for DI.
        }

        public SyntheseController(ISyntheseService service, ILogger<SyntheseController> logger)
        {
            _synthService = service;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ByAppartenance")]
        public async Task<ActionResult<SyntheseDepenseGlobalModel>> GetByAppartenanceAsync()
        {
            try
            {
                using var getTask = _synthService.GetSyntheseByAppartenanceAsync((byte)DateTime.Now.Month);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appartenanceId"></param>
        /// <returns></returns>
        [HttpGet("ForAppartenance/{appartenanceId}")]
        public async Task<ActionResult<SyntheseDepenseByAppartenanceModel>> GetByGroupAsync(Guid appartenanceId)
        {
            if (appartenanceId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                using Task<SyntheseDepenseByAppartenanceModel> getTask = _synthService.GetSyntheseForAppartenanceAsync(appartenanceId, (byte)DateTime.Now.Month);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("SyntheseMois")]
        public async Task<ActionResult<SyntheseMoisModel>> GetGlobalAsync()
        {
            try
            {
                using Task<SyntheseMoisModel> getTask = _synthService.GetSyntheseGlobal((byte)DateTime.Now.Month);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
