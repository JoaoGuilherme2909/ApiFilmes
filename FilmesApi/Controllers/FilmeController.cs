using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.filmes.Add(filme);
        _context.SaveChanges();
        return Created($"/filmes/{filme.Id}",filme);
    }

    [HttpGet]
    [ProducesResponseType<Filme>(StatusCodes.Status200OK)]
    public IActionResult VisualizarTodosOsFilmes([FromQuery]int skip = 0, [FromQuery]int take = 50)
    {
        return Ok(_mapper.Map<ReadFilmeDto>(_context.filmes.Skip(skip).Take(take)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Filme>(StatusCodes.Status200OK)]
    public IActionResult VisualizarFilmesPorId([FromRoute] int id)
    {
        var filme = _context.filmes.Where((f) => f.Id == id);
     
        if (!filme.Any()) return NotFound("Filme nao encontrado");
        

        return Ok(_mapper.Map<ReadFilmeDto>(filme));
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme([FromRoute] int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();
        
        _mapper.Map(filmeDto, filme);
        
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmePatch([FromRoute] int id
        , JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.filmes.FirstOrDefault(f => f.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id) 
    {
        var filme = _context.filmes.FirstOrDefault(f => f.Id == id);

        if (filme == null) return NotFound();

        _context.Remove(filme);

        _context.SaveChanges();

        return NoContent();
    
    }

}
