using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        [HttpGet] // this is the endpoints it helps to communication with other application which 
        //is using this api 
        public IEnumerable<VillaDto> GetVillas()
        {
            return VillaStore.villaList;

        }
        //[HttpGet("id")]  //this will expect id
        [HttpGet("{id:int}")]  // in this we expilicitly define it as int               
        public VillaDto GetVillaById(int id)
        {
            return VillaStore.villaList.FirstOrDefault(u=>u.Id==id);

        }

    }
}
