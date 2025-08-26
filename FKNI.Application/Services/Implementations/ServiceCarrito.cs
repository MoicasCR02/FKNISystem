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



        public async Task<ICollection<CarritoDTO>> FindByIdAsync(int id_usuario)
        {
            var @object = await _repository.FindByIdAsync(id_usuario);
            var objectMapped = _mapper.Map<ICollection<CarritoDTO>>(@object);
            return objectMapped;
        }

        public async Task<CarritoDTO> FindByIdCarritoAsync(int id_carrito)
        {
            var @object = await _repository.FindByIdAsync(id_carrito);
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

        public async Task UpdateAsync(CarritoDTO dto)
        {
            var objectMapped = _mapper.Map<Carrito>(dto);
            await _repository.UpdateAsync(objectMapped);
        }
    }
}
