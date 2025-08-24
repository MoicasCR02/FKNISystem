using AutoMapper;
using FKNI.Application.DTOs;
using FKNI.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Profiles
{
    public class DetallePedidoProductoProfile : Profile
    {
        public DetallePedidoProductoProfile()
        {
            CreateMap<DetallePedidoProductoDTO, DetallePedidoProducto>().ReverseMap();
        }
    }
}
