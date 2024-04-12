using INTEX.Models.DatabaseModels;

namespace INTEX.Models.ViewModels;

public class ConfirmOrderViewModel
{
    public IQueryable<LineItem> LineItems { get; set; }
    public Order? Order { get; set; }
    public Transaction? Transaction { get; set; }
}