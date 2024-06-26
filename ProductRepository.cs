public static class ProductRepository
{
  public static List<Product> Products { get; set; } = Products = new List<Product>();

  public static void Init(IConfiguration configuration)
  {
    var products = configuration.GetSection("products").Get<List<Product>>();
    Products = products;
  }

  public static void Add(Product product)
  {
    if (Products == null)
      Products = new List<Product>();

    Products.Add(product);

  }


  public static Product GetBy(string code)
  {
    return Products.FirstOrDefault(p => p.Code == code);
  }


  public static void Remove(Product product)
  {
    Products.Remove(product);
  }
}
