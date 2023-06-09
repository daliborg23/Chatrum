﻿using System.ComponentModel.DataAnnotations;

public class LoginViewModel {
    [Required]
    public string Name { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}