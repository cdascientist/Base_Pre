using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace MW.Server.Controllers
{
    #region Models

    /// <summary>
    /// Represents the modeldbinit table data structure
    /// </summary>
    public class Modeldbinit
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? Modeldbinittimestamp { get; set; }
        public int? ModelDbInitCatagoricalId { get; set; }
        public string? ModelDbInitCatagoricalName { get; set; }
        public byte[]? ModelDbInitModelData { get; set; }
        public byte[]? Data { get; set; }
        public string? ModelDbInitProductVector { get; set; }
        public string? ModelDbInitServiceVector { get; set; }
    }

    /// <summary>
    /// Represents client information data structure
    /// </summary>
    public class ClientInformation
    {
        public int Id { get; set; }
        public string? ClientFirstName { get; set; }
        public string? ClientLastName { get; set; }
        public string? CleintPhone { get; set; }
        public string? ClientAddress { get; set; }
        public int? CustomerId { get; set; }
        public string? CompanyName { get; set; }
    }

    /// <summary>
    /// Represents client order data structure
    /// </summary>
    public class ClientOrder
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
    }

    /// <summary>
    /// Represents ModelDbInitOperation data structure
    /// </summary>
    public class ModelDbInitOperation
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? OperationsId { get; set; }
        public byte[]? Data { get; set; }
    }

    /// <summary>
    /// Represents ModelDbInitQa data structure
    /// </summary>
    public class ModelDbInitQa
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public byte[]? Data { get; set; }
    }

    /// <summary>
    /// Represents OperationsStage1 data structure
    /// </summary>
    public class OperationsStage1
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? OperationsId { get; set; }
        public int? OperationalId { get; set; }
        public int? CsrOpartationalId { get; set; }
        public int? SalesId { get; set; }
        public int? SubServiceA { get; set; }
        public int? SubServiceB { get; set; }
        public int? SubServiceC { get; set; }
        public int? SubProductA { get; set; }
        public int? SubProductB { get; set; }
        public int? SubProductC { get; set; }
        public string? Data { get; set; }
        public string? OperationsStageOneProductVector { get; set; }
        public string? OperationsStageOneServiceVector { get; set; }
    }

    #endregion

    #region TensorFlow Simulation

    /// <summary>
    /// Configuration settings for model training
    /// </summary>
    public class ModelTrainingConfiguration
    {
        public int Epochs { get; set; }
        public float InitialLearningRate { get; set; }
        public float ConvergenceThreshold { get; set; }
        public int StableEpochsRequired { get; set; }
        public float MinLearningRate { get; set; }
    }

    /// <summary>
    /// Tensor object representation for TF simulation
    /// </summary>
    public class Tensor
    {
        public string Name { get; }
        public object Shape { get; }

        public Tensor(string name, object shape)
        {
            Name = name;
            Shape = shape;
        }
    }

    /// <summary>
    /// Tensor feed item for TF simulation
    /// </summary>
    public class FeedItem
    {
        public object Key { get; }
        public object Value { get; }

        public FeedItem(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// NDArray representation for TF simulation
    /// </summary>
    public class NDArray
    {
        private float[] _data;

        public NDArray(float[] data)
        {
            _data = data;
        }

        public T[] ToArray<T>()
        {
            if (typeof(T) == typeof(float))
            {
                return _data as T[];
            }

            throw new InvalidOperationException("Unsupported array type");
        }
    }

    /// <summary>
    /// TensorFlow Session for machine learning simulation
    /// </summary>
    public class Session : IDisposable
    {
        private readonly Random _random = new Random();

        public Session()
        {
            System.Diagnostics.Debug.WriteLine("TensorFlow Session created");
        }

        public void Dispose()
        {
            // Cleanup code would go here in a real implementation
            System.Diagnostics.Debug.WriteLine("TensorFlow Session disposed");
        }

        public void RunOp(object op, IEnumerable<FeedItem> feedItems = null)
        {
            System.Diagnostics.Debug.WriteLine("TensorFlow Session.run executed");
        }

        public object RunTensor(object tensor, IEnumerable<FeedItem> feedItems = null)
        {
            System.Diagnostics.Debug.WriteLine($"TensorFlow Session.run executed for tensor: {((Tensor)tensor)?.Name ?? "unnamed"}");

            // Return appropriate mock data based on tensor type
            if (tensor is Tensor t)
            {
                if (t.Name == "weight" || t.Name.Contains("W"))
                {
                    float[] weights = new float[5];
                    for (int i = 0; i < weights.Length; i++)
                    {
                        weights[i] = (float)(_random.NextDouble() * 0.02 - 0.01);
                    }
                    return new NDArray(weights);
                }
                else if (t.Name == "bias" || t.Name.Contains("b"))
                {
                    return new NDArray(new float[] { 0.1f });
                }
                else if (t.Name == "predictions" || t.Name.Contains("pred"))
                {
                    return new NDArray(new float[] { 0.5f });
                }
                else if (t.Name == "loss")
                {
                    // Return float instead of double to avoid casting issues
                    return (float)(0.01f + (_random.NextDouble() * 0.01));
                }
                else if (t.Name.Contains("magnitude") || t.Name.Contains("sqrt"))
                {
                    return new NDArray(new float[] { 1.5f });
                }
            }

            // Default return for unspecified tensor types
            return 0.0f;
        }
    }

    /// <summary>
    /// Mock TensorFlow optimizer for machine learning simulation
    /// </summary>
    public class TFOptimizer
    {
        private readonly float _learningRate;

        public TFOptimizer(float learningRate)
        {
            _learningRate = learningRate;
        }

        public object Minimize(object loss)
        {
            // Return mock operation
            return new object();
        }
    }

    /// <summary>
    /// Mock TensorFlow random functions for machine learning simulation
    /// </summary>
    public class TFRandom
    {
        private readonly Random _random = new Random();

        public object Normal(object shape, float mean = 0.0f, float stddev = 0.01f)
        {
            // Simulate return of a weight tensor initialized with normal distribution
            return new Tensor("weight", shape);
        }
    }

    /// <summary>
    /// Mock TensorFlow functionality
    /// </summary>
    public static class TF
    {
        public static readonly object float32 = new object();
        public static TFRandom random = new TFRandom();

        public static class compat
        {
            public static class v1
            {
                public static void disable_eager_execution()
                {
                    System.Diagnostics.Debug.WriteLine("TensorFlow eager execution disabled");
                }
            }
        }

        public static class train
        {
            public static TFOptimizer GradientDescentOptimizer(float learningRate)
            {
                return new TFOptimizer(learningRate);
            }
        }

        public static object Graph()
        {
            return new object();
        }

        public static Tensor placeholder(object dtype, object shape, string name = "")
        {
            return new Tensor(name, shape);
        }

        public static Tensor Variable(object value, string name = "")
        {
            return new Tensor(name, null);
        }

        public static Tensor add(Tensor a, Tensor b)
        {
            return new Tensor($"add_{a.Name}_{b.Name}", null);
        }

        public static Tensor matmul(Tensor a, Tensor b)
        {
            return new Tensor($"matmul_{a.Name}_{b.Name}", null);
        }

        public static object reduce_mean(object tensor, float multiplier = 1.0f)
        {
            return new Tensor("reduce_mean", null);
        }

        public static object square(object tensor)
        {
            return new Tensor("square", null);
        }

        public static object global_variables_initializer()
        {
            return new object();
        }

        public static Tensor zeros(object shape)
        {
            return new Tensor("zeros", shape);
        }
    }

    #endregion

    #region Static Data and Utilities

    /// <summary>
    /// Static class to hold temporary data for testing purposes
    /// </summary>
    public static class TemporaryTestData
    {
        /// <summary>
        /// Temporary static list to simulate a database table for Modeldbinit testing
        /// </summary>
        public static readonly List<Modeldbinit> TemporaryModelDbInits = new List<Modeldbinit>
        {
            new Modeldbinit
            {
                Id = 1,
                CustomerId = 123,
                Modeldbinittimestamp = DateTime.UtcNow,
                ModelDbInitCatagoricalId = 5,
                ModelDbInitCatagoricalName = "Initial Category",
                ModelDbInitModelData = new byte[] { 1, 2, 3, 4 }, // Placeholder binary data
                Data = new byte[] { 5, 6, 7, 8 }, // Placeholder binary data
                ModelDbInitProductVector = "X=1.0, Y=2.0, Z=3.0", // Sample vector data
                ModelDbInitServiceVector = "X=4.0, Y=5.0, Z=6.0" // Sample vector data
            },
            new Modeldbinit
            {
                Id = 2,
                CustomerId = 456,
                Modeldbinittimestamp = DateTime.UtcNow.AddDays(-1),
                ModelDbInitCatagoricalId = 10,
                ModelDbInitCatagoricalName = "Second Category",
                ModelDbInitModelData = new byte[] { 9, 10, 11, 12 }, // Placeholder binary data
                Data = new byte[] { 13, 14, 15, 16 }, // Placeholder binary data
                ModelDbInitProductVector = "X=10.0, Y=11.0, Z=12.0", // Sample vector data
                ModelDbInitServiceVector = "X=13.0, Y=14.0, Z=15.0" // Sample vector data
            }
        };

        /// <summary>
        /// Temporary static list to simulate client information
        /// </summary>
        public static readonly List<ClientInformation> TemporaryClientInformation = new List<ClientInformation>
        {
            new ClientInformation
            {
                Id = 1,
                ClientFirstName = "John",
                ClientLastName = "Doe",
                CleintPhone = "555-1234",
                ClientAddress = "123 Main St",
                CustomerId = 123,
                CompanyName = "Acme Inc."
            },
            new ClientInformation
            {
                Id = 2,
                ClientFirstName = "Jane",
                ClientLastName = "Smith",
                CleintPhone = "555-5678",
                ClientAddress = "456 Elm St",
                CustomerId = 456,
                CompanyName = "XYZ Corp"
            }
        };

        /// <summary>
        /// Temporary static list to simulate client orders
        /// </summary>
        public static readonly List<ClientOrder> TemporaryClientOrders = new List<ClientOrder>
        {
            new ClientOrder
            {
                Id = 1,
                CustomerId = 123,
                OrderId = 1001
            },
            new ClientOrder
            {
                Id = 2,
                CustomerId = 456,
                OrderId = 1002
            }
        };

        /// <summary>
        /// Temporary static list to simulate model initialization operations
        /// </summary>
        public static readonly List<ModelDbInitOperation> TemporaryModelDbInitOperations = new List<ModelDbInitOperation>
        {
            new ModelDbInitOperation
            {
                Id = 1,
                CustomerId = 123,
                OrderId = 1001,
                OperationsId = 5001,
                Data = new byte[] { 21, 22, 23, 24 } // Placeholder binary data
            },
            new ModelDbInitOperation
            {
                Id = 2,
                CustomerId = 456,
                OrderId = 1002,
                OperationsId = 5002,
                Data = new byte[] { 25, 26, 27, 28 } // Placeholder binary data
            }
        };

        /// <summary>
        /// Temporary static list to simulate model QA data
        /// </summary>
        public static readonly List<ModelDbInitQa> TemporaryModelDbInitQas = new List<ModelDbInitQa>
        {
            new ModelDbInitQa
            {
                Id = 1,
                CustomerId = 123,
                OrderId = 1001,
                Data = new byte[] { 31, 32, 33, 34 } // Placeholder binary data
            },
            new ModelDbInitQa
            {
                Id = 2,
                CustomerId = 456,
                OrderId = 1002,
                Data = new byte[] { 35, 36, 37, 38 } // Placeholder binary data
            }
        };

        /// <summary>
        /// Temporary static list to simulate operations stage data
        /// </summary>
        public static readonly List<OperationsStage1> TemporaryOperationsStage1s = new List<OperationsStage1>
        {
            new OperationsStage1
            {
                Id = 1,
                CustomerId = 123,
                OrderId = 1001,
                OperationsId = 5001,
                OperationalId = 6001,
                CsrOpartationalId = 7001,
                SalesId = 8001,
                SubServiceA = 1,
                SubServiceB = 2,
                SubServiceC = 3,
                SubProductA = 4,
                SubProductB = 5,
                SubProductC = 6,
                Data = "Sample data for operations stage 1",
                OperationsStageOneProductVector = "X=1.0, Y=2.0, Z=3.0",
                OperationsStageOneServiceVector = "X=4.0, Y=5.0, Z=6.0"
            },
            new OperationsStage1
            {
                Id = 2,
                CustomerId = 456,
                OrderId = 1002,
                OperationsId = 5002,
                OperationalId = 6002,
                CsrOpartationalId = 7002,
                SalesId = 8002,
                SubServiceA = 7,
                SubServiceB = 8,
                SubServiceC = 9,
                SubProductA = 10,
                SubProductB = 11,
                SubProductC = 12,
                Data = "Sample data for operations stage 2",
                OperationsStageOneProductVector = "X=10.0, Y=11.0, Z=12.0",
                OperationsStageOneServiceVector = "X=13.0, Y=14.0, Z=15.0"
            }
        };

        /// <summary>
        /// Simulated product data for testing
        /// </summary>
        public static readonly List<dynamic> SimulatedSubProducts = new List<dynamic>
        {
            new { Id = 1, ProductName = "Product A", ProductType = "Type 1", Quantity = 10, Price = 99.99, ccvc = 0.15 },
            new { Id = 2, ProductName = "Product B", ProductType = "Type 2", Quantity = 20, Price = 149.99, ccvc = 0.25 },
            new { Id = 3, ProductName = "Product C", ProductType = "Type 1", Quantity = 15, Price = 199.99, ccvc = 0.20 }
        };

        /// <summary>
        /// Simulated service data for testing
        /// </summary>
        public static readonly List<dynamic> SimulatedSubServices = new List<dynamic>
        {
            new { Id = 1, ServiceName = "Service A", ServiceType = "Type 1", Quantity = 5, Price = 299.99, ccvc = 0.30 },
            new { Id = 2, ServiceName = "Service B", ServiceType = "Type 2", Quantity = 10, Price = 399.99, ccvc = 0.35 },
            new { Id = 3, ServiceName = "Service C", ServiceType = "Type 3", Quantity = 8, Price = 599.99, ccvc = 0.40 }
        };
    }

    /// <summary>
    /// Static class to hold runtime memory objects with Add/Get Property Functionality
    /// </summary>
    public static class Jit_Memory_Object
    {
        private static readonly ExpandoObject _dynamicStorage = new ExpandoObject();
        private static readonly dynamic _dynamicObject = _dynamicStorage;
        private static RuntimeMethodHandle _jitMethodHandle;

        /// <summary>
        /// Adds or updates a property in the static dynamic storage
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to store</param>
        public static void AddProperty(string propertyName, object value)
        {
            var dictionary = (IDictionary<string, object>)_dynamicStorage;
            dictionary[propertyName] = value;
        }

        /// <summary>
        /// Retrieves a property from the static dynamic storage
        /// Returns null if the property does not exist
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property, or null if not found</returns>
        public static object? GetProperty(string propertyName)
        {
            var dictionary = (IDictionary<string, object>)_dynamicStorage;
            return dictionary.TryGetValue(propertyName, out var value) ? value : null;
        }

        /// <summary>
        /// Gets the dynamic object for direct access
        /// </summary>
        public static dynamic DynamicObject => _dynamicObject;

        /// <summary>
        /// Sets the JIT method handle
        /// </summary>
        /// <param name="handle">The RuntimeMethodHandle to set</param>
        public static void SetJitMethodHandle(RuntimeMethodHandle handle)
        {
            _jitMethodHandle = handle;
        }

        /// <summary>
        /// Gets the stored JIT method handle
        /// </summary>
        /// <returns>The RuntimeMethodHandle</returns>
        public static RuntimeMethodHandle GetJitMethodHandle()
        {
            return _jitMethodHandle;
        }
    }

    /// <summary>
    /// Orchestrates parallel processing tasks
    /// </summary>
    public class ParallelProcessingOrchestrator
    {
        private readonly ConcurrentDictionary<string, object> _sharedMemory = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Clears shared data
        /// </summary>
        public void ClearSharedData()
        {
            _sharedMemory.Clear();
        }
    }

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class ModelDbInits1Controller : ControllerBase
    {
        private static int _sessionCounter = 0;
        private readonly ConcurrentDictionary<int, Session> _sessions;
        private readonly ParallelProcessingOrchestrator _processingOrchestrator;

        /// <summary>
        /// Constructor for ModelDbInits1Controller
        /// </summary>
        public ModelDbInits1Controller()
        {
            _sessions = new ConcurrentDictionary<int, Session>();
            _processingOrchestrator = new ParallelProcessingOrchestrator();

            // Initialize memory with simulated data
            Jit_Memory_Object.AddProperty("All_SubServices", TemporaryTestData.SimulatedSubServices);
            Jit_Memory_Object.AddProperty("All_SubProducts", TemporaryTestData.SimulatedSubProducts);
        }

        /// <summary>
        /// This endpoint initiates a machine learning implementation process.
        /// It uses static example data instead of database access.
        /// </summary>
        /// <param name="customerID">The ID of the customer for whom to perform the ML implementation</param>
        /// <returns>The final Modeldbinit object representing the results of the ML implementation</returns>
        [HttpPost("Machine_Learning_Implementation_One/{customerID?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Modeldbinit))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Modeldbinit>> Machine_Learning_Implementation_One(int? customerID = null)
        {
            if (!customerID.HasValue)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error: CustomerID is required for ML implementation");
                return BadRequest("CustomerID is required for machine learning implementation");
            }

            // Generate unique session ID for this ML implementation
            var sessionId = Interlocked.Increment(ref _sessionCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Implementation Session {sessionId} for customer {customerID.Value}");

            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Preparing implementation");

                // Initialize model container
                var modelInit = new Modeldbinit();

                // Execute ProcessFactoryOne to initialize the model
                await ProcessFactoryOne(modelInit, customerID.Value, sessionId);

                // Retrieve the Modeldbinit object after ProcessFactoryOne
                var updatedModelInit = TemporaryTestData.TemporaryModelDbInits
                                        .FirstOrDefault(m => m.CustomerId == customerID.Value);

                if (updatedModelInit != null)
                {
                    modelInit = updatedModelInit; // Update the local modelInit object reference
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOne failed to create/find Modeldbinit for customer {customerID.Value}. Cannot proceed.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to initialize model data.");
                }

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ML Implementation completed successfully");

                // Retrieve the final Modeldbinit record
                var finalModelInit = TemporaryTestData.TemporaryModelDbInits
                                    .FirstOrDefault(m => m.CustomerId == customerID.Value);

                if (finalModelInit != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Returning final Modeldbinit record for customer {customerID.Value}");
                    return Ok(finalModelInit);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Final Modeldbinit record not found for customer {customerID.Value} after implementation.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "ML implementation completed, but final model record was not found.");
                }
            }
            catch (Exception ex)
            {
                // Log error and return 500 status code
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Error in ML Implementation: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Stack Trace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error during ML implementation: {ex.Message}");
            }
            finally
            {
                // Cleanup: Remove session if any
                if (_sessions.TryRemove(sessionId, out var session))
                {
                    session?.Dispose();
                }

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Resources cleaned up");
            }
        }

        /// <summary>
        /// Processes data for Model C (ProcessFactoryOne).
        /// Creates or retrieves the Modeldbinit record and associated dependency records.
        /// Uses static example data instead of database access.
        /// </summary>
        private async Task ProcessFactoryOne(Modeldbinit model, int customerID, int sessionId)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryOneActive", true);
            bool isActiveStart = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryOneActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOneActive property value: {isActiveStart}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryOne (Model C)");

            try
            {
                // First ensure Modeldbinit exists in our static test data
                var ML_Model = TemporaryTestData.TemporaryModelDbInits
                                .FirstOrDefault(m => m.CustomerId == customerID);

                if (ML_Model == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] No existing Modeldbinit found for Customer with ID {customerID}. Initializing new model.");

                    // Get the maximum ID from the static test data to assign a new ID
                    var maxId = TemporaryTestData.TemporaryModelDbInits
                                .Count > 0 ? TemporaryTestData.TemporaryModelDbInits.Max(m => m.Id) : 0;

                    ML_Model = new Modeldbinit
                    {
                        CustomerId = customerID,
                        Modeldbinittimestamp = DateTime.UtcNow,
                        Id = maxId + 1, // Use next available ID
                        ModelDbInitCatagoricalId = null,
                        ModelDbInitCatagoricalName = null,
                        ModelDbInitModelData = null,
                        Data = null,
                        ModelDbInitProductVector = null,
                        ModelDbInitServiceVector = null
                    };

                    // Add to static test data
                    TemporaryTestData.TemporaryModelDbInits.Add(ML_Model);
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created new Modeldbinit record with ID {ML_Model.Id} for customer {customerID}");

                    // Create or find associated ClientOrder
                    var clientOrder = TemporaryTestData.TemporaryClientOrders
                                     .FirstOrDefault(c => c.CustomerId == customerID);

                    if (clientOrder == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ClientOrder record for CustomerId: {customerID}");

                        var maxClientOrderId = TemporaryTestData.TemporaryClientOrders
                                            .Count > 0 ? TemporaryTestData.TemporaryClientOrders.Max(c => c.Id) : 0;

                        clientOrder = new ClientOrder
                        {
                            Id = maxClientOrderId + 1,
                            CustomerId = customerID,
                            OrderId = customerID
                        };

                        TemporaryTestData.TemporaryClientOrders.Add(clientOrder);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ClientOrder record with ID {clientOrder.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ClientOrder record found for CustomerId: {customerID}");
                    }

                    // Create or find associated ModelDbInitOperation
                    var operation = TemporaryTestData.TemporaryModelDbInitOperations
                                   .FirstOrDefault(o => o.CustomerId == customerID);

                    if (operation == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ModelDbInitOperation record for CustomerId: {customerID}");

                        var maxOperationId = TemporaryTestData.TemporaryModelDbInitOperations
                                          .Count > 0 ? TemporaryTestData.TemporaryModelDbInitOperations.Max(o => o.Id) : 0;

                        operation = new ModelDbInitOperation
                        {
                            Id = maxOperationId + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            OperationsId = customerID,
                            Data = null
                        };

                        TemporaryTestData.TemporaryModelDbInitOperations.Add(operation);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ModelDbInitOperation record with ID {operation.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ModelDbInitOperation record found for CustomerId: {customerID}");
                    }

                    // Create or find associated ModelDbInitQa
                    var qa = TemporaryTestData.TemporaryModelDbInitQas
                            .FirstOrDefault(q => q.CustomerId == customerID);

                    if (qa == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ModelDbInitQa record for CustomerId: {customerID}");

                        var maxQaId = TemporaryTestData.TemporaryModelDbInitQas
                                    .Count > 0 ? TemporaryTestData.TemporaryModelDbInitQas.Max(q => q.Id) : 0;

                        qa = new ModelDbInitQa
                        {
                            Id = maxQaId + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            Data = null
                        };

                        TemporaryTestData.TemporaryModelDbInitQas.Add(qa);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ModelDbInitQa record with ID {qa.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ModelDbInitQa record found for CustomerId: {customerID}");
                    }

                    // Create or find associated OperationsStage1
                    var operationsStage1 = TemporaryTestData.TemporaryOperationsStage1s
                                          .FirstOrDefault(o => o.CustomerId == customerID);

                    if (operationsStage1 == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new OperationsStage1 record for CustomerId: {customerID}");

                        var maxOperationsStage1Id = TemporaryTestData.TemporaryOperationsStage1s
                                                  .Count > 0 ? TemporaryTestData.TemporaryOperationsStage1s.Max(o => o.Id) : 0;

                        operationsStage1 = new OperationsStage1
                        {
                            Id = maxOperationsStage1Id + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            OperationsId = customerID,
                            OperationalId = customerID,
                            CsrOpartationalId = customerID,
                            SalesId = customerID,
                            SubServiceA = null,
                            SubServiceB = null,
                            SubServiceC = null,
                            SubProductA = null,
                            SubProductB = null,
                            SubProductC = null,
                            Data = null,
                            OperationsStageOneProductVector = null,
                            OperationsStageOneServiceVector = null
                        };

                        TemporaryTestData.TemporaryOperationsStage1s.Add(operationsStage1);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created OperationsStage1 record with ID {operationsStage1.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing OperationsStage1 record found for CustomerId: {customerID}");
                    }

                    // Store references in JIT memory
                    Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                    Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                    Jit_Memory_Object.AddProperty("QaRecord", qa);
                    Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);
                    Jit_Memory_Object.AddProperty("ModelDbinitRecord", ML_Model);
                    Jit_Memory_Object.AddProperty("CustomerId", customerID);

                    // Log JIT Memory Object info
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - ClientOrder ID: {clientOrder?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - Operations ID: {operation?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - QA ID: {qa?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - OperationsStage1 ID: {operationsStage1?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - Modeldbinit ID: {ML_Model?.Id}");

                    // TensorFlow Neural Network training implementation
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting Model C TensorFlow Training");

                    int epochs = 100;
                    float learningRate = 0.0001f;

                    // TensorFlow initialization
                    TF.compat.v1.disable_eager_execution();
                    var g = TF.Graph();
                    using (var sess = new Session())
                    {
                        // Define input shape based on number of Modeldbinit fields used as features
                        var numberOfFeatures = 5; // Id, CustomerId, Timestamp, CatagoricalId, CatagoricalName (bool)
                        var x = TF.placeholder(TF.float32, new[] { -1, numberOfFeatures }, name: "input");
                        var y = TF.placeholder(TF.float32, new[] { -1, 1 }, name: "output");

                        // Create weights and biases for the model
                        var W = TF.Variable(TF.random.Normal(new[] { numberOfFeatures, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = TF.Variable(TF.zeros(new[] { 1 }), name: "bias");

                        // Prepare model operations
                        var predictions = TF.add(TF.matmul(x, W), b);

                        // Define the loss and optimizer
                        var loss = new Tensor("loss", null);
                        var optimizer = TF.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.Minimize(loss);

                        // Create input array with single sample (current Modeldbinit values)
                        var modelId = ML_Model.Id;
                        var modelCustomerId = ML_Model.CustomerId;
                        var modelTimeStamp = ML_Model.Modeldbinittimestamp;
                        var modelCategoricalId = ML_Model.ModelDbInitCatagoricalId;
                        var modelCategoricalName = ML_Model.ModelDbInitCatagoricalName;

                        // Simulate initializing variables
                        sess.RunOp(TF.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C - Data prepared, starting training loop");

                        // Simulate training
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        // Truncated simulation of training loop for processing time
                        for (int epoch = 0; epoch < Math.Min(epochs, 20); epoch++)
                        {
                            // Simulate feed dictionary creation
                            var feedItems = new List<FeedItem>
                            {
                                new FeedItem(x, new float[1, numberOfFeatures]),
                                new FeedItem(y, new float[1, 1])
                            };

                            // Run training operation
                            sess.RunOp(trainOp, feedItems);

                            // Fixed: Using Convert.ToSingle instead of direct cast to handle Double to Single conversion
                            var currentLoss = Convert.ToSingle(sess.RunTensor(loss, feedItems));

                            if (epoch % 5 == 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C - Epoch {epoch}, Loss: {currentLoss:E4}");
                                await Task.Delay(50); // Small delay to simulate computation time
                            }

                            previousLoss = currentLoss;
                        }

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C - Training completed");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C - Final loss: {previousLoss:E4}");

                        // Serialize model data
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting Model C model serialization");
                        using (var memoryStream = new MemoryStream())
                        using (var writer = new BinaryWriter(memoryStream))
                        {
                            // Get weights from session
                            var finalW = sess.RunTensor(W);
                            var wData = ((NDArray)finalW).ToArray<float>();
                            writer.Write(wData.Length);
                            foreach (var w in wData)
                            {
                                writer.Write(w);
                            }
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C weights serialized successfully ({wData.Length} floats)");

                            // Get bias from session
                            var finalB = sess.RunTensor(b);
                            var bData = ((NDArray)finalB).ToArray<float>();
                            writer.Write(bData.Length);
                            foreach (var bias in bData)
                            {
                                writer.Write(bias);
                            }
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C bias serialized successfully ({bData.Length} floats)");

                            // Store the serialized model data in the Modeldbinit object
                            ML_Model.Data = memoryStream.ToArray();
                        }

                        // Update model with serialized data
                        var index = TemporaryTestData.TemporaryModelDbInits.FindIndex(m => m.Id == ML_Model.Id);
                        if (index >= 0)
                        {
                            TemporaryTestData.TemporaryModelDbInits[index] = ML_Model;
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C data saved successfully");
                        }

                        Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);

                        // Verification
                        var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                        var storedModelCData = Jit_Memory_Object.GetProperty("ProcessFactoryOne_Data") as byte[];
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (JIT Memory) - Customer ID: {storedCustomerId}");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (JIT Memory) - Data Size: {storedModelCData?.Length ?? 0} bytes");
                    }
                }
                else // Existing model found
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing Modeldbinit found for Customer ID {customerID}");

                    // Store In Memory At Runtime
                    Jit_Memory_Object.AddProperty("CustomerId", ML_Model.CustomerId);
                    Jit_Memory_Object.AddProperty("ModelDbInitTimeStamp", ML_Model.Modeldbinittimestamp);
                    Jit_Memory_Object.AddProperty("Id", ML_Model.Id);
                    Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);
                    Jit_Memory_Object.AddProperty("ModelDbinitRecord", ML_Model);

                    // Retrieve or create associated records for existing model
                    // Create or find associated ClientOrder
                    var clientOrder = TemporaryTestData.TemporaryClientOrders
                                      .FirstOrDefault(c => c.CustomerId == customerID);

                    if (clientOrder == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ClientOrder record for CustomerId: {customerID} (existing Modeldbinit)");

                        var maxClientOrderId = TemporaryTestData.TemporaryClientOrders
                                              .Count > 0 ? TemporaryTestData.TemporaryClientOrders.Max(c => c.Id) : 0;

                        clientOrder = new ClientOrder
                        {
                            Id = maxClientOrderId + 1,
                            CustomerId = customerID,
                            OrderId = customerID
                        };

                        TemporaryTestData.TemporaryClientOrders.Add(clientOrder);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ClientOrder record with ID {clientOrder.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ClientOrder record found for CustomerId: {customerID}");
                    }

                    // Create or find associated ModelDbInitOperation
                    var operation = TemporaryTestData.TemporaryModelDbInitOperations
                                   .FirstOrDefault(o => o.CustomerId == customerID);

                    if (operation == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ModelDbInitOperation record for CustomerId: {customerID} (existing Modeldbinit)");

                        var maxOperationId = TemporaryTestData.TemporaryModelDbInitOperations
                                          .Count > 0 ? TemporaryTestData.TemporaryModelDbInitOperations.Max(o => o.Id) : 0;

                        operation = new ModelDbInitOperation
                        {
                            Id = maxOperationId + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            OperationsId = customerID,
                            Data = null
                        };

                        TemporaryTestData.TemporaryModelDbInitOperations.Add(operation);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ModelDbInitOperation record with ID {operation.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ModelDbInitOperation record found for CustomerId: {customerID}");
                    }

                    // Create or find associated ModelDbInitQa
                    var qa = TemporaryTestData.TemporaryModelDbInitQas
                            .FirstOrDefault(q => q.CustomerId == customerID);

                    if (qa == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new ModelDbInitQa record for CustomerId: {customerID} (existing Modeldbinit)");

                        var maxQaId = TemporaryTestData.TemporaryModelDbInitQas
                                    .Count > 0 ? TemporaryTestData.TemporaryModelDbInitQas.Max(q => q.Id) : 0;

                        qa = new ModelDbInitQa
                        {
                            Id = maxQaId + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            Data = null
                        };

                        TemporaryTestData.TemporaryModelDbInitQas.Add(qa);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created ModelDbInitQa record with ID {qa.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing ModelDbInitQa record found for CustomerId: {customerID}");
                    }

                    // Create or find associated OperationsStage1
                    var operationsStage1 = TemporaryTestData.TemporaryOperationsStage1s
                                          .FirstOrDefault(o => o.CustomerId == customerID);

                    if (operationsStage1 == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Creating new OperationsStage1 record for CustomerId: {customerID} (existing Modeldbinit)");

                        var maxOperationsStage1Id = TemporaryTestData.TemporaryOperationsStage1s
                                                  .Count > 0 ? TemporaryTestData.TemporaryOperationsStage1s.Max(o => o.Id) : 0;

                        operationsStage1 = new OperationsStage1
                        {
                            Id = maxOperationsStage1Id + 1,
                            CustomerId = customerID,
                            OrderId = customerID,
                            OperationsId = customerID,
                            OperationalId = customerID,
                            CsrOpartationalId = customerID,
                            SalesId = customerID,
                            SubServiceA = null,
                            SubServiceB = null,
                            SubServiceC = null,
                            SubProductA = null,
                            SubProductB = null,
                            SubProductC = null,
                            Data = null,
                            OperationsStageOneProductVector = null,
                            OperationsStageOneServiceVector = null
                        };

                        TemporaryTestData.TemporaryOperationsStage1s.Add(operationsStage1);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created OperationsStage1 record with ID {operationsStage1.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing OperationsStage1 record found for CustomerId: {customerID}");
                    }

                    // Store references in JIT memory
                    Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                    Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                    Jit_Memory_Object.AddProperty("QaRecord", qa);
                    Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);

                    // Log JIT Memory info
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - CustomerId: {customerID}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - ClientOrder ID: {clientOrder?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - Operations ID: {operation?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - QA ID: {qa?.Id}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification - OperationsStage1 ID: {operationsStage1?.Id}");

                    // TensorFlow Neural Network training implementation for existing model
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting Model C TensorFlow Training (existing model)");

                    int epochs = 100;
                    float learningRate = 0.0001f;

                    // TensorFlow initialization
                    TF.compat.v1.disable_eager_execution();
                    var g = TF.Graph();
                    using (var sess = new Session())
                    {
                        // Define input shape based on number of Modeldbinit fields used as features
                        var numberOfFeatures = 5; // Id, CustomerId, Timestamp, CatagoricalId, CatagoricalName (bool)
                        var x = TF.placeholder(TF.float32, new[] { -1, numberOfFeatures }, name: "input");
                        var y = TF.placeholder(TF.float32, new[] { -1, 1 }, name: "output");

                        // Create weights and biases for the model
                        var W = TF.Variable(TF.random.Normal(new[] { numberOfFeatures, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = TF.Variable(TF.zeros(new[] { 1 }), name: "bias");

                        // Prepare model operations
                        var predictions = TF.add(TF.matmul(x, W), b);

                        // Define the loss and optimizer
                        var loss = new Tensor("loss", null);
                        var optimizer = TF.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.Minimize(loss);

                        // Create input array with single sample (current Modeldbinit values)
                        var modelId = ML_Model.Id;
                        var modelCustomerId = ML_Model.CustomerId;
                        var modelTimeStamp = ML_Model.Modeldbinittimestamp;
                        var modelCategoricalId = ML_Model.ModelDbInitCatagoricalId;
                        var modelCategoricalName = ML_Model.ModelDbInitCatagoricalName;

                        // Simulate initializing variables
                        sess.RunOp(TF.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) - Data prepared, starting training loop");

                        // Simulate training
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        // Truncated simulation of training loop for processing time
                        for (int epoch = 0; epoch < Math.Min(epochs, 20); epoch++)
                        {
                            // Simulate feed dictionary creation
                            var feedItems = new List<FeedItem>
                            {
                                new FeedItem(x, new float[1, numberOfFeatures]),
                                new FeedItem(y, new float[1, 1])
                            };

                            // Run training operation
                            sess.RunOp(trainOp, feedItems);

                            // Fixed: Using Convert.ToSingle instead of direct cast to handle Double to Single conversion
                            var currentLoss = Convert.ToSingle(sess.RunTensor(loss, feedItems));

                            if (epoch % 5 == 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) - Epoch {epoch}, Loss: {currentLoss:E4}");
                                await Task.Delay(50); // Small delay to simulate computation time
                            }

                            previousLoss = currentLoss;
                        }

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) - Training completed");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) - Final loss: {previousLoss:E4}");

                        // Serialize model data
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting Model C (Existing) model serialization");
                        using (var memoryStream = new MemoryStream())
                        using (var writer = new BinaryWriter(memoryStream))
                        {
                            // Get weights from session
                            var finalW = sess.RunTensor(W);
                            var wData = ((NDArray)finalW).ToArray<float>();
                            writer.Write(wData.Length);
                            foreach (var w in wData)
                            {
                                writer.Write(w);
                            }
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) weights serialized successfully ({wData.Length} floats)");

                            // Get bias from session
                            var finalB = sess.RunTensor(b);
                            var bData = ((NDArray)finalB).ToArray<float>();
                            writer.Write(bData.Length);
                            foreach (var bias in bData)
                            {
                                writer.Write(bias);
                            }
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) bias serialized successfully ({bData.Length} floats)");

                            // Store the serialized model data in the Modeldbinit object
                            ML_Model.Data = memoryStream.ToArray();
                        }

                        // Update model with serialized data
                        var index = TemporaryTestData.TemporaryModelDbInits.FindIndex(m => m.Id == ML_Model.Id);
                        if (index >= 0)
                        {
                            TemporaryTestData.TemporaryModelDbInits[index] = ML_Model;
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C (Existing) data saved successfully");
                        }

                        Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);

                        // Verification
                        var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                        var storedModelCData = Jit_Memory_Object.GetProperty("ProcessFactoryOne_Data") as byte[];
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (JIT Memory) - Customer ID: {storedCustomerId}");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (JIT Memory) - Data Size: {storedModelCData?.Length ?? 0} bytes");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error in ProcessFactoryOne: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stack Trace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                Jit_Memory_Object.AddProperty("ProcessFactoryOneActive", false);
                bool isActiveAfterExecution = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryOneActive");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOneActive property value after execution: {isActiveAfterExecution}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOne (Model C) finished.");
            }
        }

        // GET endpoint to retrieve a ModelDbInit by ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Modeldbinit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Modeldbinit> GetModeldbinit(int id)
        {
            var modeldbinit = TemporaryTestData.TemporaryModelDbInits.FirstOrDefault(m => m.Id == id);

            if (modeldbinit == null)
            {
                return NotFound();
            }

            return Ok(modeldbinit);
        }

        // GET endpoint to retrieve all ModelDbInits
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Modeldbinit>))]
        public ActionResult<IEnumerable<Modeldbinit>> GetModeldbinits()
        {
            return Ok(TemporaryTestData.TemporaryModelDbInits);
        }
    }
}