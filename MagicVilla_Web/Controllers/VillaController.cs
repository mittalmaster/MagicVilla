using AutoMapper;
using AutoMapper.Configuration.Annotations;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaService _villaService;
        public VillaController(IMapper mapper,IVillaService villaService) 
        { 
            _mapper = mapper;
            _villaService = villaService;
        
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> list = new();
            var reponse = await _villaService.GetAllAsync<APIResponse>();
            if (reponse != null && reponse.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(reponse.Result));

            }
            return View(list);

        }
        public async Task<IActionResult> CreateVilla()
        {            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villaCreateDTO)
        {
            if(ModelState.IsValid)
            {
                var reponse = await _villaService.CreateAsync<APIResponse>(villaCreateDTO);
                if(reponse!= null && reponse.IsSuccess) 
                { 
                    return RedirectToAction("IndexVilla");
                }
            }             
            return View(villaCreateDTO);

        }
        public async Task<IActionResult> UpdateVilla(int VillaId)
        {
            
            if(ModelState.IsValid)
            {
                var responce = await _villaService.GetAsync<APIResponse>(VillaId);
                if(responce!= null && responce.IsSuccess)
                {
                    var val = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(responce.Result));
                    VillaUpdateDTO v = _mapper.Map<VillaUpdateDTO>(val);
                    return View(v);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var reponse = await _villaService.UpdateAsynv<APIResponse>(villaUpdateDTO);
                if (reponse != null && reponse.IsSuccess)
                {
                    return RedirectToAction("IndexVilla");
                }
            }
            return View(villaUpdateDTO);

        }
        public async Task<IActionResult> DeleteVilla(int VillaId)
        {            
            var responce = await _villaService.GetAsync<APIResponse>(VillaId);
            if (responce != null && responce.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(responce.Result)); 
                return View(model);
            }
            
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVilla(VillaDto villaDTO)
        {            
            var reponse = await _villaService.DeleteAsync<APIResponse>(villaDTO.Id);
            if (reponse != null && reponse.IsSuccess)
            {
                return RedirectToAction("IndexVilla");
            }            
            return View(villaDTO);
        }
    }
}
