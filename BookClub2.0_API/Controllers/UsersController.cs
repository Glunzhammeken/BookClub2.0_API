using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClub2._0.Repositories;
using BookClub2._0.Models;
using BookClub2._0.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookClub2._0_API.Records;
using Microsoft.AspNetCore.Cors;


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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        // GET api/<ActorsController>/5

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User? user = _userRepository.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NoContent();
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // PUT api/<ActorsController>/5
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] UserRecord NewUserRecord)
        {
            try
            {
                User userConverted = Recordhelper.ConvertUserRecord(NewUserRecord);
                User? updated = _userRepository.UpdateUser(id, userConverted);

                if (updated != null)
                {
                    return Ok(updated);
                }
                return NotFound();

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            User deleted = _userRepository.Remove(id);
            if (deleted != null)
            {
                return Ok(deleted);

            }
            return NotFound();
        }
    }
}
