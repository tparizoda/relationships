using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext DbContext;

    public ProductsController(AppDbContext DbContext)
    {
        this.DbContext = DbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto createProduct)
    {
        if (await DbContext.Products.AnyAsync(p => p.Name == createProduct.Name))
            return Conflict("This product already exists");

        var created = DbContext.Products.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = createProduct.Name,
            Price = createProduct.Price,
            CreatedAt = createProduct.CreatedAt,
            ModifiedAt = createProduct.ModifiedAt,
            Status = createProduct.Status
        });

        await DbContext.SaveChangesAsync();

        return Ok(created.Entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        var entity = await DbContext.Products
            .Where(u => u.Id == id)
            .Include(u => u.ProductDetail)
            .FirstOrDefaultAsync();

        if (entity is null)
            return NotFound();

        return Ok(new GetProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            CreatedAt = entity.CreatedAt,
            Status = entity.Status
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var productEntity = DbContext.Products.AsQueryable();

        if (productEntity is null)
            return NotFound();

        var product = await productEntity
            .Select(u => new GetProductDto
            {
                Id = u.Id,
                Name = u.Name,
                Price = u.Price,
                CreatedAt = u.CreatedAt,
                Status = u.Status
            })
            .ToListAsync();

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, UpdateProductDto updateProduct)
    {
        var productEntity = await DbContext.Products
            .FirstOrDefaultAsync(u => u.Id == id);

        if (productEntity is null)
            return NotFound();

        productEntity.Name = updateProduct.Name;
        productEntity.Price = updateProduct.Price;
        productEntity.CreatedAt = updateProduct.CreatedAt;
        productEntity.ModifiedAt = updateProduct.ModifiedAt;
        productEntity.Status = updateProduct.Status;

        await DbContext.SaveChangesAsync();

        return Ok(productEntity.Id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var user = await DbContext.Products.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
            return NotFound();

        DbContext.Products.Remove(user);
        await DbContext.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("{id}/details")]
    public async Task<IActionResult> CreateProductDetails([FromRoute] Guid id, CreateProductDetailDto createProductDetail)
    {
        var product = await DbContext.Products
            .Where(u => u.Id == id)
            .Include(u => u.ProductDetail)
            .FirstOrDefaultAsync();

        Console.WriteLine(product.Name);

        if (product is null)
            return BadRequest($"product with id {id} does not exist");

        if (product.ProductDetail is not null)
            return BadRequest($"product already has product details");

        product.ProductDetail = new ProductDetail
        {
            Description = createProductDetail.Description,
            Color = createProductDetail.Color,
            Material = createProductDetail.Material,
            Weight = createProductDetail.Weight,
            QuantityInStock = createProductDetail.QuantityInStock,
            ManufactureDate = createProductDetail.ManufactureDate,
            ExpiryDate = createProductDetail.ExpiryDate,
            Size = createProductDetail.Size,
            Manufacturer = createProductDetail.Manufacturer,
            CountryOfOrigin = createProductDetail.CountryOfOrigin
        };

        await DbContext.SaveChangesAsync();

        return Ok(product.ProductDetail.Id);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails([FromRoute] Guid id)
    {
        var product = await DbContext.Products
            .Where(u => u.Id == id)
            .Include(u => u.ProductDetail)
            .FirstOrDefaultAsync();

        if (product is null || product.ProductDetail is null)
            return NotFound();


        return Ok(new GetProductDetailDto
        {
            Id = product.ProductDetail.Id,
            Description = product.ProductDetail.Description,
            Color = product.ProductDetail.Color,
            Material = product.ProductDetail.Material,
            Weight = product.ProductDetail.Weight,
            QuantityInStock = product.ProductDetail.QuantityInStock,
            ManufactureDate = product.ProductDetail.ManufactureDate,
            ExpiryDate = product.ProductDetail.ExpiryDate,
            Size = product.ProductDetail.Size,
            Manufacturer = product.ProductDetail.Manufacturer,
            CountryOfOrigin = product.ProductDetail.CountryOfOrigin
        });

    }

    [HttpPut("{id}/details")]
    public async Task<IActionResult> UpdateDetails([FromRoute] Guid id, UpdateProductDetailDto detailDto)
    {
        var product = await DbContext.Products
           .Where(u => u.Id == id)
           .Include(u => u.ProductDetail)
           .FirstOrDefaultAsync();

        if (product is null || product.ProductDetail is null)
            return NotFound();

        product.ProductDetail.Description = detailDto.Description;
        product.ProductDetail.Color = detailDto.Color;
        product.ProductDetail.Material = detailDto.Material;
        product.ProductDetail.Weight = detailDto.Weight;
        product.ProductDetail.QuantityInStock = detailDto.QuantityInStock;
        product.ProductDetail.ManufactureDate = detailDto.ManufactureDate;
        product.ProductDetail.ExpiryDate = detailDto.ExpiryDate;
        product.ProductDetail.Size = detailDto.Size;
        product.ProductDetail.Manufacturer = detailDto.Manufacturer;
        product.ProductDetail.CountryOfOrigin = detailDto.CountryOfOrigin;

        await DbContext.SaveChangesAsync();

        return Ok(product.ProductDetail.Id);
    }

    [HttpDelete("{id}/details")]
    public async Task<IActionResult> DeleteDetails([FromRoute] Guid id)
    {
        var detail = await DbContext.ProductDetails.FirstOrDefaultAsync(u => u.Id == id);
        if (detail is null)
            return NotFound();

        DbContext.ProductDetails.Remove(detail);
        await DbContext.SaveChangesAsync();

        return Ok();
    }


    private EProductStatus ConvertEntity(EProductStatusDto status)
   => status switch
   {
       EProductStatusDto.Inactive => EProductStatus.Inactive,
       EProductStatusDto.Soldout => EProductStatus.Soldout,
       _ => EProductStatus.Active,
   };

}