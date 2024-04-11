using INTEX.Models.DatabaseModels;

namespace longEX.Models.MachineLearning;
using Microsoft.ML.OnnxRuntime.Tensors;
using INTEX.Models.ViewModels;

public class FraudPredictionInput
{
    private long CustomerAge { get; set; }
    private long TransactionId { get; set; }
    private long OrderedTime24H { get; set; }
    private long OrderAmount { get; set; }
    
    // All following are long representations of booleans with 1 = true
    private long CustomerFromIndia { get; set; }
    private long CustomerFromRussia { get; set; }
    private long CustomerFromUsa { get; set; }
    private long CustomerFromUk { get; set; }
    private long CustomerIsMale { get; set; }
    private long OrderedOnMonday { get; set; }
    private long OrderedOnSaturday { get; set; }
    private long OrderedOnSunday { get; set; }
    private long OrderedOnThursday { get; set; }
    private long OrderedOnTuesday { get; set; }
    private long OrderedOnWednesday { get; set; }
    private long EnteredPin { get; set; }
    private long EnteredTap { get; set; }
    private long OrderedOnline { get; set; }
    private long OrderedPos { get; set; }
    private long BilledFromIndia { get; set; }
    private long BilledFromRussia { get; set; }
    private long BilledFromUsa { get; set; }
    private long BilledFromUk { get; set; }
    private long ShippedToIndia { get; set; }
    private long ShippedToRussia { get; set; }
    private long ShippedToUsa { get; set; }
    private long ShippedToUk { get; set; }
    private long BanksWithHsbc { get; set; }
    private long BanksWithHalifax { get; set; }
    private long BanksWithLloyds { get; set; }
    private long BanksWithMetro { get; set; }
    private long BanksWithMonzo { get; set; }
    private long BanksWithRbs { get; set; }
    private long CardTypeVisa { get; set; }

    public FraudPredictionInput(ConfirmOrderViewModel model)
    {
        Order order = model.LineItems.First().Order!;
        Customer customer = order.Customer;
        Transaction transaction = model.Transaction;
        CustomerAge = DateOnly.FromDateTime(DateTime.Today).Year - customer.BirthDate.Year;
        TransactionId = transaction.Id;
        OrderedTime24H = order.DateTime.Hour;
        OrderAmount = Convert.ToInt64(transaction.Amount);
        CustomerFromIndia = customer.HomeAddress.Country == "India" ? 1 : 0;
        CustomerFromRussia = customer.HomeAddress.Country == "Russia" ? 1 : 0;
        CustomerFromUsa = customer.HomeAddress.Country == "USA" ? 1 : 0;
        CustomerFromUk = customer.HomeAddress.Country == "United Kingdom" ? 1 : 0;
        CustomerIsMale = customer.Gender == "Male" ? 1 : 0;
        OrderedOnMonday = order.DateTime.DayOfWeek == DayOfWeek.Monday ? 1 : 0;
        OrderedOnSaturday = order.DateTime.DayOfWeek == DayOfWeek.Saturday ? 1 : 0;
        OrderedOnSunday = order.DateTime.DayOfWeek == DayOfWeek.Sunday ? 1 : 0;
        OrderedOnThursday = order.DateTime.DayOfWeek == DayOfWeek.Thursday ? 1 : 0;
        OrderedOnTuesday = order.DateTime.DayOfWeek == DayOfWeek.Tuesday ? 1 : 0;
        OrderedOnWednesday = order.DateTime.DayOfWeek == DayOfWeek.Wednesday ? 1 : 0;
        EnteredPin = transaction.EntryMode == "PIN" ? 1 : 0;
        EnteredTap = transaction.EntryMode == "Tap" ? 1 : 0;
        OrderedOnline = order.Type == "Online" ? 1 : 0;
        OrderedPos = order.Type == "POS" ? 1 : 0;
        BilledFromIndia = transaction.BillingAddress.Country == "India" ? 1 : 0;
        BilledFromRussia = transaction.BillingAddress.Country == "Russia" ? 1 : 0;
        BilledFromUsa = transaction.BillingAddress.Country == "USA" ? 1 : 0;
        BilledFromUk = transaction.BillingAddress.Country == "United Kingdom" ? 1 : 0;
        ShippedToIndia = order.ShippingAddress.Country == "India" ? 1 : 0;
        ShippedToRussia = order.ShippingAddress.Country == "Russia" ? 1 : 0;
        ShippedToUsa = order.ShippingAddress.Country == "USA" ? 1 : 0;
        ShippedToUk = order.ShippingAddress.Country == "United Kingdom" ? 1 : 0;
        BanksWithHsbc = transaction.Bank == "HSBC" ? 1 : 0;
        BanksWithHalifax = transaction.Bank == "Halifax" ? 1 : 0;
        BanksWithLloyds = transaction.Bank == "Lloyds" ? 1 : 0;
        BanksWithMetro = transaction.Bank == "Metro" ? 1 : 0;
        BanksWithMonzo = transaction.Bank == "Monzo" ? 1 : 0;
        BanksWithRbs = transaction.Bank == "RBS" ? 1 : 0;
        CardTypeVisa = transaction.CardType == "Visa" ? 1 : 0;
    }

    public Tensor<long> AsTensor()
    {
        long[] data =
        [
            CustomerAge, TransactionId, OrderedTime24H, OrderAmount, 
            CustomerFromIndia, CustomerFromRussia, CustomerFromUsa, CustomerFromUk,
            CustomerIsMale,
            OrderedOnMonday, OrderedOnSaturday, OrderedOnSunday, OrderedOnThursday, OrderedOnTuesday, OrderedOnWednesday,
            EnteredPin, EnteredTap,
            OrderedOnline, OrderedPos,
            BilledFromIndia, BilledFromRussia, BilledFromUsa, BilledFromUk,
            ShippedToIndia, ShippedToRussia, ShippedToUsa, ShippedToUk,
            BanksWithHsbc, BanksWithHalifax, BanksWithLloyds, BanksWithMetro, BanksWithMonzo, BanksWithRbs,
            CardTypeVisa
        ];
        int[] dimensions = [1, 34];
        return new DenseTensor<long>(data, dimensions);
    }
}