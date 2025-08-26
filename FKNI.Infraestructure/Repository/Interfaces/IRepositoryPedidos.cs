using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedidos
    {
        Task<ICollection<Pedidos>> ListAsync();
        Task<Pedidos> FindByIdAsync(int id_pedido);
        Task<int> AddAsync(Pedidos entity);
    }
}
