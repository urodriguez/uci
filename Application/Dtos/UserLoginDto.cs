﻿namespace Application.Dtos
{
    public class UserLoginDto : IDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}