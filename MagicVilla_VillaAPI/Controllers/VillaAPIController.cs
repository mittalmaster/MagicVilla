using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers
{
    //route to that controller
    //[Route("api/[controller]")] // this will take VillaAPIController and in future if name of 
    //file changes it will give error so thats why we are using 
    //hardcoded VillaAPI it will not change
    [Route("api/VillaAPI")]

    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger;
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}
        protected APIResponse _response;
        private readonly IVillaRepository _dbVilla;
        private readonly ILoggerCustom _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository villaRepository,ILoggerCustom logger, ApplicationDbContext db,IMapper mapper)
        {
            _dbVilla = villaRepository;
            _logger = logger;
            _db = db;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet] // this is the endpoints it helps to communication with other application which 
        //is using this api 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.Info("Getting all the Villas", "");
                //ActionResult is very useful when we have multiple return type
                IEnumerable<Villa> villaList = await _dbVilla.GetAll();
                _response.Result = _mapper.Map<List<VillaDto>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages=new List<string> { ex.ToString()};
                _response.IsSuccess= false;

            }
            return _response;
        }

        //[HttpGet("id")]  //this will expect id
        [HttpGet("{id:int}")]  // in this we expilicitly define it as int
        //define the possible outcomes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize("Roles = admin")]

        //other way to write but above is better more readable and not hardcoded
       // [ProducesResponseType(200,Type=typeof(VillaDto))] //by this we can simply write ActionResult
        // in return type of below function 
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]

        public async Task<ActionResult<APIResponse>> GetVillaById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Info("Get the Error while getting with " + id, "Error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.Get(u=>u.Id == id);//best way to get and pass linq operation
                if (villa is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<APIResponse>> CreateVilla(VillaCreateDTO villaCreate)
        {
            try
            {
                //Custom Errors 
                if (await _dbVilla.Get(u => u.Name.ToLower() == villaCreate.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "This Villa Already Exist ");
                }
                if (villaCreate == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = villaCreate;
                    return BadRequest(_response);
                }
                await _dbVilla.Create(_mapper.Map<Villa>(villaCreate));
                //await _dbVilla.Save();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = villaCreate;
                return Ok(_response.Result);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:int}",Name ="DeleteVilla")]
        [Authorize("Roles = Custom")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id) //for IActionResult no need to define return type
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var deletedItem = await _dbVilla.Get(u => u.Id == id);
                if (deletedItem is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbVilla.Remove(deletedItem); //for remove there is no await 
                //await _dbVilla.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody]VillaUpdateDTO villaUpdate)
        {
            try
            {
                if (villaUpdate == null || id != villaUpdate.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                await _dbVilla.Update(_mapper.Map<Villa>(villaUpdate));                
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPatch("{id:int}",Name ="UpdatePartialDetail")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialDetail(int id ,JsonPatchDocument<VillaUpdateDTO> patchUpdateDocument)
        {
            if(patchUpdateDocument == null || id == 0)
            {
                return BadRequest();
            }
            var villa =await _dbVilla.Get(v => v.Id== id,false);
            if(villa==null)
            {
                return NotFound();
            }
           
            var temp = _mapper.Map<VillaUpdateDTO>(villa);
            patchUpdateDocument.ApplyTo(temp, ModelState);
            //{ "op": "add", "path": "/biscuits/1", "value": { "name": "Ginger Nut" } }
            //while adding from Swagger API enters values in this format 

            await _dbVilla.Update(_mapper.Map<Villa>(temp));            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }


    }
}
