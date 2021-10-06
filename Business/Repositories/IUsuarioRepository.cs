﻿using Dio.BackendComNETCore.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dio.BackendComNETCore.Business.Repositories
{
    public interface IUsuarioRepository
    {
        void Adicionar(Usuario usuario);
        void Commit();
        Task<Usuario> ObterUsuarioAsync(string login);
    }
}
