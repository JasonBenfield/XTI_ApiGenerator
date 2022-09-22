// Generated Code
namespace FakeWebAppClient;
public sealed partial class AddProductModel
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public FileUpload Attachment { get; set; } = new FileUpload();
    public AddSubProductModel SubProduct { get; set; } = new AddSubProductModel();
}