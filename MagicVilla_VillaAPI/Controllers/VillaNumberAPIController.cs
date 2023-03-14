using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberApi")]
    [ApiController]
    public class VillaNumberAPIController : Controller
    {
        protected APIResponse _response;
        private IVillaNumberRepository _dbVillaNumber;
        private IVillaRepository _dbVilla; 
        private readonly ILoggerCustom _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaNumberAPIController(IVillaRepository villaRepository,IVillaNumberRepository villaNumberRepository,ILoggerCustom logger, ApplicationDbContext db, IMapper mapper)
        {
            _dbVilla = villaRepository;
            _dbVillaNumber = villaNumberRepository;
            _db = db;
            _logger = logger;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _response.Result = _mapper.Map<IEnumerable<VillaNumberDTO>>(await _dbVillaNumber.GetAll());
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            { 
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("villaNumber:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNumber )
        {
            try
            {
                if(villaNumber < 1 )
                {
                    ModelState.AddModelError("", "Invalid Villa Number it should be greater than 0");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }
                var getvillanumber  = await _dbVillaNumber.Get(u => u.VillaNo == villaNumber);
                if(getvillanumber==null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = getvillanumber;
                    return NotFound(_response);
                }
                _response.Result=getvillanumber;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);   


            }
            catch(Exception ex)           
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber(VillaNumberCreateDTO CreateVillaNumber)
        {
            try
            {
                if(CreateVillaNumber == null || await _dbVillaNumber.Get(u => u.VillaNo == CreateVillaNumber.VillaNo) != null)
                {
                    ModelState.AddModelError("","Either Null or already exist ");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = CreateVillaNumber;
                    return BadRequest(_response);

                }  
                //
                if(await  _dbVilla.Get(u=>u.Id == CreateVillaNumber.VillaId)==null)
                {
                    ModelState.AddModelError("CustomError", "Villa with that VillaID not Exist ");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = CreateVillaNumber;
                    return BadRequest(_response);

                }
                await _dbVillaNumber.Create(_mapper.Map<VillaNumber>(CreateVillaNumber));
                
                _response.Result = CreateVillaNumber;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id , [FromBody]VillaNumberUpdateDTO villaUpdateNumber)
        {
            try
            {
                if(villaUpdateNumber == null || villaUpdateNumber.VillaNo!=id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if(await _dbVilla.Get(u=>u.Id == villaUpdateNumber.VillaId,false)==null)
                {
                    ModelState.AddModelError("", "Villa with that Id not Exist ");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                await _dbVillaNumber.Update(_mapper.Map<VillaNumber>(villaUpdateNumber));
               
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return View();
        }
        [HttpDelete("{id:int}",Name = "Villa Number")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                var villNumber = await _dbVillaNumber.Get(u => u.VillaNo == id);
                if(villNumber == null)
                {
                    _response.Result = id;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                await _dbVillaNumber.Remove(villNumber);
                         
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

    }
}
