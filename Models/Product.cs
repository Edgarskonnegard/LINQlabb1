using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set;}

    [Required]
    [MaxLength(100)]
    public string Name { get; set;}

    [MaxLength(500)]
    public string Description { get; set;}
    public decimal Price { get; set;}
    public int StockQuantity { get; set;}
    public int CategoryId { get; set;}
    public int SupplierId { get; set;}

    public Category Category { get; set;}
    public Supplier Supplier { get; set;}
    public ICollection<OrderDetail> OrderDetails { get; set;} = new List<OrderDetail>();

}