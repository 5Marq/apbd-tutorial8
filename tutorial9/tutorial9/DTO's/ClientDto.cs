using System.ComponentModel.DataAnnotations;

namespace tutorial9.DTO_s;

public class ClientDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
}