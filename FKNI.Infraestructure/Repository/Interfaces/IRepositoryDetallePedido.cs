using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryDetallePedido
    {
        Task<DetallePedido> FindByIdAsync(int id_Detalle);
        Task<ICollection<DetallePedido>> ListAsync();
        Task<int> AddAsync(DetallePedido entity);
    }
}
