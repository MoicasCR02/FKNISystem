using AutoMapper;
using FKNI.Application.DTOs;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Implementations
{
    public class ServiceDetallePedido : IServiceDetallePedido
    {
        private readonly IRepositoryDetallePedido _repository;
        private readonly IMapper _mapper;
        public ServiceDetallePedido(IRepositoryDetallePedido repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DetallePedidoDTO> FindByIdAsync(int id_detallePedido)
        {
            var @object = await _repository.FindByIdAsync(id_detallePedido);
            var objectMapped = _mapper.Map<DetallePedidoDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<DetallePedidoDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            // Map List<Usurios> a ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<DetallePedidoDTO>>(list);
            // Return lista
            return collection;
        }
    }
}
