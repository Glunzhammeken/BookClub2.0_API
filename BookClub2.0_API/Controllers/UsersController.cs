using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClub2._0.Repositories;
using BookClub2._0.Models;
using BookClub2._0;
using BookClub2._0.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using BookClub2._0_API.Records;


namespace BookClub2._0_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IRepository _userRepository;

        public UsersController(IRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UsersController>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet()]
        public ActionResult<IEnumerable<User>> Get()
        {
            IEnumerable<User> result = _userRepository.GetUsers();
            if (result.Count() > 0)
            {
                return Ok(result);
            }
            return NoContent();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<User> Post([FromBody] UserRecord NewUserRecord)
        {
            try
            {
                User userConverted = Recordhelper.ConvertUserRecord(NewUserRecord);
                User user = _userRepository.Add(userConverted);
                return Created("*/*" + userConverted.Id, user);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Indeholder nulls" + ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest("Out of range" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("" + ex.Message);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
