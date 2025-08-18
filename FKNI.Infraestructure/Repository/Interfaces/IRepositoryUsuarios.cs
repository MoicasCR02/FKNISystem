using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuarios
    {
        Task<ICollection<Usuarios>> ListAsync();
        Task<Usuarios> FindByIdAsync(int IdUsuario);
        Task<int> AddAsync(Usuarios entity);
        Task<Usuarios> LoginNameAsync(string id);
    }
}
