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
            All_SubProducts = new List<dynamic>();
            All_SubServices = new List<dynamic>();

            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] TensorFlow Controller Initialized");

            Task.Run(async () =>
            {
                try
                {
                    /// <summary>
                    /// Step Four Lets capture the data based upon the current DB context 
                    /// </summary>
                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubProduct A data");
                    var All_subproductsA = await _context.SubProductAs
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
                    All_SubProducts.AddRange(All_subproductsA);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subproductsA.Count} SubProduct A ALL records");

                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubProduct B data");
                    var All_subproductsB = await _context.SubProductBs
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
                    All_SubProducts.AddRange(All_subproductsB);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subproductsB.Count} SubProduct B ALL records");

                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubProduct C data");
                    var All_subproductsC = await _context.SubProductCs
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
                    All_SubProducts.AddRange(All_subproductsC);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subproductsC.Count} SubProduct C ALL records");

                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubService A data");
                    var All_subservicesA = await _context.SubServiceAs
                        .AsNoTracking()
                        .Select(p => new {
                            p.Id,
                            p.ServiceName,
                            p.ServiceType,
                            p.Quantity,
                            Price = (float)p.Price
                        })
                        .ToListAsync();
                    All_SubServices.AddRange(All_subservicesA);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subservicesA.Count} SubService A ALL records");

                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubService B data");
                    var All_subservicesB = await _context.SubServiceBs
                        .AsNoTracking()
                        .Select(p => new {
                            p.Id,
                            p.ServiceName,
                            p.ServiceType,
                            p.Quantity,
                            Price = (float)p.Price
                        })
                        .ToListAsync();
                    All_SubServices.AddRange(All_subservicesB);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subservicesB.Count} SubService B ALL records");

                    System.Diagnostics.Debug.WriteLine("Fetching ALL SubService C data");
                    var All_subservicesC = await _context.SubServiceCs
                        .AsNoTracking()
                        .Select(p => new {
                            p.Id,
                            p.ServiceName,
                            p.ServiceType,
                            p.Quantity,
                            Price = (float)p.Price
                        })
                        .ToListAsync();
                    All_SubServices.AddRange(All_subservicesC);
                    System.Diagnostics.Debug.WriteLine($"Found {All_subservicesC.Count} SubService C ALL records");


                    /// <summary>
                    /// Step Five Lets Store in Runtim Memory
                    /// </summary>
                   Jit_Memory_Object.AddProperty("All_SubServices", All_SubServices);
                   Jit_Memory_Object.AddProperty("All_SubProducts", All_SubProducts);

                    // Add confirmation debug writes
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stored All_SubServices in JIT memory with {All_SubServices.Count} total records");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stored All_SubProducts in JIT memory with {All_SubProducts.Count} total records");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error fetching data: {ex.Message}");
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
            // Generate unique session ID for this ML implementation
            var sessionId = Interlocked.Increment(ref _sessionCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Implementation Session {sessionId}");

            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Preparing parallel implementation");

                // Initialize model containers and results storage
                var modelInit = new ModelDbInit(); // Main container for model results
                var modelAResults = new ConcurrentDictionary<string, object>(); // Thread-safe storage for Model A results
                var modelBResults = new ConcurrentDictionary<string, object>(); // Thread-safe storage for Model B results

                // Create TensorFlow sessions for parallel processing
                var sessionA = new Session(); // TensorFlow session for Model A
                var sessionB = new Session(); // TensorFlow session for Model B

                // Register sessions in the session manager with unique IDs
                _sessions.TryAdd(sessionId * 2, sessionA);     // Even numbered session for Model A
                _sessions.TryAdd(sessionId * 2 + 1, sessionB); // Odd numbered session for Model B

                // Execute Model C (ProcessFactoryOne) independently first
                await ProcessFactoryOne(modelInit, id, name, customerID, sessionId);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting parallel model training");

                // Execute Models A and B (ProcessFactoryTwo and ProcessFactoryThree) in parallel
                await Task.WhenAll(
                    ProcessFactoryTwo(modelInit, id, name, customerID, sessionId, sessionA, modelAResults),
                    ProcessFactoryThree(modelInit, id, name, customerID, sessionId, sessionB, modelBResults)
                );

                // Execute Model D (ProcessFactoryFour) after parallel processing completes
                // Uses results from Models A and B for final processing
                await ProcessFactoryFour(modelInit, id, name, customerID, sessionId, modelAResults, modelBResults);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ML Implementation completed successfully");
                return Ok(modelInit);
            }
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












        private async Task ProcessFactoryOne(ModelDbInit model, int id, string name, int customerID, int sessionId)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryOneActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryOneActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOneActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryOne (Model C)");

            try
            {
                // First ensure ModelDbInit exists
                var ML_Model = await _context.ModelDbInits
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.CustomerId == customerID);

                if (ML_Model == null)
                {

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
                    var allSubProducts = (List<dynamic>)Jit_Memory_Object.GetProperty("All_SubProducts");
                    var allSubServices = (List<dynamic>)Jit_Memory_Object.GetProperty("All_SubServices");

                    var allNames = allSubProducts.Select(p => p.ProductName)
                        .Concat(allSubServices.Select(s => s.ServiceName))
                        .ToArray();

                    var allPrices = allSubProducts.Select(p => (float)p.Price)
                        .Concat(allSubServices.Select(s => (float)s.Price))
                        .ToArray();

                    System.Diagnostics.Debug.WriteLine($"Total items for processing: {allNames.Length}");

                    await Task.Run(async () =>
                    {
                        try
                        {
                            int epochs = 100;
                            float learningRate = 0.0001f;

                            tf.compat.v1.disable_eager_execution();
                            var g = tf.Graph();
                            using (var sess = tf.Session(g))
                            {
                                var x = tf.placeholder(tf.float32, shape: new[] { -1, allNames.Length + 1 }, name: "input");
                                var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                                var W = tf.Variable(tf.random.normal(new[] { allNames.Length + 1, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                                var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                                var predictions = tf.add(tf.matmul(x, W), b);
                                var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                                var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                                var trainOp = optimizer.minimize(loss);

                                var priceData = new float[allPrices.Length, 1];
                                for (int i = 0; i < allPrices.Length; i++)
                                {
                                    priceData[i, 0] = allPrices[i];
                                }

                                var oneHotNames = new float[allNames.Length, allNames.Length];
                                for (int i = 0; i < allNames.Length; i++)
                                {
                                    oneHotNames[i, i] = 1.0f;
                                }

                                var inputData = new float[allNames.Length, allNames.Length + 1];
                                for (int i = 0; i < allNames.Length; i++)
                                {
                                    inputData[i, 0] = allPrices[i];
                                    for (int j = 0; j < allNames.Length; j++)
                                    {
                                        inputData[i, j + 1] = oneHotNames[i, j];
                                    }
                                }

                                sess.run(tf.global_variables_initializer());

                                System.Diagnostics.Debug.WriteLine("Model C - Data shapes:");
                                System.Diagnostics.Debug.WriteLine($"Input data shape: {inputData.GetLength(0)} x {inputData.GetLength(1)}");
                                System.Diagnostics.Debug.WriteLine($"Price data shape: {priceData.GetLength(0)} x {priceData.GetLength(1)}");
                                System.Diagnostics.Debug.WriteLine($"One-hot encoding shape: {oneHotNames.GetLength(0)} x {oneHotNames.GetLength(1)}");

                                System.Diagnostics.Debug.WriteLine($"Model C - Starting training with learning rate: {learningRate}");
                                float previousLoss = float.MaxValue;
                                int stableCount = 0;

                                for (int epoch = 0; epoch < epochs; epoch++)
                                {
                                    var feedDict = new Dictionary<Tensor, object>
                            {
                                { x, inputData },
                                { y, priceData }
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

                            Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                            Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                            Jit_Memory_Object.AddProperty("QaRecord", qa);
                            Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);

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
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Existing model found for Customer ID {customerID}");

                    Jit_Memory_Object.AddProperty("CustomerId", ML_Model.CustomerId);
                    Jit_Memory_Object.AddProperty("ModelDbInitTimeStamp", ML_Model.ModelDbInitTimeStamp);
                    Jit_Memory_Object.AddProperty("Id", ML_Model.Id);
                    Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", ML_Model.Data);

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

                    Jit_Memory_Object.AddProperty("ClientOrderRecord", clientOrder);
                    Jit_Memory_Object.AddProperty("OperationsRecord", operation);
                    Jit_Memory_Object.AddProperty("QaRecord", qa);
                    Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1);

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

            System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Retrieving OperationsStage1 Record from JIT Memory");
            var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
            var storedData = Jit_Memory_Object.GetProperty("ProcessFactoryOne_Data") as byte[];

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved stored ProcessFactoryOne_Data CustomerId: {storedCustomerId}");
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved stored ProcessFactoryOne_Data size: {storedData?.Length ?? 0} bytes");

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

                var allSubProducts = Jit_Memory_Object.GetProperty("All_SubProducts") as List<dynamic>;
                var filteredProducts = allSubProducts?.Where(p => stageSubProducts.Contains((int)p.Id)).ToList();
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Filtered products count: {filteredProducts?.Count ?? 0}");
                Jit_Memory_Object.AddProperty("FilteredProducts", filteredProducts);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("OperationsStage1Record not found in JIT Memory");
            }

            System.Diagnostics.Debug.WriteLine("Phase One: Initializing Data Clustering Implementation");
            var FilteredProducts = (List<dynamic>)Jit_Memory_Object.GetProperty("FilteredProducts");
            System.Diagnostics.Debug.WriteLine($"Found {FilteredProducts.Count} for Data Clustering");

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
                    var uniqueNames = new List<string> { "name1", "name2", "name3" };

                    tf.compat.v1.disable_eager_execution();
                    var g = tf.Graph();
                    using (var sess = tf.Session(g))
                    {
                        var x = tf.placeholder(tf.float32, shape: new[] { -1, 4 }, name: "input");
                        var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                        var W = tf.Variable(tf.random.normal(new[] { 4, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                        var predictions = tf.add(tf.matmul(x, W), b);
                        var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                        var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.minimize(loss);

                        var priceData = new float[,] {
                    { 100f/1000f },
                    { 200f/1000f },
                    { 300f/1000f }
                };

                        var oneHotNames = new float[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            oneHotNames[i, i] = 1.0f;
                        }

                        var inputData = new float[3, 4];
                        for (int i = 0; i < 3; i++)
                        {
                            inputData[i, 0] = priceData[i, 0];
                            for (int j = 0; j < 3; j++)
                            {
                                inputData[i, j + 1] = oneHotNames[i, j];
                            }
                        }

                        sess.run(tf.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine("Model A - Data shapes:");
                        System.Diagnostics.Debug.WriteLine($"Input data shape: {string.Join(",", inputData.GetLength(0))} x {string.Join(",", inputData.GetLength(1))}");
                        System.Diagnostics.Debug.WriteLine($"Price data shape: {string.Join(",", priceData.GetLength(0))} x {string.Join(",", priceData.GetLength(1))}");

                        System.Diagnostics.Debug.WriteLine($"Model A - Starting training with learning rate: {learningRate}");
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            var feedDict = new Dictionary<Tensor, object>
                    {
                        { x, inputData },
                        { y, priceData }
                    };

                            var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                            sess.run(trainOp, feedItems);
                            var currentLoss = (float)sess.run(loss, feedItems);

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

                        var finalW = sess.run(W);
                        var finalB = sess.run(b);
                        results.TryAdd("FinalWeights", finalW.ToArray());
                        results.TryAdd("FinalBias", finalB.ToArray());

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
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("OperationsStage1Record not found in JIT Memory");
            }

            await Task.Run(() =>
            {
                try
                {
                    int epochs = 100;
                    float learningRate = 0.0001f;
                    var uniqueNames = new List<string> { "name1", "name2", "name3" };

                    tf.compat.v1.disable_eager_execution();
                    var g = tf.Graph();
                    using (var sess = tf.Session(g))
                    {
                        var x = tf.placeholder(tf.float32, shape: new[] { -1, 4 }, name: "input");
                        var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                        var W = tf.Variable(tf.random.normal(new[] { 4, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                        var predictions = tf.add(tf.matmul(x, W), b);
                        var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                        var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.minimize(loss);

                        var priceData = new float[,] {
                    { 150f/1000f },
                    { 250f/1000f },
                    { 350f/1000f }
                };

                        var oneHotNames = new float[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            oneHotNames[i, (i + 1) % 3] = 1.0f;
                        }

                        var inputData = new float[3, 4];
                        for (int i = 0; i < 3; i++)
                        {
                            inputData[i, 0] = priceData[i, 0];
                            for (int j = 0; j < 3; j++)
                            {
                                inputData[i, j + 1] = oneHotNames[i, j];
                            }
                        }

                        sess.run(tf.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine("Model B - Data shapes:");
                        System.Diagnostics.Debug.WriteLine($"Input data shape: {string.Join(",", inputData.GetLength(0))} x {string.Join(",", inputData.GetLength(1))}");
                        System.Diagnostics.Debug.WriteLine($"Price data shape: {string.Join(",", priceData.GetLength(0))} x {string.Join(",", priceData.GetLength(1))}");

                        System.Diagnostics.Debug.WriteLine($"Model B - Starting training with learning rate: {learningRate}");
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            var feedDict = new Dictionary<Tensor, object>
                    {
                        { x, inputData },
                        { y, priceData }
                    };

                            var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                            sess.run(trainOp, feedItems);
                            var currentLoss = (float)sess.run(loss, feedItems);

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

                        var finalW = sess.run(W);
                        var finalB = sess.run(b);
                        results.TryAdd("FinalWeights", finalW.ToArray());
                        results.TryAdd("FinalBias", finalB.ToArray());

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

            await Task.Run(() =>
            {
                try
                {
                    int epochs = 100;
                    float learningRate = 0.0001f;
                    var uniqueNames = new List<string> { "name1", "name2", "name3" };

                    tf.compat.v1.disable_eager_execution();
                    var g = tf.Graph();
                    using (var sess = tf.Session(g))
                    {
                        var x = tf.placeholder(tf.float32, shape: new[] { -1, 4 }, name: "input");
                        var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                        var W = tf.Variable(tf.random.normal(new[] { 4, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                        var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                        var predictions = tf.add(tf.matmul(x, W), b);
                        var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                        var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                        var trainOp = optimizer.minimize(loss);

                        var priceData = new float[,] {
                    { 175f/1000f },
                    { 275f/1000f },
                    { 375f/1000f }
                };

                        var oneHotNames = new float[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            oneHotNames[i, (i + 3) % 3] = 1.0f;
                        }

                        var inputData = new float[3, 4];
                        for (int i = 0; i < 3; i++)
                        {
                            inputData[i, 0] = priceData[i, 0];
                            for (int j = 0; j < 3; j++)
                            {
                                inputData[i, j + 1] = oneHotNames[i, j];
                            }
                        }

                        sess.run(tf.global_variables_initializer());

                        System.Diagnostics.Debug.WriteLine("Model D - Data shapes:");
                        System.Diagnostics.Debug.WriteLine($"Input data shape: {string.Join(",", inputData.GetLength(0))} x {string.Join(",", inputData.GetLength(1))}");
                        System.Diagnostics.Debug.WriteLine($"Price data shape: {string.Join(",", priceData.GetLength(0))} x {string.Join(",", priceData.GetLength(1))}");

                        System.Diagnostics.Debug.WriteLine($"Model D - Starting training with learning rate: {learningRate}");
                        float previousLoss = float.MaxValue;
                        int stableCount = 0;

                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            var feedDict = new Dictionary<Tensor, object>
                    {
                        { x, inputData },
                        { y, priceData }
                    };

                            var feedItems = feedDict.Select(kv => new FeedItem(kv.Key, kv.Value)).ToArray();

                            sess.run(trainOp, feedItems);
                            var currentLoss = (float)sess.run(loss, feedItems);

                            if (float.IsNaN(currentLoss) || float.IsInfinity(currentLoss))
                            {
                                System.Diagnostics.Debug.WriteLine($"Model D - Training diverged at epoch {epoch}. Stopping training.");
                                break;
                            }

                            if (Math.Abs(previousLoss - currentLoss) < 1e-6)
                            {
                                stableCount++;
                                if (stableCount > 5)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Model D - Training converged at epoch {epoch}");
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
                                System.Diagnostics.Debug.WriteLine($"Model D - Epoch {epoch}, Loss: {currentLoss:E4}");
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"Model D - Training completed");
                        System.Diagnostics.Debug.WriteLine($"Model D - Final loss: {previousLoss:E4}");

                        // Save results to database
                        System.Diagnostics.Debug.WriteLine($"Model D - Saving results to database");
                        
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Model D training: {ex.Message}");
                    throw;
                }
            });
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