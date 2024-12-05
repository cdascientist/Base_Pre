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
                            Price = (float)p.Price
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
                            Price = (float)p.Price
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
                            Price = (float)p.Price
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

            /// <summary>
            /// Step Six Retrieve the inital base model by CustomerID
            /// </summary>
            System.Diagnostics.Debug.WriteLine("Fetching Model from database");
            var ML_Model = await _context.ModelDbInits
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerId == customerID);
            System.Diagnostics.Debug.WriteLine($"Model fetch completed: {(ML_Model != null ? "Found" : "Not Found")}");

            /// <summary>
            /// Step Seven Lets Perform Actions if the model is not found or found
            /// </summary>
            if (ML_Model == null)
            {
                /// <summary>
                /// Step Eight Lets Data Prep with base reference from all products and services
                /// </summary>
                System.Diagnostics.Debug.WriteLine($"No existing model found for Customer with ID {customerID}. Initializing new model creation.");

                // Retrieve products and services from JIT memory
                var allSubProducts = (List<dynamic>)Jit_Memory_Object.GetProperty("All_SubProducts");
                var allSubServices = (List<dynamic>)Jit_Memory_Object.GetProperty("All_SubServices");

                // Extract and combine names and prices (converting prices to float)
                var allNames = allSubProducts.Select(p => p.ProductName)
                    .Concat(allSubServices.Select(s => s.ServiceName))
                    .ToArray();

                var allPrices = allSubProducts.Select(p => (float)p.Price)
                    .Concat(allSubServices.Select(s => (float)s.Price))
                    .ToArray();

                System.Diagnostics.Debug.WriteLine($"Total items for processing: {allNames.Length}");

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
                            var x = tf.placeholder(tf.float32, shape: new[] { -1, allNames.Length + 1 }, name: "input");
                            var y = tf.placeholder(tf.float32, shape: new[] { -1, 1 }, name: "output");

                            var W = tf.Variable(tf.random.normal(new[] { allNames.Length + 1, 1 }, mean: 0.0f, stddev: 0.01f), name: "weight");
                            var b = tf.Variable(tf.zeros(new[] { 1 }), name: "bias");

                            var predictions = tf.add(tf.matmul(x, W), b);
                            var loss = tf.reduce_mean(tf.square(predictions - y)) * 0.5f;

                            var optimizer = tf.train.GradientDescentOptimizer(learningRate);
                            var trainOp = optimizer.minimize(loss);

                            // Reshape price data for tensor - properly shaped 2D array
                            var priceData = new float[allPrices.Length, 1];
                            for (int i = 0; i < allPrices.Length; i++)
                            {
                                priceData[i, 0] = allPrices[i];
                            }

                            // Create one-hot encoding matrix
                            var oneHotNames = new float[allNames.Length, allNames.Length];
                            for (int i = 0; i < allNames.Length; i++)
                            {
                                oneHotNames[i, i] = 1.0f;
                            }

                            // Prepare input data with proper dimensions
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
                            /// <summary>
                            /// Part 7 
                            /// Save the Model to the in-Runtime memory     
                            /// </summary>
                            System.Diagnostics.Debug.WriteLine("Starting model serialization process");
                            using (var memoryStream = new MemoryStream())
                            using (var writer = new BinaryWriter(memoryStream))
                            {
                                /// Write model weights - convert NDArray to float array
                                var wNDArray = (NDArray)sess.run(W);
                                var wData = wNDArray.ToArray<float>();
                                writer.Write(wData.Length);
                                foreach (var w in wData)
                                {
                                    writer.Write(w);
                                }
                                System.Diagnostics.Debug.WriteLine("Model weights serialized successfully");

                                /// Write model bias - convert NDArray to float array
                                var bNDArray = (NDArray)sess.run(b);
                                var bData = bNDArray.ToArray<float>();
                                writer.Write(bData.Length);
                                foreach (var bias in bData)
                                {
                                    writer.Write(bias);
                                }
                                System.Diagnostics.Debug.WriteLine("Model bias serialized successfully");

                                /// Save to in-memory object as separate properties
                                Jit_Memory_Object.AddProperty("CustomerId", customerID);
                                Jit_Memory_Object.AddProperty("ProcessFactoryOne_Data", memoryStream.ToArray());
                                System.Diagnostics.Debug.WriteLine("Model By Customer ID data saved to in-memory object successfully");

                                /// Verify the stored model in Memory 
                                var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                                var storedModelData = Jit_Memory_Object.GetProperty("ProcessFactoryOne_Data") as byte[];
                                System.Diagnostics.Debug.WriteLine($"Verification - Customer ID: {storedCustomerId}");
                                System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {storedModelData?.Length ?? 0} bytes");
                            }
                        }
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
                // Store model properties in JIT memory
                Jit_Memory_Object.AddProperty("CustomerId", ML_Model.CustomerId);
                Jit_Memory_Object.AddProperty("ModelDbInitTimeStamp", ML_Model.ModelDbInitTimeStamp);
                Jit_Memory_Object.AddProperty("Id", ML_Model.Id);
                Jit_Memory_Object.AddProperty("Data", ML_Model.Data);

                // Verify stored properties
                var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
                var storedTimestamp = Jit_Memory_Object.GetProperty("ModelDbInitTimeStamp");
                var storedId = Jit_Memory_Object.GetProperty("Id");
                var storedData = Jit_Memory_Object.GetProperty("Data");

                System.Diagnostics.Debug.WriteLine($"Verification - CustomerId: {storedCustomerId}");
                System.Diagnostics.Debug.WriteLine($"Verification - Timestamp: {storedTimestamp}");
                System.Diagnostics.Debug.WriteLine($"Verification - Id: {storedId}");
                System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {(storedData as byte[])?.Length ?? 0} bytes");

            }
        }



















        private async Task ProcessFactoryTwo(ModelDbInit model, int id, string name, int customerID, int sessionId,
      Session session, ConcurrentDictionary<string, object> results)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryTwoActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryTwoActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryTwoActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryTwo (Model A)");

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