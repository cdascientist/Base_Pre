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
using Accord.Math;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Reflection;
using Tensorflow.Contexts;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Accord.Math.Distances;

namespace Base_Pre.Server.Controllers
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

    [Route("api/[controller]")]
    [ApiController]
    public class ModelDbInitsController : ControllerBase
    {
        private readonly PrimaryDbContext _context;
        private static readonly ConditionalWeakTable<object, Jit_Memory_Object> jitMemory =
            new ConditionalWeakTable<object, Jit_Memory_Object>();

        public ModelDbInitsController(PrimaryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelDbInit>>> GetModelDbInits()
        {
            return await _context.ModelDbInits.ToListAsync();
        }

        [HttpGet("Machine_Learning_Implementation_One/GetAllProducts")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllProducts()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Starting GetAllProducts operation");

                // Get products from SubProduct A (SubProductum)
                var subProductsA = await _context.SubProductAs
                    .AsNoTracking()
                    .Select(p => new {
                        Source = "SubProduct_A",
                        Id = p.Id,
                        ProductName = p.ProductName,
                        ProductType = p.ProductType,
                        Quantity = p.Quantity,
                        Price = p.Price
                    })
                    .ToListAsync();
                System.Diagnostics.Debug.WriteLine($"Retrieved {subProductsA.Count} products from SubProduct A");

                // Get products from SubProduct B
                var subProductsB = await _context.SubProductBs
                    .AsNoTracking()
                    .Select(p => new {
                        Source = "SubProduct_B",
                        Id = p.Id,
                        ProductName = p.ProductName,
                        ProductType = p.ProductType,
                        Quantity = p.Quantity,
                        Price = p.Price
                    })
                    .ToListAsync();
                System.Diagnostics.Debug.WriteLine($"Retrieved {subProductsB.Count} products from SubProduct B");

                // Get products from SubProduct C
                var subProductsC = await _context.SubProductCs
                    .AsNoTracking()
                    .Select(p => new {
                        Source = "SubProduct_C",
                        Id = p.Id,
                        ProductName = p.ProductName,
                        ProductType = p.ProductType,
                        Quantity = p.Quantity,
                        Price = p.Price
                    })
                    .ToListAsync();
                System.Diagnostics.Debug.WriteLine($"Retrieved {subProductsC.Count} products from SubProduct C");

                // Combine all products
                var allProducts = subProductsA
                    .Concat(subProductsB)
                    .Concat(subProductsC)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"Total products retrieved: {allProducts.Count}");

                return Ok(allProducts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllProducts: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost("Machine_Learning_Implementation_One/{id}/{name}/{productType}")]
        public async Task<ActionResult<ModelDbInit>> Machine_Learning_Implementation_One(int id, string name, string productType)
        {
            try
            {
                tf.enable_eager_execution();
                System.Diagnostics.Debug.WriteLine("TensorFlow eager execution enabled");

                ModelDbInit modelInit = new ModelDbInit();
                System.Diagnostics.Debug.WriteLine($"Created new model instance for ProductType: {productType}");

                var jitObject = new Jit_Memory_Object();
                jitMemory.Add(this, jitObject);
                System.Diagnostics.Debug.WriteLine("JIT memory initialized");

                Jit_Memory_Object.SetJitMethodHandle(MethodBase.GetCurrentMethod().MethodHandle);
                System.Diagnostics.Debug.WriteLine("JIT method handle stored");

                // Store ProductType in JIT memory for use across stages
                Jit_Memory_Object.AddProperty("ProductType", productType);
                System.Diagnostics.Debug.WriteLine($"ProductType {productType} stored in JIT memory");

                // Stage 1
                System.Diagnostics.Debug.WriteLine($"Starting Stage 1 for ProductType: {productType}");
                await ProcessFactoryOne(modelInit, id, name, productType, jitObject);
                System.Diagnostics.Debug.WriteLine("Stage 1 completed");

                // Run stages 2 and 3 in parallel
                System.Diagnostics.Debug.WriteLine($"Starting Stages 2 and 3 in parallel for ProductType: {productType}");
                await Task.WhenAll(
                    Task.Run(() => {
                        System.Diagnostics.Debug.WriteLine("Starting Stage 2");
                        ProcessFactoryTwo(modelInit, id, name, productType, jitObject);
                        System.Diagnostics.Debug.WriteLine("Stage 2 completed");
                    }),
                    Task.Run(() => {
                        System.Diagnostics.Debug.WriteLine("Starting Stage 3");
                        ProcessFactoryThree(modelInit, id, name, productType, jitObject);
                        System.Diagnostics.Debug.WriteLine("Stage 3 completed");
                    })
                );

                // Stage 4
                System.Diagnostics.Debug.WriteLine("Starting Stage 4");
                ProcessFactoryFour(modelInit, id, name, productType, jitObject);
                System.Diagnostics.Debug.WriteLine("Stage 4 completed");

                System.Diagnostics.Debug.WriteLine("Processing completed without database save");
                return Ok(modelInit);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                jitMemory.Remove(this);
                System.Diagnostics.Debug.WriteLine("JIT memory cleaned up");
            }
        }

        private async Task ProcessFactoryOne(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryOne: Setting ModelDbInitCatagoricalName to {name}");
            model.ModelDbInitCatagoricalName = name;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryOne: Setting ModelDbInitModelData to true");
            model.ModelDbInitModelData = true;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryOne: Adding Stage1Complete property");
            Jit_Memory_Object.AddProperty("Stage1Complete", true);

            System.Diagnostics.Debug.WriteLine("Fetching Model from database");
            var ML_Model = await _context.ModelDbInits
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerId == 2);
                 ///TODO !!!!!!: Dyanmically Set Cutomer ID 
            System.Diagnostics.Debug.WriteLine($"Model fetch completed: {(ML_Model != null ? "Found" : "Not Found")}");

            if (ML_Model == null)
            {
                System.Diagnostics.Debug.WriteLine($"No existing model found for ProductType {productType}. Initializing new model creation.");
                model.ModelDbInitModelData = false;
                Jit_Memory_Object.AddProperty("NewModelRequired", true);

                try
                {
                    System.Diagnostics.Debug.WriteLine($"Starting subproduct data collection for ProductType {productType}");

                    var allSubProducts = new List<object>();



                    /// <summary>
                    /// Sample Data for new Customer for initial model 
                    /// </summary>
                    /// NEW Get PRODUCTS from database to train on Lets load all the prices results into a local variable -2

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct A data");
                    var subproductsA = await _context.SubProductAs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    allSubProducts.AddRange(subproductsA);
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsA.Count} SubProduct A records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct B data");
                    var subproductsB = await _context.SubProductBs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    allSubProducts.AddRange(subproductsB);
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsB.Count} SubProduct B records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct C data");
                    var subproductsC = await _context.SubProductCs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    allSubProducts.AddRange(subproductsC);
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsC.Count} SubProduct C records");


                    /// <summary>
                    /// Store Sample Data in the Just in Time Compiler Memeory 
                    /// </summary>
                    /// 
                    // Store combined list in JIT memory
                    Jit_Memory_Object.AddProperty("AllSubProducts", allSubProducts);
                    System.Diagnostics.Debug.WriteLine($"Total subproducts found: {allSubProducts.Count}");

                    model.ModelDbInitModelData = allSubProducts.Any();

                    System.Diagnostics.Debug.WriteLine("Subproduct data collection completed");
                    if (allSubProducts == null)
                    {
                        Console.WriteLine("Aqusition of sample Data Failed, no records found");
                    }
                    else 
                    {
                        System.Diagnostics.Debug.WriteLine($"Products Sample Data found of Type:{productType}");
                        /// <summary>
                        /// Catagorize Sample Data 
                        /// </summary>
                        /// 
                        /// Names
                        var allSubProductsNames = allSubProducts.Select(p =>
                            ((dynamic)p).ProductName
                        ).ToArray();
                        /// Prices
                        var allSubProductsPrices = allSubProducts.Select(p =>
                            ((dynamic)p).Price
                        ).ToArray();
                        /// <summary>
                        /// NEW Get PRODUCTS from database to train on - 3
                        /// From that list we will show the number of record that are acquired 
                        /// Then we will clarify the range of all the records acquired in terms
                        /// of specified columns
                        /// </summary>
                        System.Diagnostics.Debug.WriteLine($"Training data initialized. Number of samples: {allSubProductsNames.Length}");
                        System.Diagnostics.Debug.WriteLine($"Price range: {allSubProductsPrices.Min()} to {allSubProductsPrices.Max()}");
                        /// <summary>
                        /// MODEL ARCHITECTURE MODIFICATIONS FOR COMBINED FEATURES
                        /// 
                        /// 1. Input Layer Changes:
                        ///    - Original: Single input dimension (price only)
                        ///    - Modified: Multiple input dimensions (price + one-hot encoded names)
                        ///    - Input shape: [batch_size, 1 + number_of_unique_names]
                        /// 
                        /// 2. Weight Matrix (W) Modifications:
                        ///    - Original: Shape was [1, 1] for price only
                        ///    - Modified: Shape is [input_dim, 1] where input_dim = 1 + uniqueNames.Count
                        ///    - Each row in W now corresponds to:
                        ///      * Row 0: Price weight
                        ///      * Row 1 to N: Weights for each unique name
                        /// 
                        /// 3. Bias Vector (b) Adjustments:
                        ///    - Maintains shape [1] but now accounts for combined features
                        ///    - Acts on the weighted sum of all features
                        /// 
                        /// 4. Forward Pass Calculation:
                        ///    - Original: prediction = price_data * W + b
                        ///    - Modified: prediction = concat(price_data, name_data) * W + b
                        ///    - Matrix multiplication now incorporates both price and name influences
                        /// 
                        /// 5. Training Considerations:
                        ///    - Loss function remains MSE but operates on higher dimensional inputs
                        ///    - Gradient updates affect both price and name feature weights
                        ///    - Learning rate applied uniformly across all feature weights
                        /// </summary>
                        /// 
                        System.Diagnostics.Debug.WriteLine("Phase One: Initializing the creation of Neural Network...");
                        System.Diagnostics.Debug.WriteLine("Initializing tensors for both prices and names");


                        Tensor priceTrainData;
                        Tensor nameTrainData;
                        try 
                        {
                            /// <summary>
                            /// Create separate tensors for prices and names
                            /// Names are encoded as indices and then one-hot encoded
                            /// </summary>
                            
                            /// Prices
                            priceTrainData = tf.convert_to_tensor(allSubProductsPrices, dtype: TF_DataType.TF_FLOAT);
                            priceTrainData = tf.reshape(priceTrainData, new[] { -1, 1 }); // Reshape to 2D

                            ///Names
                            // Create dictionary for name encoding
                            var uniqueNames = allSubProductsNames.Distinct().ToList();
                            var nameToIndex = uniqueNames.Select((name, index) => new { name, index })
                                                           .ToDictionary(x => x.name, x => x.index);

                            // Convert names to indices
                            var nameIndices = allSubProductsNames.Select(name => nameToIndex[name]).ToArray();

                            /// <summary>
                            /// One-hot encoding process:
                            /// 1. Creates a matrix of size [num_samples, num_unique_names]
                            /// 2. Each row represents one sample
                            /// 3. Each column represents one unique name
                            /// 4. Matrix contains 1.0 where name matches, 0.0 elsewhere
                            /// </summary>
                            /// 
                            var oneHotNames = new float[nameIndices.Length, uniqueNames.Count];
                            for (int i = 0; i < nameIndices.Length; i++)
                            {
                                oneHotNames[i, nameIndices[i]] = 1.0f;
                            }
                            nameTrainData = tf.convert_to_tensor(oneHotNames, dtype: TF_DataType.TF_FLOAT);

                            /// <summary>
                            /// Feature combination process:
                            /// 1. Concatenate price and name tensors along axis 1 (columns)
                            /// 2. Results in tensor of shape [num_samples, 1 + num_unique_names]
                            /// 3. Each row contains: [price, name_1_hot, name_2_hot, ..., name_n_hot]
                            /// </summary>
                            /// 
                            var combinedTrainData = tf.concat(new[] { priceTrainData, nameTrainData }, axis: 1);

                            System.Diagnostics.Debug.WriteLine($"Combined tensor shape: {string.Join(", ", combinedTrainData.shape)}");

                            /// <summary>
                            /// Modified model initialization:
                            /// 1. inputDim = 1 (price) + uniqueNames.Count (one-hot encoded names)
                            /// 2. W matrix shape: [inputDim, 1] for mapping combined features to price
                            /// 3. b vector shape: [1] for single output bias
                            /// </summary>
                            /// 
                            System.Diagnostics.Debug.WriteLine("Initializing model variables for combined features");
                            /// Price dimension + name dimensions
                            var inputDim = 1 + uniqueNames.Count; 
                            var W = tf.Variable(tf.random.normal(new[] { inputDim, 1 }));
                            var b = tf.Variable(tf.zeros(new[] { 1 }));

                            System.Diagnostics.Debug.WriteLine($"Modified W shape: {string.Join(", ", W.shape)}, b shape: {string.Join(", ", b.shape)}");

                            /// <summary>
                            /// Part 2 
                            /// Then lets define the model initial specification  
                            /// </summary>
                            Console.WriteLine("Initializing training parameters");
                            int epochs = 100;
                            float learningRate = 1e-2f;

                            System.Diagnostics.Debug.WriteLine("Starting training process with combined features");
                            for (int epoch = 0; epoch < epochs; epoch++)
                            {
                                try
                                {
                                    using (var tape = tf.GradientTape())
                                    {
                                        var predictions = tf.matmul(combinedTrainData, W) + b;
                                        // Modified loss function to work with combined features
                                        var loss = tf.reduce_mean(tf.square(predictions - priceTrainData));

                                        var gradients = tape.gradient(loss, new[] { W, b });

                                        W.assign_sub(gradients[0] * learningRate);
                                        b.assign_sub(gradients[1] * learningRate);

                                        if (epoch % 10 == 0)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Training Epoch {epoch}, Loss: {loss.numpy()}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error in training loop at epoch {epoch}: {ex.Message}");
                                    throw new Exception($"Training failed at epoch {epoch}", ex);
                                }
                            }
                            /// <summary>
                            /// Part 7 
                            /// Save the Model to the in-Runtime memory     
                            /// </summary>
                            System.Diagnostics.Debug.WriteLine("Starting model serialization process");
                            using (var memoryStream = new MemoryStream())
                            using (var writer = new BinaryWriter(memoryStream))
                            {
                                /// Write model weights
                                var wData = W.numpy().ToArray<float>();
                                writer.Write(wData.Length);
                                foreach (var w in wData)
                                {
                                    writer.Write(w);
                                }
                                System.Diagnostics.Debug.WriteLine("Model weights serialized successfully");

                                /// Write model bias
                                var bData = b.numpy().ToArray<float>();
                                writer.Write(bData.Length);
                                foreach (var bias in bData)
                                {
                                    writer.Write(bias);
                                }
                                System.Diagnostics.Debug.WriteLine("Model bias serialized successfully");

                                /// Save to in-memory object as separate properties
                                ///TODO !!!!!!: Dyanmically Set Cutomer ID 
                                Jit_Memory_Object.AddProperty("CustomerId", 2);
                                Jit_Memory_Object.AddProperty("Data", memoryStream.ToArray());
                                System.Diagnostics.Debug.WriteLine("Model By Customer ID data saved to in-memory object successfully");

                                /// Verify the stored model to in Memory 
                                var storedCutomerID = Jit_Memory_Object.GetProperty("ModelName");
                                var storedModelData = Jit_Memory_Object.GetProperty("Data") as byte[];
                                System.Diagnostics.Debug.WriteLine($"Verification - Customer ID: {storedCutomerID}");
                                System.Diagnostics.Debug.WriteLine($"Verification - Data Size: {storedModelData?.Length ?? 0} bytes");


                                /// <summary>
                                /// Implement Reinfomrment learning on model using Accord  
                                /// Approximate a relative Cetroid based on type of price and train on relative value base upon instance of Object Creation
                                /// </summary>
                                System.Diagnostics.Debug.WriteLine("Phase two: Initializing Data K Clustering Implementation");
                                System.Diagnostics.Debug.WriteLine($"Found {allSubProducts.Count} products with Type '{productType}' for Data Clustering");

                                /// Extract prices and convert to double
                                System.Diagnostics.Debug.WriteLine("Extracting prices for clustering");
                                var prices = allSubProductsPrices.Select(p => new double[] { (double)p.Price }).ToArray();

                                /// Define clustering parameters
                                int numClusters = 3; // Define relative clusters and differentiate median
                                int numIterations = 100;

                                System.Diagnostics.Debug.WriteLine($"Clustering parameters: clusters={numClusters}, iterations={numIterations}");

                                /// Create k-means algorithm
                                var kmeans = new Accord.MachineLearning.KMeans(numClusters)
                                {
                                    MaxIterations = numIterations,
                                    Distance = new SquareEuclidean()
                                };
                                /// Compute the algorithm
                                System.Diagnostics.Debug.WriteLine("Starting k-means clustering");
                                var clusters = kmeans.Learn(prices);

                                /// Get the cluster centroids
                                var centroids = clusters.Centroids;

                                System.Diagnostics.Debug.WriteLine("K-means clustering completed");

                                /// Get cluster assignments for each point
                                var assignments = clusters.Decide(prices);

                                /// Log final results
                                System.Diagnostics.Debug.WriteLine("Final clustering results:");
                                for (int i = 0; i < prices.Length; i++)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Price: {prices[i][0]:F4}, Cluster: {assignments[i]}");
                                }

                                System.Diagnostics.Debug.WriteLine("Final centroids:");
                                for (int i = 0; i < numClusters; i++)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Centroid {i}: {centroids[i][0]:F4}");
                                }

                                Jit_Memory_Object.AddProperty("Centroid_1", (float)centroids[0][0]);
                                Jit_Memory_Object.AddProperty("Centroid_2", (float)centroids[1][0]);
                                Jit_Memory_Object.AddProperty("Centroid_3", (float)centroids[2][0]);

                                /// Verify the stored Centroids
                                var Centroid_1 = Jit_Memory_Object.GetProperty("Centroid_1");
                                var Centroid_2 = Jit_Memory_Object.GetProperty("Centroid_2");
                                var Centroid_3 = Jit_Memory_Object.GetProperty("Centroid_3");

                                Console.WriteLine($"Verification - Centroid_1: {Centroid_1}");
                                Console.WriteLine($"Verification - Centroid_2: {Centroid_2}");
                                Console.WriteLine($"Verification - Centroid_3: {Centroid_3}");

                                /// Find the largest centroid and its information
                                var centroidValues = centroids.Select((c, i) => new { Value = c[0], Index = i }).ToList();
                                var largestCentroid = centroidValues.OrderByDescending(c => c.Value).First();

                                /// Count points in each cluster
                                var clusterCounts = assignments.GroupBy(a => a).ToDictionary(g => g.Key, g => g.Count());

                                /// Find cluster with most points
                                var largestCluster = clusterCounts.OrderByDescending(kvp => kvp.Value).First();

                                /// Calculate average of points in the cluster with most points
                                var pointsInLargestCluster = prices
                                    .Select((p, i) => new { Price = p[0], ClusterIndex = assignments[i] })
                                    .Where(p => p.ClusterIndex == largestCluster.Key)
                                    .Select(p => p.Price)
                                    .ToList();

                                var largestClusterAverage = pointsInLargestCluster.Average();

                                /// Store the information
                                Jit_Memory_Object.AddProperty("Largest_Centroid_Value", (float)largestCentroid.Value);
                                Jit_Memory_Object.AddProperty("Largest_Centroid_Index", largestCentroid.Index);
                                Jit_Memory_Object.AddProperty("Most_Points_Cluster_Index", largestCluster.Key);
                                Jit_Memory_Object.AddProperty("Most_Points_Cluster_Count", largestCluster.Value);
                                Jit_Memory_Object.AddProperty("Most_Points_Cluster_Value", (float)centroids[largestCluster.Key][0]);
                                Jit_Memory_Object.AddProperty("Most_Points_Cluster_Average", (float)largestClusterAverage);

                                /// Verify the stored values
                                Console.WriteLine("\n=== Extended Clustering Analysis ===");
                                Console.WriteLine($"Largest Centroid Value: {Jit_Memory_Object.GetProperty("Largest_Centroid_Value"):F4}");
                                Console.WriteLine($"Largest Centroid Index: {Jit_Memory_Object.GetProperty("Largest_Centroid_Index")}");
                                Console.WriteLine($"Cluster with Most Points - Index: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Index")}");
                                Console.WriteLine($"Cluster with Most Points - Count: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Count")}");
                                Console.WriteLine($"Cluster with Most Points - Value: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Value"):F4}");
                                Console.WriteLine($"Cluster with Most Points - Average: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Average"):F4}");
                                Console.WriteLine("================================\n");

                                // Calculate a simple metric: average distance to assigned centroid
                                double totalDistance = 0;
                                for (int i = 0; i < prices.Length; i++)
                                {
                                    double distance = Math.Abs(prices[i][0] - centroids[assignments[i]][0]);
                                    totalDistance += distance;
                                }
                                double avgDistance = totalDistance / prices.Length;

                                System.Diagnostics.Debug.WriteLine($"Average distance to assigned centroid: {avgDistance:F4}");

                            }

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Tensor initialization failed: {ex.Message}");
                            throw new Exception("Failed to initialize tensor from price data.", ex);
                        }

                    }

                    ///Identification of the model
                    System.Diagnostics.Debug.WriteLine($"Customer Model found: {ML_Model.ModelDbInitCatagoricalName}");
                    System.Diagnostics.Debug.WriteLine($"Customer ID: {ML_Model.CustomerId}");
                    System.Diagnostics.Debug.WriteLine($"Model Data Size: {(ML_Model.Data?.Length ?? 0)} bytes");



                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error during subproduct data collection for ProductType {productType}: {ex.Message}");
                    throw;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Existing ML Model found for ProductType {productType} - processing existing model data");
                model.ModelDbInitModelData = true;
                Jit_Memory_Object.AddProperty("ExistingModelFound", true);
                Jit_Memory_Object.AddProperty("ExistingMLModel", ML_Model);
                System.Diagnostics.Debug.WriteLine("Existing model data stored in JIT memory");
            }
        }








        private void ProcessFactoryTwo(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Processing ProductType {productType}");
            model.ModelDbInitModelData = true;

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Setting CustomerId to {id}");
            model.CustomerId = id;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Adding Stage2Complete property");
            Jit_Memory_Object.AddProperty("Stage2Complete", true);
        }

        private void ProcessFactoryThree(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Processing ProductType {productType}");
            model.ModelDbInitModelData = true;

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Setting CustomerId to {id}");
            model.CustomerId = id;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Adding Stage3Complete property");
            Jit_Memory_Object.AddProperty("Stage3Complete", true);
        }

        private void ProcessFactoryFour(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryFour: Processing ProductType {productType}");
            model.ModelDbInitTimeStamp = DateTime.Now;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryFour: Setting ModelDbInitModelData to true");
            model.ModelDbInitModelData = true;

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryFour: Setting CustomerId to {id}");
            model.CustomerId = id;

            System.Diagnostics.Debug.WriteLine("ProcessFactoryFour: Retrieving completion status of previous stages");
            var stage1Complete = Jit_Memory_Object.GetProperty("Stage1Complete");
            var stage2Complete = Jit_Memory_Object.GetProperty("Stage2Complete");
            var stage3Complete = Jit_Memory_Object.GetProperty("Stage3Complete");

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryFour: Stage completion status for ProductType {productType} - Stage1: {stage1Complete}, Stage2: {stage2Complete}, Stage3: {stage3Complete}");
        }
    }
}