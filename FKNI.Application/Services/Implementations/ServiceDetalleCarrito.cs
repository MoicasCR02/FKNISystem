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
    public class ServiceDetalleCarrito : IServiceDetalleCarrito
    {

        private readonly IRepositoryDetalleCarrito _repository;
        private readonly IMapper _mapper;
        public ServiceDetalleCarrito(IRepositoryDetalleCarrito repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(DetalleCarritoDTO dto)
        {
            // Map LibroDTO to Libro
            var objectMapped = _mapper.Map<DetalleCarrito>(dto);

            // Return
            return await _repository.AddAsync(objectMapped);
        }
    }
}























