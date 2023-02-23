using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

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

        private readonly ILoggerCustom _logger;
        private readonly ApplicationDbContext _db;
        public VillaAPIController(ILoggerCustom logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }



        [HttpGet] // this is the endpoints it helps to communication with other application which 
        //is using this api 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Info("Getting all the Villas","");
            //ActionResult is very useful when we have multiple return type 
            
            return Ok(_db.Villas.ToList());

        }
        //[HttpGet("id")]  //this will expect id
        [HttpGet("{id:int}")]  // in this we expilicitly define it as int
        //define the possible outcomes
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //other way to write but above is better more readable and not hardcoded
       // [ProducesResponseType(200,Type=typeof(VillaDto))] //by this we can simply write ActionResult
        // in return type of below function 
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]

        public ActionResult<VillaDto> GetVillaById(int id)
        {
            if(id ==0)
            {
                _logger.Info("Get the Error while getting with " +id,"Error");
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            
            if(villa is null)
            {
                return NotFound();
            }
            return Ok(villa);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult<VillaDto> CreateVilla(VillaCreateDTO villaDto)
        {
            //Custom Errors 
            if(_db.Villas.FirstOrDefault(u=>u.Name.ToLower() == villaDto.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError","This Villa Already Exist ");
            }

            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            
            var villaTemp = new Villa();
           
            villaTemp.Name = villaDto.Name;
            villaTemp.Details = villaDto.Details;
            villaTemp.Occupancy = villaDto.Occupancy;
            villaTemp.Rate = villaDto.Rate;
            villaTemp.Amenity = villaDto.Amenity;
            villaTemp.Sqft = villaDto.Sqft;
            villaTemp.CreateDate = DateTime.Now;
            villaTemp.UpdatedDate = DateTime.Now;
            villaTemp.ImageUrl = villaDto.ImageUrl;
            _db.Villas.Add(villaTemp);
            _db.SaveChanges();

            return Ok(villaDto);

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [HttpDelete("{id:int}",Name ="DeleteVilla")]
        public IActionResult DeleteVilla(int id) //for IActionResult no need to define return type
        { 
            if(id == 0)
            {
                return BadRequest();
            }
            var deletedItem = _db.Villas.FirstOrDefault(u => u.Id == id);
            if(deletedItem is null)
            {
                return NotFound();
            }
            _db.Villas.Remove(deletedItem);
            _db.SaveChanges();
            return NoContent();        
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaUpdateDTO villaDto)
        {
            if(villaDto == null || id!=villaDto.Id) 
            {
                return BadRequest();
            }
            var villaTemp = new Villa();
            villaTemp.Id = villaDto.Id;
            villaTemp.Name = villaDto.Name;
            villaTemp.Details = villaDto.Details;
            villaTemp.Occupancy = villaDto.Occupancy;
            villaTemp.Rate = villaDto.Rate;
            villaTemp.Amenity = villaDto.Amenity;
            villaTemp.Sqft = villaDto.Sqft;
            villaTemp.CreateDate = DateTime.Now;
            villaTemp.UpdatedDate = DateTime.Now;
            villaTemp.ImageUrl = villaDto.ImageUrl;
            _db.Villas.Update(villaTemp);
            _db.SaveChanges();
            return NoContent();


        }

        [HttpPatch("{id:int}",Name ="UpdatePartialDetail")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialDetail(int id ,JsonPatchDocument<VillaUpdateDTO> patchDocument)
        {
            if(patchDocument == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id== id);
            if(villa==null)
            {
                return NotFound();
            }
            var villaTemp = new VillaUpdateDTO();
            villaTemp.Id = villa.Id;
            villaTemp.Name = villa.Name;
            villaTemp.Details = villa.Details;
            villaTemp.Occupancy = villa.Occupancy;
            villaTemp.Rate = villa.Rate;
            villaTemp.Amenity = villa.Amenity;
            villaTemp.Sqft = villa.Sqft;
            
            villaTemp.ImageUrl = villa.ImageUrl;
            patchDocument.ApplyTo(villaTemp, ModelState);
            //{ "op": "add", "path": "/biscuits/1", "value": { "name": "Ginger Nut" } }
            //while adding from Swagger API enters values in this format 
            var UpdatedVilla = new Villa();
            UpdatedVilla.Id = villaTemp.Id;
            UpdatedVilla.Name = villaTemp.Name;
            UpdatedVilla.Details = villaTemp.Details;
            UpdatedVilla.Occupancy= villaTemp.Occupancy;
            UpdatedVilla.Rate = villaTemp.Rate;
            UpdatedVilla.Amenity= villaTemp.Amenity;
            UpdatedVilla.Sqft = villaTemp.Sqft;
            _db.Villas.Update(UpdatedVilla);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();

        }


    }
}
