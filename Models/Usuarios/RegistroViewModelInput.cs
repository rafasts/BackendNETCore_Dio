﻿using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dio.BackendComNETCore.Models.Usuarios
{
    public class RegistroViewModelInput
    {
        [Required(ErrorMessage = "O Login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatório")]
        public string Senha { get; set; }
    }
}