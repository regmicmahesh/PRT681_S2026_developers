using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMusic.Models;

public class MusicGenreViewModel
{
    public List<Music>? MusicList { get; set; }
    public SelectList? Genres { get; set; }
    public string? MusicGenre { get; set; }
    public string? SearchString { get; set; }
}
