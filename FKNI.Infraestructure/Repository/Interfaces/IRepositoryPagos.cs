using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPagos
    {
        Task<Pagos> FindByIdAsync(int id_pago);
        Task<ICollection<Pagos>> ListAsync();
        Task<int> AddAsync(Pagos entity);
    }
}
