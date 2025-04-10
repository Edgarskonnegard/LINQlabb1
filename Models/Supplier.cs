using System.ComponentModel.DataAnnotations;

public class Supplier
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public string ContactPerson { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public string Phone { get; set; }

    public ICollection<Product> Products{ get; set; } = new List<Product>();
}