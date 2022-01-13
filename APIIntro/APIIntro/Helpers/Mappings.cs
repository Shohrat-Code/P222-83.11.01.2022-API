using APIIntro.Models;
using APIIntro.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntro.Helpers
{
    public class Mappings:Profile
    {
        public Mappings()
        {
            CreateMap<Employee, DtoEmployee>().ReverseMap();
            CreateMap<Position, DtoPosition>().ReverseMap();
        }
    }
}
