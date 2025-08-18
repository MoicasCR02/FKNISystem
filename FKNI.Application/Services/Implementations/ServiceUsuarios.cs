using AutoMapper;
using FKNI.Application.Config;
using FKNI.Application.DTOs;
using FKNI.Application.Services.Interfaces;
using FKNI.Application.Utils;
using FKNI.Infraestructure.Models;
using FKNI.Infraestructure.Repository.Implementations;
using FKNI.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Implementations
{
    public class ServiceUsuarios : IServiceUsuarios
    {
        private readonly IRepositoryUsuarios _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;
        public ServiceUsuarios(IRepositoryUsuarios repository, IMapper mapper, IOptions<AppConfig> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }
        public async Task<UsuariosDTO> FindByIdAsync(int id_usuario)
        {
            var @object = await _repository.FindByIdAsync(id_usuario);
            var objectMapped = _mapper.Map<UsuariosDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<UsuariosDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            // Map List<Usurios> a ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<UsuariosDTO>>(list);
            // Return lista
            return collection;
        }


        public async Task<int> AddAsync(UsuariosDTO dto)
        {
            var objectMapped = _mapper.Map<Usuarios>(dto);

            // Hashear contraseña
            objectMapped.Contrasena = PasswordHasher.HashPassword(dto.Contrasena);

            return await _repository.AddAsync(objectMapped);
        }


        public async Task<UsuariosDTO?> LoginAsync(string username, string password)
        {
            // 1. Buscar usuario por nombre de usuario
            var usuario = await _repository.LoginNameAsync(username);

            if (usuario == null)
            {
                return null; // Usuario no existe
            }

            // 2. Verificar contraseña
            bool isPasswordValid = PasswordHasher.VerifyPassword(password, usuario.Contrasena);

            if (!isPasswordValid)
            {
                return null; // Contraseña incorrecta
            }

            // 3. Mapear a DTO si es válido
            return _mapper.Map<UsuariosDTO>(usuario);
        }

    }
}
