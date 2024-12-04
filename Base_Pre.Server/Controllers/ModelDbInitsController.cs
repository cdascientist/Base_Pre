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



    public partial class ModelDbInit
    {
        [NotMapped]  // Add this attribute to prevent EF Core from trying to map the dictionary
        public Dictionary<string, float> ModelMetrics { get; set; }
        public bool TrainingCompleted { get; set; }
        public DateTime CompletionTimestamp { get; set; }
        public byte[] ModelParameters { get; set; }
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

        public ModelDbInitsController(PrimaryDbContext context)
        {
            _context = context;
            _sessions = new ConcurrentDictionary<int, Session>();
            _processingOrchestrator = new ParallelProcessingOrchestrator();
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] TensorFlow Controller Initialized");
        }

        [HttpPost("Machine_Learning_Implementation_One/{id}/{name}/{productType}")]
        public async Task<ActionResult<ModelDbInit>> Machine_Learning_Implementation_One(
            int id,
            string name,
            string productType)
        {
            var sessionId = Interlocked.Increment(ref _sessionCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Implementation Session {sessionId}");
            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Preparing parallel implementation");

                var modelInit = new ModelDbInit();
                var modelAResults = new ConcurrentDictionary<string, object>();
                var modelBResults = new ConcurrentDictionary<string, object>();

                var sessionA = new Session();
                var sessionB = new Session();

                _sessions.TryAdd(sessionId * 2, sessionA);
                _sessions.TryAdd(sessionId * 2 + 1, sessionB);

                await ProcessFactoryOne(modelInit, id, name, productType, sessionId);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting parallel model training");

                await Task.WhenAll(
                    ProcessFactoryTwo(modelInit, id, name, productType, sessionId, sessionA, modelAResults),
                    ProcessFactoryThree(modelInit, id, name, productType, sessionId, sessionB, modelBResults)
                );

                await ProcessFactoryFour(modelInit, id, name, productType, sessionId, modelAResults, modelBResults);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ML Implementation completed successfully");
                return Ok(modelInit);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Error in ML Implementation: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                if (_sessions.TryRemove(sessionId * 2, out var sessionA))
                    sessionA?.Dispose();
                if (_sessions.TryRemove(sessionId * 2 + 1, out var sessionB))
                    sessionB?.Dispose();

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Resources cleaned up");
            }
        }










        private async Task ProcessFactoryOne(ModelDbInit model, int id, string name, string productType, int sessionId)
        {
            Jit_Memory_Object.AddProperty("ProcessFactoryOneActive", true);
            bool isActive = (bool)Jit_Memory_Object.GetProperty("ProcessFactoryOneActive");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: ProcessFactoryOneActive property value: {isActive}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Session {sessionId}: Starting ProcessFactoryOne (Model C)");

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
                    { 125f/1000f },
                    { 225f/1000f },
                    { 325f/1000f }
                };

                        var oneHotNames = new float[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            oneHotNames[i, (i + 2) % 3] = 1.0f;
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

                        System.Diagnostics.Debug.WriteLine("Model C - Data shapes:");
                        System.Diagnostics.Debug.WriteLine($"Input data shape: {string.Join(",", inputData.GetLength(0))} x {string.Join(",", inputData.GetLength(1))}");
                        System.Diagnostics.Debug.WriteLine($"Price data shape: {string.Join(",", priceData.GetLength(0))} x {string.Join(",", priceData.GetLength(1))}");

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
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Model C training: {ex.Message}");
                    throw;
                }
            });
        }



















        private async Task ProcessFactoryTwo(ModelDbInit model, int id, string name, string productType, int sessionId,
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

        private async Task ProcessFactoryThree(ModelDbInit model, int id, string name, string productType, int sessionId,
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


















        private async Task ProcessFactoryFour(ModelDbInit model, int id, string name, string productType, int sessionId,
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
                        model.TrainingCompleted = true;
                        model.CompletionTimestamp = DateTime.UtcNow;
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