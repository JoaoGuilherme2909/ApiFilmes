﻿using System.ComponentModel.DataAnnotations;

namespace FilmesApi.models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title cannot be null")]
    public string Titulo { get; set; }
    
    [Required(ErrorMessage = "Gender cannot be null")]
    [MaxLength(50, ErrorMessage = "Gender can't be longer then 50 caracteres")]
    public string Genero { get; set; }
    
    [Required]
    [Range(70, 600, ErrorMessage = "Movie must have at least 70 minutes and cannot be longer then 600")]
    public int Duracao { get; set; } 
}
