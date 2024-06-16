using System.ComponentModel.DataAnnotations;

namespace tutorial9.DTO_s;

public class AddClientDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Telephone { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; }
}