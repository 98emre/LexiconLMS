using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lexicon.Api.Entities;
using Lexicon.Api.Repositories;
using AutoMapper;
using Lexicon.Api.Dtos.CourseDtos;

namespace Lexicon.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly IUnitOfWork _UoW;
    private readonly IMapper _mapper;

    public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _UoW = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var courses = await _UoW.Courses.GetAllAsync();

        if (courses == null || !courses.Any())
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetCourse(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var course = await _UoW.Courses.GetAsync(id);

            return Ok(_mapper.Map<CourseDto>(course));
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCourse(int id, CoursePostDto coursePost)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var existingCourse = await _UoW.Courses.GetAsync(id);
            _mapper.Map(coursePost, existingCourse);

            _UoW.Courses.Update(existingCourse);
            await _UoW.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _UoW.Users.GetAsync(id) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CoursePostDto>> PostCourse(CoursePostDto coursePostDto)
    {
        var course = _mapper.Map<Course>(coursePostDto);

        try
        {
            _UoW.Courses.Add(course);
            await _UoW.SaveAsync();
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var course = await _UoW.Courses.GetAsync(id);

            _UoW.Courses.Delete(course.CourseId);
            await _UoW.SaveAsync();
        }

        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }
}
