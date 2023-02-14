using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MagicVilla_VillaAPI.Logging;

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
        public VillaAPIController(ILoggerCustom logger)
        {
            _logger = logger;

        }



        [HttpGet] // this is the endpoints it helps to communication with other application which 
        //is using this api 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Info("Getting all the Villas","");
            //ActionResult is very useful when we have multiple return type 
            
            return Ok(VillaStore.villaList);

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

            var villa = VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
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
        public ActionResult<VillaDto> CreateVilla(VillaDto villaDto)
        {
            //Custom Errors 
            if(VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower() == villaDto.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError","This Villa Already Exist ");
            }

            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if(villaDto.Id > 0) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);

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
            var deletedItem = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if(deletedItem is null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(deletedItem);
            return NoContent();        
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDto villa)
        {
            if(villa == null || id!=villa.Id) 
            {
                return BadRequest();
            }
            var vil = VillaStore.villaList.FirstOrDefault(u=>u.Id== id);
            vil.Name = villa.Name;
            //vil.SqFeet = villa.SqFeet;
            //vil.Place = villa.Place;
            return NoContent();


        }

        [HttpPatch("{id:int}",Name ="UpdatePartialDetail")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialDetail(int id ,JsonPatchDocument<VillaDto> patchDocument)
        {
            if(patchDocument == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id== id);
            if(villa==null)
            {
                return NotFound();
            }
            patchDocument.ApplyTo(villa, ModelState);
            //{ "op": "add", "path": "/biscuits/1", "value": { "name": "Ginger Nut" } }
            //while adding from Swagger API enters values in this format 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();

        }


    }
}
