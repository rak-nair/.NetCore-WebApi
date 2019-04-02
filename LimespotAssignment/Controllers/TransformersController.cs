using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models;
using LimespotAssignment.Models.Input;
using LimespotAssignment.Services.Interfaces;
using LimespotAssignment.Services.ResponseMessages;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace LimespotAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransformersController : ControllerBase
    {
        #region Public Methods
        IDatastore _datastore;
        IWarResolver _warResolver;
        ModelFactory _factory;

        public TransformersController(IDatastore datastore, IWarResolver warResolver, ModelFactory factory)
        {
            _datastore = datastore;
            _warResolver = warResolver;
            _factory = factory;
        }

        //Placeholder Index
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Welcome to Transformer land!");
        }

        //Add Autobot
        [HttpPost("add/autobot", Name = "AddAutobot")]
        public async Task<IActionResult> AddAutobot([FromBody] TransformerViewModel transformer)
        {
            return await AddTransformer(transformer, TransformerType.Autobots, "AddAutobot");
        }

        //Add Decepticon
        [HttpPost("add/decepticon", Name = "AddDecepticon")]
        public async Task<IActionResult> AddDecepticon([FromBody] TransformerViewModel transformer)
        {
            return await AddTransformer(transformer, TransformerType.Decepticons, "AddDecepticon");
        }

        //Get Transformer
        [HttpGet("get", Name = "GetTransformer")]
        public async Task<IActionResult> GetTransformer(int transformerid)
        {
            var transformer = await _datastore.GetTransformerDetailsAsync(transformerid);

            if (transformer != null)
                return Ok(transformer);
            else
                return BadRequest(Responses.INVALID_TRANSFORMER_ID);
        }

        //Update Transformer
        [HttpPatch("update", Name = "UpdateTransformer")]
        public async Task<IActionResult> UpdateTransformer(int transformerid, [FromBody] TransformerUpdateViewModel updatedTransformerViewModel)
        {
            if (ModelState.IsValid)
            {
                var transformerObject = await GetTransformer(transformerid);

                var transformer = GetTransformerObjectFromActionResult(transformerObject);

                if (transformer != null)
                {
                    var updatedTransformerObject =
                        _factory.UpdateTransformer(updatedTransformerViewModel, transformer as Transformer);
                    var result = await _datastore.UpdateTransformerAsync(transformerid, updatedTransformerObject);
                    return Ok(result);
                }
            }

            return BadRequest(Responses.INVALID_TRANSFORMER_ID);
        }

        //Remove Transformer
        [HttpDelete("remove", Name = "RemoveTransformer")]
        public async Task<IActionResult> RemoveTransformer(int transformerid)
        {
            var transformerObject = await GetTransformer(transformerid);

            var transformer = GetTransformerObjectFromActionResult(transformerObject);

            if (transformer != null)
            {
                var result = await _datastore.RemoveTransformerAsync(transformerid);
                if (result)
                    return StatusCode((int)HttpStatusCode.NoContent);
            }

            return BadRequest(Responses.INVALID_TRANSFORMER_ID);
        }

        //List all autobots
        [HttpGet("list/autobots", Name = "ListAutobots")]
        public async Task<IActionResult> ListAutobots()
        {
            var allAutobots = await _datastore.GetAllAutobotsAsync();

            return Ok(allAutobots);
        }

        //List all decepticons
        [HttpGet("list/decepticons", Name = "ListDecepticons")]
        public async Task<IActionResult> ListDecepticons()
        {
            var alldecepticons = await _datastore.GetAllDecepticonsAsync();

            return Ok(alldecepticons);
        }

        //Overall Score
        [HttpGet("overallscore", Name = "GetOverallScore")]
        public async Task<IActionResult> GetOverallScore(int transformerid)
        {
            var transformerObject = await GetTransformer(transformerid);

            var transformer = GetTransformerObjectFromActionResult(transformerObject);

            if (transformer != null)
            {
                var result = await _datastore.GetTransformerScoreAsync(transformerid);
                return Ok(result);
            }

            return BadRequest(Responses.INVALID_TRANSFORMER_ID);
        }

        [HttpGet("wagewar", Name = "WageWar")]
        public async Task<IActionResult> WageWar()
        {
            var combatants = await _datastore.GetAllTransformersAsync();
            var result = await _warResolver.WageWarAsync(combatants);

            return Ok(result);
        }
        #endregion

        #region Private Methods
        //Get TransformerObject from ActionResult
        ITransformer GetTransformerObjectFromActionResult(IActionResult result)
        {
            if (result as OkObjectResult != null)
            {
                var transformerObject = ((OkObjectResult)result).Value as ITransformer;
                if (transformerObject != null)
                    return transformerObject;
            }

            return null;
        }

        //Add Transformer object
        async Task<IActionResult> AddTransformer([FromBody] TransformerViewModel transformer, TransformerType type, string strCreatedAtRoute)
        {
            if (ModelState.IsValid)
            {
                var transformerObject = _factory.Create(transformer);
                transformerObject.Allegiance = type;

                transformerObject = await _datastore.AddTransformerAsync(transformerObject);
                return CreatedAtRoute(strCreatedAtRoute, transformerObject);
            }
            else
                return BadRequest(Responses.ERROR_INVALID_INPUTS_ADD_TRANSFORMER);
        }
        #endregion
    }
}