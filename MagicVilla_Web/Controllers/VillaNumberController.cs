using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dtos;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IMapper mapper, IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var reponse = await _villaNumberService.GetAllAsync<APIResponse>();
            if (reponse != null && reponse.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(reponse.Result));

            }
            return View(list);

        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM list = new();
            var reponse = await _villaService.GetAllAsync<APIResponse>();
            if (reponse != null && reponse.IsSuccess)
            {
                list.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(reponse.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }) ;

            }

            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberCreate)
        {
            if (ModelState.IsValid)
            {
                var reponse = await _villaNumberService.CreateAsync<APIResponse>(villaNumberCreate.VillaNumber);
                if (reponse != null && reponse.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }
            return View(villaNumberCreate);

        }
        public async Task<IActionResult> UpdateVillaNumber(int VillaNo)
        {

            if (ModelState.IsValid)
            {
                var list = new VillaNumberUpdateVM();
                var responce = await _villaNumberService.GetAsync<APIResponse>(VillaNo);             
                if (responce != null && responce.IsSuccess)
                {
                    var x = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(responce.Result));
                    list.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(x);

                }
                if(list.VillaNumber!=null)
                {
                    var x = await _villaService.GetAllAsync<APIResponse>();
                    list.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(x.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
                    return View(list);
                }

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberUpdate)
        {
            if (ModelState.IsValid)
            {                
                var reponse = await _villaNumberService.UpdateAsynv<APIResponse>(villaNumberUpdate.VillaNumber);
                if (reponse != null && reponse.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }
            return View(villaNumberUpdate);

        }
        public async Task<IActionResult> DeleteVillaNumber(int VillaNo)
        {
            var x =new VillaNumberDeleteVM();
            var responce = await _villaNumberService.GetAsync<APIResponse>(VillaNo);
            if (responce != null && responce.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(responce.Result));
                x.VillaNumber = model;
                var r = await _villaService.GetAsync<APIResponse>(model.VillaId);
                x.VillaName = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(r.Result)).Name;
                return View(x);                
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var reponse = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
            if (reponse != null && reponse.IsSuccess)
            {
                return RedirectToAction("IndexVillaNumber");
            }
            return View(model);
        }
    }
}
