using System.ComponentModel.DataAnnotations;
namespace backend.Models;

public class Product
{
    public int Id { get; set; }
    [MaxLength(50)]
    public int ProductTypeId { get; set; } // FK (Kroger API as "categories")
    
    [MaxLength(50)]
    public string Description { get; set; }
    
    [MaxLength(50)]
    public string Brand { get; set; }
    
    [MaxLength(50)]
    public string Size { get; set; }
    
    public long Upc { get; set; }
    
    // Navigation properties
    public ProductType ProductType { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
}

// On Kroger API, this would be "categories"
public class ProductType
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    // Navigation property
    public List<Product> Products { get; set; }
}

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; } // FK (ProductTypeId)

    [MaxLength(50)]
    public string Perspective { get; set; }

    [MaxLength(20)]
    public string Size { get; set; }

    [MaxLength(2083)] 
    public string Url { get; set; }

    // Navigation property
    public Product Product { get; set; }
}