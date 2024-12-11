using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Base_Pre.Server.Models;
using Tensorflow;
using Tensorflow.NumPy;
using static Tensorflow.Binding;
using Accord.MachineLearning;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.IO;
using Base_Pre.Server.Infrastructure;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Accord.Math.Distances;
using System.Diagnostics;
using Tensorflow.Clustering;

namespace Base_Pre.Server.Models
{


    /// <summary>
    /// Step One - Lets Create the Runtime memory object with Add/Get Property Functionality 
    /// </summary>
    public class Jit_Memory_Object
    {
        private static readonly ExpandoObject _dynamicStorage = new ExpandoObject();
        private static readonly dynamic _dynamicObject = _dynamicStorage;
        private static RuntimeMethodHandle _jitMethodHandle;

        public static void AddProperty(string propertyName, object value)
        {
            var dictionary = (IDictionary<string, object>)_dynamicStorage;
            dictionary[propertyName] = value;
        }

        public static object GetProperty(string propertyName)
        {
            var dictionary = (IDictionary<string, object>)_dynamicStorage;
            return dictionary.TryGetValue(propertyName, out var value) ? value : null;
        }

        public static dynamic DynamicObject => _dynamicObject;

        public static void SetJitMethodHandle(RuntimeMethodHandle handle)
        {
            _jitMethodHandle = handle;
        }

        public static RuntimeMethodHandle GetJitMethodHandle()
        {
            return _jitMethodHandle;
        }
    }



   
}

namespace Base_Pre.Server.Infrastructure
{
    public class ModelTrainingConfiguration
    {
        public int Epochs { get; set; }
        public float InitialLearningRate { get; set; }
        public float ConvergenceThreshold { get; set; }
        public int StableEpochsRequired { get; set; }
        public float MinLearningRate { get; set; }
    }
}

