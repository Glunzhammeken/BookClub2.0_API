using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClub2._0.Repositories;
using BookClub2._0.Models;
using BookClub2._0.Interfaces;
using BookClub2._0_API.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookClub2._0_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookClubController : ControllerBase
    {
        private readonly IBookClubRepository _bookClubRepository;
        private readonly IUserRepository _userRepository;
        public BookClubController(IBookClubRepository bookClubRepository, IUserRepository userRepository)
        {
            _bookClubRepository = bookClubRepository;
            _userRepository = userRepository;

        }

        // GET: api/BookClub
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<BookClub>> Get()
        {
            IEnumerable<BookClub> result = _bookClubRepository.GetAll();
            if (result.Any())
            {
                return Ok(result);
            }
            return NoContent();
        }

        // GET: api/BookClub/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{id}")]
        public ActionResult<BookClub> Get(int id)
        {
            BookClub? bookClub = _bookClubRepository.GetById(id);

            if (bookClub != null)
            {
                return Ok(bookClub);
            }
            return NoContent();
        }

        // POST: api/BookClub
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<BookClub> Post([FromBody] BookClubRecord NewBookClubRecord,int ownerId)
        {
            try
            {
                BookClub bookClubConverted = RecordhelperBookclubRecords.ConvertBookClubRecord(NewBookClubRecord);
                BookClub bookClub = _bookClubRepository.Add(bookClubConverted, ownerId);
                return Created($"api/bookclub/{bookClub.Id}", bookClub);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Missing required field: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{bookClubId}/members")]
        public ActionResult AddMember(int bookClubId, int memberId)
        {
            try
            {
                var bookClub = _bookClubRepository.GetById(bookClubId);
                if (bookClub == null)
                {
                    return NotFound($"BookClub with ID {bookClubId} not found.");
                }

                if (bookClub.Members.Any(m => m.Id == memberId))
                {
                    return BadRequest("User is already a member of the BookClub.");
                }
                var memberToAdd = _userRepository.GetUserById(memberId);
                bookClub.Members.Add(memberToAdd);
                
                return Ok($"User with ID {memberId} added to BookClub with ID {bookClubId}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        // PUT: api/BookClub/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<BookClub> Put(int id, [FromBody] BookClubRecord NewBookClubRecord)
        {
            try
            {
                BookClub bookClubConverted = RecordhelperBookclubRecords.ConvertBookClubRecord(NewBookClubRecord);
                BookClub? updated = _bookClubRepository.Update(id, bookClubConverted);

                if (updated != null)
                {
                    return Ok(updated);
                }
                return NotFound($"BookClub with ID {id} not found.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Missing required field: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
        }

        // DELETE: api/BookClub/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public ActionResult<BookClub> Delete(int id)
        {
            try
            {
                BookClub deleted = _bookClubRepository.Remove(id);
                if (deleted != null)
                {
                    return Ok(deleted);
                }
                return NotFound($"BookClub with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{bookClubId}/members/{memberId}")]
        public ActionResult RemoveMember(int bookClubId, int memberId)
        {
            try
            {
                var bookClub = _bookClubRepository.GetById(bookClubId);
                if (bookClub == null)
                {
                    return NotFound($"BookClub with ID {bookClubId} not found.");
                }

                var member = bookClub.Members.FirstOrDefault(m => m.Id == memberId);
                if (member == null)
                {
                    return BadRequest("User is not a member of the BookClub.");
                }

                bookClub.Members.Remove(member);
                
                return Ok($"User with ID {memberId} removed from BookClub with ID {bookClubId}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


    }
}
