namespace shpop.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
