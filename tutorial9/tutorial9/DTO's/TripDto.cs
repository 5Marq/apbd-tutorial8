using System.ComponentModel.DataAnnotations;

namespace tutorial9.DTO_s;

public class TripDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(220)]
    public string Description { get; set; }
    
    [Required]
    public DateTime DateFrom { get; set; }
    
    [Required]
    public DateTime DateTo { get; set; }
    
    [Required]
    public int MaxPeople { get; set; }

    [Required] 
    public List<CountryDto> Countries { get; set; }
    
    [Required]
    public List<ClientDto> Clients { get; set; }
    
}
