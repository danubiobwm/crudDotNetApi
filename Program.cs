using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);



// app.MapGet("/", () => "Hello World!");
// app.MapGet("/user", () => new { name = "John", email = "john@email.com" });
// app.MapGet("/addHeader", (HttpResponse response) =>
// {
//   response.Headers.Add("Test", "Danubio");
//   return "Hello";
// });

//Products Router

app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) =>
{
  var category = context.Categories.Where(cat => cat.Id == productRequest.CategoryId).First();
  var product = new Product
  {
    Code = productRequest.Code,
    Name = productRequest.Name,
    Description = productRequest.Description,
    Category = category
  };
  // if(productRequest.Tags != null){
  //   product.Tags = new List<Tag>();
  //   foreach(var item in productRequest.Tags){
  //     product.Tags.Add(new Tag{Name = item});
  //   }
  // }
  context.Products.Add(product);
  context.SaveChanges();
  return Results.Created($"/products/{product.Id}", product.Id);
});



app.MapGet("/product/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
  var product = context.Products.Where(p => p.Id == id).First();

  if (product != null)
  {
    Console.WriteLine("Product found");
    return Results.Ok(product);
  }
  return Results.NotFound();
});

app.MapPut("/product/{id}", ([FromRoute] int id, ProductRequest productRequest, ApplicationDbContext context) =>
{
  var product = context.Products
  .Include(p=>p.Category)
  .Where(p=>p.Id==id).First();

    var category = context.Categories.Where(cat => cat.Id == productRequest.CategoryId).First();

    product.Code = productRequest.Code;
    product.Name = productRequest.Name;
    product.Description = productRequest.Description;
    product.Category = category;

  return Results.Ok();

});


app.MapDelete("/product/{id}", ([FromRoute] int id,  ApplicationDbContext context) =>
{
  var productDelete =  context.Products.Where(p => p.Id == id).First();
  context.Products.Remove(productDelete);
  context.SaveChanges();
  return Results.Ok();
});

app.Run();
