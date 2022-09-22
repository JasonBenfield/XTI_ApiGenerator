using Microsoft.AspNetCore.Http;

namespace FakeWebApp.Api;

public sealed class AddProductModel
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Attachment { get; set; }
    public AddSubProductModel SubProduct { get; set; } = new AddSubProductModel();
}

public sealed class AddSubProductModel
{
    public IFormFile[] Attachments { get; set; } = new IFormFile[0];
}
