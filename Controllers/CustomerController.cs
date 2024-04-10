using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ML.OnnxRuntime;
using Microsoft.AspNetCore.Http;
using Microsoft.ML.OnnxRuntime.Tensors;
using SQLitePCL;

namespace INTEX.Controllers;

public class CustomerController : Controller
{
    private readonly IRepo _repo;

    private readonly fraudContext = _context;
    private readonly InferenceSession _session;
    private readonly ILogger<CustomerController> _logger;
    private ILogger<CustomerController>? logger;
    private object _context;

    public CustomerController(IRepo repo, ApplicationDbContext context)
    {
        _repo = repo;
        _context = context;
        _logger = logger;

        try
        {
            _session = new InferenceSession("C:\\Users\\andre\\Source\\Repos\\INTEX\\fraudModel_pickle.onnx");
            _logger.LogInformation("ONNX model loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading the ONNX model: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult Cart(LineItem[] lineItems)
    {
        Order order = new Order();
        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }




    [HttpPost]
    public IActionResult Predict(
        int age, float time, float amount, string countryOfResidence, 
        string gender, string dayOfWeek, string entryMode, 
        string typeOfTransaction, string countryOfTransaction, 
        string shippingAddress, string bank, string typeOfCard)
    {
        Order order = new Order();


        var record = _repo.Orders.ToList();
        var predictions = new List<OrderPrediction>();


        // Convert string inputs into their respective dummy variables
        var class_type_dict = new Dictionary<int, string>
        {
            { 0, "Not Fraud" },
            { 1, "Fraud" }
        };


        class PredictionParams
    {
        
    }


        ['age_45', 'time_dummy', 'country_america']

        _repo.PredictionParamsFromOrder(order)




        try
        {
            var input = new List<float> { age, time, amount };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            };

            using (var results = _session.Run(inputs)) // makes the prediction with the inputs from the form (i.e. class_type 1-7)
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                if (prediction != null && prediction.Length > 0)
                {
                    // Use the prediction to get the animal type from the dictionary
                    var fraudResult = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                    ViewBag.Prediction = fraudResult;
                }
                else
                {
                    ViewBag.Prediction = "Error: Unable to make a prediction.";
                }
            }

            _logger.LogInformation("Prediction executed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during prediction: {ex.Message}");
            ViewBag.Prediction = "Error during prediction.";
        }

        return View("Index");
    }

    public IActionResult ShowPredictions()
    {
        var records = _repo.Whatever();

        var records = _context.zoo_animals.ToList();  // Fetch all records
        var predictions = new List<FraudPrediction>();  // Your ViewModel for the view

        // Dictionary mapping the numeric prediction to an animal type
        var class_type_dict = new Dictionary<int, string>
            {
                { 1, "mammal" },
                { 2, "bird" },
                { 3, "reptile" },
                { 4, "fish" },
                { 5, "amphibian" },
                { 6, "bug" },
                { 7, "invertebrate" }
            };

        foreach (var record in records)
        {
            var input = new List<float>
                {
                    record.Hair, record.Feathers, record.Eggs, record.Milk,
                    record.Airborne, record.Aquatic, record.Predator, record.Toothed,
                    record.Backbone, record.Breathes, record.Venomous, record.Fins,
                    record.Legs, record.Tail, record.Domestic, record.Catsize
                };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

            string predictionResult;
            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                predictionResult = prediction != null && prediction.Length > 0 ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
            }

            predictions.Add(new AnimalPrediction { Animal = record, Prediction = predictionResult }); // Adds the animal information and prediction for that animal to AnimalPrediction viewmodel
        }

        return View(predictions);
    }




    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult OrderConfirmation(int orderId)
    {
        return View();
    }
    

    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult CustomerInfo()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult CustomerInfo(Customer customer)
    {
        return RedirectToAction("Index", "Home");
    }
}