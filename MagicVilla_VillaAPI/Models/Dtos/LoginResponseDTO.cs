﻿namespace MagicVilla_VillaAPI.Models.Dtos
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
