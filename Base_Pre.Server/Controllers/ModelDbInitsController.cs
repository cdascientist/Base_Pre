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
            System.Diagnostics.Debug.WriteLine($"Model fetch completed: {(ML_Model != null ? "Found" : "Not Found")}");

            if (ML_Model == null)
            {
                System.Diagnostics.Debug.WriteLine($"No existing model found for ProductType {productType}. Initializing new model creation.");
                model.ModelDbInitModelData = false;
                Jit_Memory_Object.AddProperty("NewModelRequired", true);

                try
                {
                    System.Diagnostics.Debug.WriteLine($"Starting subproduct data collection for ProductType {productType}");

                    // Execute queries sequentially to avoid DbContext threading issues
                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct A data");
                    var subproductsA = await _context.SubProductAs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsA.Count} SubProduct A records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct B data");
                    var subproductsB = await _context.SubProductBs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsB.Count} SubProduct B records");

                    System.Diagnostics.Debug.WriteLine("Fetching SubProduct C data");
                    var subproductsC = await _context.SubProductCs
                        .AsNoTracking()
                        .Where(p => p.ProductType == productType)
                        .ToListAsync();
                    System.Diagnostics.Debug.WriteLine($"Found {subproductsC.Count} SubProduct C records");

                    // Store lists in JIT memory
                    Jit_Memory_Object.AddProperty("SubProductsA", subproductsA);
                    Jit_Memory_Object.AddProperty("SubProductsB", subproductsB);
                    Jit_Memory_Object.AddProperty("SubProductsC", subproductsC);

                    model.ModelDbInitModelData = (subproductsA.Any() ||
                                                subproductsB.Any() ||
                                                subproductsC.Any());

                    System.Diagnostics.Debug.WriteLine("Subproduct data collection completed");
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