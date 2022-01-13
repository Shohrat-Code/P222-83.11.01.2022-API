using APIIntro.Data;
using APIIntro.Models;
using APIIntro.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PositionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetEmployees()
        {
            List<DtoPosition> dtoPositions = new List<DtoPosition>();

            foreach (var item in _context.Positions.ToList())
            {
                DtoPosition dtoPosition = _mapper.Map<Position, DtoPosition>(item);

                dtoPositions.Add(dtoPosition);
            }
            return Ok(dtoPositions);
        }
    }
}
