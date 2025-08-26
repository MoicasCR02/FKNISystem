using AutoMapper;
using FKNI.Application.DTOs;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Models;
using FKNI.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Services.Implementations
{
    public class ServicePedidos : IServicePedidos
    {
        private readonly IRepositoryPedidos _repository;
        private readonly IMapper _mapper;
        public ServicePedidos(IRepositoryPedidos repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PedidosDTO> FindByIdAsync(int id_pedido)
        {
            var @object = await _repository.FindByIdAsync(id_pedido);
            var objectMapped = _mapper.Map<PedidosDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<PedidosDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            // Map List<Usurios> a ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<PedidosDTO>>(list);
            // Return lista
            return collection;
        }
        public async Task<int> AddAsync(PedidosDTO dto)
        {
            // Map LibroDTO to Libro
            var objectMapped = _mapper.Map<Pedidos>(dto);

            // Return
            return await _repository.AddAsync(objectMapped);
        }
    }
}
