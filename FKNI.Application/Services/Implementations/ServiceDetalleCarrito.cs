using AutoMapper;
using FKNI.Application.DTOs;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Models;
using FKNI.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<DetalleCarritoDTO>> FindByIdAsync(int id_carrito)
        {
            var @object = await _repository.FindByIdAsync(id_carrito);
            var objectMapped = _mapper.Map<ICollection<DetalleCarritoDTO>>(@object);
            return objectMapped;
        }

        public async Task<DetalleCarritoDTO> FindByExist(int id_carrito, int id_producto,string talla)
        {
            var @object = await _repository.FindByIdExists(id_carrito,id_producto,talla);
            var objectMapped = _mapper.Map<DetalleCarritoDTO>(@object);
            return objectMapped;
        }


        public async Task<int> AddAsync(DetalleCarritoDTO dto)
        {
            // Map LibroDTO to Libro
            var objectMapped = _mapper.Map<DetalleCarrito>(dto);

            // Return
            return await _repository.AddAsync(objectMapped);
        }

        public async Task UpdateAsync(DetalleCarritoDTO dto)
        {
            var entity = _mapper.Map<DetalleCarrito>(dto);
            // Este mapea el dto al objeto existente SIN cambiar el Id
            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task<DetalleCarritoDTO> DeleteAsync(int id_producto, int id_carrito, string talla)
        {
            var @object = await _repository.DeleteAsync(id_producto, id_carrito, talla);
            var objectMapped = _mapper.Map<DetalleCarritoDTO>(@object);
            return objectMapped;
            
        }
    }
}























