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
    public class ServicePagos : IServicePagos
    {
        private readonly IRepositoryPagos _repository;
        private readonly IMapper _mapper;
        public ServicePagos(IRepositoryPagos repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PagosDTO> FindByIdAsync(int id_pago)
        {
            var @object = await _repository.FindByIdAsync(id_pago);
            var objectMapped = _mapper.Map<PagosDTO>(@object);
            return objectMapped;
        }
        public async Task<ICollection<PagosDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            // Map List<Usurios> a ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<PagosDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task<int> AddAsync(PagosDTO dto)
        {
            // Map LibroDTO to Libro
            var objectMapped = _mapper.Map<Pagos>(dto);
            // Return
            return await _repository.AddAsync(objectMapped);
        }
    }
}
