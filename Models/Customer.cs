using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}