namespace Base_Pre.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelDbInitsController : ControllerBase
    {
        private readonly PrimaryDbContext _context;
        private readonly ConcurrentDictionary<int, Session> _sessions;
        private readonly ParallelProcessingOrchestrator _processingOrchestrator;
        private static int _sessionCounter = 0;

        private List<dynamic> All_SubProducts;
        private List<dynamic> All_SubServices;

        public ModelDbInitsController(PrimaryDbContext context)
        {
            _context = context;
            _sessions = new ConcurrentDictionary<int, Session>();
            _processingOrchestrator = new ParallelProcessingOrchestrator();


            /// <summary>
            /// Step Two 
            /// </summary>

            /// <summary>
            /// Step Three lets store all the products and services in the constructor
            /// </summary>
            /// <summary>
            /// Subsequent Usage:
            /// 5.3
            /// </summary>
            All_SubProducts = new List<dynamic>();
            All_SubServices = new List<dynamic>();

            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] TensorFlow Controller Initialized");

            Task.Run(async () =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Starting parallel data retrieval");

                    // Create options for new context instances
                    var optionsBuilder = new DbContextOptionsBuilder<PrimaryDbContext>();
                    optionsBuilder.UseSqlServer(_context.Database.GetConnectionString());

                    // Function to create a new context instance
                    PrimaryDbContext CreateNewContext()
                    {
                        return new PrimaryDbContext(optionsBuilder.Options);
                    }

                    // Create tasks with separate context instances for each query
                    var tasks = new List<Task>();

                    // Product tasks
                    var productTaskA = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubProductAs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ProductName,
                                p.ProductType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(productTaskA);

                    var productTaskB = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubProductBs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ProductName,
                                p.ProductType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(productTaskB);

                    var productTaskC = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubProductCs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ProductName,
                                p.ProductType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(productTaskC);

                    // Service tasks
                    var serviceTaskA = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubServiceAs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ServiceName,
                                p.ServiceType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(serviceTaskA);

                    var serviceTaskB = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubServiceBs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ServiceName,
                                p.ServiceType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(serviceTaskB);

                    var serviceTaskC = Task.Run(async () =>
                    {
                        using var context = CreateNewContext();
                        return await context.SubServiceCs
                            .AsNoTracking()
                            .Select(p => new {
                                p.Id,
                                p.ServiceName,
                                p.ServiceType,
                                p.Quantity,
                                Price = (float)p.Price,
                                ccvc = (float)p.Ccvc
                            })
                            .ToListAsync();
                    });
                    tasks.Add(serviceTaskC);

                    // Execute all queries in parallel
                    System.Diagnostics.Debug.WriteLine("Executing parallel queries");
                    await Task.WhenAll(tasks);

                    // Process product results
                    var productsA = await productTaskA;
                    var productsB = await productTaskB;
                    var productsC = await productTaskC;

                    All_SubProducts.AddRange(productsA);
                    All_SubProducts.AddRange(productsB);
                    All_SubProducts.AddRange(productsC);

                    System.Diagnostics.Debug.WriteLine($"Found {productsA.Count} SubProduct A records");
                    System.Diagnostics.Debug.WriteLine($"Found {productsB.Count} SubProduct B records");
                    System.Diagnostics.Debug.WriteLine($"Found {productsC.Count} SubProduct C records");

                    // Process service results
                    var servicesA = await serviceTaskA;
                    var servicesB = await serviceTaskB;
                    var servicesC = await serviceTaskC;

                    All_SubServices.AddRange(servicesA);
                    All_SubServices.AddRange(servicesB);
                    All_SubServices.AddRange(servicesC);

                    System.Diagnostics.Debug.WriteLine($"Found {servicesA.Count} SubService A records");
                    System.Diagnostics.Debug.WriteLine($"Found {servicesB.Count} SubService B records");
                    System.Diagnostics.Debug.WriteLine($"Found {servicesC.Count} SubService C records");

                    // Store in runtime memory
                    Jit_Memory_Object.AddProperty("All_SubServices", All_SubServices);
                    Jit_Memory_Object.AddProperty("All_SubProducts", All_SubProducts);

                    // Log final counts
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stored All_SubServices in JIT memory with {All_SubServices.Count} total records");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stored All_SubProducts in JIT memory with {All_SubProducts.Count} total records");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in parallel data retrieval: {ex.Message}");
                    throw;
                }
            }).GetAwaiter().GetResult();
        }





      




        /// <summary>
        /// Subsequent Usage:
        /// 1. This endpoint is called when new product/service data needs to be processed
        /// 2. The implementation runs four models in a specific sequence:
        ///    - Model C runs first (ProcessFactoryOne)
        ///    - Models A and B run in parallel (ProcessFactoryTwo and ProcessFactoryThree)
        ///    - Model D runs last (ProcessFactoryFour)
        /// 3. Results are stored in ModelDbInit object which can be used for:
        ///    - Database persistence
        ///    - Result analysis
        ///    - Performance monitoring
        ///    - Model validation
        /// 4. The endpoint ensures proper resource cleanup through session disposal
        /// 5. All processing steps are logged with timestamps for debugging and monitoring
        /// </summary>
        [HttpPost("Machine_Learning_Implementation_One/{id}/{name}/{customerID}")]
        public async Task<ActionResult<ModelDbInit>> Machine_Learning_Implementation_One(
           int id,
           string name,
           int customerID)
        {
            /// <summary>
            /// Subsequent Usage:
            /// 
            /// </summary>
            // Generate unique session ID for this ML implementation
            var sessionId = Interlocked.Increment(ref _sessionCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Implementation Session {sessionId}");

            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Preparing parallel implementation");

                /// <summary>
                /// Subsequent Usage:
                /// 
                /// </summary>
                // Initialize model containers and results storage
                var modelInit = new ModelDbInit(); // Main container for model results
                var modelAResults = new ConcurrentDictionary<string, object>(); // Thread-safe storage for Model A results
                var modelBResults = new ConcurrentDictionary<string, object>(); // Thread-safe storage for Model B results

                // Create TensorFlow sessions for parallel processing
                var sessionA = new Session(); // TensorFlow session for Model A
                var sessionB = new Session(); // TensorFlow session for Model B

                /// <summary>
                /// Subsequent Usage:
                /// 
                /// </summary>
                // Register sessions in the session manager with unique IDs
                _sessions.TryAdd(sessionId * 2, sessionA);     // Even numbered session for Model A
                _sessions.TryAdd(sessionId * 2 + 1, sessionB); // Odd numbered session for Model B







                /// <summary>
                /// Subsequent Usage:
                /// 1
                /// </summary>
                // Execute Model C (ProcessFactoryOne) independently first
                await ProcessFactoryOne(modelInit, id, name, customerID, sessionId);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting parallel model training");

                /// <summary>
                /// Subsequent Usage:
                /// 2
                /// </summary>
                // Execute Models A and B (ProcessFactoryTwo and ProcessFactoryThree) in parallel
                await Task.WhenAll(
                    ProcessFactoryTwo(modelInit, id, name, customerID, sessionId, sessionA, modelAResults),
                    ProcessFactoryThree(modelInit, id, name, customerID, sessionId, sessionB, modelBResults)
                );


                /// <summary>
                /// Subsequent Usage:
                /// 3
                /// </summary>
                // Execute Model D (ProcessFactoryFour) after parallel processing completes
                // Uses results from Models A and B for final processing
                await ProcessFactoryFour(modelInit, id, name, customerID, sessionId, modelAResults, modelBResults);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ML Implementation completed successfully");
                return Ok(modelInit);
            }
            /// <summary>
            /// Subsequent Usage:
            /// 
            /// </summary>
            catch (Exception ex)
                    {
                        // Log error and return 500 status code
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Error in ML Implementation: {ex.Message}");
                        return StatusCode(500, $"Internal server error: {ex.Message}");
                    }
                    finally
                    {
                        // Cleanup: Remove and dispose TensorFlow sessions
                        if (_sessions.TryRemove(sessionId * 2, out var sessionA))
                            sessionA?.Dispose();
                        if (_sessions.TryRemove(sessionId * 2 + 1, out var sessionB))
                            sessionB?.Dispose();

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Resources cleaned up");
                    }
        }











        /// <summary>
        /// Subsequent Usage:
        /// 4
        /// </summary>
        private async Task ProcessFactoryOne(ModelDbInit model, int id, string name, int customerID, int sessionId)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryOneActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryOneActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOneActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryOne (Model C)");

            try
            {

                /// <summary>
                /// Subsequent Usage:
                /// 5
                /// </summary>
                // First ensure ModelDbInit exists
                var ML_Model = await _context.ModelDbInits
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.CustomerId == customerID);


                /// <summary>
                /// Subsequent Usage:
                /// 5.1
                /// </summary>
                if (ML_Model == null)
                {

                                            /// <summary>
                                            /// Subsequent Usage:
                                            /// 5.2
                                            /// </summary>
                                            // Get the maximum ID from the ModelDbInit table 
                                            var maxId = await _context.ModelDbInits
                                                .MaxAsync(m => (int?)m.Id) ?? 0;

                                            System.Diagnostics.Debug.WriteLine($"No existing model found for Customer with ID {customerID}. Initializing new model.");
                                            ML_Model = new ModelDbInit
                                            {
                                                CustomerId = customerID,
                                                ModelDbInitTimeStamp = DateTime.UtcNow,
                                                Id = maxId + 1, // Use next available ID
                                                ModelDbInitCatagoricalId = null, // Add missing required field
                                                ModelDbInitCatagoricalName = null, // Add missing required field
                                                ModelDbInitModelData = false // Add missing required field with default
                                            };
                                            _context.ModelDbInits.Add(ML_Model);
                                            await _context.SaveChangesAsync();




                                            /// <summary>
                                            /// Subsequent Usage:
                                            /// 5.3
                                            /// Create variables to use ModelDbInit fields as input params
                                            /// </summary>
                                            var modelId = ML_Model.Id;
                                            var modelCustomerId = ML_Model.CustomerId;
                                            var modelTimeStamp = ML_Model.ModelDbInitTimeStamp;
                                            var modelCategoricalId = ML_Model.ModelDbInitCatagoricalId;
                                            var modelCategoricalName = ML_Model.ModelDbInitCatagoricalName;
                                            var modelData = ML_Model.ModelDbInitModelData;









                    /// <summary>
                    /// Subsequent Usage:
                    /// 5.32 Model Basis derived from ModelDbInit fields only
                    /// </summary>
                    await Task.Run(async () =>
                    {
                        try
                        {
                            /// <summary>
                            /// Subsequent Usage:
                            /// 5.33 Structure Training Architecture with ModelDbInit fields
                            /// </summary>
                            int epochs = 100;
                            float learningRate = 0.0001f;

                            tf.compat.v1.disable_eager_execution();
                            var g = tf.Graph();
                            using (var sess = tf.Session(g))
                            {
                                // Define input shape based on number of ModelDbInit fields
                                var numberOfFeatures = 6; // Number of ModelDbInit fields
                                var x = tf.placeholder(tf.float32, shape: new[] { -1, numberOfFeatures }, name: "input");
                                var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                                var W = tf.Variable(tf.random.normal(new[] { numberOfFeatures, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                                var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                                var predictions = tf.add(tf.matmul(x, W), b);
                                var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                                var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                                var trainOp = optimizer.minimize(loss);

                                // Create input array with single sample (current ModelDbInit values)
                                var inputData = new float[1, numberOfFeatures];
                                inputData[0, 0] = modelId;
                                inputData[0, 1] = modelCustomerId.HasValue ? (float)modelCustomerId.Value : 0f; // Fixed nullable int conversion
                                inputData[0, 2] = ((DateTimeOffset)modelTimeStamp).ToUnixTimeSeconds();
                                inputData[0, 3] = modelCategoricalId.HasValue ? (float)modelCategoricalId.Value : 0f;
                                inputData[0, 4] = modelCategoricalName != null ? 1f : 0f;
                                inputData[0, 5] = modelData.HasValue ? (modelData.Value ? 1f : 0f) : 0f;

                                // Create target data (using modelId as example target - adjust as needed)
                                var targetData = new float[1, 1];
                                targetData[0, 0] = modelId; // Example target - adjust based on your needs

                                sess.run(tf.global_variables_initializer());

                                System.Diagnostics.Debug.WriteLine("Model C - Data shapes:");
                                System.Diagnostics.Debug.WriteLine($"Input data shape: {inputData.GetLength(0)} x {inputData.GetLength(1)}");
                                System.Diagnostics.Debug.WriteLine($"Target data shape: {targetData.GetLength(0)} x {targetData.GetLength(1)}");

                                System.Diagnostics.Debug.WriteLine($"Model C - Starting training with learning rate: {learningRate}");
                                float previousLoss = float.MaxValue;
                                int stableCount = 0;

                                for (int epoch = 0; epoch < epochs; epoch++)
                                {
                                    var feedDict = new Dictionary<Tensor, object>
                                        {
                                            { x, inputData },
                                            { y, targetData }
                                        };

                                    var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                                    sess.run(trainOp, feedItems);
                                    var currentLoss = (float)sess.run(loss, feedItems);

                                    if (float.IsNaN(currentLoss) || float.IsInfinity(currentLoss))
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Model C - Training diverged at epoch {epoch}. Stopping training.");
                                        break;
                                    }

                                    if (Math.Abs(previousLoss - currentLoss) < 1e-6)
                                    {
                                        stableCount++;
                                        if (stableCount > 5)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Model C - Training converged at epoch {epoch}");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        stableCount = 0;
                                    }
                                    previousLoss = currentLoss;

                                    if (epoch % 10 == 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Model C - Epoch {epoch}, Loss: {currentLoss:E4}");
                                    }
                                }

                                System.Diagnostics.Debug.WriteLine($"Model C - Training completed");
                                System.Diagnostics.Debug.WriteLine($"Model C - Final loss: {previousLoss:E4}");

                                System.Diagnostics.Debug.WriteLine("Starting model serialization process");
                                using (var memoryStream = new MemoryStream())
                                using (var writer = new BinaryWriter(memoryStream))
                                {
                                    var wNDArray = (NDArray)sess.run(W);
                                    var wData = wNDArray.ToArray<float>();
                                    writer.Write(wData.Length);
                                    foreach (var w in wData)
                                    {
                                        writer.Write(w);
                                    }
                                    System.Diagnostics.Debug.WriteLine("Model weights serialized successfully");

                                    var bNDArray = (NDArray)sess.run(b);
                                    var bData = bNDArray.ToArray<float>();
                                    writer.Write(bData.Length);
                                    foreach (var bias in bData)
                                    {
                                        writer.Write(bias);
                                    }
                                    System.Diagnostics.Debug.WriteLine("Model bias serialized successfully");

                                    ML_Model.Data = memoryStream.ToArray();
                                    await _context.SaveChangesAsync();

                                    Jit_Memory_Object.AddProperty("CustomerId", customerID);
                                    Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);
                                    System.Diagnostics.Debug.WriteLine("Model data saved successfully");

                                    var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                                    var storedModelData = Jit_Memory_Object.GetProperty("ProcessFactoryOne_Data") as byte[];
                                    System.Diagnostics.Debug.WriteLine($"Verification - Customer ID: {storedCustomerId}");
                                    System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {storedModelData?.Length ?? 0} bytes");
                                }
                            }



                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies...
                                                    /// </summary>
                                                    // Create associated records after ModelDbInit exists
                                                    var clientOrder = await _context.ClientOrders
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(c => c.CustomerId == customerID);

                                                    if (clientOrder == null)
                                                    {
                                                        System.Diagnostics.Debug.WriteLine($"Creating new Client_Order record for CustomerId: {customerID}");
                                                        clientOrder = new ClientOrder
                                                        {
                                                            CustomerId = customerID,
                                                            OrderId = customerID
                                                        };
                                                        _context.ClientOrders.Add(clientOrder);
                                                        await _context.SaveChangesAsync();
                                                    }


                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies Continued...
                                                    /// </summary>
                                                    var operation = await _context.ModelDbInitOperations
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(o => o.CustomerId == customerID);

                                                    if (operation == null)
                                                    {
                                                        operation = new ModelDbInitOperation
                                                        {
                                                            CustomerId = customerID,
                                                            OrderId = customerID,
                                                            OperationsId = customerID,
                                                            Data = null
                                                        };
                                                        _context.ModelDbInitOperations.Add(operation);
                                                        await _context.SaveChangesAsync();
                                                    }



                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies Continued...
                                                    /// </summary>
                                                    var qa = await _context.ModelDbInitQas
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(q => q.CustomerId == customerID);

                                                    if (qa == null)
                                                    {
                                                        qa = new ModelDbInitQa
                                                        {
                                                            CustomerId = customerID,
                                                            OrderId = customerID,
                                                            Data = null
                                                        };
                                                        _context.ModelDbInitQas.Add(qa);
                                                        await _context.SaveChangesAsync();
                                                    }



                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies Continued...
                                                    /// </summary>
                                                    var operationsStage1 = await _context.OperationsStage1s
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(o => o.CustomerId == customerID);

                                                    if (operationsStage1 == null)
                                                    {
                                                        operationsStage1 = new OperationsStage1
                                                        {
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
                                                            Data = null
                                                        };
                                                        _context.OperationsStage1s.Add(operationsStage1);
                                                        await _context.SaveChangesAsync();
                                                    }


                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies Continued- Add Blank Objects At Runtime
                                                    /// </summary>
                                                    Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                                                    Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                                                    Jit_Memory_Object.AddProperty("QaRecord", qa);
                                                    Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);


                                                    /// <summary>
                                                    /// Subsequent Usage:
                                                    /// 5.1 Create Subsequent Data Dependencies Continued- Add Blank Objects At Runtime INFO
                                                    /// </summary>
                                                    System.Diagnostics.Debug.WriteLine($"Verification - ClientOrder ID: {clientOrder.Id}");
                                                    System.Diagnostics.Debug.WriteLine($"Verification - Operations ID: {operation.Id}");
                                                    System.Diagnostics.Debug.WriteLine($"Verification - QA ID: {qa.Id}");
                                                    System.Diagnostics.Debug.WriteLine($"Verification - OperationsStage1 ID: {operationsStage1.Id}");
                                                }
                                                catch (Exception ex)
                                                {
                                                    System.Diagnostics.Debug.WriteLine($"Error in Model C training: {ex.Message}");
                                                    throw;
                                                }
                                            });
                }


                /// <summary>
                /// Subsequent Usage:
                /// 6 If ModelDBInit exsists based upon Cusntomer ID
                /// </summary>
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing model found for Customer ID {customerID}");

                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 If ModelDBInit exsists based upon Cusntomer ID - Store In Memory At Runtime
                    /// </summary>
                    Jit_Memory_Object.AddProperty("CustomerId", ML_Model.CustomerId);
                    Jit_Memory_Object.AddProperty("ModelDbInitTimeStamp", ML_Model.ModelDbInitTimeStamp);
                    Jit_Memory_Object.AddProperty("Id", ML_Model.Id);
                    Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);


                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Retrieve based upon customer ID or Create 
                    /// </summary>
                    var clientOrder = await _context.ClientOrders
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CustomerId == customerID);

                    if (clientOrder == null)
                    {
                        clientOrder = new ClientOrder
                        {
                            CustomerId = customerID,
                            OrderId = customerID
                        };
                        _context.ClientOrders.Add(clientOrder);
                        await _context.SaveChangesAsync();
                    }



                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Retrieve based upon customer ID or Create 
                    /// </summary>
                    var operation = await _context.ModelDbInitOperations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.CustomerId == customerID);

                    if (operation == null)
                    {
                        operation = new ModelDbInitOperation
                        {
                            CustomerId = customerID,
                            OrderId = customerID,
                            OperationsId = customerID,
                            Data = null
                        };
                        _context.ModelDbInitOperations.Add(operation);
                        await _context.SaveChangesAsync();
                    }



                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Retrieve based upon customer ID or Create 
                    /// </summary>
                    var qa = await _context.ModelDbInitQas
                        .AsNoTracking()
                        .FirstOrDefaultAsync(q => q.CustomerId == customerID);

                    if (qa == null)
                    {
                        qa = new ModelDbInitQa
                        {
                            CustomerId = customerID,
                            OrderId = customerID,
                            Data = null
                        };
                        _context.ModelDbInitQas.Add(qa);
                        await _context.SaveChangesAsync();
                    }



                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Retrieve based upon customer ID or Create 
                    /// </summary>
                    var operationsStage1 = await _context.OperationsStage1s
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.CustomerId == customerID);

                    if (operationsStage1 == null)
                    {
                        operationsStage1 = new OperationsStage1
                        {
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
                            Data = null
                        };
                        _context.OperationsStage1s.Add(operationsStage1);
                        await _context.SaveChangesAsync();
                    }


                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Store in memory at Runtime  
                    /// </summary>
                    Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                    Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                    Jit_Memory_Object.AddProperty("QaRecord", qa);
                    Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);


                    /// <summary>
                    /// Subsequent Usage:
                    /// 6 Store in memory at Runtime INFO
                    /// </summary>
                    System.Diagnostics.Debug.WriteLine($"Verification - CustomerId: {customerID}");
                    System.Diagnostics.Debug.WriteLine($"Verification - ClientOrder ID: {clientOrder.Id}");
                    System.Diagnostics.Debug.WriteLine($"Verification - Operations ID: {operation.Id}");
                    System.Diagnostics.Debug.WriteLine($"Verification - QA ID: {qa.Id}");
                    System.Diagnostics.Debug.WriteLine($"Verification - OperationsStage1 ID: {operationsStage1.Id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in database operations: {ex.Message}");
                throw;
            }
        }

















      
        private async Task ProcessFactoryTwo(ModelDbInit model, int id, string name, int customerID, int sessionId,
     Session session, ConcurrentDictionary<string, object> results)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryTwoActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryTwoActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryTwoActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryTwo (Model A)");



            /// <summary>
            /// Subsequent Usage: 1
            /// Model A Get the newly gernerated or already generated  ModelDbInit Subsequent Data OperationsStage1 from ModelDBInit->ModelDBInitOperations->OperationsStage1
            /// </summary>
            var operationsStage1Record = Jit_Memory_Object.GetProperty("OperationsStage1Record") as OperationsStage1;
            var stageSubProducts = new List<int?>();

            if (operationsStage1Record != null)
            {
                if (operationsStage1Record.SubProductA.HasValue ||
                    operationsStage1Record.SubProductB.HasValue ||
                    operationsStage1Record.SubProductC.HasValue)
                {
                    stageSubProducts.Add(operationsStage1Record.SubProductA);
                    stageSubProducts.Add(operationsStage1Record.SubProductB);
                    stageSubProducts.Add(operationsStage1Record.SubProductC);
                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Collected {stageSubProducts.Count} SubProduct IDs from OperationsStage1");
                }

                /// <summary>
                /// Subsequent Usage: 2
                /// Model A Filter Products Based Upon Product ID's assigned to Operation Stage One 
                /// </summary>
                var allSubProducts = Jit_Memory_Object.GetProperty("All_SubProducts") as List<dynamic>;
                var filteredProducts = allSubProducts?.Where(p => stageSubProducts.Contains((int)p.Id)).ToList();
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Filtered products count: {filteredProducts?.Count ?? 0}");
                Jit_Memory_Object.AddProperty("FilteredProducts", filteredProducts);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Model A OperationsStage1Record not found in JIT Memory");
            }

            /// <summary>
            /// Subsequent Usage: 3
            /// Model A Initializing Data Clustering Implementation 
            /// </summary>
            System.Diagnostics.Debug.WriteLine("Model A Phase One: Initializing Data Clustering Implementation");
            var FilteredProducts = (List<dynamic>)Jit_Memory_Object.GetProperty("FilteredProducts");
            System.Diagnostics.Debug.WriteLine($"Found {FilteredProducts.Count} for Data Clustering");


            /// <summary>
            /// Subsequent Usage: 4
            /// Model A Extract CCVC point values to create trajectory vector
            /// </summary>
            // Extract the ccvc values
            System.Diagnostics.Debug.WriteLine("Extracting ccvc's for clustering");
            foreach (dynamic product in FilteredProducts)
            {
                System.Diagnostics.Debug.WriteLine($"Product Data:");
                System.Diagnostics.Debug.WriteLine($"ID: {product.Id}");
                System.Diagnostics.Debug.WriteLine($"Product Name: {product.ProductName}");
                System.Diagnostics.Debug.WriteLine($"Product Type: {product.ProductType}");
                System.Diagnostics.Debug.WriteLine($"Quantity: {product.Quantity}");
                System.Diagnostics.Debug.WriteLine($"Price: {product.Price}");
                System.Diagnostics.Debug.WriteLine($"CCVC: {product.ccvc}");
                System.Diagnostics.Debug.WriteLine("-------------------");
            }

            // Create array of CCVC values with explicit type conversion
            var ccvcList = new List<double[]>();
            var ccvcValues = new List<double>();  // For median calculation
            foreach (dynamic product in FilteredProducts)
            {
                double ccvcValue = Convert.ToDouble(product.ccvc);
                ccvcList.Add(new double[] { ccvcValue });
                ccvcValues.Add(ccvcValue);
            }
            double[][] ccvcArray = ccvcList.ToArray();

            // Calculate median of all CCVC values
            var sortedCcvc = ccvcValues.OrderBy(v => v).ToList();
            double medianCcvc = sortedCcvc.Count % 2 == 0
                ? (sortedCcvc[sortedCcvc.Count / 2 - 1] + sortedCcvc[sortedCcvc.Count / 2]) / 2
                : sortedCcvc[sortedCcvc.Count / 2];

            System.Diagnostics.Debug.WriteLine($"Median CCVC value: {medianCcvc:F4}");

            // Define clustering parameters based on median
            int numClusters = 3;
            int numIterations = 100;

            System.Diagnostics.Debug.WriteLine($"Clustering parameters: clusters={numClusters}, iterations={numIterations}");

            // Create k-means algorithm with initialized centroids
            var kmeans_ModelA = new Accord.MachineLearning.KMeans(numClusters)
            {
                MaxIterations = numIterations,
                Distance = new SquareEuclidean()
            };

            // Compute the algorithm
            System.Diagnostics.Debug.WriteLine("Starting k-means clustering");
            var clusters = kmeans_ModelA.Learn(ccvcArray);

            // Store clustering results with median information
            Jit_Memory_Object.AddProperty("ModelA_Clusters", clusters);
            Jit_Memory_Object.AddProperty("ModelA_ClusterCenters", kmeans_ModelA.Centroids);

            // Calculate median from centroids
            var modelACentroids = kmeans_ModelA.Centroids;
            System.Diagnostics.Debug.WriteLine("\nProcessing Model A centroids in 3D space:");

            var centroidValues = modelACentroids.Select(c => c[0]).OrderBy(v => v).ToList();

            // Get individual centroids
            double centroid1 = centroidValues[0];
            double centroid2 = centroidValues[1];
            double centroid3 = centroidValues[2];

            System.Diagnostics.Debug.WriteLine("\nProcessing Model A Median Vector:");

            // Use individual centroids for coordinates
            double x = centroid1;
            double y = centroid2;
            double z = centroid3;

            // Create the vector
            double[] vector = { x, y, z };

            // Calculate the magnitude
            double magnitude = Math.Sqrt(x * x + y * y + z * z);

            // Calculate normalized vector components
            double nx = (magnitude > 0) ? x / magnitude : 0;
            double ny = (magnitude > 0) ? y / magnitude : 0;
            double nz = (magnitude > 0) ? z / magnitude : 0;

            System.Diagnostics.Debug.WriteLine($"Individual Centroid Values: {centroid1:F4}, {centroid2:F4}, {centroid3:F4}");
            System.Diagnostics.Debug.WriteLine($"Vector Coordinates: X={x:F4}, Y={y:F4}, Z={z:F4}");
            System.Diagnostics.Debug.WriteLine($"Vector Magnitude: {magnitude:F4}");
            System.Diagnostics.Debug.WriteLine($"Normalized Vector: nx={nx:F4}, ny={ny:F4}, nz={nz:F4}");

            // Store the vector data
            Jit_Memory_Object.AddProperty("ModelA_MedianVector", vector);
            Jit_Memory_Object.AddProperty("ModelA_MedianVector_Normalized", new double[] { nx, ny, nz });
            Jit_Memory_Object.AddProperty("ModelA_MedianMagnitude", magnitude);

            
            
            
            
            
            System.Diagnostics.Debug.WriteLine("Phase Two: Initializing Neural Network Implementation");
            await Task.Run(() =>
            {
                try
                {
                    int epochs = 100;
                    float learningRate = 0.0001f;

                    // Get unique product names from filtered products
                    var uniqueNames = FilteredProducts
                        .Select(p => (string)p.ProductName)
                        .Distinct()
                        .ToList();

                    // Create input vectors from price and centroid data
                    var inputVectors = new float[FilteredProducts.Count, 4]; // 1 for price, 3 for vector coordinates
                    for (int i = 0; i < FilteredProducts.Count; i++)
                    {
                        inputVectors[i, 0] = (float)FilteredProducts[i].Price / 1000f; // Normalized price
                        inputVectors[i, 1] = (float)x; // Vector x coordinate
                        inputVectors[i, 2] = (float)y; // Vector y coordinate
                        inputVectors[i, 3] = (float)z; // Vector z coordinate
                    }

                    tf.compat.v1.disable_eager_execution();
                    var g = tf.Graph();
                    using (var sess = tf.Session(g))
                    {
                        // Input placeholders
                        var x_data = tf.placeholder(tf.float32, shape: new[] { -1, 4 }, name: "x_data");
                        var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                        // Compute vector magnitude
                        var magnitude = tf.sqrt(tf.reduce_sum(tf.square(x_data), axis: 1));
                        var mag_expanded = tf.expand_dims(magnitude, axis: 1);

                        // Normalize vectors
                        var epsilon = tf.constant(1e-8f);
                        var x_normalized = x_data / (mag_expanded + epsilon);

                        // Apply magnitude-based scaling
                        var scale = mag_expanded / (1.0f + mag_expanded);
                        var x_modified = x_data * scale;

                        // One-hot encoding for product names
                        var nameFeatures = tf.placeholder(tf.float32, shape: new[] { -1, uniqueNames.Count }, name: "names");

                        // Combine modified vectors with name features
                        var combinedInput = tf.concat(new[] { x_modified, nameFeatures }, axis: 1);

                        // Neural network layers
                        var inputDim = 4 + uniqueNames.Count; // Modified vectors (4) + name features
                        var W = tf.Variable(tf.random.normal(new[] { inputDim, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                        var predictions = tf.add(tf.matmul(combinedInput, W), b);
                        var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                        var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.minimize(loss);

                        // Prepare training data
                        var oneHotNames = new float[FilteredProducts.Count, uniqueNames.Count];
                        for (int i = 0; i < FilteredProducts.Count; i++)
                        {
                            var nameIndex = uniqueNames.IndexOf((string)FilteredProducts[i].ProductName);
                            oneHotNames[i, nameIndex] = 1.0f;
                        }

                        var priceData = new float[FilteredProducts.Count, 1];
                        for (int i = 0; i < FilteredProducts.Count; i++)
                        {
                            priceData[i, 0] = (float)FilteredProducts[i].Price / 1000f;
                        }

                        sess.run(tf.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine("Model A - Data shapes:");
                        System.Diagnostics.Debug.WriteLine($"Input vectors shape: {inputVectors.GetLength(0)} x {inputVectors.GetLength(1)}");
                        System.Diagnostics.Debug.WriteLine($"Name features shape: {oneHotNames.GetLength(0)} x {oneHotNames.GetLength(1)}");
                        System.Diagnostics.Debug.WriteLine($"Price data shape: {priceData.GetLength(0)} x {priceData.GetLength(1)}");

                        System.Diagnostics.Debug.WriteLine($"Model A - Starting training with learning rate: {learningRate}");
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            var feedDict = new Dictionary<Tensor, object>
                {
                    { x_data, inputVectors },
                    { nameFeatures, oneHotNames },
                    { y, priceData }
                };

                            var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                            sess.run(trainOp, feedItems);
                            var currentLoss = (float)sess.run(loss, feedItems);

                            // Store magnitude values for analysis
                            if (epoch % 10 == 0)
                            {
                                var magnitudeValues = ((NDArray)sess.run(magnitude, feedItems)).ToArray<float>();
                                results.TryAdd($"Magnitude_Epoch_{epoch}", magnitudeValues);
                            }

                            if (float.IsNaN(currentLoss) || float.IsInfinity(currentLoss))
                            {
                                System.Diagnostics.Debug.WriteLine($"Model A - Training diverged at epoch {epoch}. Stopping training.");
                                break;
                            }

                            if (Math.Abs(previousLoss - currentLoss) < 1e-6)
                            {
                                stableCount++;
                                if (stableCount > 5)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Model A - Training converged at epoch {epoch}");
                                    break;
                                }
                            }
                            else
                            {
                                stableCount = 0;
                            }
                            previousLoss = currentLoss;

                            if (epoch % 10 == 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"Model A - Epoch {epoch}, Loss: {currentLoss:E4}");
                            }

                            results.TryAdd($"Loss_Epoch_{epoch}", currentLoss);
                        }

                        System.Diagnostics.Debug.WriteLine("Starting model serialization process");
                        using (var memoryStream = new MemoryStream())
                        using (var writer = new BinaryWriter(memoryStream))
                        {
                            var wNDArray = (NDArray)sess.run(W);
                            var wData = wNDArray.ToArray<float>();
                            writer.Write(wData.Length);
                            foreach (var w in wData)
                            {
                                writer.Write(w);
                            }
                            System.Diagnostics.Debug.WriteLine("Model weights serialized successfully");

                            var bNDArray = (NDArray)sess.run(b);
                            var bData = bNDArray.ToArray<float>();
                            writer.Write(bData.Length);
                            foreach (var bias in bData)
                            {
                                writer.Write(bias);
                            }
                            System.Diagnostics.Debug.WriteLine("Model bias serialized successfully");

                            // Store magnitude-related data
                            var finalMagnitudeArray = ((NDArray)sess.run(magnitude, new[] { new FeedItem(x_data, inputVectors) })).ToArray<float>();
                            writer.Write(finalMagnitudeArray.Length);
                            foreach (var mag in finalMagnitudeArray)
                            {
                                writer.Write(mag);
                            }

                            model.Data = memoryStream.ToArray(); // Use the model parameter instead of ML_Model
                           

                            Jit_Memory_Object.AddProperty("ModelA_Data", model.Data); // Use model.Data here
                            System.Diagnostics.Debug.WriteLine("Model data saved successfully");

                            var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                            var storedModelAData = Jit_Memory_Object.GetProperty("ModelA_Data") as byte[];
                            System.Diagnostics.Debug.WriteLine($"Verification - Customer ID: {storedCustomerId}");
                            System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {storedModelAData?.Length ?? 0} bytes");
                        }

                        // Store final model state
                        var finalW = sess.run(W);
                        var finalB = sess.run(b);
                        var finalMagnitudes = ((NDArray)sess.run(magnitude, new[] { new FeedItem(x_data, inputVectors) })).ToArray<float>();

                        results.TryAdd("FinalWeights", ((NDArray)finalW).ToArray<float>());
                        results.TryAdd("FinalBias", ((NDArray)finalB).ToArray<float>());
                        results.TryAdd("FinalMagnitudes", finalMagnitudes);
                        results.TryAdd("ProductNames", uniqueNames);

                        // Store additional model data in JIT memory
                        Jit_Memory_Object.AddProperty("ModelA_FinalWeights", ((NDArray)finalW).ToArray<float>());
                        Jit_Memory_Object.AddProperty("ModelA_FinalBias", ((NDArray)finalB).ToArray<float>());
                        Jit_Memory_Object.AddProperty("ModelA_FinalMagnitudes", finalMagnitudes);

                        System.Diagnostics.Debug.WriteLine($"Model A - Training completed");
                        System.Diagnostics.Debug.WriteLine($"Model A - Final loss: {previousLoss:E4}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Model A training: {ex.Message}");
                    throw;
                }
            });
        }

















        private async Task ProcessFactoryThree(ModelDbInit model, int id, string name, int customerID, int sessionId,
      Session session, ConcurrentDictionary<string, object> results)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryThreeActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryThreeActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryThreeActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryThree (Model B)");

            var operationsStage1Record = Jit_Memory_Object.GetProperty("OperationsStage1Record") as OperationsStage1;
            var stageSubService = new List<int?>();

            if (operationsStage1Record != null)
            {
                if (operationsStage1Record.SubServiceA.HasValue ||
                    operationsStage1Record.SubServiceB.HasValue ||
                    operationsStage1Record.SubServiceC.HasValue)
                {
                    stageSubService.Add(operationsStage1Record.SubServiceA);
                    stageSubService.Add(operationsStage1Record.SubServiceB);
                    stageSubService.Add(operationsStage1Record.SubServiceC);
                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Collected {stageSubService.Count} Sub Service IDs from OperationsStage1");
                }

                var allSubServices = Jit_Memory_Object.GetProperty("All_SubServices") as List<dynamic>;
                var filteredServices = allSubServices?.Where(p => stageSubService.Contains((int)p.Id)).ToList();
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Filtered services count: {filteredServices?.Count ?? 0}");
                Jit_Memory_Object.AddProperty("FilteredServices", filteredServices);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Model B OperationsStage1Record not found in JIT Memory");
                return;
            }

            var FilteredServices = (List<dynamic>)Jit_Memory_Object.GetProperty("FilteredServices");
            if (FilteredServices == null || !FilteredServices.Any())
            {
                System.Diagnostics.Debug.WriteLine("No filtered services available for processing");
                return;
            }

            System.Diagnostics.Debug.WriteLine("Model B Phase One: Initializing Data Clustering Implementation");
            System.Diagnostics.Debug.WriteLine($"Found {FilteredServices.Count} for Data Clustering");

            // Create array of CCVC values for clustering
            var ccvcList = new List<double[]>();
            var ccvcValues = new List<double>();

            System.Diagnostics.Debug.WriteLine("Extracting service CCVCs for clustering");
            foreach (dynamic service in FilteredServices)
            {
                System.Diagnostics.Debug.WriteLine($"Service Data:");
                System.Diagnostics.Debug.WriteLine($"ID: {service.Id}");
                System.Diagnostics.Debug.WriteLine($"Service Name: {service.ServiceName}");
                System.Diagnostics.Debug.WriteLine($"Service Type: {service.ServiceType}");
                System.Diagnostics.Debug.WriteLine($"Quantity: {service.Quantity}");
                System.Diagnostics.Debug.WriteLine($"Price: {service.Price}");
                System.Diagnostics.Debug.WriteLine($"CCVC: {service.ccvc}");
                System.Diagnostics.Debug.WriteLine("-------------------");

                double ccvcValue = Convert.ToDouble(service.ccvc);
                ccvcList.Add(new double[] { ccvcValue });
                ccvcValues.Add(ccvcValue);
            }

            double[][] ccvcArray = ccvcList.ToArray();

            // Calculate median of CCVC values
            var sortedCcvc = ccvcValues.OrderBy(v => v).ToList();
            double medianCcvc = sortedCcvc.Count % 2 == 0
                ? (sortedCcvc[sortedCcvc.Count / 2 - 1] + sortedCcvc[sortedCcvc.Count / 2]) / 2
                : sortedCcvc[sortedCcvc.Count / 2];

            System.Diagnostics.Debug.WriteLine($"CCVC values: {string.Join(", ", ccvcValues)}");
            System.Diagnostics.Debug.WriteLine($"Median CCVC value: {medianCcvc:F4}");

            // Define clustering parameters
            int numClusters = 3;
            int numIterations = 100;

            System.Diagnostics.Debug.WriteLine($"Clustering parameters: clusters={numClusters}, iterations={numIterations}");

            // Create k-means algorithm
            var kmeans_ModelB = new Accord.MachineLearning.KMeans(numClusters)
            {
                MaxIterations = numIterations,
                Distance = new SquareEuclidean()
            };

            // Compute the algorithm
            System.Diagnostics.Debug.WriteLine("Starting k-means clustering");
            var clusters = kmeans_ModelB.Learn(ccvcArray);

            // Store clustering results
            Jit_Memory_Object.AddProperty("ModelB_Clusters", clusters);
            Jit_Memory_Object.AddProperty("ModelB_ClusterCenters", kmeans_ModelB.Centroids);

            // Get centroids and sort them
            var centroidValues = kmeans_ModelB.Centroids
                .Select(c => c[0])
                .OrderBy(v => v)
                .ToList();

            System.Diagnostics.Debug.WriteLine("\nProcessing Model B centroids in 3D space:");

            // Use these values for the 3D vector
            double x = centroidValues[0];  // Lowest CCVC centroid
            double y = centroidValues[1];  // Middle CCVC centroid
            double z = centroidValues[2];  // Highest CCVC centroid

            // Create the vector
            double[] vector = { x, y, z };

            // Calculate the magnitude
            double magnitude = Math.Sqrt(x * x + y * y + z * z);

            // Calculate normalized vector components
            double nx = (magnitude > 0) ? x / magnitude : 0;
            double ny = (magnitude > 0) ? y / magnitude : 0;
            double nz = (magnitude > 0) ? z / magnitude : 0;

            System.Diagnostics.Debug.WriteLine($"Individual Centroid Values: {x:F4}, {y:F4}, {z:F4}");
            System.Diagnostics.Debug.WriteLine($"Vector Coordinates: X={x:F4}, Y={y:F4}, Z={z:F4}");
            System.Diagnostics.Debug.WriteLine($"Vector Magnitude: {magnitude:F4}");
            System.Diagnostics.Debug.WriteLine($"Normalized Vector: nx={nx:F4}, ny={ny:F4}, nz={nz:F4}");

            // Store the vector data
            Jit_Memory_Object.AddProperty("ModelB_MedianVector", vector);
            Jit_Memory_Object.AddProperty("ModelB_MedianVector_Normalized", new double[] { nx, ny, nz });
            Jit_Memory_Object.AddProperty("ModelB_MedianMagnitude", magnitude);

            System.Diagnostics.Debug.WriteLine("Phase Two: Initializing Neural Network Implementation");
            await Task.Run(() =>
            {
            try
            {
                int epochs = 100;
                float learningRate = 0.0001f;

                // Get unique service names
                var uniqueNames = FilteredServices
                    .Select(p => (string)p.ServiceName)
                    .Distinct()
                    .ToList();

                // Create input vectors from CCVC and centroid data
                var inputVectors = new float[FilteredServices.Count, 4];
                for (int i = 0; i < FilteredServices.Count; i++)
                {
                    inputVectors[i, 0] = (float)FilteredServices[i].ccvc;  // Use CCVC instead of price
                    inputVectors[i, 1] = (float)x; // Vector x coordinate
                    inputVectors[i, 2] = (float)y; // Vector y coordinate
                    inputVectors[i, 3] = (float)z; // Vector z coordinate
                }

                tf.compat.v1.disable_eager_execution();
                var g = tf.Graph();
                using (var sess = tf.Session(g))
                {
                    // Input placeholders
                    var x_data = tf.placeholder(tf.float32, shape: new[] { -1, 4 }, name: "x_data");
                    var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                    // Compute vector magnitude
                    var magnitude = tf.sqrt(tf.reduce_sum(tf.square(x_data), axis: 1));
                    var mag_expanded = tf.expand_dims(magnitude, axis: 1);

                    // Normalize vectors
                    var epsilon = tf.constant(1e-8f);
                    var x_normalized = x_data / (mag_expanded + epsilon);

                    // Apply magnitude-based scaling
                    var scale = mag_expanded / (1.0f + mag_expanded);
                    var x_modified = x_data * scale;

                    // One-hot encoding for service names
                    var nameFeatures = tf.placeholder(tf.float32, shape: new[] { -1, uniqueNames.Count }, name: "names");

                    // Combine modified vectors with name features
                    var combinedInput = tf.concat(new[] { x_modified, nameFeatures }, axis: 1);

                    // Neural network layers
                    var inputDim = 4 + uniqueNames.Count; // Modified vectors (4) + name features
                    var W = tf.Variable(tf.random.normal(new[] { inputDim, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                    var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                    var predictions = tf.add(tf.matmul(combinedInput, W), b);
                    var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                    var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                    var trainOp = optimizer.minimize(loss);

                    // Prepare training data
                    var oneHotNames = new float[FilteredServices.Count, uniqueNames.Count];
                    for (int i = 0; i < FilteredServices.Count; i++)
                    {
                        var nameIndex = uniqueNames.IndexOf((string)FilteredServices[i].ServiceName);
                        oneHotNames[i, nameIndex] = 1.0f;
                    }

                    var ccvcData = new float[FilteredServices.Count, 1];
                    for (int i = 0; i < FilteredServices.Count; i++)
                    {
                        ccvcData[i, 0] = (float)FilteredServices[i].ccvc;
                    }

                    sess.run(tf.global_variables_initializer());

                    System.Diagnostics.Debug.WriteLine("Model B - Data shapes:");
                    System.Diagnostics.Debug.WriteLine($"Input vectors shape: {inputVectors.GetLength(0)} x {inputVectors.GetLength(1)}");
                    System.Diagnostics.Debug.WriteLine($"Name features shape: {oneHotNames.GetLength(0)} x {oneHotNames.GetLength(1)}");
                    System.Diagnostics.Debug.WriteLine($"CCVC data shape: {ccvcData.GetLength(0)} x {ccvcData.GetLength(1)}");

                    System.Diagnostics.Debug.WriteLine($"Model B - Starting training with learning rate: {learningRate}");
                    float previousLoss = float.MaxValue;
                    int stableCount = 0;

                    for (int epoch = 0; epoch < epochs; epoch++)
                    {
                        var feedDict = new Dictionary<Tensor, object>
                    {
                        { x_data, inputVectors },
                        { nameFeatures, oneHotNames },
                        { y, ccvcData }
                    };

                        var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                        sess.run(trainOp, feedItems);
                        var currentLoss = (float)sess.run(loss, feedItems);

                        // Store magnitude values for analysis
                        if (epoch % 10 == 0)
                        {
                            var magnitudeValues = ((NDArray)sess.run(magnitude, feedItems)).ToArray<float>();
                            results.TryAdd($"Magnitude_Epoch_{epoch}", magnitudeValues);
                        }

                        if (float.IsNaN(currentLoss) || float.IsInfinity(currentLoss))
                        {
                            System.Diagnostics.Debug.WriteLine($"Model B - Training diverged at epoch {epoch}. Stopping training.");
                            break;
                        }

                        if (Math.Abs(previousLoss - currentLoss) < 1e-6)
                        {
                            stableCount++;
                            if (stableCount > 5)
                            {
                                System.Diagnostics.Debug.WriteLine($"Model B - Training converged at epoch {epoch}");
                                break;
                            }
                        }
                        else
                        {
                            stableCount = 0;
                        }
                        previousLoss = currentLoss;

                        if (epoch % 10 == 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"Model B - Epoch {epoch}, Loss: {currentLoss:E4}");
                        }

                        results.TryAdd($"Loss_Epoch_{epoch}", currentLoss);
                    }

                    System.Diagnostics.Debug.WriteLine("Starting model serialization process");
                    using (var memoryStream = new MemoryStream())
                    using (var writer = new BinaryWriter(memoryStream))
                    {
                        var wNDArray = (NDArray)sess.run(W);
                        var wData = wNDArray.ToArray<float>();
                        writer.Write(wData.Length);
                        foreach (var w in wData)
                        {
                            writer.Write(w);
                        }
                        System.Diagnostics.Debug.WriteLine("Model weights serialized successfully");

                        var bNDArray = (NDArray)sess.run(b);
                        var bData = bNDArray.ToArray<float>();
                        writer.Write(bData.Length);
                        foreach (var bias in bData)
                        {
                            writer.Write(bias);
                        }
                        System.Diagnostics.Debug.WriteLine("Model bias serialized successfully");

                        var finalMagnitudeArray = ((NDArray)sess.run(magnitude, new[] { new FeedItem(x_data, inputVectors) })).ToArray<float>();
                        writer.Write(finalMagnitudeArray.Length);
                        foreach (var mag in finalMagnitudeArray)
                        {
                            writer.Write(mag);
                        }

                        model.Data = memoryStream.ToArray();
                        Jit_Memory_Object.AddProperty("ModelB_Data", model.Data);
                        System.Diagnostics.Debug.WriteLine("Model data saved successfully");

                            var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                            var storedModelBData = Jit_Memory_Object.GetProperty("ModelB_Data") as byte[];
                            System.Diagnostics.Debug.WriteLine($"Verification - Customer ID: {storedCustomerId}");
                            System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {storedModelBData?.Length ?? 0} bytes");
                        }

                        // Store final model state
                        var finalW = sess.run(W);
                        var finalB = sess.run(b);
                        var finalMagnitudes = ((NDArray)sess.run(magnitude, new[] { new FeedItem(x_data, inputVectors) })).ToArray<float>();

                        results.TryAdd("FinalWeights", ((NDArray)finalW).ToArray<float>());
                        results.TryAdd("FinalBias", ((NDArray)finalB).ToArray<float>());
                        results.TryAdd("FinalMagnitudes", finalMagnitudes);
                        results.TryAdd("ServiceNames", uniqueNames);

                        // Store additional model data in JIT memory
                        Jit_Memory_Object.AddProperty("ModelB_FinalWeights", ((NDArray)finalW).ToArray<float>());
                        Jit_Memory_Object.AddProperty("ModelB_FinalBias", ((NDArray)finalB).ToArray<float>());
                        Jit_Memory_Object.AddProperty("ModelB_FinalMagnitudes", finalMagnitudes);

                        System.Diagnostics.Debug.WriteLine($"Model B - Training completed");
                        System.Diagnostics.Debug.WriteLine($"Model B - Final loss: {previousLoss:E4}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Model B training: {ex.Message}");
                    throw;
                }
            });
        }











        private async Task ProcessFactoryFour(ModelDbInit model, int id, string name, int customerID, int sessionId,
       ConcurrentDictionary<string, object> modelAResults, ConcurrentDictionary<string, object> modelBResults)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryFourActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryFourActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryFourActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryFour (Model D)");

            // Retrieve OperationsStage1 record
            var operationsStage1Record = Jit_Memory_Object.GetProperty("OperationsStage1Record") as OperationsStage1;
            if (operationsStage1Record == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: OperationsStage1Record not found in JIT Memory");
                return;
            }

            // Retrieve Model A and Model B data
            var modelAData = Jit_Memory_Object.GetProperty("ModelA_Data") as byte[];
            var modelBData = Jit_Memory_Object.GetProperty("ModelB_Data") as byte[];

            if (modelAData == null || modelBData == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: Model A or Model B data not found in JIT Memory");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Retrieved Model A Data Size: {modelAData.Length} bytes");
            System.Diagnostics.Debug.WriteLine($"Retrieved Model B Data Size: {modelBData.Length} bytes");

            // Get CustomerId from JIT Memory
            var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
            if (storedCustomerId == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: CustomerId not found in JIT Memory");
                return;
            }

            // Deserialize and combine models' data
            using (var modelAStream = new MemoryStream(modelAData))
            using (var modelBStream = new MemoryStream(modelBData))
            using (var modelAReader = new BinaryReader(modelAStream))
            using (var modelBReader = new BinaryReader(modelBStream))
            {
                try
                {
                    // Read Model A weights and biases
                    int weightALength = modelAReader.ReadInt32();
                    var weightsA = new float[weightALength];
                    for (int i = 0; i < weightALength; i++)
                    {
                        weightsA[i] = modelAReader.ReadSingle();
                    }

                    int biasALength = modelAReader.ReadInt32();
                    var biasesA = new float[biasALength];
                    for (int i = 0; i < biasALength; i++)
                    {
                        biasesA[i] = modelAReader.ReadSingle();
                    }

                    // Read Model B weights and biases
                    int weightBLength = modelBReader.ReadInt32();
                    var weightsB = new float[weightBLength];
                    for (int i = 0; i < weightBLength; i++)
                    {
                        weightsB[i] = modelBReader.ReadSingle();
                    }

                    int biasBLength = modelBReader.ReadInt32();
                    var biasesB = new float[biasBLength];
                    for (int i = 0; i < biasBLength; i++)
                    {
                        biasesB[i] = modelBReader.ReadSingle();
                    }

                    System.Diagnostics.Debug.WriteLine($"Model A - Weights: {weightALength}, Biases: {biasALength}");
                    System.Diagnostics.Debug.WriteLine($"Model B - Weights: {weightBLength}, Biases: {biasBLength}");

                    await Task.Run(() =>
                    {
                        try
                        {
                            int epochs = 100;
                            float learningRate = 0.0001f;

                            tf.compat.v1.disable_eager_execution();
                            var g = tf.Graph();
                            using (var sess = tf.Session(g))
                            {
                                // Create combined input placeholder with dimensions for both models
                                var combinedInputDim = weightALength + weightBLength;
                                var x = tf.placeholder(tf.float32, shape: new[] { -1, combinedInputDim }, name: "combined_input");
                                var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                                // Combine weights from both models
                                var combinedWeights = new float[weightALength + weightBLength];
                                Array.Copy(weightsA, 0, combinedWeights, 0, weightALength);
                                Array.Copy(weightsB, 0, combinedWeights, weightALength, weightBLength);

                                // Create combined weight variable
                                var W = tf.Variable(
                                    tf.constant(combinedWeights.Select(w => (float)w).ToArray(), shape: new[] { combinedInputDim, 1 }),
                                    name: "combined_weights"
                                );

                                // Combine biases (taking average of both models)
                                var combinedBias = (biasesA.Average() + biasesB.Average()) / 2;
                                var b = tf.Variable(tf.constant(new[] { combinedBias }), name: "combined_bias");

                                // Create combined model
                                var predictions = tf.add(tf.matmul(x, W), b);
                                var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                                var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                                var trainOp = optimizer.minimize(loss);

                                // Prepare combined input data
                                var combinedInputData = new float[3, combinedInputDim];
                                for (int i = 0; i < 3; i++)
                                {
                                    // Fill first part with Model A features
                                    for (int j = 0; j < weightALength; j++)
                                    {
                                        combinedInputData[i, j] = weightsA[j];
                                    }
                                    // Fill second part with Model B features
                                    for (int j = 0; j < weightBLength; j++)
                                    {
                                        combinedInputData[i, weightALength + j] = weightsB[j];
                                    }
                                }

                                // Prepare target data
                                var targetData = new float[3, 1];
                                for (int i = 0; i < 3; i++)
                                {
                                    targetData[i, 0] = (biasesA[0] + biasesB[0]) / 2;
                                }

                                sess.run(tf.global_variables_initializer());

                                System.Diagnostics.Debug.WriteLine("Combined Model - Data shapes:");
                                System.Diagnostics.Debug.WriteLine($"Combined input shape: {combinedInputData.GetLength(0)} x {combinedInputData.GetLength(1)}");
                                System.Diagnostics.Debug.WriteLine($"Target data shape: {targetData.GetLength(0)} x {targetData.GetLength(1)}");

                                System.Diagnostics.Debug.WriteLine($"Combined Model - Starting training with learning rate: {learningRate}");
                                float previousLoss = float.MaxValue;
                                int stableCount = 0;

                                for (int epoch = 0; epoch < epochs; epoch++)
                                {
                                    var feedDict = new Dictionary<Tensor, object>
                            {
                                { x, combinedInputData },
                                { y, targetData }
                            };

                                    var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                                    sess.run(trainOp, feedItems);
                                    var currentLoss = (float)sess.run(loss, feedItems);

                                    if (float.IsNaN(currentLoss) || float.IsInfinity(currentLoss))
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Combined Model - Training diverged at epoch {epoch}. Stopping training.");
                                        break;
                                    }

                                    if (Math.Abs(previousLoss - currentLoss) < 1e-6)
                                    {
                                        stableCount++;
                                        if (stableCount > 5)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Combined Model - Training converged at epoch {epoch}");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        stableCount = 0;
                                    }
                                    previousLoss = currentLoss;

                                    if (epoch % 10 == 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Combined Model - Epoch {epoch}, Loss: {currentLoss:E4}");
                                    }
                                }

                                // Save combined model
                                System.Diagnostics.Debug.WriteLine("Starting combined model serialization");
                                using (var memoryStream = new MemoryStream())
                                using (var writer = new BinaryWriter(memoryStream))
                                {
                                    var finalW = (NDArray)sess.run(W);
                                    var wData = finalW.ToArray<float>();
                                    writer.Write(wData.Length);
                                    foreach (var w in wData)
                                    {
                                        writer.Write(w);
                                    }

                                    var finalB = (NDArray)sess.run(b);
                                    var bData = finalB.ToArray<float>();
                                    writer.Write(bData.Length);
                                    foreach (var bias in bData)
                                    {
                                        writer.Write(bias);
                                    }

                                    model.Data = memoryStream.ToArray();

                                    // Convert model data to Base64 string for storage
                                    var base64ModelData = Convert.ToBase64String(model.Data);

                                    // Update OperationsStage1 record in database
                                    try
                                    {
                                        // Find and update the OperationsStage1 record
                                        var dbOperationsStage1 = _context.OperationsStage1s
                                            .FirstOrDefault(o => o.CustomerId == (int)storedCustomerId);

                                        if (dbOperationsStage1 != null)
                                        {
                                            dbOperationsStage1.Data = base64ModelData;
                                            _context.OperationsStage1s.Update(dbOperationsStage1);
                                            _context.SaveChanges();

                                            System.Diagnostics.Debug.WriteLine($"Successfully updated OperationsStage1 Data field for CustomerId: {storedCustomerId}");
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error: OperationsStage1 record not found for CustomerId: {storedCustomerId}");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Error updating OperationsStage1: {ex.Message}");
                                        throw;
                                    }

                                    Jit_Memory_Object.AddProperty("CombinedModel_Data", model.Data);
                                    System.Diagnostics.Debug.WriteLine("Combined model serialized successfully");
                                    System.Diagnostics.Debug.WriteLine($"Combined Model - Final loss: {previousLoss:E4}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in Combined Model training: {ex.Message}");
                            throw;
                        }
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in model combination: {ex.Message}");
                    throw;
                }
            }
        }










    }




    public class ParallelProcessingOrchestrator
    {
        private readonly ConcurrentDictionary<string, object> _sharedMemory = new ConcurrentDictionary<string, object>();

        public void ClearSharedData()
        {
            _sharedMemory.Clear();
        }
    }
}