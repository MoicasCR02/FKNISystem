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
    public class ServiceCarrito : IServiceCarrito
    {
        private readonly IRepositoryCarrito _repository;
        private readonly IMapper _mapper;
        public ServiceCarrito(IRepositoryCarrito repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }



        public async Task<CarritoDTO> FindByIdAsync(int id_usuario)
        {
            var @object = await _repository.FindByIdAsync(id_usuario);
            var objectMapped = _mapper.Map<CarritoDTO>(@object);
            return objectMapped;
        }

        public async Task<int> AddAsync(CarritoDTO dto)
        {
            // Map LibroDTO to Libro
            var objectMapped = _mapper.Map<Carrito>(dto);

            // Return
            return await _repository.AddAsync(objectMapped);
        }
    }
}
