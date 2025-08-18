using FKNI.Application.DTOs;
using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Interfaces
{
    public interface IServiceUsuarios
        {
            Task<ICollection<UsuariosDTO>> ListAsync();
            Task<UsuariosDTO> FindByIdAsync(int id_usuario);
            Task<int> AddAsync(UsuariosDTO dto);
            Task<UsuariosDTO> LoginAsync(string id, string password);
        }
}
