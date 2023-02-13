using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI.Data
{
    public class VillaStore
    {
        public static List<VillaDto> villaList=new List<VillaDto> { 
                
                new VillaDto { Id=1,Name="Pool View",SqFeet=200,Place="London"},
                new VillaDto{Id=2,Name="Beach View",SqFeet=400,Place="India"}
            };
    }
}
