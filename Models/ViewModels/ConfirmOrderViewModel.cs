using INTEX.Models.DatabaseModels;

namespace INTEX.Models.ViewModels;

public class ConfirmOrderViewModel
{
    public IQueryable<LineItem> LineItems { get; set; }
    public Transaction Transaction { get; set; }
}