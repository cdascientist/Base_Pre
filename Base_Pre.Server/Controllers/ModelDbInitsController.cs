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
using System.Security.Cryptography.X509Certificates;

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
        // Add these tensor declarations here
        private Tensor ProcessFactoryTwo_priceTrainData;
        private Tensor ProcessFactoryTwo_nameTrainData;
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
                /// <summary>
                /// MODEL NOT FOUND By Customer ID
                /// </summary>
                System.Diagnostics.Debug.WriteLine($"No existing model found for ProductType {productType}. Initializing new model creation.");
                model.ModelDbInitModelData = false;
                Jit_Memory_Object.AddProperty("NewModelRequired", true);

                try
                {
                    System.Diagnostics.Debug.WriteLine($"Starting subproduct data collection for ProductType {productType}");

                    var allSubProducts = new List<dynamic>();

                    /// <summary>
                    /// Sample Data for new Customer for initial model 
                    /// </summary>
                    /// NEW Get PRODUCTS from database to train on Lets load all the prices results into a local variable -2

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct A data");
                    var subproductsA = await _context.SubProductAs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .Select(p => new {
                            p.ProductName,
                            Price = (float)p.Price
                        })
                        .ToListAsync();
                    allSubProducts.AddRange(subproductsA);
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsA.Count} SubProduct A records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct B data");
                    var subproductsB = await _context.SubProductBs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .Select(p => new {
                            p.ProductName,
                            Price = (float)p.Price
                        })
                        .ToListAsync();
                    allSubProducts.AddRange(subproductsB);
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsB.Count} SubProduct B records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct C data");
                    var subproductsC = await _context.SubProductCs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .Select(p => new {
                            p.ProductName,
                            Price = (float)p.Price
                        })
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
                            p.ProductName
                        ).ToArray();
                        /// Prices
                        var allSubProductsPrices = allSubProducts.Select(p =>
                            p.Price
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
                            var combinedTrainData = tf.concat(new[] { priceTrainData, nameTrainData }, axis: 1);
                            System.Diagnostics.Debug.WriteLine($"Combined tensor shape: {string.Join(", ", combinedTrainData.shape)}");

                            /// <summary>
                            /// Modified model initialization:
                            /// 1. inputDim = 1 (price) + uniqueNames.Count (one-hot encoded names)
                            /// 2. W matrix shape: [inputDim, 1] for mapping combined features to price
                            /// 3. b vector shape: [1] for single output bias
                            /// </summary>
                            System.Diagnostics.Debug.WriteLine("Initializing model variables for combined features");
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
                                /// TEMP- SAVE MODEL TO DB  
                                /// 
                                /// </summary>
                                /// 
                                var customerId = (int)Jit_Memory_Object.GetProperty("CustomerId");

                                var modelToSave = new ModelDbInit
                                {
                                    // Id will be auto-generated by the database
                                    CustomerId = customerId, // Using CustomerId from JIT memory
                                    ModelDbInitTimeStamp = DateTime.Now,
                                    ModelDbInitCatagoricalId = null,
                                    ModelDbInitCatagoricalName = null, // Set to null as requested
                                    ModelDbInitModelData = true,
                                    Data = memoryStream.ToArray(),
                                    ClientInformation = null
                                };

                                try
                                {
                                    System.Diagnostics.Debug.WriteLine("Starting to save new model to database");

                                    // Check if entry already exists for this customer ID
                                    var existingModel = await _context.ModelDbInits
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.CustomerId == customerId);

                                    if (existingModel != null)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Updating existing model for Customer_ID: {customerId}");
                                        modelToSave.Id = existingModel.Id;
                                        _context.Entry(modelToSave).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Creating new model for Customer_ID: {customerId}");
                                        _context.ModelDbInits.Add(modelToSave);
                                    }

                                    await _context.SaveChangesAsync();
                                    System.Diagnostics.Debug.WriteLine($"Model saved successfully. id: {modelToSave.Id}, Customer_ID: {modelToSave.CustomerId}");

                                    // Update the current model with saved data
                                    model.Id = modelToSave.Id;
                                    model.CustomerId = modelToSave.CustomerId;
                                    model.ModelDbInitTimeStamp = modelToSave.ModelDbInitTimeStamp;
                                    model.ModelDbInitCatagoricalId = modelToSave.ModelDbInitCatagoricalId;
                                    model.ModelDbInitCatagoricalName = modelToSave.ModelDbInitCatagoricalName;
                                    model.ModelDbInitModelData = modelToSave.ModelDbInitModelData;
                                    model.Data = modelToSave.Data;

                                    // Add metadata to JIT memory
                                    Jit_Memory_Object.AddProperty("SavedModelId", modelToSave.Id);
                                    Jit_Memory_Object.AddProperty("ModelSaveTime", modelToSave.ModelDbInitTimeStamp);
                                    Jit_Memory_Object.AddProperty("NewModelCreated", existingModel == null);

                                    System.Diagnostics.Debug.WriteLine("Model metadata updated in memory");
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error saving model to database: {ex.Message}");
                                    throw new Exception("Failed to save model to database", ex);
                                }

                                try
                                {
                                    var savedModel = await _context.ModelDbInits
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.Id == modelToSave.Id);

                                    if (savedModel != null)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"Model verification successful. Found in database with id: {savedModel.Id}");
                                        System.Diagnostics.Debug.WriteLine($"Saved model data size: {savedModel.Data?.Length ?? 0} bytes");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Warning: Could not verify saved model in database");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error verifying saved model: {ex.Message}");
                                }
                                /// <summary>
                                /// TEMP- SAVE MODEL TO DB  
                                /// 
                                /// </summary>
                                ///







                                /// <summary>
                                /// Implement Reinfomrment learning on model using Accord  
                                /// Approximate a relative Cetroid based on type of price and train on relative value base upon instance of Object Creation
                                /// </summary>
                                System.Diagnostics.Debug.WriteLine("Phase two: Initializing Data K Clustering Implementation");
                                System.Diagnostics.Debug.WriteLine($"Found {allSubProducts.Count} products with Type '{productType}' for Data Clustering");

                                // Check if all prices are identical
                                var distinctPrices = allSubProductsPrices.Distinct().ToList();
                                if (distinctPrices.Count == 1)
                                {
                                    System.Diagnostics.Debug.WriteLine("All prices are identical. Skipping K-means clustering.");
                                    float singlePrice = distinctPrices[0];

                                    // Store the single price as all centroids
                                    Jit_Memory_Object.AddProperty("Centroid_1", singlePrice);
                                    Jit_Memory_Object.AddProperty("Centroid_2", singlePrice);
                                    Jit_Memory_Object.AddProperty("Centroid_3", singlePrice);

                                    // Store additional metrics
                                    Jit_Memory_Object.AddProperty("Largest_Centroid_Value", singlePrice);
                                    Jit_Memory_Object.AddProperty("Largest_Centroid_Index", 0);
                                    Jit_Memory_Object.AddProperty("Most_Points_Cluster_Index", 0);
                                    Jit_Memory_Object.AddProperty("Most_Points_Cluster_Count", allSubProductsPrices.Length);
                                    Jit_Memory_Object.AddProperty("Most_Points_Cluster_Value", singlePrice);
                                    Jit_Memory_Object.AddProperty("Most_Points_Cluster_Average", singlePrice);

                                    System.Diagnostics.Debug.WriteLine($"Single price value: {singlePrice:F4}");
                                }
                                else
                                {
                                    /// Extract prices and convert to double
                                    System.Diagnostics.Debug.WriteLine("Extracting prices for clustering");
                                    var prices = allSubProductsPrices.Select(p => new double[] { (double)p }).ToArray();

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

                                    System.Diagnostics.Debug.WriteLine($"Verification - Centroid_1: {Centroid_1}");
                                    System.Diagnostics.Debug.WriteLine($"Verification - Centroid_2: {Centroid_2}");
                                    System.Diagnostics.Debug.WriteLine($"Verification - Centroid_3: {Centroid_3}");

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
                                    System.Diagnostics.Debug.WriteLine("\n=== Extended Clustering Analysis ===");
                                    System.Diagnostics.Debug.WriteLine($"Largest Centroid Value: {Jit_Memory_Object.GetProperty("Largest_Centroid_Value"):F4}");
                                    System.Diagnostics.Debug.WriteLine($"Largest Centroid Index: {Jit_Memory_Object.GetProperty("Largest_Centroid_Index")}");
                                    System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Index: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Index")}");
                                    System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Count: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Count")}");
                                    System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Value: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Value"):F4}");
                                    System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Average: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Average"):F4}");
                                    System.Diagnostics.Debug.WriteLine("================================\n");

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
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Tensor initialization failed: {ex.Message}");
                            throw new Exception("Failed to initialize tensor from price data.", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error during subproduct data collection for ProductType {productType}: {ex.Message}");
                    throw;
                }
            }
            else
            {

                /// <summary>
                /// MODEL FOUND By Customer ID
                /// </summary>
                System.Diagnostics.Debug.WriteLine($"Existing ML Model found for Customer ID {ML_Model.CustomerId}");
                model.ModelDbInitModelData = true;
                Jit_Memory_Object.AddProperty("ExistingModelFound", true);

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

                System.Diagnostics.Debug.WriteLine($"Starting subproduct data collection for ProductType {productType}");




                // Initialize collection lists
                var allSubProducts = new List<dynamic>();
                var All_allSubProducts = new List<dynamic>();
                var All_SubServices = new List<dynamic>();

                /// <summary>
                /// SECTION 1: Filtered Product Data Collection
                /// Collect products filtered by product type
                /// </summary>
                System.Diagnostics.Debug.WriteLine("Fetching SubProduct A data");
                var subproductsA = await _context.SubProductAs
                    .AsNoTracking()
                    .Where(p => p.ProductType == productType)
                    .Select(p => new {
                        p.ProductName,
                        Price = (float)p.Price
                    })
                    .ToListAsync();
                allSubProducts.AddRange(subproductsA);
                System.Diagnostics.Debug.WriteLine($"Found {subproductsA.Count} SubProduct A records");

                System.Diagnostics.Debug.WriteLine("Fetching SubProduct B data");
                var subproductsB = await _context.SubProductBs
                    .AsNoTracking()
                    .Where(p => p.ProductType == productType)
                    .Select(p => new {
                        p.ProductName,
                        Price = (float)p.Price
                    })
                    .ToListAsync();
                allSubProducts.AddRange(subproductsB);
                System.Diagnostics.Debug.WriteLine($"Found {subproductsB.Count} SubProduct B records");

                System.Diagnostics.Debug.WriteLine("Fetching SubProduct C data");
                var subproductsC = await _context.SubProductCs
                    .AsNoTracking()
                    .Where(p => p.ProductType == productType)
                    .Select(p => new {
                        p.ProductName,
                        Price = (float)p.Price
                    })
                    .ToListAsync();
                allSubProducts.AddRange(subproductsC);
                System.Diagnostics.Debug.WriteLine($"Found {subproductsC.Count} SubProduct C records");

                /// <summary>
                /// SECTION 2: Complete Product Data Collection
                /// Collect all products without filtering
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
                All_allSubProducts.AddRange(All_subproductsA);
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
                All_allSubProducts.AddRange(All_subproductsB);
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
                All_allSubProducts.AddRange(All_subproductsC);
                System.Diagnostics.Debug.WriteLine($"Found {All_subproductsC.Count} SubProduct C ALL records");

                /// <summary>
                /// SECTION 3: Complete Service Data Collection
                /// Collect all services without filtering
                /// </summary>
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
                /// SECTION 4: Store Data in JIT Memory
                /// Store all collected data in JIT memory for later use
                /// </summary>
                Jit_Memory_Object.AddProperty("AllSubProducts", allSubProducts);
                Jit_Memory_Object.AddProperty("All_SubProducts", All_allSubProducts);
                Jit_Memory_Object.AddProperty("All_SubServices", All_SubServices);






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
                        p.ProductName
                    ).ToArray();
                    /// Prices
                    var allSubProductsPrices = allSubProducts.Select(p =>
                        p.Price
                    ).ToArray();

                    /// <summary>
                    /// Implement Reinfomrment learning on model using Accord  
                    /// Approximate a relative Cetroid based on type of price and train on relative value base upon instance of Object Creation
                    /// </summary>
                    System.Diagnostics.Debug.WriteLine("Phase two: Initializing Data K Clustering Implementation");
                    System.Diagnostics.Debug.WriteLine($"Found {allSubProducts.Count} products with Type '{productType}' for Data Clustering");

                    // Check if all prices are identical
                    var distinctPrices = allSubProductsPrices.Distinct().ToList();
                    if (distinctPrices.Count == 1)
                    {
                        System.Diagnostics.Debug.WriteLine("All prices are identical. Skipping K-means clustering.");
                        float singlePrice = distinctPrices[0];

                        // Store the single price as all centroids
                        Jit_Memory_Object.AddProperty("Centroid_1", singlePrice);
                        Jit_Memory_Object.AddProperty("Centroid_2", singlePrice);
                        Jit_Memory_Object.AddProperty("Centroid_3", singlePrice);

                        // Store additional metrics
                        Jit_Memory_Object.AddProperty("Largest_Centroid_Value", singlePrice);
                        Jit_Memory_Object.AddProperty("Largest_Centroid_Index", 0);
                        Jit_Memory_Object.AddProperty("Most_Points_Cluster_Index", 0);
                        Jit_Memory_Object.AddProperty("Most_Points_Cluster_Count", allSubProductsPrices.Length);
                        Jit_Memory_Object.AddProperty("Most_Points_Cluster_Value", singlePrice);
                        Jit_Memory_Object.AddProperty("Most_Points_Cluster_Average", singlePrice);

                        System.Diagnostics.Debug.WriteLine($"Single price value: {singlePrice:F4}");
                    }
                    else
                    {
                        /// Extract prices and convert to double
                        System.Diagnostics.Debug.WriteLine("Extracting prices for clustering");
                        var prices = allSubProductsPrices.Select(p => new double[] { (double)p }).ToArray();

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

                        System.Diagnostics.Debug.WriteLine($"Verification - Centroid_1: {Centroid_1}");
                        System.Diagnostics.Debug.WriteLine($"Verification - Centroid_2: {Centroid_2}");
                        System.Diagnostics.Debug.WriteLine($"Verification - Centroid_3: {Centroid_3}");

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
                        System.Diagnostics.Debug.WriteLine("\n=== Extended Clustering Analysis ===");
                        System.Diagnostics.Debug.WriteLine($"Largest Centroid Value: {Jit_Memory_Object.GetProperty("Largest_Centroid_Value"):F4}");
                        System.Diagnostics.Debug.WriteLine($"Largest Centroid Index: {Jit_Memory_Object.GetProperty("Largest_Centroid_Index")}");
                        System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Index: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Index")}");
                        System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Count: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Count")}");
                        System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Value: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Value"):F4}");
                        System.Diagnostics.Debug.WriteLine($"Cluster with Most Points - Average: {Jit_Memory_Object.GetProperty("Most_Points_Cluster_Average"):F4}");
                        System.Diagnostics.Debug.WriteLine("================================\n");

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

            }

            // First get the CustomerID from JIT memory
            System.Diagnostics.Debug.WriteLine("Retrieving Customer ID from JIT Memory");
            var CustomerId = (int)Jit_Memory_Object.GetProperty("CustomerId");
            System.Diagnostics.Debug.WriteLine($"Retrieved CustomerId: {CustomerId}");

            // Check if ClientOrder exists
            System.Diagnostics.Debug.WriteLine($"Checking for existing Client_Order with CustomerId: {CustomerId}");
            var existingOrder = await _context.ClientOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == CustomerId);

            ClientOrder orderRecord;
            if (existingOrder == null)
            {
                System.Diagnostics.Debug.WriteLine($"No existing Client_Order found for CustomerId: {CustomerId}. Creating new record.");
                orderRecord = new ClientOrder
                {
                    CustomerId = CustomerId,
                    OrderId = CustomerId
                };
                _context.ClientOrders.Add(orderRecord);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Created new Client_Order record. Id: {orderRecord.Id}, OrderId: {orderRecord.OrderId}");
            }
            else
            {
                orderRecord = existingOrder;
                System.Diagnostics.Debug.WriteLine($"Found existing Client_Order. Id: {orderRecord.Id}, OrderId: {orderRecord.OrderId}");
            }

            // Check if Operations record exists
            System.Diagnostics.Debug.WriteLine($"Checking for existing Operations record with CustomerId: {CustomerId}");
            var existingOperation = await _context.ModelDbInitOperations
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.CustomerId == CustomerId);

            ModelDbInitOperation operationRecord;
            if (existingOperation == null)
            {
                System.Diagnostics.Debug.WriteLine($"Creating new Model_DB_Init_Operations record for CustomerId: {CustomerId}");
                operationRecord = new ModelDbInitOperation
                {
                    CustomerId = CustomerId,
                    OrderId = CustomerId,
                    OperationsId = CustomerId,
                    Data = null
                };
                _context.ModelDbInitOperations.Add(operationRecord);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Created new Operations record. Id: {operationRecord.Id}");
            }
            else
            {
                operationRecord = existingOperation;
                System.Diagnostics.Debug.WriteLine($"Found existing Operations record. Id: {operationRecord.Id}");
            }

            // Check if QA record exists
            System.Diagnostics.Debug.WriteLine($"Checking for existing QA record with CustomerId: {CustomerId}");
            var existingQa = await _context.ModelDbInitQas
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.CustomerId == CustomerId);

            ModelDbInitQa qaRecord;
            if (existingQa == null)
            {
                System.Diagnostics.Debug.WriteLine($"Creating new Model_DB_Init_QA record for CustomerId: {CustomerId}");
                qaRecord = new ModelDbInitQa
                {
                    CustomerId = CustomerId,
                    OrderId = CustomerId,
                    Data = null
                };
                _context.ModelDbInitQas.Add(qaRecord);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Created new QA record. Id: {qaRecord.Id}");
            }
            else
            {
                qaRecord = existingQa;
                System.Diagnostics.Debug.WriteLine($"Found existing QA record. Id: {qaRecord.Id}");
            }

            // Check if OperationsStage1 record exists
            System.Diagnostics.Debug.WriteLine($"Checking for existing OperationsStage1 record with CustomerId: {CustomerId}");
            var existingOperationsStage1 = await _context.OperationsStage1s
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.CustomerId == CustomerId);

            OperationsStage1 operationsStage1Record;
            if (existingOperationsStage1 == null)
            {
                System.Diagnostics.Debug.WriteLine($"Creating new OperationsStage1 record for CustomerId: {CustomerId}");
                operationsStage1Record = new OperationsStage1
                {
                    CustomerId = CustomerId,
                    OrderId = CustomerId,
                    OperationsId = CustomerId,
                    OperationalId = CustomerId,
                    CsrOpartationalId = CustomerId,
                    SalesId = CustomerId,
                    SubServiceA = null,
                    SubServiceB = null,
                    SubServiceC = null,
                    SubProductA = null,
                    SubProductB = null,
                    SubProductC = null,
                    Data = null
                };

                _context.OperationsStage1s.Add(operationsStage1Record);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Created new OperationsStage1 record. Id: {operationsStage1Record.Id}");
            }
            else
            {
                operationsStage1Record = existingOperationsStage1;
                System.Diagnostics.Debug.WriteLine($"Found existing OperationsStage1 record. Id: {operationsStage1Record.Id}");
            }

            // Store complete objects in JIT memory
            System.Diagnostics.Debug.WriteLine("Storing complete records in JIT memory");
            Jit_Memory_Object.AddProperty("ClientOrderRecord", orderRecord);
            Jit_Memory_Object.AddProperty("OperationsRecord", operationRecord);
            Jit_Memory_Object.AddProperty("QaRecord", qaRecord);
            Jit_Memory_Object.AddProperty("OperationsStage1Record", operationsStage1Record);
            System.Diagnostics.Debug.WriteLine("Records stored in JIT memory successfully");


        }
























        private void ProcessFactoryTwo(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
           
            model.ModelDbInitModelData = true;

           



            // Retrieve OperationsStage1 record and Data
            System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Retrieving OperationsStage1 Record from JIT Memory");
            var operationsStage1Record = Jit_Memory_Object.GetProperty("OperationsStage1Record") as OperationsStage1;
            var operationsStage1Data = operationsStage1Record?.Data;
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved OperationsStage1 Data: {operationsStage1Data ?? "null"}");

            // Get and verify stored model information
            var storedId = Jit_Memory_Object.GetProperty("Id");
            var storedCustomerId = Jit_Memory_Object.GetProperty("CustomerId");
            var storedData = Jit_Memory_Object.GetProperty("Data") as byte[];

            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved stored Id: {storedId}");
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved stored CustomerId: {storedCustomerId}");
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved stored Data size: {storedData?.Length ?? 0} bytes");

            // Collect SubProduct fields from operationsStage1Record
            var stageSubProducts = new List<int?>();
            if (operationsStage1Record != null)
            {
                stageSubProducts.Add(operationsStage1Record.SubProductA);
                stageSubProducts.Add(operationsStage1Record.SubProductB);
                stageSubProducts.Add(operationsStage1Record.SubProductC);
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Collected {stageSubProducts.Count} SubProduct IDs from OperationsStage1");
            }

           






            // Filter products by stage SubProduct IDs
            var allSubProducts = Jit_Memory_Object.GetProperty("All_SubProducts") as List<dynamic>;
            var filteredProducts = allSubProducts?.Where(p => stageSubProducts.Contains((int)p.Id)).ToList();
            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Filtered products count: {filteredProducts?.Count ?? 0}");

            if (filteredProducts != null && filteredProducts.Any())
            {
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Retrieved All_SubProducts - Count: {filteredProducts.Count}");

                // Lists for processed data
                var combinedNames = new List<string>();
                var combinedPrices = new List<float>();

                try
                {
                    // Process each product
                    foreach (dynamic product in filteredProducts)
                    {
                        if (product == null) continue;

                        try
                        {
                            string productName = Convert.ToString(product.ProductName);
                            float productPrice = Convert.ToSingle(product.Price);

                            if (!string.IsNullOrEmpty(productName) && productPrice > 0)
                            {
                                combinedNames.Add(productName);
                                combinedPrices.Add(productPrice);
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Processed product - Name: {productName}, Price: {productPrice}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Error processing individual product: {ex.Message}");
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Successfully processed {combinedNames.Count} products");

                    // Store processed data in JIT memory
                    Jit_Memory_Object.AddProperty("Combined_Names", combinedNames);
                    Jit_Memory_Object.AddProperty("Combined_Prices", combinedPrices);

                    // Proceed only if we have valid data
                    if (combinedPrices.Any() && combinedNames.Any())
                    {
                        // Create price tensor
                        var priceTrainData = tf.convert_to_tensor(combinedPrices.ToArray(), dtype: TF_DataType.TF_FLOAT);
                        priceTrainData = tf.reshape(priceTrainData, new[] { -1, 1 });
                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Created price tensor with shape: {string.Join(", ", priceTrainData.shape)}");

                        // Create name tensor with one-hot encoding
                        var uniqueNames = combinedNames.Distinct().ToList();
                        var nameToIndex = uniqueNames.Select((productName, index) => new { productName, index })
                                             .ToDictionary(x => x.productName, x => x.index);
                        var nameIndices = combinedNames.Select(name => nameToIndex[name]).ToArray();
                        var oneHotNames = new float[nameIndices.Length, uniqueNames.Count];

                        // Perform one-hot encoding
                        for (int i = 0; i < nameIndices.Length; i++)
                        {
                            oneHotNames[i, nameIndices[i]] = 1.0f;
                        }

                        var nameTrainData = tf.convert_to_tensor(oneHotNames, dtype: TF_DataType.TF_FLOAT);
                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Created name tensor with shape: {string.Join(", ", nameTrainData.shape)}");

                        // Store tensors in JIT memory
                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_PriceTensor", priceTrainData);
                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_NameTensor", nameTrainData);

                        // Combine features for training
                        var combinedTrainData = tf.concat(new[] { priceTrainData, nameTrainData }, axis: 1);
                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Created combined tensor with shape: {string.Join(", ", combinedTrainData.shape)}");

                        // Initialize model parameters
                        var inputDim = 1 + uniqueNames.Count;
                        var W = tf.Variable(tf.random.normal(new[] { inputDim, 1 }));
                        var b = tf.Variable(tf.zeros(new[] { 1 }));

                        // Training parameters
                        int epochs = 100;
                        float learningRate = 1e-2f;
                        float convergenceThreshold = 1e-6f;
                        float previousLoss = float.MaxValue;
                        int stableEpochs = 0;

                        // Training loop with additional safeguards
                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            using (var tape = tf.GradientTape())
                            {
                                var predictions = tf.matmul(combinedTrainData, W) + b;
                                var loss = tf.reduce_mean(tf.square(predictions - priceTrainData));
                                float currentLoss = loss.numpy();

                                // Check for valid loss value
                                if (!float.IsNaN(currentLoss) && !float.IsInfinity(currentLoss))
                                {
                                    var gradients = tape.gradient(loss, new[] { W, b });
                                    W.assign_sub(gradients[0] * learningRate);
                                    b.assign_sub(gradients[1] * learningRate);

                                    // Convergence check
                                    if (Math.Abs(previousLoss - currentLoss) < convergenceThreshold)
                                    {
                                        stableEpochs++;
                                        if (stableEpochs >= 5)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Converged at epoch {epoch}");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        stableEpochs = 0;
                                    }

                                    previousLoss = currentLoss;

                                    if (epoch % 10 == 0)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Epoch {epoch}, Loss: {currentLoss}");
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Invalid loss detected at epoch {epoch}");
                                    learningRate *= 0.5f;
                                    if (learningRate < 1e-6f)
                                    {
                                        throw new Exception("Training unstable - learning rate too small");
                                    }
                                }
                            }
                        }

                        // Model serialization and merging
                        System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Starting model serialization and merging");
                        using (var memoryStream = new MemoryStream())
                        using (var writer = new BinaryWriter(memoryStream))
                        {
                            // Get original model data size
                            var originalSize = storedData?.Length ?? 0;
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Original model size: {originalSize} bytes");

                            // Write header information
                            writer.Write(DateTime.UtcNow.Ticks);  // Timestamp
                            writer.Write(originalSize);           // Size of original model

                            // Write stored model data if it exists
                            if (storedData != null && storedData.Length > 0)
                            {
                                writer.Write(storedData);
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Writing stored model data: {storedData.Length} bytes");
                            }

                            // Write new model identifier
                            writer.Write("MODEL_V2");  // Version identifier

                            // Write new model weights
                            var wData = W.numpy().ToArray<float>();
                            writer.Write(wData.Length);
                            foreach (var w in wData)
                            {
                                writer.Write(w);
                            }
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Writing new model weights: {wData.Length} elements");

                            // Write new model bias
                            var bData = b.numpy().ToArray<float>();
                            writer.Write(bData.Length);
                            foreach (var bias in bData)
                            {
                                writer.Write(bias);
                            }
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Writing new model bias: {bData.Length} elements");

                            // Write training metadata
                            writer.Write(inputDim);
                            writer.Write(uniqueNames.Count);
                            foreach (var uniqueName in uniqueNames)
                            {
                                writer.Write(uniqueName);
                            }

                            // Get merged model data
                            var mergedModelData = memoryStream.ToArray();
                            var newSize = mergedModelData.Length;

                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: New model size: {newSize} bytes");
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Size difference: {newSize - originalSize} bytes");

                            // Verify merged data
                            using (var verifyStream = new MemoryStream(mergedModelData))
                            using (var reader = new BinaryReader(verifyStream))
                            {
                                try
                                {
                                    var timestamp = reader.ReadInt64();  // Read timestamp
                                    var storedSize = reader.ReadInt32(); // Read stored size

                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Verifying merge - Timestamp: {new DateTime(timestamp).ToString("yyyy-MM-dd HH:mm:ss")}");
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Verifying merge - Original size matches: {storedSize == originalSize}");

                                    // Read and verify original model if it existed
                                    if (storedSize > 0)
                                    {
                                        var originalData = reader.ReadBytes(storedSize);
                                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Original model data verified: {originalData.Length == storedSize}");
                                    }

                                    // Verify new model identifier
                                    var modelVersion = reader.ReadString();
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Model version verified: {modelVersion}");

                                    // Verify weights and bias data
                                    var weightCount = reader.ReadInt32();
                                    var weights = new float[weightCount];
                                    for (int i = 0; i < weightCount; i++)
                                    {
                                        weights[i] = reader.ReadSingle();
                                    }

                                    var biasCount = reader.ReadInt32();
                                    var biases = new float[biasCount];
                                    for (int i = 0; i < biasCount; i++)
                                    {
                                        biases[i] = reader.ReadSingle();
                                    }

                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Weights verified: {weights.Length == wData.Length}");
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Biases verified: {biases.Length == bData.Length}");

                                    // Verify metadata
                                    var verifiedInputDim = reader.ReadInt32();
                                    var verifiedNameCount = reader.ReadInt32();
                                    var verifiedNames = new List<string>();
                                    for (int i = 0; i < verifiedNameCount; i++)
                                    {
                                        verifiedNames.Add(reader.ReadString());
                                    }

                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Input dimension verified: {verifiedInputDim == inputDim}");
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Unique names verified: {verifiedNames.Count == uniqueNames.Count}");

                                    // Store verified merged data
                                    if (verifiedInputDim == inputDim && verifiedNames.Count == uniqueNames.Count)
                                    {
                                        // Update the stored data with merged model
                                        storedData = mergedModelData;

                                        // Store new model data in separate property
                                        Jit_Memory_Object.AddProperty("ProcessStageTwo_Data", mergedModelData);
                                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Created ProcessStageTwo_Data. Size: {mergedModelData.Length} bytes");

                                        // Store all components
                                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_ModelWeights", wData);
                                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_ModelBias", bData);
                                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_ModelData", mergedModelData);
                                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_InputDim", inputDim);
                                        Jit_Memory_Object.AddProperty("ProcessFactoryTwo_UniqueNames", uniqueNames);

                                        // Update model object
                                        model.Data = storedData;

                                        System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Model merge verified and all storage locations updated successfully");

                                        // Verify the update
                                        var verifyStoredData = Jit_Memory_Object.GetProperty("ProcessStageTwo_Data") as byte[];
                                        if (verifyStoredData?.Length == mergedModelData.Length)
                                        {
                                            System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: ProcessStageTwo_Data verification successful");
                                        }
                                        else
                                        {
                                            throw new Exception("Failed to verify ProcessStageTwo_Data update");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Model merge verification failed");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Model merge verification failed: {ex.Message}");
                                    throw new Exception("Failed to verify merged model data", ex);
                                }
                            }
                        }

                        System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Machine learning implementation completed successfully");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: No valid data for tensor creation");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryTwo: Error during processing: {ex.Message}");
                    throw;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: No AllSubProducts found in JIT memory");
            }

            System.Diagnostics.Debug.WriteLine("ProcessFactoryTwo: Adding Stage2Complete property");
            Jit_Memory_Object.AddProperty("Stage2Complete", true);
        }




























        private void ProcessFactoryThree(ModelDbInit model, int id, string name, string productType, Jit_Memory_Object jitObject)
        {
            try
            {
                model.ModelDbInitModelData = true;

                // Retrieve OperationsStage1 record and Data
                System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Retrieving OperationsStage1 Record from JIT Memory");
                var operationsStage1Record = Jit_Memory_Object.GetProperty("OperationsStage1Record") as OperationsStage1;
                var operationsStage1Data = operationsStage1Record?.Data;
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Retrieved OperationsStage1 Data: {operationsStage1Data ?? "null"}");

                // Get and verify stored model information
                var storedIdPT3 = Jit_Memory_Object.GetProperty("Id");
                var storedCustomerIdPT3 = Jit_Memory_Object.GetProperty("CustomerId");
                var storedDataPT3 = Jit_Memory_Object.GetProperty("Data") as byte[];

                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Retrieved stored Id: {storedIdPT3}");
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Retrieved stored CustomerId: {storedCustomerIdPT3}");
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Retrieved stored Data size: {storedDataPT3?.Length ?? 0} bytes");

                // Collect SubService fields from operationsStage1Record
                var stageSubServices = new List<int?>();
                if (operationsStage1Record != null)
                {
                    stageSubServices.Add(operationsStage1Record.SubServiceA);
                    stageSubServices.Add(operationsStage1Record.SubServiceB);
                    stageSubServices.Add(operationsStage1Record.SubServiceC);
                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Collected {stageSubServices.Count} SubService IDs from OperationsStage1");
                }

                // Filter services by stage SubService IDs
                var allServices = Jit_Memory_Object.GetProperty("All_SubServices") as List<dynamic>;
                var filteredServices = allServices?.Where(s => stageSubServices.Contains((int)s.Id)).ToList();
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Filtered services count: {filteredServices?.Count ?? 0}");

                if (filteredServices != null && filteredServices.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Retrieved All_SubServices - Count: {filteredServices.Count}");

                    // Lists for processed data
                    var combinedNamesPT3 = new List<string>();
                    var combinedPricesPT3 = new List<float>();

                    try
                    {
                        // Process each service
                        foreach (dynamic service in filteredServices)
                        {
                            if (service == null) continue;

                            try
                            {
                                string serviceName = Convert.ToString(service.ServiceName);
                                float servicePrice = Convert.ToSingle(service.Price);

                                if (!string.IsNullOrEmpty(serviceName) && servicePrice > 0)
                                {
                                    combinedNamesPT3.Add(serviceName);
                                    combinedPricesPT3.Add(servicePrice);
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Processed service - Name: {serviceName}, Price: {servicePrice}");
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Error processing individual service: {ex.Message}");
                                continue;
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Successfully processed {combinedNamesPT3.Count} services");

                        // Store processed data in JIT memory with PT3 suffix
                        Jit_Memory_Object.AddProperty("Combined_Names_PT3", combinedNamesPT3);
                        Jit_Memory_Object.AddProperty("Combined_Prices_PT3", combinedPricesPT3);

                        if (combinedPricesPT3.Any() && combinedNamesPT3.Any())
                        {
                            // Create price tensor
                            var priceTrainDataPT3 = tf.convert_to_tensor(combinedPricesPT3.ToArray(), dtype: TF_DataType.TF_FLOAT);
                            priceTrainDataPT3 = tf.reshape(priceTrainDataPT3, new[] { -1, 1 });
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Created price tensor with shape: {string.Join(", ", priceTrainDataPT3.shape)}");

                            // Create name tensor with one-hot encoding
                            var uniqueNamesPT3 = combinedNamesPT3.Distinct().ToList();
                            var nameToIndexPT3 = uniqueNamesPT3.Select((serviceName, index) => new { serviceName, index })
                                                  .ToDictionary(x => x.serviceName, x => x.index);
                            var nameIndicesPT3 = combinedNamesPT3.Select(name => nameToIndexPT3[name]).ToArray();
                            var oneHotNamesPT3 = new float[nameIndicesPT3.Length, uniqueNamesPT3.Count];

                            for (int i = 0; i < nameIndicesPT3.Length; i++)
                            {
                                oneHotNamesPT3[i, nameIndicesPT3[i]] = 1.0f;
                            }

                            var nameTrainDataPT3 = tf.convert_to_tensor(oneHotNamesPT3, dtype: TF_DataType.TF_FLOAT);
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Created name tensor with shape: {string.Join(", ", nameTrainDataPT3.shape)}");

                            // Store tensors in JIT memory
                            Jit_Memory_Object.AddProperty("ProcessFactoryThree_PriceTensor", priceTrainDataPT3);
                            Jit_Memory_Object.AddProperty("ProcessFactoryThree_NameTensor", nameTrainDataPT3);

                            // Combine features for training
                            var combinedTrainDataPT3 = tf.concat(new[] { priceTrainDataPT3, nameTrainDataPT3 }, axis: 1);
                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Created combined tensor with shape: {string.Join(", ", combinedTrainDataPT3.shape)}");

                            // Initialize model parameters
                            var inputDimPT3 = 1 + uniqueNamesPT3.Count;
                            var WPT3 = tf.Variable(tf.random.normal(new[] { inputDimPT3, 1 }), name: "WPT3");
                            var bPT3 = tf.Variable(tf.zeros(new[] { 1 }), name: "bPT3");

                            // Training parameters
                            int epochsPT3 = 100;
                            float learningRatePT3 = 1e-2f;
                            float convergenceThresholdPT3 = 1e-6f;
                            float previousLossPT3 = float.MaxValue;
                            int stableEpochsPT3 = 0;

                            for (int epoch = 0; epoch < epochsPT3; epoch++)
                            {
                                using (var tape = tf.GradientTape())
                                {
                                    var predictionsPT3 = tf.matmul(combinedTrainDataPT3, WPT3) + bPT3;
                                    var lossPT3 = tf.reduce_mean(tf.square(predictionsPT3 - priceTrainDataPT3));
                                    float currentLoss = lossPT3.numpy();

                                    if (!float.IsNaN(currentLoss) && !float.IsInfinity(currentLoss))
                                    {
                                        var gradients = tape.gradient(lossPT3, new[] { WPT3, bPT3 });
                                        WPT3.assign_sub(gradients[0] * learningRatePT3);
                                        bPT3.assign_sub(gradients[1] * learningRatePT3);

                                        if (Math.Abs(previousLossPT3 - currentLoss) < convergenceThresholdPT3)
                                        {
                                            stableEpochsPT3++;
                                            if (stableEpochsPT3 >= 5)
                                            {
                                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Converged at epoch {epoch}");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            stableEpochsPT3 = 0;
                                        }

                                        previousLossPT3 = currentLoss;

                                        if (epoch % 10 == 0)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Epoch {epoch}, Loss: {currentLoss}");
                                        }
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Invalid loss detected at epoch {epoch}");
                                        learningRatePT3 *= 0.5f;
                                        if (learningRatePT3 < 1e-6f)
                                        {
                                            throw new Exception("Training unstable - learning rate too small");
                                        }
                                    }
                                }
                            }

                            // Model serialization
                            System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Starting model serialization");
                            using (var memoryStream = new MemoryStream())
                            using (var writer = new BinaryWriter(memoryStream))
                            {
                                var originalSize = storedDataPT3?.Length ?? 0;
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Original model size: {originalSize} bytes");

                                writer.Write(DateTime.UtcNow.Ticks);
                                writer.Write(originalSize);

                                if (storedDataPT3 != null && storedDataPT3.Length > 0)
                                {
                                    writer.Write(storedDataPT3);
                                    System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Writing stored model data: {storedDataPT3.Length} bytes");
                                }

                                writer.Write("MODEL_V2_PT3");

                                var wDataPT3 = WPT3.numpy().ToArray<float>();
                                writer.Write(wDataPT3.Length);
                                foreach (var w in wDataPT3)
                                {
                                    writer.Write(w);
                                }
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Writing new model weights: {wDataPT3.Length} elements");

                                var bDataPT3 = bPT3.numpy().ToArray<float>();
                                writer.Write(bDataPT3.Length);
                                foreach (var bias in bDataPT3)
                                {
                                    writer.Write(bias);
                                }
                                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Writing new model bias: {bDataPT3.Length} elements");

                                writer.Write(inputDimPT3);
                                writer.Write(uniqueNamesPT3.Count);
                                foreach (var uniqueName in uniqueNamesPT3)
                                {
                                    writer.Write(uniqueName);
                                }

                                var mergedModelDataPT3 = memoryStream.ToArray();

                                // Store all components
                                Jit_Memory_Object.AddProperty("ProcessStageThree_Data", mergedModelDataPT3);
                                Jit_Memory_Object.AddProperty("ProcessFactoryThree_ModelWeights", wDataPT3);
                                Jit_Memory_Object.AddProperty("ProcessFactoryThree_ModelBias", bDataPT3);
                                Jit_Memory_Object.AddProperty("ProcessFactoryThree_InputDim", inputDimPT3);
                                Jit_Memory_Object.AddProperty("ProcessFactoryThree_UniqueNames", uniqueNamesPT3);

                                System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Model serialization completed successfully");
                            }

                            System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Machine learning implementation completed successfully");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: No valid data for tensor creation");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Error during processing: {ex.Message}");
                        throw;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: No services found in JIT memory");
                }

                System.Diagnostics.Debug.WriteLine("ProcessFactoryThree: Adding Stage3Complete property");
                Jit_Memory_Object.AddProperty("Stage3Complete", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProcessFactoryThree: Fatal error: {ex.Message}");
                throw;
            }
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