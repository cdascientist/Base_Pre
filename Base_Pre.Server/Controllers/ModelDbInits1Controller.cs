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
using Tensorflow; // Use actual TensorFlow.NET library
using Tensorflow.NumPy;
using static Tensorflow.Binding;
// Essential framework and utility imports for creating the web controller,
// managing concurrent data structures, handling asynchronous operations,
// performing I/O, using LINQ for data querying, and reflection/dynamic features.
// These form the foundational dependencies for the application's execution environment.
// Added: TensorFlow.NET import for actual ML operations.
using Accord.MachineLearning;
using Accord.Math.Distances;

using System.Text;
using System.Numerics;
using Accord.IO;

namespace HighlyDescriptiveArbitraryNamespace.OperationalComponents
{
    // Defines a highly descriptive, arbitrary namespace for the operational components.

    #region Data Structure Representations

    // This region defines the structures representing various data records used within the system.
    // These structures act as schemas for the simulated data storage and in-memory data handling,
    // serving as primary data dependencies for the processing logic.

    /// <summary>
    /// Represents the primary record storing outcomes of the ML initiation process.
    /// This is the central data entity being generated and updated throughout the workflow.
    /// </summary>
    public class CoreMlOutcomeRecord
    {
        // Properties define the fields of the core outcome record.
        // These fields hold identifiers, timestamps, categorical information,
        // serialized model data, and resulting vectors.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // Updated by SequentialInitialProcessingUnitC (Factory One) with simulated model data.
        // Updated by SequentialFinalProcessingUnitD (Factory Four) with final vector results.
        // Referenced by ParallelProcessingUnitA (Factory Two) and ParallelProcessingUnitB (Factory Three).
        // Retrieved as the final output by the orchestrating controller method.

        public int RecordIdentifier { get; set; } // Unique identifier for the record
        public int? AssociatedCustomerIdentifier { get; set; } // Link to a customer entity
        public DateTime? OutcomeGenerationTimestamp { get; set; } // Timestamp of when the outcome was generated/updated
        public int? CategoricalClassificationIdentifier { get; set; } // Identifier for a categorical classification
        public string? CategoricalClassificationDescription { get; set; } // Descriptive name for the classification
        public byte[]? SerializedSimulatedModelData { get; set; } // Binary data representing the trained simulated model (weights)
        public byte[]? AncillaryBinaryDataPayload { get; set; } // Additional binary data payload (bias)
        public string? DerivedProductFeatureVector { get; set; } // String representation of the resulting product vector
        public string? DerivedServiceBenefitVector { get; set; } // String representation of the resulting service vector
    }

    /// <summary>
    /// Represents contextual information about an associated customer.
    /// A supplementary record linked to the customer identifier.
    /// </summary>
    public class AssociatedCustomerContext
    {
        // Properties define the fields for customer context.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // Referenced during the workflow for context.
        public int ContextIdentifier { get; set; }
        public string? CustomerPrimaryGivenName { get; set; }
        public string? CustomerFamilyName { get; set; }
        public string? CustomerContactPhoneNumber
        {
            get; set;
        }
        public string? CustomerStreetAddress { get; set; }
        public int? CustomerLinkIdentifier { get; set; } // Foreign key linking to a customer concept
        public string? AffiliatedCompanyName { get; set; }
    }

    /// <summary>
    /// Represents a specific work order associated with a customer.
    /// Another supplementary record linked to the customer and potentially an order identifier.
    /// </summary>
    public class OperationalWorkOrderRecord
    {
        // Properties define the fields for a work order record.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // Referenced during the workflow for context.
        public int OrderRecordIdentifier { get; set; }
        public int? CustomerLinkIdentifier { get; set; }
        public int? SpecificOrderIdentifier { get; set; }
    }

    /// <summary>
    /// Represents an event or operation related to the ML initiation.
    /// Records actions or metadata during the process.
    /// </summary>
    public class MlInitialOperationEvent
    {
        // Properties define the fields for an operational event record.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // Referenced during the workflow.
        public int EventIdentifier { get; set; }
        public int? CustomerLinkIdentifier { get; set; }
        public int? RelatedOrderIdentifier { get; set; }
        public int? InternalOperationIdentifier { get; set; }
        public byte[]? EventPayloadData { get; set; } // Binary data specific to the event
    }

    /// <summary>
    /// Represents data resulting from Quality Assurance (QA) checks on the ML outcome.
    /// Stores validation results related to the process.
    /// </summary>
    public class MlOutcomeValidationRecord
    {
        // Properties define the fields for a validation record.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // Referenced during the workflow.
        public int ValidationRecordIdentifier { get; set; }
        public int? CustomerLinkIdentifier { get; set; }
        public int? RelatedOrderIdentifier { get; set; }
        public byte[]? ValidationResultData { get; set; } // Binary data containing QA results
    }

    /// <summary>
    /// Represents data collected or generated during the initial operational stage.
    /// Contains details about related products, services, and vectors for this stage.
    /// </summary>
    public class InitialOperationalStageData
    {
        // Properties define the fields for initial stage data.
        // Operational Process Dependency: Created/retrieved by SequentialInitialProcessingUnitC (Factory One).
        // May be used as input or context for ML steps.
        public int StageIdentifier { get; set; }
        public int? CustomerLinkIdentifier { get; set; }
        public int? RelatedOrderIdentifier { get; set; }
        public int? InternalOperationIdentifier { get; set; }
        public int? ProcessOperationalIdentifier { get; set; }
        public int? CustomerServiceOperationIdentifier { get; set; }
        public int? SalesProcessIdentifier { get; set; }
        public int? LinkedSubServiceA { get; set; } // Links to simulated service data
        public int? LinkedSubServiceB { get; set; } // Links to simulated service data
        public int? LinkedSubServiceC { get; set; } // Links to simulated service data
        public int? LinkedSubProductA { get; set; } // Links to simulated product data
        public int? LinkedSubProductB { get; set; } // Links to simulated product data
        public int? LinkedSubProductC { get; set; } // Links to simulated product data
        public string? StageSpecificData { get; set; } // String data for this stage
        public string? StageProductVectorSnapshot { get; set; } // Product vector state at this stage
        public string? StageServiceVectorSnapshot { get; set; } // Service vector state at this stage
    }

    #endregion

    #region TensorFlow.NET Components (Actual Implementation)

    // This region contains actual TensorFlow.NET components for ML operations.
    // These replace the simulated components and provide the functional dependencies
    // for the processing units that perform ML tasks.

    // Removed SimulatedMlTensor, SimulatedMlInputFeedEntry, SimulatedNdArray, SimulatedMlSession,
    // SimulatedGradientAdjustmentMechanism, SimulatedRandomDistributionSource, SimulatedMlEngine
    // as they are replaced by TensorFlow.NET types/methods.

    #endregion

    #region Persistent And Transient Storage

    // This region defines static classes simulating data persistence and runtime memory management.
    // These components provide the data storage dependencies for the processing factories.

    /// <summary>
    /// Static class simulating persistent data storage (like a database) for testing.
    /// Holds various lists of data records.
    /// </summary>
    public static class InMemoryTestDataSet
    {
        // Holds static lists simulating database tables.
        // Operational Process Dependency: SequentialInitialProcessingUnitC reads from and writes to these lists.
        // InitiateMlOutcomeGeneration reads the final result from SimulatedCoreOutcomes.
        // GetOutcomeRecordByIdentifier and GetAllOutcomeRecords endpoints read from SimulatedCoreOutcomes.
        // The controller constructor populates RuntimeProcessingContext with sample product/service data from here.

        /// <summary>
        /// Simulated static list representing the collection of CoreMlOutcomeRecord entities.
        /// </summary>
        public static readonly List<CoreMlOutcomeRecord> SimulatedCoreOutcomes = new List<CoreMlOutcomeRecord>
        {
            // Initial dummy data representing existing records.
            // Operational Process Dependency: The source for retrieving existing records by SequentialInitialProcessingUnitC.
            // The destination for saving new or updated records by SequentialInitialProcessingUnitC and SequentialFinalProcessingUnitD.
            new CoreMlOutcomeRecord
            {
                RecordIdentifier = 1,
                AssociatedCustomerIdentifier = 123,
                OutcomeGenerationTimestamp = DateTime.UtcNow.AddDays(-10),
                CategoricalClassificationIdentifier = 5,
                CategoricalClassificationDescription = "Initial Historical Category",
                SerializedSimulatedModelData = new byte[] { 1, 2, 3, 4 }, // Placeholder binary data
                AncillaryBinaryDataPayload = new byte[] { 5, 6, 7, 8 }, // Placeholder binary data
                DerivedProductFeatureVector = "P_Vect_Init: X=1.0, Y=2.0, Z=3.0", // Sample vector data
                DerivedServiceBenefitVector = "S_Vect_Init: X=4.0, Y=5.0, Z=6.0" // Sample vector data
            },
            new CoreMlOutcomeRecord
            {
                RecordIdentifier = 2,
                AssociatedCustomerIdentifier = 456,
                OutcomeGenerationTimestamp = DateTime.UtcNow.AddDays(-1),
                CategoricalClassificationIdentifier = 10,
                CategoricalClassificationDescription = "Second Recent Category",
                SerializedSimulatedModelData = new byte[] { 9, 10, 11, 12 }, // Placeholder binary data
                AncillaryBinaryDataPayload = new byte[] { 13, 14, 15, 16 }, // Placeholder binary data
                DerivedProductFeatureVector = "P_Vect_Init: X=10.0, Y=11.0, Z=12.0", // Sample vector data
                DerivedServiceBenefitVector = "S_Vect_Init: X=13.0, Y=14.0, Z=15.0" // Sample vector data
            }
        };

        /// <summary>
        /// Simulated static list representing customer context records.
        /// </summary>
        public static readonly List<AssociatedCustomerContext> SimulatedCustomerContexts = new List<AssociatedCustomerContext>
        {
             // Initial dummy data.
            // Operational Process Dependency: SequentialInitialProcessingUnitC checks/adds records here, linked to the customer.
            new AssociatedCustomerContext
            {
                ContextIdentifier = 1,
                CustomerPrimaryGivenName = "John",
                CustomerFamilyName = "Doe",
                CustomerContactPhoneNumber = "555-1234-Sim",
                CustomerStreetAddress = "123 Main St Sim",
                CustomerLinkIdentifier = 123,
                AffiliatedCompanyName = "Acme Inc. Sim"
            },
            new AssociatedCustomerContext
            {
                ContextIdentifier = 2,
                CustomerPrimaryGivenName = "Jane",
                CustomerFamilyName = "Smith",
                CustomerContactPhoneNumber = "555-5678-Sim",
                CustomerStreetAddress = "456 Elm St Sim",
                CustomerLinkIdentifier = 456,
                AffiliatedCompanyName = "XYZ Corp Sim"
            }
        };

        /// <summary>
        /// Simulated static list representing operational work order records.
        /// </summary>
        public static readonly List<OperationalWorkOrderRecord> SimulatedWorkOrders = new List<OperationalWorkOrderRecord>
        {
            // Initial dummy data.
            // Operational Process Dependency: SequentialInitialProcessingUnitC checks/adds records here, linked to the customer/order.
            new OperationalWorkOrderRecord
            {
                OrderRecordIdentifier = 1,
                CustomerLinkIdentifier = 123,
                SpecificOrderIdentifier = 1001
            },
            new OperationalWorkOrderRecord
            {
                OrderRecordIdentifier = 2,
                CustomerLinkIdentifier = 456,
                SpecificOrderIdentifier = 1002
            }
        };

        /// <summary>
        /// Simulated static list representing ML initial operation events.
        /// </summary>
        public static readonly List<MlInitialOperationEvent> SimulatedOperationalEvents = new List<MlInitialOperationEvent>
        {
            // Initial dummy data.
            // Operational Process Dependency: SequentialInitialProcessingUnitC checks/adds records here, linked to the model initialization.
            new MlInitialOperationEvent
            {
                EventIdentifier = 1,
                CustomerLinkIdentifier = 123,
                RelatedOrderIdentifier = 1001,
                InternalOperationIdentifier = 5001,
                EventPayloadData = new byte[] { 21, 22, 23, 24 } // Placeholder binary data
            },
            new MlInitialOperationEvent
            {
                EventIdentifier = 2,
                CustomerLinkIdentifier = 456,
                RelatedOrderIdentifier = 1002,
                InternalOperationIdentifier = 5002,
                EventPayloadData = new byte[] { 25, 26, 27, 28 } // Placeholder binary data
            }
        };

        /// <summary>
        /// Simulated static list representing ML outcome validation records.
        /// </summary>
        public static readonly List<MlOutcomeValidationRecord> SimulatedOutcomeValidations = new List<MlOutcomeValidationRecord>
        {
            // Initial dummy data.
            // Operational Process Dependency: SequentialInitialProcessingUnitC checks/adds records here, linked to the model initialization.
            new MlOutcomeValidationRecord
            {
                ValidationRecordIdentifier = 1,
                CustomerLinkIdentifier = 123,
                RelatedOrderIdentifier = 1001,
                ValidationResultData = new byte[] { 31, 32, 33, 34 } // Placeholder binary data
            },
            new MlOutcomeValidationRecord
            {
                ValidationRecordIdentifier = 2,
                CustomerLinkIdentifier = 456,
                RelatedOrderIdentifier = 1002,
                ValidationResultData = new byte[] { 35, 36, 37, 38 } // Placeholder binary data
            }
        };

        /// <summary>
        /// Simulated static list representing initial operational stage data records.
        /// </summary>
        public static readonly List<InitialOperationalStageData> SimulatedInitialOperationalStages = new List<InitialOperationalStageData>
        {
            // Initial dummy data.
            // Operational Process Dependency: SequentialInitialProcessingUnitC checks/adds records here, linked to the operation stage.
            new InitialOperationalStageData
            {
                StageIdentifier = 1,
                CustomerLinkIdentifier = 123,
                RelatedOrderIdentifier = 1001,
                InternalOperationIdentifier = 5001,
                ProcessOperationalIdentifier = 6001,
                CustomerServiceOperationIdentifier = 7001,
                SalesProcessIdentifier = 8001,
                LinkedSubServiceA = 1, // Links to SampleServiceOfferings
                LinkedSubServiceB = 2, // Links to SampleServiceOfferings
                LinkedSubServiceC = 3, // Links to SampleServiceOfferings
                LinkedSubProductA = 4, // Links to SampleProductInventory
                LinkedSubProductB = 5, // Links to SampleProductInventory
                LinkedSubProductC = 6, // Links to SampleProductInventory
                StageSpecificData = "Sample data for initial operational stage record 1",
                StageProductVectorSnapshot = "Stage1_P_Vect: X=1.0, Y=2.0, Z=3.0",
                StageServiceVectorSnapshot = "Stage1_S_Vect: X=4.0, Y=5.0, Z=6.0"
            },
            new InitialOperationalStageData
            {
                StageIdentifier = 2,
                CustomerLinkIdentifier = 456,
                RelatedOrderIdentifier = 1002,
                InternalOperationIdentifier = 5002,
                ProcessOperationalIdentifier = 6002,
                CustomerServiceOperationIdentifier = 7002,
                SalesProcessIdentifier = 8002,
                LinkedSubServiceA = 7, // Links to SampleServiceOfferings
                LinkedSubServiceB = 8, // Links to SampleServiceOfferings
                LinkedSubServiceC = 9, // Links to SampleServiceOfferings
                LinkedSubProductA = 10, // Links to SampleProductInventory
                LinkedSubProductB = 11, // Links to SampleProductInventory
                LinkedSubProductC = 12, // Links to SampleProductInventory
                StageSpecificData = "Sample data for initial operational stage record 2",
                StageProductVectorSnapshot = "Stage1_P_Vect: X=10.0, Y=11.0, Z=12.0",
                StageServiceVectorSnapshot = "Stage1_S_Vect: X=13.0, Y=14.0, Z=15.0"
            }
        };

        /// <summary>
        /// Simulated product data entries for testing.
        /// Represents a separate dataset linked by ID.
        /// </summary>
        public static readonly List<dynamic> SampleProductInventory = new List<dynamic>
        {
            // Dummy product data.
            // Operational Process Dependency: Loaded into RuntimeProcessingContext by the controller constructor.
            // Could potentially be used by processing units to retrieve details based on LinkedSubProduct IDs in InitialOperationalStageData.
            new { Identifier = 1, ItemDesignation = "Product A Alpha", Categorization = "Type 1 Assembly", QuantityAvailable = 10, MonetaryValue = 99.99, CostContributionValue = 0.15 },
            new { Identifier = 2, ItemDesignation = "Product B Beta", Categorization = "Type 2 Component", QuantityAvailable = 20, MonetaryValue = 149.99, CostContributionValue = 0.25 },
            new { Identifier = 3, ItemDesignation = "Product C Gamma", Categorization = "Type 3 Module", QuantityAvailable = 15, MonetaryValue = 199.99, CostContributionValue = 0.20 }
        };

        /// <summary>
        /// Simulated service data entries for testing.
        /// Represents a separate dataset linked by ID.
        /// </summary>
        public static readonly List<dynamic> SampleServiceOfferings = new List<dynamic>
        {
            // Dummy service data.
            // Operational Process Dependency: Loaded into RuntimeProcessingContext by the controller constructor.
            // Could potentially be used by processing units to retrieve details based on LinkedSubService IDs in InitialOperationalStageData.
            new { Identifier = 1, ServiceNameDescriptor = "Service A Alpha", Categorization = "Tier 1 Support", FulfillmentQuantity = 5, MonetaryValue = 299.99, CostContributionValue = 0.30 },
            new { Identifier = 2, ServiceNameDescriptor = "Service B Beta", Categorization = "Tier 2 Consulting", FulfillmentQuantity = 10, MonetaryValue = 399.99, CostContributionValue = 0.35 },
            new { Identifier = 3, ServiceNameDescriptor = "Service C Gamma", Categorization = "Tier 3 Managed", FulfillmentQuantity = 8, MonetaryValue = 599.99, CostContributionValue = 0.40 }
        };
    }

    /// <summary>
    /// Static class simulating transient, runtime memory storage accessible within a request context.
    /// Uses a dynamic object to hold properties added during the request lifecycle.
    /// </summary>
    public static class RuntimeProcessingContext
    {
        // Provides static, request-scoped (conceptually, though globally static here) storage using ExpandoObject.
        // Operational Process Dependency: SequentialInitialProcessingUnitC stores references to created/found records and intermediate data here.
        // The controller constructor stores initial product/service data here.
        // This allows different processing units (factories) to share state and data during the execution of a single API call.
        private static readonly ExpandoObject _volatileKeyValueStore = new ExpandoObject();
        private static readonly dynamic _dynamicAccessView = _volatileKeyValueStore;
        private static RuntimeMethodHandle _cachedMethodHandle; // Not directly used in the core ML flow demonstrated

        /// <summary>
        /// Adds or updates a value associated with a string key in the runtime storage.
        /// </summary>
        /// <param name="keyDescriptor">The string descriptor for the value</param>
        /// <param name="contextValue">The value to store</param>
        public static void StoreContextValue(string keyDescriptor, object contextValue)
        {
            // Stores data for later retrieval in the same request.
            // Operational Process Dependency: Called by SequentialInitialProcessingUnitC to store references to records and simulated model data.
            // Called by the controller constructor to store initial data from InMemoryTestDataSet.
            var dictionary = (IDictionary<string, object>)_volatileKeyValueStore;
            dictionary[keyDescriptor] = contextValue;
        }

        /// <summary>
        /// Retrieves a value from the runtime storage using its string key descriptor.
        /// Returns null if the key descriptor does not exist.
        /// </summary>
        /// <param name="keyDescriptor">The string descriptor for the value</param>
        /// <returns>The stored value, or null if the key was not found</returns>
        public static object? RetrieveContextValue(string keyDescriptor)
        {
            // Retrieves data stored earlier in the same request.
            // Operational Process Dependency: Called by SequentialInitialProcessingUnitC for verification and potentially by other units
            // if they needed data stored by SequentialInitialProcessingUnitC (e.g., retrieving the CoreMlOutcomeRecord).
            var dictionary = (IDictionary<string, object>)_volatileKeyValueStore;
            return dictionary.TryGetValue(keyDescriptor, out var value) ? value : null;
        }

        /// <summary>
        /// Gets a dynamic view of the runtime storage for flexible access.
        /// </summary>
        public static dynamic DynamicContextView => _dynamicAccessView;
        // Provides direct dynamic access to the underlying ExpandoObject.

        /// <summary>
        /// Sets a cached method handle (not directly used in ML flow).
        /// </summary>
        /// <param name="handle">The RuntimeMethodHandle to cache</param>
        public static void CacheMethodHandle(RuntimeMethodHandle handle)
        {
            _cachedMethodHandle = handle;
        }
        // Not directly involved in the core ML process flow demonstrated.

        /// <summary>
        /// Gets the stored cached method handle (not directly used in ML flow).
        /// </summary>
        /// <returns>The RuntimeMethodHandle</returns>
        public static RuntimeMethodHandle RetrieveCachedMethodHandle()
        {
            return _cachedMethodHandle;
        }
        // Not directly involved in the core ML process flow demonstrated.
    }

    /// <summary>
    /// Component intended for orchestrating concurrent processing tasks.
    /// Manages shared resources for parallel operations (currently minimal).
    /// </summary>
    public class ConcurrentOperationManager
    {
        // Intended for managing shared state or coordination for parallel tasks.
        // Operational Process Dependency: An instance is held by the controller. Its ResetSharedDataStore method is available (though not explicitly called in this flow).
        // The ConcurrentDictionary within it *could* be used for thread-safe sharing, similar to the results dictionaries used in ParallelProcessingUnitA/B/SequentialFinalProcessingUnitD.
        private readonly ConcurrentDictionary<string, object> _parallelSharedDataStore = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Clears data within the shared data store.
        /// </summary>
        public void ResetSharedDataStore()
        {
            _parallelSharedDataStore.Clear();
        }
    }

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    // Defines the API route and indicates this class is an API controller.
    // This class orchestrates the entire ML outcome generation process triggered by the endpoint.
    public class MlProcessOrchestrationController : ControllerBase
    {
        private static int _requestSessionSequenceCounter = 0; // Counter for generating unique request session IDs
        private readonly ConcurrentDictionary<int, Session> _activeMlSessions; // Tracks active actual ML sessions by unique ID
        private readonly ConcurrentOperationManager _operationConcurrencyManager; // Instance of the concurrency manager

        /// <summary>
        /// Constructor for the ML process orchestration controller.
        /// Initializes session tracking and the concurrency manager.
        /// Populates the runtime processing context with initial simulated data.
        /// </summary>
        public MlProcessOrchestrationController()
        {
            // Initializes the concurrent dictionary for tracking actual ML sessions and the concurrency manager.
            _activeMlSessions = new ConcurrentDictionary<int, Session>();
            _operationConcurrencyManager = new ConcurrentOperationManager();

            // Initialize runtime memory with simulated data from the test dataset.
            // This makes simulated product/service data easily accessible to processing logic via RuntimeProcessingContext.
            // Operational Process Dependency: Data loaded here can be retrieved via RuntimeProcessingContext.RetrieveContextValue
            // by processing units that need lookups based on IDs from InitialOperationalStageData.
            RuntimeProcessingContext.StoreContextValue("All_Simulated_Service_Offerings", InMemoryTestDataSet.SampleServiceOfferings);
            RuntimeProcessingContext.StoreContextValue("All_Simulated_Product_Inventory", InMemoryTestDataSet.SampleProductInventory);

            // Configure TensorFlow.NET (equivalent of disabling eager execution in older TF versions)
            // tf.compat.v1.disable_eager_execution(); // Already non-eager by default for graph mode
        }

        /// <summary>
        /// This endpoint initiates the machine learning outcome generation process for a specific customer.
        /// It orchestrates a sequence involving initial setup, parallel processing, and final aggregation,
        /// using simulated services and actual TensorFlow.NET operations for ML and data persistence simulation.
        /// </summary>
        /// <param name="customerIdentifier">The unique identifier of the customer for whom to perform the ML outcome generation.</param>
        /// <returns>The final CoreMlOutcomeRecord representing the results of the ML outcome generation.</returns>
        [HttpPost("InitiateOutcomeGeneration/{customerIdentifier?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoreMlOutcomeRecord))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CoreMlOutcomeRecord>> InitiateMlOutcomeGeneration(int? customerIdentifier = null)
        {
            // This method orchestrates the entire multi-step workflow.

            /// <summary>
            /// Operational Step 1: Input Validation and Workflow Initialization
            /// </summary>
            // Validate the mandatory customerIdentifier parameter.
            // Operational Process Dependency: This is the initial input gate. Prevents any further processing if invalid.
            // If validation passes, proceeds to set up the request context and session tracking.
            if (!customerIdentifier.HasValue)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error: Customer identifier is required to initiate ML outcome generation.");
                return BadRequest("Customer identifier is required to initiate machine learning outcome generation.");
            }

            // Generate a unique identifier for tracking this specific workflow execution request.
            // Operational Process Dependency: Used for logging and uniquely identifying actual ML sessions created for this request in the _activeMlSessions dictionary.
            var requestSequenceIdentifier = Interlocked.Increment(ref _requestSessionSequenceCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Outcome Generation Workflow Session {requestSequenceIdentifier} for customer {customerIdentifier.Value}");

            // Create dedicated TensorFlow.NET sessions for the parallel processing units (Units A and B).
            // These are functional dependencies for ParallelProcessingUnitA and ParallelProcessingUnitB.
            // Note: Creating Session objects is resource-intensive. In a production scenario, consider pooling sessions or using a different TF.NET pattern.
            Session? modelAProcessingSession = null;
            Session? modelBProcessingSession = null;
            CoreMlOutcomeRecord? outcomeRecordAfterStepOne = null;
            CoreMlOutcomeRecord? finalOutcomeRecord = null;


            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Preparing resources for parallel operations.");

                /// <summary>
                /// Operational Step 2: Prepare Resources for Processing Units
                /// </summary>
                // Initialize a container for the core outcome record and thread-safe storage for results from parallel units.
                // 'currentOutcomeRecord' will initially be an empty object reference; SequentialInitialProcessingUnitC will establish the actual record instance.
                // 'modelAConcurrentResults' and 'modelBConcurrentResults' are used by ParallelProcessingUnitA and ParallelProcessingUnitB to return results to SequentialFinalProcessingUnitD.
                // Operational Process Dependency: These objects are created here and passed as parameters to the subsequent processing unit methods to facilitate data flow and result aggregation.
                var currentOutcomeRecord = new CoreMlOutcomeRecord(); // Container for the primary record being processed
                var modelAConcurrentResults = new ConcurrentDictionary<string, object>(); // Thread-safe store for results from Unit A
                var modelBConcurrentResults = new ConcurrentDictionary<string, object>(); // Thread-safe store for results from Unit B


                // Create actual TensorFlow.NET sessions for the parallel processing units (Units A and B).
                // Using separate graphs/sessions for independent parallel tasks is common.
                // The Session itself *is* disposable and is handled in the finally block.
                modelAProcessingSession = tf.Session(tf.Graph()); // Pass graph to session constructor
                modelBProcessingSession = tf.Session(tf.Graph()); // Pass graph to session constructor


                /// <summary>
                /// Operational Step 3: Register Actual ML Sessions for Management
                /// </summary>
                // Register the created actual ML sessions in the controller's session manager using unique IDs derived from the requestSequenceIdentifier.
                // Operational Process Dependency: This is necessary for proper resource disposal in the 'finally' block at the end of the workflow.
                _activeMlSessions.TryAdd(requestSequenceIdentifier * 2, modelAProcessingSession);     // Even numbered ID for Unit A session
                _activeMlSessions.TryAdd(requestSequenceIdentifier * 2 + 1, modelBProcessingSession); // Odd numbered ID for Unit B session

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Executing Sequential Initial Processing Unit (C).");

                /// <summary>
                /// Operational Step 4 (Sequential): Execute Initial Processing Unit (C)
                /// </summary>
                // Execute SequentialInitialProcessingUnitC (actual "ProcessFactoryOne"). This step runs sequentially first.
                // This unit is responsible for creating or retrieving the core CoreMlOutcomeRecord for the customer and establishing associated dependency records in simulated persistence if necessary, or loading existing ones.
                // It performs actual ML training (Model C) using TensorFlow.NET and saves the resulting model data (weights/biases).
                // Operational Process Dependency: Requires the initial 'currentOutcomeRecord' container, the customerIdentifier, and the requestSequenceIdentifier for context and logging.
                // Internally depends on InMemoryTestDataSet and RuntimeProcessingContext.
                // Subsequent Usage: The successful completion of this unit and its saved model data are dependencies for the parallel processing units (A and B) as they may use this model for inference/further processing.
                // It populates InMemoryTestDataSet and RuntimeProcessingContext with the initial/updated CoreMlOutcomeRecord and related data.
                await SequentialInitialProcessingUnitC(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier);

                // Retrieve the CoreMlOutcomeRecord object from simulated persistence *after* SequentialInitialProcessingUnitC has potentially created or updated it.
                // This ensures the orchestrator method has the latest state of the record before passing it to parallel units.
                // Operational Process Dependency: Depends on SequentialInitialProcessingUnitC successfully creating/findling and adding/updating the record in InMemoryTestDataSet.
                // Subsequent Usage: The 'outcomeRecordAfterStepOne' reference is used to check if Step 4 was successful and is then assigned to 'currentOutcomeRecord' to be passed to parallel units.
                outcomeRecordAfterStepOne = InMemoryTestDataSet.SimulatedCoreOutcomes
                                        .FirstOrDefault(r => r.AssociatedCustomerIdentifier == customerIdentifier.Value);

                // Check if SequentialInitialProcessingUnitC successfully established the core outcome record.
                // Operational Process Dependency: This check depends on the result of the retrieval attempt after Step 4.
                // Subsequent Usage: If the record is null, the workflow cannot continue, and an error is returned. Otherwise, the workflow proceeds to the parallel steps.
                if (outcomeRecordAfterStepOne != null)
                {
                    currentOutcomeRecord = outcomeRecordAfterStepOne; // Update the local reference to the retrieved record
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Core outcome record established successfully by Unit C (ID: {currentOutcomeRecord.RecordIdentifier}). Proceeding to parallel units.");
                }
                else
                {
                    // Error handling if Step 4 failed to establish the record.
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Initial Processing Unit C failed to create/find CoreMlOutcomeRecord for customer {customerIdentifier.Value}. Workflow cannot proceed.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to establish initial model data. Cannot start parallel processing units.");
                }

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Parallel Processing Units (A and B).");

                /// <summary>
                /// Operational Step 5 (Parallel): Execute Parallel Processing Units (A and B)
                /// </summary>
                // Execute ParallelProcessingUnitA (actual "ProcessFactoryTwo") and ParallelProcessingUnitB (actual "ProcessFactoryThree") concurrently using Task.WhenAll.
                // Operational Process Dependency: Both units depend on the core 'currentOutcomeRecord' object established by SequentialInitialProcessingUnitC (Step 4).
                // They also depend on their respective allocated actual ML sessions (modelAProcessingSession, modelBProcessingSession) and thread-safe result dictionaries (modelAConcurrentResults, modelBConcurrentResults).
                // Subsequent Usage: The main workflow waits here until both parallel tasks complete. Their outputs are stored in 'modelAConcurrentResults' and 'modelBConcurrentResults'.
                await Task.WhenAll(
                    ParallelProcessingUnitA(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier, modelAProcessingSession, modelAConcurrentResults),
                    ParallelProcessingUnitB(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier, modelBProcessingSession, modelBConcurrentResults)
                );

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Units A and B completed. Starting Sequential Final Processing Unit (D).");

                /// <summary>
                /// Operational Step 6 (Sequential): Execute Final Processing Unit (D)
                /// </summary>
                // Execute SequentialFinalProcessingUnitD (actual "ProcessFactoryFour"). This step runs sequentially after the parallel units have completed.
                // It is intended to combine or use the results from the parallel units to perform final calculations or updates to the core outcome record.
                // Operational Process Dependency: Depends on the 'currentOutcomeRecord' object (established/updated by Step 4).
                // Crucially depends on the results gathered by ParallelProcessingUnitA and ParallelProcessingUnitB (passed via modelAConcurrentResults and modelBConcurrentResults).
                // Depends on InMemoryTestDataSet for saving the final state of the outcome record.
                // Subsequent Usage: Finalizes the state of the CoreMlOutcomeRecord before it's retrieved and returned by the orchestrating method.
                await SequentialFinalProcessingUnitD(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier, modelAConcurrentResults, modelBConcurrentResults);

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ML Outcome Generation workflow completed successfully.");

                /// <summary>
                /// Operational Step 7 (Sequential): Retrieve Final Outcome and Return
                /// </summary>
                // Retrieve the final CoreMlOutcomeRecord from the simulated database after all processing steps have completed and been saved.
                // Operational Process Dependency: Depends on SequentialFinalProcessingUnitD having successfully updated the record in InMemoryTestDataSet.
                // Subsequent Usage: This is the final output returned by the API endpoint.
                finalOutcomeRecord = InMemoryTestDataSet.SimulatedCoreOutcomes
                                    .FirstOrDefault(r => r.AssociatedCustomerIdentifier == customerIdentifier.Value);

                // Check if the final record was found (should be, if steps 4-6 succeeded and saved).
                // Operational Process Dependency: Depends on the result of the final retrieval attempt.
                // Subsequent Usage: Determines whether to return a success response with the data or an error if the final state isn't retrievable.
                if (finalOutcomeRecord != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Returning final CoreMlOutcomeRecord (ID: {finalOutcomeRecord.RecordIdentifier}) for customer {customerIdentifier.Value}.");
                    return Ok(finalOutcomeRecord); // Return the final record on success
                }
                else
                {
                    // Fallback error if the final record isn't found in simulated storage, despite previous steps seemingly succeeding.
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Final CoreMlOutcomeRecord not found for customer {customerIdentifier.Value} after implementation. Potential data saving issue.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "ML outcome generation completed, but the final outcome record could not be retrieved.");
                }

            }
            /// <summary>
            /// Operational Step 8 (Workflow Cleanup and Error Handling)
            /// </summary>
            // Catch any exceptions thrown during the orchestration or within the processing units.
            // Operational Process Dependency: Catches errors from validation, resource setup, or any of the called processing unit methods (which might re-throw).
            // Subsequent Usage: Logs the error and returns a 500 Internal Server Error response to the client.
            // The 'finally' block will execute regardless of whether an exception occurred or not.
            catch (Exception ex)
            {
                // Log detailed error information.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Unhandled Error during ML Outcome Generation Workflow: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Stack Trace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error during ML outcome generation workflow: {ex.Message}"); // Return error response
            }
            finally
            {
                // Cleanup: Remove and dispose the actual ML sessions created for this request from the manager.
                // Operational Process Dependency: Depends on the sessions being added to the _activeMlSessions dictionary in Step 3.
                // Subsequent Usage: Releases actual resources associated with this request.
                if (_activeMlSessions.TryRemove(requestSequenceIdentifier * 2, out var sessionA))
                    sessionA?.Dispose();
                if (_activeMlSessions.TryRemove(requestSequenceIdentifier * 2 + 1, out var sessionB))
                    sessionB?.Dispose();

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Associated actual ML session resources cleaned up.");
            }
        }




        /// <summary>
        /// Processes data for Model C (SequentialInitialProcessingUnitC).
        /// This is the *first sequential* processing step in the workflow.
        /// It is responsible for ensuring the core CoreMlOutcomeRecord exists for the customer,
        /// creating it and associated dependency records in simulated persistence if necessary, or loading existing ones.
        /// It performs actual machine learning training using TensorFlow.NET (Model C) and saves the resulting model data.
        /// </summary>
        /// <param name="outcomeRecord">A reference to the CoreMlOutcomeRecord container/instance to work with.</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        private async Task SequentialInitialProcessingUnitC(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier)
        {
            // Declare variables outside the try block to ensure they are accessible in catch/finally
            CoreMlOutcomeRecord? retrievedOrNewOutcomeRecord = null;
            bool isNewRecord = false; // Declared outside try block
            Session? mlSession = null; // Local session for this sequential unit
            Tensorflow.Graph? graph = null; // Graph object

            RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_ActiveStatus", true);
            bool isActiveStart = (bool)RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_ActiveStatus")!;
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialProcessingUnitC ActiveStatus property value: {isActiveStart}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Sequential Initial Processing Unit C (Actual Model C).");

            // Disable eager execution before defining any TensorFlow operations for graph mode
            // This needs to be done early in the application lifecycle, preferably once.
            // Doing it per method might have unintended consequences or be inefficient.
            // Assuming for the scope of this method modification that repeating it here is intended
            // per the original code structure, despite best practices.
            tf.compat.v1.disable_eager_execution();
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Disabled eager execution for TensorFlow operations.");


            // Helper method definitions (included within this method as per constraint)
            #region Helper Methods (Required by this method)

            /// <summary>
            /// Transforms word-based samples into numerical embeddings using a simplified embedding technique.
            /// </summary>
            float[][] TransformWordsToEmbeddings(string[] wordSamples)
            {
                int embeddingDimensions = 10; // Fixed embedding dimension
                float[][] embeddings = new float[wordSamples.Length][];

                for (int i = 0; i < wordSamples.Length; i++)
                {
                    embeddings[i] = new float[embeddingDimensions];
                    string[] words = wordSamples[i].Split(' ');

                    for (int j = 0; j < words.Length; j++)
                    {
                        string word = words[j];
                        int hashBase = word.GetHashCode();
                        for (int k = 0; k < embeddingDimensions; k++)
                        {
                            int valueInt = Math.Abs(hashBase * (k + 1) * (j + 1) * 31);
                            float value = (valueInt % 1000) / 1000.0f;
                            embeddings[i][k] += value * (1.0f / (j + 1.0f));
                        }
                    }

                    float magnitudeSq = 0;
                    for (int k = 0; k < embeddingDimensions; k++) magnitudeSq += embeddings[i][k] * embeddings[i][k];
                    float magnitude = (float)Math.Sqrt(magnitudeSq);
                    if (magnitude > 1e-6f)
                    {
                        for (int k = 0; k < embeddingDimensions; k++) embeddings[i][k] /= magnitude;
                    }
                }
                return embeddings;
            }

            /// <summary>
            /// Converts a jagged array to a multidimensional array.
            /// </summary>
            float[,] ConvertJaggedToMultidimensional(float[][] jaggedArray)
            {
                if (jaggedArray == null || jaggedArray.Length == 0 || jaggedArray[0] == null) return new float[0, 0];
                int rows = jaggedArray.Length;
                int cols = jaggedArray[0].Length;
                float[,] result = new float[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    if (jaggedArray[i] == null || jaggedArray[i].Length != cols)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Row {i} in jagged array has inconsistent length.");
                        int currentCols = jaggedArray[i]?.Length ?? 0;
                        for (int j = 0; j < Math.Min(cols, currentCols); j++) result[i, j] = jaggedArray[i][j];
                    }
                    else
                    {
                        System.Buffer.BlockCopy(jaggedArray[i], 0, result, i * cols * sizeof(float), cols * sizeof(float));
                    }
                }
                return result;
            }

            /// <summary>
            /// Extracts a batch from a multidimensional array using indices.
            /// </summary>
            float[,] ExtractBatch(float[,] data, int[] batchIndices, int startIdx, int count)
            {
                if (data == null || batchIndices == null || data.GetLength(0) == 0 || batchIndices.Length == 0 || count <= 0) return new float[0, data.GetLength(1)];
                // Corrected bounds check logic to be less strict initially but check actual access
                if (startIdx < 0 || startIdx >= batchIndices.Length || startIdx + count > batchIndices.Length)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Batch indices out of bounds for ExtractBatch (startIdx: {startIdx}, count: {count}, indices length: {batchIndices.Length}).");
                    return new float[0, data.GetLength(1)];
                }


                int cols = data.GetLength(1);
                float[,] batch = new float[count, cols];
                for (int i = 0; i < count; i++)
                {
                    int srcIdx = batchIndices[startIdx + i];
                    if (srcIdx < 0 || srcIdx >= data.GetLength(0))
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error: Invalid index {srcIdx} accessed in source data for ExtractBatch.");
                        // Return partial batch extracted so far
                        var partialBatch = new float[i, cols];
                        System.Buffer.BlockCopy(batch, 0, partialBatch, 0, i * cols * sizeof(float));
                        return partialBatch;
                    }
                    System.Buffer.BlockCopy(data, srcIdx * cols * sizeof(float), batch, i * cols * sizeof(float), cols * sizeof(float));
                }
                return batch;
            }

            /// <summary>
            /// Shuffles an array randomly.
            /// </summary>
            void ShuffleArray(int[] shuffleIndices)
            {
                if (shuffleIndices == null) return;
                Random rng = new Random();
                int n = shuffleIndices.Length;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    int temp = shuffleIndices[k];
                    shuffleIndices[k] = shuffleIndices[n];
                    shuffleIndices[n] = temp;
                }
            }

            // Helper method to serialize a float array to a byte array
            byte[] SerializeFloatArray(float[] data)
            {
                if (data == null) return new byte[0];
                var byteList = new List<byte>();
                foreach (var f in data) byteList.AddRange(BitConverter.GetBytes(f));
                return byteList.ToArray();
            }

            // Helper method to deserialize a byte array back to a float array
            float[] DeserializeFloatArray(byte[] data)
            {
                if (data == null || data.Length == 0) return new float[0]; // Handle empty array case
                if (data.Length % 4 != 0) // Size of float is 4 bytes
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Byte array length ({data.Length}) is not a multiple of 4 for deserialization.");
                    return new float[0]; // Return empty array on invalid size
                }
                var floatArray = new float[data.Length / 4];
                System.Buffer.BlockCopy(data, 0, floatArray, 0, data.Length);
                return floatArray;
            }

            #endregion


            try
            {
                // ------------------------------------------
                // Find or Create Core Outcome Record and Dependencies
                // ------------------------------------------
                retrievedOrNewOutcomeRecord = InMemoryTestDataSet.SimulatedCoreOutcomes
                                        .FirstOrDefault(r => r.AssociatedCustomerIdentifier == customerIdentifier);

                isNewRecord = (retrievedOrNewOutcomeRecord == null); // isNewRecord is now in scope

                if (isNewRecord)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: No existing CoreMlOutcomeRecord found for Customer Identifier {customerIdentifier}. Initializing new record and associated dependencies.");

                    var nextAvailableRecordIdentifier = InMemoryTestDataSet.SimulatedCoreOutcomes
                                        .Count > 0 ? InMemoryTestDataSet.SimulatedCoreOutcomes.Max(r => r.RecordIdentifier) : 0;

                    retrievedOrNewOutcomeRecord = new CoreMlOutcomeRecord // Assign to the outer-scoped variable
                    {
                        AssociatedCustomerIdentifier = customerIdentifier,
                        OutcomeGenerationTimestamp = DateTime.UtcNow,
                        RecordIdentifier = nextAvailableRecordIdentifier + 1,
                        CategoricalClassificationIdentifier = null,
                        CategoricalClassificationDescription = null,
                        SerializedSimulatedModelData = new byte[0], // Initialize as empty, will be populated after training
                        AncillaryBinaryDataPayload = new byte[0],  // Initialize as empty
                        DerivedProductFeatureVector = null,
                        DerivedServiceBenefitVector = null
                    };

                    // Note: In a real scenario, adding to the list here might need locking if this unit could be called concurrently
                    // for the *same* customer ID, which isn't the case in this specific workflow design (sequential start).
                    // For simplicity in this example, assuming sequential access to InMemoryTestDataSet for a given customer ID within this unit.
                    lock (InMemoryTestDataSet.SimulatedCoreOutcomes)
                    {
                        InMemoryTestDataSet.SimulatedCoreOutcomes.Add(retrievedOrNewOutcomeRecord);
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created new CoreMlOutcomeRecord with Identifier {retrievedOrNewOutcomeRecord.RecordIdentifier} for customer {customerIdentifier}");

                    // Simulate creation/lookup of associated dependency records - simplified
                    // For a real system, these would be fetched from a database or other service.
                    // Adding simple checks and add-if-not-exists logic for simulation.
                    var associatedCustomer = InMemoryTestDataSet.SimulatedCustomerContexts.FirstOrDefault(c => c.CustomerLinkIdentifier == customerIdentifier);
                    if (associatedCustomer == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new AssociatedCustomerContext record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedCustomerContexts.Count > 0 ? InMemoryTestDataSet.SimulatedCustomerContexts.Max(c => c.ContextIdentifier) : 0; associatedCustomer = new AssociatedCustomerContext { ContextIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, CustomerPrimaryGivenName = $"Simulated FN {customerIdentifier}", CustomerFamilyName = $"Simulated LN {customerIdentifier}", CustomerContactPhoneNumber = $"555-cust-{customerIdentifier}", CustomerStreetAddress = $"123 Main St Sim {customerIdentifier}", AffiliatedCompanyName = $"Acme Inc. Sim {customerIdentifier}" }; lock (InMemoryTestDataSet.SimulatedCustomerContexts) InMemoryTestDataSet.SimulatedCustomerContexts.Add(associatedCustomer); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created AssociatedCustomerContext record with Identifier {associatedCustomer.ContextIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing AssociatedCustomerContext record found for Customer {customerIdentifier}"); }

                    var associatedWorkOrder = InMemoryTestDataSet.SimulatedWorkOrders.FirstOrDefault(o => o.CustomerLinkIdentifier == customerIdentifier);
                    if (associatedWorkOrder == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new OperationalWorkOrderRecord for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedWorkOrders.Count > 0 ? InMemoryTestDataSet.SimulatedWorkOrders.Max(o => o.OrderRecordIdentifier) : 0; associatedWorkOrder = new OperationalWorkOrderRecord { OrderRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, SpecificOrderIdentifier = customerIdentifier + 9000 }; lock (InMemoryTestDataSet.SimulatedWorkOrders) InMemoryTestDataSet.SimulatedWorkOrders.Add(associatedWorkOrder); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created OperationalWorkOrderRecord with Identifier {associatedWorkOrder.OrderRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing OperationalWorkOrderRecord found for Customer {customerIdentifier}"); }

                    var operationalEventRecord = InMemoryTestDataSet.SimulatedOperationalEvents.FirstOrDefault(e => e.CustomerLinkIdentifier == customerIdentifier);
                    if (operationalEventRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlInitialOperationEvent record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedOperationalEvents.Count > 0 ? InMemoryTestDataSet.SimulatedOperationalEvents.Max(e => e.EventIdentifier) : 0; operationalEventRecord = new MlInitialOperationEvent { EventIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, EventPayloadData = new byte[] { (byte)customerIdentifier, 0xAA } }; lock (InMemoryTestDataSet.SimulatedOperationalEvents) InMemoryTestDataSet.SimulatedOperationalEvents.Add(operationalEventRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlInitialOperationEvent record with Identifier {operationalEventRecord.EventIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlInitialOperationEvent record found for Customer {customerIdentifier}"); }

                    var validationRecord = InMemoryTestDataSet.SimulatedOutcomeValidations.FirstOrDefault(v => v.CustomerLinkIdentifier == customerIdentifier);
                    if (validationRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlOutcomeValidationRecord for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedOutcomeValidations.Count > 0 ? InMemoryTestDataSet.SimulatedOutcomeValidations.Max(v => v.ValidationRecordIdentifier) : 0; validationRecord = new MlOutcomeValidationRecord { ValidationRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, ValidationResultData = new byte[] { (byte)customerIdentifier, 0xBB } }; lock (InMemoryTestDataSet.SimulatedOutcomeValidations) InMemoryTestDataSet.SimulatedOutcomeValidations.Add(validationRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlOutcomeValidationRecord record with Identifier {validationRecord.ValidationRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlOutcomeValidationRecord record found for Customer {customerIdentifier}"); }

                    var initialStageDataRecord = InMemoryTestDataSet.SimulatedInitialOperationalStages.FirstOrDefault(s => s.CustomerLinkIdentifier == customerIdentifier);
                    if (initialStageDataRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new InitialOperationalStageData record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedInitialOperationalStages.Count > 0 ? InMemoryTestDataSet.SimulatedInitialOperationalStages.Max(s => s.StageIdentifier) : 0; initialStageDataRecord = new InitialOperationalStageData { StageIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, ProcessOperationalIdentifier = customerIdentifier + 7000, CustomerServiceOperationIdentifier = customerIdentifier + 6000, SalesProcessIdentifier = customerIdentifier + 5000, LinkedSubServiceA = 1, LinkedSubServiceB = 2, LinkedSubServiceC = 3, LinkedSubProductA = 1, LinkedSubProductB = 2, LinkedSubProductC = 3, StageSpecificData = $"Simulated Stage Data for Customer {customerIdentifier}", StageProductVectorSnapshot = $"Stage1_P_Simulated:{customerIdentifier}", StageServiceVectorSnapshot = $"Stage1_S_Simulated:{customerIdentifier}" }; lock (InMemoryTestDataSet.SimulatedInitialOperationalStages) InMemoryTestDataSet.SimulatedInitialOperationalStages.Add(initialStageDataRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created InitialOperationalStageData record with Identifier {initialStageDataRecord.StageIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing InitialOperationalStageData record found for Customer {customerIdentifier}"); }


                    // Store references to newly created/found records in RuntimeContext
                    RuntimeProcessingContext.StoreContextValue("AssociatedCustomerContextRecord", associatedCustomer);
                    RuntimeProcessingContext.StoreContextValue("OperationalWorkOrderRecord", associatedWorkOrder);
                    RuntimeProcessingContext.StoreContextValue("MlInitialOperationEventRecord", operationalEventRecord);
                    RuntimeProcessingContext.StoreContextValue("MlOutcomeValidationRecord", validationRecord);
                    RuntimeProcessingContext.StoreContextValue("InitialOperationalStageDataRecord", initialStageDataRecord);
                    RuntimeProcessingContext.StoreContextValue("CurrentCoreOutcomeRecord", retrievedOrNewOutcomeRecord);
                    RuntimeProcessingContext.StoreContextValue("CurrentCustomerIdentifier", customerIdentifier);

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - AssociatedCustomerContext Identifier: {associatedCustomer?.ContextIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - OperationalWorkOrderRecord Identifier: {associatedWorkOrder?.OrderRecordIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - MlInitialOperationEventRecord Identifier: {operationalEventRecord?.EventIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - MlOutcomeValidationRecord Identifier: {validationRecord?.ValidationRecordIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - InitialOperationalStageDataRecord Identifier: {initialStageDataRecord?.StageIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - CurrentCoreOutcomeRecord Identifier: {retrievedOrNewOutcomeRecord?.RecordIdentifier}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing CoreMlOutcomeRecord found for Customer Identifier {customerIdentifier}. Proceeding with existing record {retrievedOrNewOutcomeRecord!.RecordIdentifier}.");
                    // Store reference to the existing record in RuntimeContext if not already done
                    if (RuntimeProcessingContext.RetrieveContextValue("CurrentCoreOutcomeRecord") == null)
                    {
                        RuntimeProcessingContext.StoreContextValue("CurrentCoreOutcomeRecord", retrievedOrNewOutcomeRecord);
                        RuntimeProcessingContext.StoreContextValue("CurrentCustomerIdentifier", customerIdentifier);
                    }
                }


                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Actual Model C Training/Inference with combined numerical and word data.");

                // ------------------------------------------
                // Prepare combined input data (Numerical + Word Embeddings) for Model C
                // ------------------------------------------
                // Define sample numerical data - using fixed samples mirroring Step 7 data
                float[][] numericalSamples = new float[][] {
      new float[] { 0.3f, 0.7f, 0.1f, 0.85f },
      new float[] { 0.5f, 0.2f, 0.9f, 0.35f },
      new float[] { 0.8f, 0.6f, 0.4f, 0.55f },
      new float[] { 0.1f, 0.8f, 0.6f, 0.25f },
      new float[] { 0.7f, 0.3f, 0.2f, 0.95f },
      new float[] { 0.4f, 0.5f, 0.7f, 0.65f },
      new float[] { 0.2f, 0.9f, 0.3f, 0.15f },
      new float[] { 0.6f, 0.1f, 0.8f, 0.75f },
      new float[] { 0.35f, 0.65f, 0.15f, 0.80f },
      new float[] { 0.55f, 0.25f, 0.85f, 0.30f },
      new float[] { 0.75f, 0.55f, 0.45f, 0.60f },
      new float[] { 0.15f, 0.75f, 0.55f, 0.20f },
      new float[] { 0.65f, 0.35f, 0.25f, 0.90f },
      new float[] { 0.45f, 0.45f, 0.65f, 0.70f },
      new float[] { 0.25f, 0.85f, 0.35f, 0.10f },
      new float[] { 0.50f, 0.15f, 0.75f, 0.80f }
  };

                // Define sample word samples - using fixed samples mirroring Step 7 data
                string[] wordSamples = new string[] {
      "market growth potential high", "customer satisfaction excellent", "product quality superior",
      "service delivery timely", "price competitiveness average", "brand recognition strong",
      "operational efficiency optimal", "supply chain resilient", "market segment expanding",
      "customer retention excellent", "product innovation substantial", "service response immediate",
      "price positioning competitive", "brand loyalty increasing", "operational costs decreasing",
      "supply reliability consistent"
  };

                // Ensure the number of samples matches
                int sampleCount = Math.Min(numericalSamples.Length, wordSamples.Length);
                bool skipTrainingFlag = false;

                if (sampleCount == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: No valid training data generated for Model C. Skipping training.");
                    if (retrievedOrNewOutcomeRecord != null) // Ensure record exists before updating it
                    {
                        retrievedOrNewOutcomeRecord.SerializedSimulatedModelData = new byte[0]; // Store empty array to indicate no data
                        retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = new byte[0];  // Store empty array
                    }
                    skipTrainingFlag = true; // Set flag to skip training block
                }


                if (!skipTrainingFlag)
                {
                    // Transform word samples into embeddings using the helper method
                    float[][] wordEmbeddings = TransformWordsToEmbeddings(wordSamples.Take(sampleCount).ToArray());

                    // Convert jagged arrays to multidimensional arrays for TensorFlow
                    float[,] numericalData = ConvertJaggedToMultidimensional(numericalSamples.Take(sampleCount).ToArray());
                    float[,] wordData = ConvertJaggedToMultidimensional(wordEmbeddings);

                    int numericalFeatureCount = numericalData.GetLength(1);
                    int wordFeatureCount = wordData.GetLength(1);
                    int totalInputFeatureCount = numericalFeatureCount + wordFeatureCount;

                    // Define simple target values based on combined numerical data (matching Step 7 target logic)
                    float[,] targetValues = new float[sampleCount, 1];
                    for (int i = 0; i < sampleCount; i++)
                    {
                        if (numericalData.GetLength(1) < 4) // Check column length safety
                        {
                            targetValues[i, 0] = 0.0f; // Default target if row is invalid
                            continue;
                        }
                        float x = numericalData[i, 0];
                        float y = numericalData[i, 1];
                        float z = numericalData[i, 2];
                        float p = numericalData[i, 3];

                        // Use a more complex formula that includes non-linear terms (matching Step 7 target logic)
                        targetValues[i, 0] = x * (float)Math.Cos(p) +
                                           y * (float)Math.Sin(p) +
                                           z * (float)Math.Cos(p / 2f) +
                                           x * y * z * 0.1f;
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created {sampleCount} combined numerical and word samples for Model C training.");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Numerical features: {numericalFeatureCount}, Word embedding features: {wordFeatureCount}. Total input features: {totalInputFeatureCount}");


                    // ------------------------------------------
                    // TensorFlow Graph Definition and Training (Model C)
                    // ------------------------------------------
                    // Create graph and session outside the using block that was causing issues
                    graph = tf.Graph();
                    mlSession = tf.Session(graph); // Create session with the new graph

                    // Define operations using the created graph and session
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 4 - Initializing Model C Architecture.");

                    // Define placeholders for numerical and word inputs
                    Tensor numericalInputPlaceholder = tf.placeholder(tf.float32, shape: (-1, numericalFeatureCount), name: "numerical_input_C");
                    Tensor wordInputPlaceholder = tf.placeholder(tf.float32, shape: (-1, wordFeatureCount), name: "word_input_C");
                    Tensor targetOutputPlaceholder = tf.placeholder(tf.float32, shape: (-1, 1), name: "target_output_C");

                    // Concatenate the numerical and word input placeholders
                    var combinedInput = tf.concat(new[] { numericalInputPlaceholder, wordInputPlaceholder }, axis: 1, name: "combined_input_C");

                    // Try to load existing model parameters from the outcome record
                    // Use the outer-scoped retrievedOrNewOutcomeRecord
                    var existingWeights = retrievedOrNewOutcomeRecord?.SerializedSimulatedModelData == null || retrievedOrNewOutcomeRecord.SerializedSimulatedModelData.Length == 0 ?
                        null : DeserializeFloatArray(retrievedOrNewOutcomeRecord.SerializedSimulatedModelData);
                    var existingBias = retrievedOrNewOutcomeRecord?.AncillaryBinaryDataPayload == null || retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload.Length == 0 ?
                        null : DeserializeFloatArray(retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload);

                    // Check if existing parameters match the expected shape for the *combined* input
                    // Model C now trains a model with 1 hidden layer (matching Step 7 structure)
                    // Weights1: [totalInputFeatureCount, hiddenLayerSize]
                    // Weights2: [hiddenLayerSize, 1]
                    // Bias1: [hiddenLayerSize]
                    // Bias2: [1]

                    // For simplicity, let's hardcode a hidden layer size for Model C here, similar to the fallback in Unit A
                    int hiddenLayerSize = 64; // Default hidden size for Model C

                    // Need to check if the combined size of existing weights matches W1 + W2 size and bias size matches B1 + B2 size
                    // Total expected weights = (totalInputFeatureCount * hiddenLayerSize) + (hiddenLayerSize * 1)
                    int expectedTotalWeightCount = (totalInputFeatureCount * hiddenLayerSize) + (hiddenLayerSize * 1);
                    int expectedTotalBiasCount = hiddenLayerSize + 1;
                    // We don't check total param count as weights and biases are stored separately.


                    bool useExistingParams = existingWeights != null && existingBias != null &&
                                             existingWeights.Length == expectedTotalWeightCount && existingBias.Length == expectedTotalBiasCount;


                    ResourceVariable weights1, weights2, bias1, bias2;


                    if (!useExistingParams)
                    {
                        // Initialize new weights and bias if none exist or shapes don't match
                        weights1 = tf.Variable(tf.random.normal((totalInputFeatureCount, hiddenLayerSize), stddev: 0.1f), name: "weights1_C");
                        bias1 = tf.Variable(tf.zeros(hiddenLayerSize, dtype: tf.float32), name: "bias1_C");
                        weights2 = tf.Variable(tf.random.normal((hiddenLayerSize, 1), stddev: 0.1f), name: "weights2_C");
                        bias2 = tf.Variable(tf.zeros(1, dtype: tf.float32), name: "bias2_C");

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C - Initializing NEW model parameters for combined input ({totalInputFeatureCount} -> {hiddenLayerSize} -> 1).");
                    }
                    else
                    {
                        // Load existing weights and bias
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C - Loading EXISTING model parameters for combined input ({totalInputFeatureCount} -> {hiddenLayerSize} -> 1).");

                        // Split the combined byte arrays back into original float arrays based on expected sizes
                        float[] loadedWeights1 = new float[totalInputFeatureCount * hiddenLayerSize];
                        float[] loadedBias1 = new float[hiddenLayerSize];
                        float[] loadedWeights2 = new float[hiddenLayerSize * 1];
                        float[] loadedBias2 = new float[1];

                        int weightOffset = 0;
                        System.Buffer.BlockCopy(existingWeights!, weightOffset, loadedWeights1, 0, loadedWeights1.Length * sizeof(float)); // Use null forgiving !
                        weightOffset += loadedWeights1.Length * sizeof(float);
                        System.Buffer.BlockCopy(existingWeights!, weightOffset, loadedWeights2, 0, loadedWeights2.Length * sizeof(float));

                        int biasOffset = 0;
                        System.Buffer.BlockCopy(existingBias!, biasOffset, loadedBias1, 0, loadedBias1.Length * sizeof(float)); // Use null forgiving !
                        biasOffset += loadedBias1.Length * sizeof(float);
                        System.Buffer.BlockCopy(existingBias!, biasOffset, loadedBias2, 0, loadedBias2.Length * sizeof(float));


                        // Create tensors and variables
                        weights1 = tf.Variable(tf.constant(loadedWeights1.reshape(totalInputFeatureCount, hiddenLayerSize), dtype: tf.float32), name: "weights1_C");
                        bias1 = tf.Variable(tf.constant(loadedBias1.reshape(hiddenLayerSize), dtype: tf.float32), name: "bias1_C"); // Reshape bias1 to 1D
                        weights2 = tf.Variable(tf.constant(loadedWeights2.reshape(hiddenLayerSize, 1), dtype: tf.float32), name: "weights2_C");
                        bias2 = tf.Variable(tf.constant(loadedBias2.reshape(1), dtype: tf.float32), name: "bias2_C"); // Reshape bias2 to 1D


                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C - Successfully loaded parameters.");
                    }

                    // Simple feedforward network structure (Input -> ReLU Hidden -> Output)
                    var hidden = tf.nn.relu(tf.add(tf.matmul(combinedInput, weights1), bias1), name: "hidden_C");
                    var predictions = tf.add(tf.matmul(hidden, weights2), bias2, name: "predictions_C");

                    // Loss function (Mean Squared Error)
                    Tensor lossOp = tf.reduce_mean(tf.square(tf.subtract(predictions, targetOutputPlaceholder)), name: "loss_C");

                    // Optimizer (Adam)
                    var optimizer = tf.train.AdamOptimizer(0.001f);
                    Operation trainOp = optimizer.minimize(lossOp);

                    // Global variables initializer
                    Operation initOp = tf.global_variables_initializer();

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] TensorFlow operations defined within Model C graph.");

                    // Initialize variables
                    mlSession.run(initOp);
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C - Actual TensorFlow.NET variables initialized.");

                    // ------------------------------------------
                    // Training Loop
                    // ------------------------------------------
                    int numEpochs = 50; // Define number of training epochs
                    int batchSize = 4; // Define batch size
                    int numBatches = (batchSize > 0 && sampleCount > 0) ? (int)Math.Ceiling((double)sampleCount / batchSize) : 0;
                    int[] indices = Enumerable.Range(0, sampleCount).ToArray(); // Indices for shuffling

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C - Starting Actual Training Loop for {numEpochs} epochs with {numBatches} batches.");

                    for (int epoch = 0; epoch < numEpochs; epoch++)
                    {
                        ShuffleArray(indices); // Shuffle data indices each epoch
                        float epochLoss = 0.0f;

                        for (int batch = 0; batch < numBatches; batch++)
                        {
                            int startIdx = batch * batchSize;
                            // Corrected to use batchSize and sampleCount
                            int endIdx = Math.Min(startIdx + batchSize, sampleCount);
                            int batchCount = endIdx - startIdx;

                            if (batchCount <= 0) continue; // Skip if batch is empty

                            // Extract batches for numerical, word, and target data using shuffled indices
                            float[,] batchNumerical = ExtractBatch(numericalData, indices, startIdx, batchCount);
                            float[,] batchWord = ExtractBatch(wordData, indices, startIdx, batchCount);
                            float[,] batchTarget = ExtractBatch(targetValues, indices, startIdx, batchCount);

                            // Create feed dictionary for the batch
                            var feeds = new FeedItem[] {
                   new FeedItem(numericalInputPlaceholder, batchNumerical), // Feed numerical batch
                   new FeedItem(wordInputPlaceholder, batchWord),           // Feed word batch
                   new FeedItem(targetOutputPlaceholder, batchTarget)       // Feed target batch
               };

                            // Run a single training step, fetching the loss
                            var fetches = new ITensorOrOperation[] { lossOp, trainOp }; // Use ITensorOrOperation[]
                            var results = mlSession.run(fetches, feeds);
                            float currentBatchLoss = (float)((Tensor)results[0]).numpy()[0];
                            epochLoss += currentBatchLoss;

                            if (batch % 10 == 0 || batch == numBatches - 1)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Epoch {epoch + 1}/{numEpochs}, Batch {batch + 1}/{numBatches}, Actual Batch Loss: {currentBatchLoss:E4}");
                            }
                        }

                        if (numBatches > 0)
                        {
                            epochLoss /= numBatches;
                        }
                        else
                        {
                            epochLoss = float.NaN; // Indicate no batches were processed
                        }

                        if (epoch % 10 == 0 || epoch == numEpochs - 1)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Epoch {epoch + 1}/{numEpochs}, Average Epoch Loss: {(float.IsNaN(epochLoss) ? "N/A" : epochLoss.ToString("E4"))}");
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model C training completed.");

                    // ------------------------------------------
                    // Extract and Serialize Final Model Parameters
                    // ------------------------------------------
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Actual Model C parameter serialization.");

                    // Get the final weights and bias values using the session
                    // Removed explicit cast (ITensorOrOperation) from ResourceVariable as they implicitly convert
                    var finalWeightsAndBias = mlSession.run(new[] {
                weights1,
                bias1,
                weights2,
                bias2
            });

                    // Cast results to Tensor
                    var finalWeights1Tensor = finalWeightsAndBias[0] as Tensor;
                    var finalBias1Tensor = finalWeightsAndBias[1] as Tensor;
                    var finalWeights2Tensor = finalWeightsAndBias[2] as Tensor;
                    var finalBias2Tensor = finalWeightsAndBias[3] as Tensor;


                    if (finalWeights1Tensor != null && finalBias1Tensor != null && finalWeights2Tensor != null && finalBias2Tensor != null)
                    {
                        var finalWeights1 = finalWeights1Tensor.ToArray<float>();
                        var finalBias1 = finalBias1Tensor.ToArray<float>();
                        var finalWeights2 = finalWeights2Tensor.ToArray<float>();
                        var finalBias2 = finalBias2Tensor.ToArray<float>();

                        // Combine weights and biases into the two byte arrays
                        var combinedWeights = finalWeights1.Concat(finalWeights2).ToArray();
                        var combinedBias = finalBias1.Concat(finalBias2).ToArray();

                        retrievedOrNewOutcomeRecord!.SerializedSimulatedModelData = SerializeFloatArray(combinedWeights); // Use null-forgiving operator since we know it's not null here
                        retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = SerializeFloatArray(combinedBias);

                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C actual model parameters serialized to byte arrays (Weights size: {retrievedOrNewOutcomeRecord.SerializedSimulatedModelData.Length}, Bias size: {retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload.Length}).");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C - Failed to fetch/serialize actual weights or bias tensors.");
                        retrievedOrNewOutcomeRecord!.SerializedSimulatedModelData = new byte[0]; // Store empty array on failure
                        retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = new byte[0]; // Store empty array on failure
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Actual Model C parameter serialization completed.");
                }


                // ------------------------------------------
                // Save Final State to Simulated Persistence and Runtime Context
                // ------------------------------------------
                // Ensure the record is updated in the simulated database regardless of training success/failure/skip
                if (retrievedOrNewOutcomeRecord != null)
                {
                    var recordIndex = InMemoryTestDataSet.SimulatedCoreOutcomes.FindIndex(r => r.RecordIdentifier == retrievedOrNewOutcomeRecord.RecordIdentifier);
                    if (recordIndex >= 0)
                    {
                        // Use lock for thread safety when writing to the static list
                        lock (InMemoryTestDataSet.SimulatedCoreOutcomes)
                        {
                            InMemoryTestDataSet.SimulatedCoreOutcomes[recordIndex] = retrievedOrNewOutcomeRecord; // Update the static list entry
                        }
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C actual parameter data saved successfully in simulated persistent storage.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error - CoreMlOutcomeRecord with Identifier {retrievedOrNewOutcomeRecord.RecordIdentifier} not found in simulated storage during final update attempt!");
                    }

                    // Store the serialized model parameters in RuntimeContext for potential use by other units (like Model B)
                    RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_SerializedModelData", retrievedOrNewOutcomeRecord.SerializedSimulatedModelData);
                    RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_AncillaryData", retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload);
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C actual model parameter data stored in Runtime Processing Context.");

                    // Verification log
                    var contextCustomerIdentifier = RuntimeProcessingContext.RetrieveContextValue("CurrentCustomerIdentifier");
                    var contextProcessOneData = RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_SerializedModelData") as byte[];
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - Customer Identifier: {contextCustomerIdentifier}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - Serialized Model Data Size: {contextProcessOneData?.Length ?? 0} bytes");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: No outcome record to save to simulated persistence.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Unhandled Error in Sequential Initial Processing Unit C: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stack Trace: {ex.StackTrace}");

                // Ensure the outcome record data is set to empty arrays on failure
                if (retrievedOrNewOutcomeRecord != null)
                {
                    retrievedOrNewOutcomeRecord.SerializedSimulatedModelData = new byte[0];
                    retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = new byte[0];
                    retrievedOrNewOutcomeRecord.CategoricalClassificationDescription = (retrievedOrNewOutcomeRecord.CategoricalClassificationDescription ?? "") + " (TrainingError)";
                    // Attempt to save the error state to persistence
                    var recordIndex = InMemoryTestDataSet.SimulatedCoreOutcomes.FindIndex(r => r.RecordIdentifier == retrievedOrNewOutcomeRecord.RecordIdentifier);
                    if (recordIndex >= 0)
                    {
                        lock (InMemoryTestDataSet.SimulatedCoreOutcomes)
                        {
                            InMemoryTestDataSet.SimulatedCoreOutcomes[recordIndex] = retrievedOrNewOutcomeRecord;
                        }
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Updated simulated persistent storage with error state.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error occurred but retrievedOrNewOutcomeRecord was null.");
                }

                throw; // Re-throw to be caught by the orchestrator
            }
            finally
            {
                // Dispose the session if it was created
                mlSession?.Dispose();
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C TF Session disposed.");
                // Graph disposal is not strictly necessary as it's not IDisposable, but clearing the reference is good practice
                // Note: In complex scenarios with multiple units sharing graph definition across calls,
                // graph management needs careful consideration (e.g., a single graph per app lifetime).
                // Here, a new graph is created per call to this unit.
                graph = null;
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Graph reference cleared.");

                RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_ActiveStatus", false);
                bool isActiveAfterExecution = (bool)RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_ActiveStatus")!;
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialProcessingUnitC ActiveStatus property value after execution: {isActiveAfterExecution}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Initial Processing Unit C (Actual Model C) finished.");
            }
        }

























        /// <summary>
        /// Processes data simulating Model A (ParallelProcessingUnitA).
        /// This method is designed to run in parallel with ParallelProcessingUnitB (Actual Model B).
        /// It orchestrates a multi-stage data processing and machine learning workflow,
        /// including data collection, feature engineering, tensor generation, quality checks,
        /// model training using TensorFlow.NET, and generating performance projections.
        /// </summary>
        /// <param name="outcomeRecord">The core CoreMlOutcomeRecord object being processed.</param>
        /// <param name="customerIdentifier">The identifier for the associated customer.</param>
        /// <param name="requestSequenceIdentifier">A unique identifier for the current workflow request session.</param>
        /// <param name="mlSession">A dedicated actual TensorFlow.NET Session environment for ML tasks within this unit.</param>
        /// <param name="unitResultsStore">A thread-safe dictionary to store results and state for subsequent processing units.</param>
        private async Task ParallelProcessingUnitA(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier, Session mlSession, ConcurrentDictionary<string, object> unitResultsStore)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Parallel Processing Unit A for customer {customerIdentifier}.");

            try
            {
                //==========================================================================
                // General Utility Methods (Accessible by all Stages)
                //==========================================================================

                /// <summary>
                /// Converts a simple mathematical expression into a regular expression pattern.
                /// </summary>
                string ConvertExpressionToRegex(string expression)
                {
                    // For the simple expression "1+1", create a regex pattern with capture groups
                    if (expression == "1+1")
                    {
                        return @"(\d+)([\+\-\*\/])(\d+)";
                    }

                    // For more complex expressions, implement a more sophisticated parser
                    string pattern = expression.Replace("1", @"(\d+)");
                    pattern = pattern.Replace("+", @"([\+\-\*\/])");

                    return pattern;
                }

                /// <summary>
                /// Converts a regular expression pattern into an n-dimensional compute-safe expression.
                /// </summary>
                string ConvertRegexToNDimensionalExpression(string regexPattern)
                {
                    // Convert the pattern to an n-dimensional expression
                    if (regexPattern == @"(\d+)([\+\-\*\/])(\d+)" || (regexPattern?.Contains(@"(\d+)") == true && regexPattern?.Contains(@"([\+\-\*\/])") == true)) // Added null check and boolean logic
                    {
                        return "ND(x,y,z,p)=Vx*cos(p)+Vy*sin(p)+Vz*cos(p/2)";
                    }

                    // Default fallback for unrecognized patterns
                    return "ND(x,y,z,p)=x+y+z";
                }

                /// <summary>
                /// Transforms word-based samples into numerical embeddings using a simplified embedding technique.
                /// </summary>
                float[][] TransformWordsToEmbeddings(string[] wordSamples)
                {
                    // Simple word embedding function - in a real implementation, this would use
                    // proper word embeddings from a pre-trained model or custom embeddings

                    // For this example, we'll convert each word sample to a 10-dimensional vector
                    // using a consistent but simplified approach
                    int embeddingDimensions = 10;
                    float[][] embeddings = new float[wordSamples.Length][];

                    for (int i = 0; i < wordSamples.Length; i++)
                    {
                        embeddings[i] = new float[embeddingDimensions];

                        // Split the sample into words
                        string[] words = wordSamples[i].Split(' ');

                        // For each word, contribute to the embedding
                        for (int j = 0; j < words.Length; j++)
                        {
                            string word = words[j];

                            // Use a simple hash function to generate values from words
                            // Ensure the hash function is consistent and spreads values
                            int hashBase = word.GetHashCode();
                            for (int k = 0; k < embeddingDimensions; k++)
                            {
                                // Generate a value based on hash, dimension, and word index
                                // Using a prime multiplier to help distribute values
                                int valueInt = Math.Abs(hashBase * (k + 1) * (j + 1) * 31); // Multiply by prime 31
                                float value = (valueInt % 1000) / 1000.0f; // Map to 0-1 range

                                // Add to the embedding with position-based weighting (inverse word index)
                                embeddings[i][k] += value * (1.0f / (j + 1.0f)); // Use 1.0f for float division
                            }
                        }

                        // Normalize the embedding vector
                        float magnitudeSq = 0;
                        for (int k = 0; k < embeddingDimensions; k++)
                        {
                            magnitudeSq += embeddings[i][k] * embeddings[i][k];
                        }

                        float magnitude = (float)Math.Sqrt(magnitudeSq);
                        if (magnitude > 1e-6f) // Use tolerance
                        {
                            for (int k = 0; k < embeddingDimensions; k++)
                            {
                                embeddings[i][k] /= magnitude;
                            }
                        }
                    }

                    return embeddings;
                }

                /// <summary>
                /// Applies the n-dimensional expression to curvature coefficients.
                /// </summary>
                float[] ApplyNDimensionalExpressionToCurvature(float[] coefficients, string ndExpression)
                {
                    // Parse the expression to determine how to modify coefficients
                    // For our example derived from "1+1", we'll implement the
                    // equivalent of "adding 1" to appropriate coefficients

                    float[] modifiedCoefficients = new float[coefficients.Length];
                    System.Buffer.BlockCopy(coefficients, 0, modifiedCoefficients, 0, coefficients.Length * sizeof(float)); // Use System.Buffer

                    // Apply the expression's effect to the coefficients
                    if (ndExpression.StartsWith("ND(x,y,z,p)="))
                    {
                        // Extract the dimensional scaling factors from the expression
                        // For ND(x,y,z,p)=Vx*cos(p)+Vy*sin(p)+Vz*cos(p/2), the 'V' factors are implicitly 1 in this simplified mapping.
                        // The "1+1" idea suggests a doubling or a primary dimension influence.
                        // Let's map the "1+1" effect to amplify the primary diagonal coefficients (xx, yy, zz).
                        float amplificationFactor = 2.0f; // Represents the "1+1" result influencing amplitude

                        if (modifiedCoefficients.Length > 0) modifiedCoefficients[0] *= amplificationFactor;  // xx coefficient
                        if (modifiedCoefficients.Length > 1) modifiedCoefficients[1] *= amplificationFactor;  // yy coefficient
                        if (modifiedCoefficients.Length > 2) modifiedCoefficients[2] *= amplificationFactor;  // zz coefficient

                        // Scale cross-terms relative to the primary terms' amplification
                        float crossTermScale = (amplificationFactor + amplificationFactor) / 2.0f; // Simple average influence
                        if (modifiedCoefficients.Length > 3) modifiedCoefficients[3] *= crossTermScale;  // xy coefficient
                        if (modifiedCoefficients.Length > 4) modifiedCoefficients[4] *= crossTermScale;  // xz coefficient
                        if (modifiedCoefficients.Length > 5) modifiedCoefficients[5] *= crossTermScale;  // yz coefficient

                        // Apply a lesser influence to higher-order terms if they exist
                        float higherOrderScale = (crossTermScale + 1.0f) / 2.0f; // Average of cross-term scale and default 1.0
                        if (modifiedCoefficients.Length > 6) modifiedCoefficients[6] *= higherOrderScale;
                        if (modifiedCoefficients.Length > 7) modifiedCoefficients[7] *= higherOrderScale;
                        if (modifiedCoefficients.Length > 8) modifiedCoefficients[8] *= higherOrderScale;
                    }

                    return modifiedCoefficients;
                }


                /// <summary>
                /// Generates weight matrices from our n-dimensional expression.
                /// </summary>
                float[,] GenerateWeightsFromExpression(string expression, int inputDim, int outputDim)
                {
                    // Create a weight matrix based on our expression
                    float[,] weights = new float[inputDim, outputDim];

                    // Use a pseudorandom generator with fixed seed for reproducibility
                    Random rand = new Random(42 + inputDim * 100 + outputDim); // Vary seed slightly for different matrix sizes

                    // Fill the matrix with values derived from our expression
                    for (int i = 0; i < inputDim; i++)
                    {
                        for (int j = 0; j < outputDim; j++)
                        {
                            // Base weight value (small random initialization)
                            float baseWeight = (float)(rand.NextDouble() * 0.04 - 0.02); // Range -0.02 to +0.02

                            // Apply influence from our expression's structure
                            // For "1+1" -> ND(x,y,z,p)=Vx*cos(p)+Vy*sin(p)+Vz*cos(p/2)
                            // We want the weights to reflect the oscillatory and dimensional coupling nature.
                            // Use a combination of indices and trigonometric functions.
                            float expressionInfluence = (float)(
                                Math.Cos((i + j) * Math.PI / (inputDim + outputDim)) + // Cosine based on combined indices
                                Math.Sin(i * Math.PI / inputDim) * 0.5 +               // Sine based on input index
                                Math.Cos(j * Math.PI / (outputDim * 2.0)) * 0.5         // Cosine based on output index (half frequency)
                            );

                            // Scale the influence and add it to the base weight
                            // The scale factor ensures the expression doesn't dominate initialization completely
                            float influenceScale = 0.1f; // Small influence during initialization
                            weights[i, j] = baseWeight + expressionInfluence * influenceScale;
                        }
                    }

                    // Enhance the "outermost vertices" (corners) to emphasize the boundary condition
                    float cornerBoost = 1.5f; // Factor to multiply corner weights by
                    if (inputDim > 0 && outputDim > 0)
                    {
                        weights[0, 0] *= cornerBoost;                   // Top-left
                        weights[0, outputDim - 1] *= cornerBoost;        // Top-right
                        weights[inputDim - 1, 0] *= cornerBoost;         // Bottom-left
                        weights[inputDim - 1, outputDim - 1] *= cornerBoost; // Bottom-right (outermost conceptual vertex)
                    }


                    return weights;
                }

                /// <summary>
                /// Calculates a basis vector from sample coordinates along a specific dimension.
                /// </summary>
                Vector3 CalculateBasisVector(Vector3[] coordinates, int dimension)
                {
                    if (coordinates == null || coordinates.Length == 0 || dimension < 0 || dimension > 2) // Added null/empty/dimension checks
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Invalid input for CalculateBasisVector. Returning zero vector.");
                        return Vector3.Zero;
                    }

                    // Start with a zero vector
                    Vector3 basis = Vector3.Zero;

                    // For each coordinate, extract the component from the specified dimension
                    // and use it to weight the contribution of that coordinate to the basis vector
                    foreach (var coord in coordinates)
                    {
                        if (coord == null) continue; // Skip null coordinates

                        // Get the component value for the specified dimension
                        float component = dimension == 0 ? coord.X : (dimension == 1 ? coord.Y : coord.Z);

                        // Add the weighted coordinate to the basis vector
                        basis += new Vector3(
                            coord.X * component,
                            coord.Y * component,
                            coord.Z * component
                        );
                    }

                    // Normalize the basis vector to unit length
                    float magnitude = (float)Math.Sqrt(basis.X * basis.X + basis.Y * basis.Y + basis.Z * basis.Z);
                    if (magnitude > 1e-6f) // Use tolerance
                    {
                        basis.X /= magnitude;
                        basis.Y /= magnitude;
                        basis.Z /= magnitude;
                    }

                    return basis;
                }

                /// <summary>
                /// Calculates coefficients that represent how the curvature varies in the sample space.
                /// </summary>
                float[] CalculateCurvatureCoefficients(Vector3[] coordinates, Vector3[] values)
                {
                    if (coordinates == null || values == null || coordinates.Length == 0 || coordinates.Length != values.Length)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Invalid input for CalculateCurvatureCoefficients. Returning zero coefficients.");
                        return new float[9]; // Return zero array if input is invalid
                    }


                    // We'll use 9 coefficients to represent the curvature in 3D space
                    float[] coefficients = new float[9];

                    // For each coordinate-value pair
                    for (int i = 0; i < coordinates.Length; i++)
                    {
                        Vector3 coord = coordinates[i];
                        Vector3 value = values[i];

                        if (coord == null || value == null) continue; // Skip invalid pairs


                        // Calculate the squared components of the coordinate
                        float x2 = coord.X * coord.X;
                        float y2 = coord.Y * coord.Y;
                        float z2 = coord.Z * coord.Z;

                        // Calculate the cross-components
                        float xy = coord.X * coord.Y;
                        float xz = coord.X * coord.Z;
                        float yz = coord.Y * coord.Z;

                        // Calculate the dot product of coordinate and value
                        float dot = coord.X * value.X + coord.Y * value.Y + coord.Z * value.Z;

                        // Update the coefficients based on this sample
                        coefficients[0] += x2 * dot; // xx component
                        coefficients[1] += y2 * dot; // yy component
                        coefficients[2] += z2 * dot; // zz component
                        coefficients[3] += xy * dot; // xy component
                        coefficients[4] += xz * dot; // xz component
                        coefficients[5] += yz * dot; // yz component
                        coefficients[6] += x2 * y2 * dot; // xxyy component (higher order)
                        coefficients[7] += x2 * z2 * dot; // xxzz component (higher order)
                        coefficients[8] += y2 * z2 * dot; // yyzz component (higher order)
                    }

                    // Normalize the coefficients by the number of samples
                    if (coordinates.Length > 0) // Avoid division by zero
                    {
                        for (int i = 0; i < coefficients.Length; i++)
                        {
                            coefficients[i] /= coordinates.Length;
                        }
                    }


                    return coefficients;
                }

                /// <summary>
                /// Calculates the eigenvalues of the curvature tensor.
                /// </summary>
                float[] CalculateEigenvalues(float[] coefficients)
                {
                    if (coefficients == null || coefficients.Length < 6)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Invalid input for CalculateEigenvalues. Returning default eigenvalues.");
                        return new float[] { 1.0f, 1.0f, 1.0f }; // Return default values
                    }


                    // Construct a simplified 3x3 matrix from the first 6 coefficients
                    float[,] matrix = new float[3, 3];
                    matrix[0, 0] = coefficients[0]; // xx
                    matrix[1, 1] = coefficients[1]; // yy
                    matrix[2, 2] = coefficients[2]; // zz
                    matrix[0, 1] = matrix[1, 0] = coefficients[3]; // xy
                    matrix[0, 2] = matrix[2, 0] = coefficients[4]; // xz
                    matrix[1, 2] = matrix[2, 1] = coefficients[5]; // yz

                    // Compute the trace (sum of diagonal elements)
                    float trace = matrix[0, 0] + matrix[1, 1] + matrix[2, 2];

                    // Compute the determinant
                    float det = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
                                matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
                                matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);

                    // Use a simplified approach to estimate eigenvalues
                    float[] eigenvalues = new float[3];
                    eigenvalues[0] = trace / 3.0f + 0.1f * det; // Approximation for first eigenvalue
                    eigenvalues[1] = trace / 3.0f;              // Approximation for second eigenvalue
                    eigenvalues[2] = trace / 3.0f - 0.1f * det; // Approximation for third eigenvalue

                    return eigenvalues;
                }

                /// <summary>
                /// Converts eigenvalues to weights for loss function.
                /// </summary>
                float[] ConvertEigenvaluesToWeights(float[] eigenvalues)
                {
                    if (eigenvalues == null || eigenvalues.Length == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Eigenvalues array is null or empty for weights. Returning default weights.");
                        return new float[] { 1.0f }; // Return a default weight
                    }

                    // Create weights based on eigenvalues
                    float[] weights = new float[eigenvalues.Length];

                    // Normalize eigenvalues to create weights that sum to something meaningful
                    // Use absolute values, and shift/scale to ensure positive weights
                    float sumAbsEigenvalues = 0.0f;
                    for (int i = 0; i < eigenvalues.Length; i++)
                    {
                        // Use absolute values to ensure positive weights
                        weights[i] = Math.Abs(eigenvalues[i]);
                        sumAbsEigenvalues += weights[i];
                    }

                    // Normalize and ensure minimum weight, or use a relative weighting
                    if (sumAbsEigenvalues > 1e-6f) // Use tolerance
                    {
                        // Use a relative weighting: higher absolute eigenvalue means higher weight
                        float maxAbsEigenvalue = weights.Max();
                        if (maxAbsEigenvalue > 1e-6f)
                        {
                            for (int i = 0; i < weights.Length; i++)
                            {
                                // Scale weights relative to max, with a minimum base value
                                weights[i] = 0.5f + 0.5f * (weights[i] / maxAbsEigenvalue); // Weights between 0.5 and 1.0
                            }
                        }
                        else
                        {
                            // Fallback if max is zero (all eigenvalues are zero)
                            for (int i = 0; i < weights.Length; i++)
                            {
                                weights[i] = 1.0f; // Equal weights
                            }
                        }

                        // If we only need a single scalar weight for the overall loss
                        // For the loss function structure `tf.reduce_mean(tf.multiply(rawLoss, curvatureWeightTensor))`
                        // where rawLoss is (batch_size, 1), curvatureWeightTensor should also be (batch_size, 1) or (1, 1).
                        // Let's average the calculated weights to get a single scalar weight for the batch loss.
                        return new float[] { weights.Average() }; // Return average weight as a scalar array
                    }
                    else
                    {
                        // Default equal weights if eigenvalues sum to zero (all eigenvalues are zero or near zero)
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Sum of absolute eigenvalues is zero. Returning default weights.");
                        return new float[] { 1.0f }; // Return a default scalar weight
                    }
                }


                /// <summary>
                /// Calculates a mask that identifies the outermost vertices in a tensor.
                /// This version is a simplified conceptual mask for a 2D tensor.
                /// </summary>
                Tensor CalculateOutermostVertexMask(Tensor input)
                {
                    // Get the shape of the input tensor
                    var shape = tf.shape(input); // Shape is (batch_size, features)
                    var batchSize = tf.slice(shape, begin: new int[] { 0 }, size: new int[] { 1 });
                    var features = tf.slice(shape, begin: new int[] { 1 }, size: new int[] { 1 });

                    // Create a feature-wise weight that emphasizes the ends (conceptual vertices)
                    var featureIndices = tf.cast(tf.range(0, features), dtype: tf.float32); // Indices 0 to features-1
                    var normalizedIndices = tf.divide(featureIndices, tf.cast(features - 1, tf.float32)); // Normalize to 0-1

                    // Use a pattern that is high at 0 and 1, low in the middle
                    // abs(normalizedIndices - 0.5) gives values from 0.5 to 0 then back to 0.5
                    // Multiplying by 2 gives values from 1.0 to 0 then back to 1.0
                    var featureMask = tf.multiply(tf.abs(normalizedIndices - 0.5f), 2.0f, name: "feature_vertex_mask"); // Shape (features,)


                    // Expand the mask to match the batch dimension (batch_size, features)
                    var batchSizeInt = tf.cast(batchSize, tf.int32);
                    // Tile requires an array for multiples
                    var expandedMask = tf.tile(tf.reshape(featureMask, shape: new int[] { 1, -1 }), multiples: tf.concat(new[] { batchSizeInt, tf.constant(new int[] { 1 }) }, axis: 0), name: "expanded_vertex_mask");


                    // The expression modulator (shape batch_size, 3) needs to interact with this.
                    // This simple mask concept works better if the expression modulation is applied
                    // feature-wise or to the weights themselves, as done in GenerateWeightsFromExpression
                    // and the network structure above.
                    // For this specific 'CalculateOutermostVertexMask' function called in the TF graph,
                    // it's conceptual. A direct application could be masking weights or gradients.
                    // Since the modulation is applied to the hidden layer activation via a gate,
                    // this specific `CalculateOutermostVertexMask` tensor function is not strictly needed
                    // for the current graph structure, but kept for conceptual use if a mask was needed.

                    // Returning a simple broadcasted tensor for demonstration, or could return the featureMask.
                    // Let's return the feature mask expanded to match the input shape for conceptual use.
                    return expandedMask;
                }


                /// <summary>
                /// Converts a jagged array to a multidimensional array.
                /// </summary>
                float[,] ConvertJaggedToMultidimensional(float[][] jaggedArray)
                {
                    if (jaggedArray == null || jaggedArray.Length == 0 || jaggedArray[0] == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Jagged array is null or empty. Returning empty multidimensional array.");
                        return new float[0, 0];
                    }

                    int rows = jaggedArray.Length;
                    int cols = jaggedArray[0].Length;

                    float[,] result = new float[rows, cols];

                    for (int i = 0; i < rows; i++)
                    {
                        if (jaggedArray[i] == null || jaggedArray[i].Length != cols)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Row {i} in jagged array has inconsistent length ({jaggedArray[i]?.Length ?? 0} vs {cols}). Returning partial result.");
                            // Handle inconsistent rows by padding or skipping, depending on desired behavior.
                            // Here, we'll just fill the valid portion and leave the rest as default(float) (0.0f).
                            int currentCols = jaggedArray[i]?.Length ?? 0;
                            for (int j = 0; j < Math.Min(cols, currentCols); j++)
                            {
                                result[i, j] = jaggedArray[i][j];
                            }
                        }
                        else
                        {
                            System.Buffer.BlockCopy(jaggedArray[i], 0, result, i * cols * sizeof(float), cols * sizeof(float)); // Use System.Buffer
                        }
                    }

                    return result;
                }


                /// <summary>
                /// Extracts a batch from a multidimensional array using indices.
                /// </summary>
                float[,] ExtractBatch(float[,] data, int[] batchIndices, int startIdx, int count) // Renamed parameter 'indices' to 'batchIndices'
                {
                    if (data == null || batchIndices == null || data.GetLength(0) == 0 || batchIndices.Length == 0) // Added data/indices empty check, used batchIndices
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Invalid or empty input data/indices for ExtractBatch. Returning empty batch.");
                        return new float[0, 0];
                    }
                    if (batchIndices.Length < startIdx + count || data.GetLength(0) < batchIndices.Length) // Moved indices/data size check after empty check, used batchIndices
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Indices or data size mismatch for ExtractBatch. Returning empty batch.");
                        return new float[0, 0];
                    }

                    if (count <= 0) return new float[0, data.GetLength(1)]; // Return empty batch if count is non-positive

                    int cols = data.GetLength(1);
                    float[,] batch = new float[count, cols];

                    for (int i = 0; i < count; i++)
                    {
                        int srcIdx = batchIndices[startIdx + i]; // Used batchIndices
                        if (srcIdx < 0 || srcIdx >= data.GetLength(0))
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error: Invalid index {srcIdx} in ExtractBatch indices array at batch index {i}. Stopping batch extraction.");
                            // Return partial batch extracted so far
                            var partialBatch = new float[i, cols];
                            System.Buffer.BlockCopy(batch, 0, partialBatch, 0, i * cols * sizeof(float)); // Use System.Buffer
                            return partialBatch;
                        }
                        System.Buffer.BlockCopy(data, srcIdx * cols * sizeof(float), batch, i * cols * sizeof(float), cols * sizeof(float)); // Use System.Buffer
                    }

                    return batch;
                }

                /// <summary>
                /// Shuffles an array randomly.
                /// </summary>
                void ShuffleArray(int[] shuffleIndices) // Renamed parameter 'array' to 'shuffleIndices'
                {
                    if (shuffleIndices == null) return;
                    Random rng = new Random();
                    int n = shuffleIndices.Length;

                    while (n > 1)
                    {
                        n--;
                        int k = rng.Next(n + 1);
                        int temp = shuffleIndices[k];
                        shuffleIndices[k] = shuffleIndices[n];
                        shuffleIndices[n] = temp;
                    }
                }

                /// <summary>
                /// Serializes model metadata to JSON.
                /// </summary>
                string SerializeMetadata(Dictionary<string, object> metadata)
                {
                    // In a real implementation, use a proper JSON serializer (e.g., System.Text.Json or Newtonsoft.Json)
                    // This is a simplified version for the example
                    if (metadata == null) return "{}";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{");

                    bool first = true;
                    foreach (var entry in metadata)
                    {
                        if (!first)
                        {
                            sb.Append(",");
                        }

                        sb.Append($"\"{entry.Key}\":");

                        if (entry.Value is string strValue)
                        {
                            // Escape quotes in string values
                            sb.Append($"\"{strValue.Replace("\"", "\\\"")}\"");
                        }
                        else if (entry.Value is float[] floatArray)
                        {
                            sb.Append("[");
                            sb.Append(string.Join(",", floatArray.Select(f => f.ToString("F6"))));
                            sb.Append("]");
                        }
                        else if (entry.Value is double[] doubleArray) // Handle double arrays too
                        {
                            sb.Append("[");
                            sb.Append(string.Join(",", doubleArray.Select(d => d.ToString("F6"))));
                            sb.Append("]");
                        }
                        else if (entry.Value is int[] intArray) // Handle int arrays too
                        {
                            sb.Append("[");
                            sb.Append(string.Join(",", intArray));
                            sb.Append("]");
                        }
                        else if (entry.Value is Vector3 vector) // Handle Vector3
                        {
                            sb.Append($"[\"{vector.X:F6}\",\"{vector.Y:F6}\",\"{vector.Z:F6}\"]");
                        }
                        else if (entry.Value is float floatValue)
                        {
                            sb.Append(floatValue.ToString("F6")); // Format float
                        }
                        else if (entry.Value is double doubleValue)
                        {
                            sb.Append(doubleValue.ToString("F6")); // Format double
                        }
                        else if (entry.Value is int intValue)
                        {
                            sb.Append(intValue.ToString()); // Format int
                        }
                        else if (entry.Value is bool boolValue) // Handle bool values
                        {
                            sb.Append(boolValue.ToString().ToLower()); // Format bool to lowercase
                        }
                        else if (entry.Value is float[,] float2DArray) // Handle 2D float arrays (predictions)
                        {
                            sb.Append("[");
                            for (int i = 0; i < float2DArray.GetLength(0); i++)
                            {
                                if (i > 0) sb.Append(",");
                                sb.Append("[");
                                for (int j = 0; j < float2DArray.GetLength(1); j++)
                                {
                                    if (j > 0) sb.Append(",");
                                    sb.Append(float2DArray[i, j].ToString("F6"));
                                }
                                sb.Append("]");
                            }
                            sb.Append("]");
                        }
                        else if (entry.Value != null) // Catch other types, use ToString()
                        {
                            // Attempt to serialize other value types, handle potential issues
                            try
                            {
                                string valueStr = entry.Value.ToString();
                                // Basic check to see if it looks like a simple value (number, bool)
                                // or if it needs quoting (string)
                                if (float.TryParse(valueStr, out _) || double.TryParse(valueStr, out _) || bool.TryParse(valueStr, out _))
                                {
                                    sb.Append(valueStr);
                                }
                                else
                                {
                                    sb.Append($"\"{valueStr.Replace("\"", "\\\"")}\""); // Quote as string with escaping
                                }

                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: Failed to serialize metadata value for key '{entry.Key}': {ex.Message}");
                                sb.Append("\"SerializationError\""); // Indicate serialization failure
                            }
                        }
                        else // Value is null
                        {
                            sb.Append("null");
                        }

                        first = false;
                    }

                    sb.Append("}");
                    return sb.ToString();
                }


                // Helper function to process an array with K-means clustering
                void ProcessArrayWithKMeans(double[] dataArray, string arrayName, ConcurrentDictionary<string, object> resultsStore)
                {
                    // Step H.1: Validate input data for clustering
                    if (dataArray == null || dataArray.Length < 3 || dataArray.All(d => d == dataArray[0])) // Added null check and check for all same values
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Not enough distinct data points in {arrayName} for K-means clustering or data is constant. Skipping.");
                        // Store default/empty results to avoid downstream errors
                        resultsStore[$"{arrayName}_Category"] = "InsufficientData";
                        resultsStore[$"{arrayName}_NormalizedValue"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedX"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedY"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedZ"] = 0.0;
                        return;
                    }

                    try
                    {
                        // Step H.2: Convert 1D array to 2D array format required by Accord
                        double[][] points = new double[dataArray.Length][];
                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            points[i] = new double[] { dataArray[i] };
                        }

                        // Step H.3: Initialize K-means algorithm with k=3 clusters
                        // Ensure k is not greater than the number of data points
                        int k = Math.Min(3, points.Length);
                        if (k < 1) k = 1; // Ensure k is at least 1

                        var kmeans = new Accord.MachineLearning.KMeans(k);

                        // Step H.4: Configure distance metric for clustering
                        kmeans.Distance = new Accord.Math.Distances.SquareEuclidean();

                        // Step H.5: Compute and retrieve the centroids
                        try
                        {
                            var clusters = kmeans.Learn(points);
                            double[] centroids = clusters.Centroids.Select(c => c[0]).ToArray();

                            // Step H.6: Sort centroids in descending order and select top 3 (or fewer if k is less than 3)
                            Array.Sort(centroids);
                            Array.Reverse(centroids);
                            int numCentroids = Math.Min(3, centroids.Length);

                            // Pad centroids if less than 3 for consistent XYZ calculation
                            double[] paddedCentroids = new double[3];
                            for (int i = 0; i < numCentroids; i++) paddedCentroids[i] = centroids[i];


                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] K-means centroids for {arrayName}: [{string.Join(", ", centroids.Take(numCentroids).Select(c => c.ToString("F4")))}]");


                            // Step H.8: Calculate the central point (average of centroids)
                            double centralPoint = centroids.Take(numCentroids).DefaultIfEmpty(0).Average(); // Use DefaultIfEmpty(0) for safety
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Central point for {arrayName}: {centralPoint}");

                            // Step H.9: Normalize the central point
                            double maxAbsCentroid = centroids.Take(numCentroids).Select(Math.Abs).DefaultIfEmpty(0).Max(); // Use DefaultIfEmpty(0) for safety
                            double normalizedValue = maxAbsCentroid > 1e-6 ? centralPoint / maxAbsCentroid : 0; // Use tolerance for comparison

                            // Step H.10: Categorize the normalized value
                            string category;
                            if (normalizedValue < -0.33)
                                category = "Negative High";
                            else if (normalizedValue < 0)
                                category = "Negative Low";
                            else if (normalizedValue < 0.33)
                                category = "Positive Low";
                            else if (normalizedValue < 0.66)
                                category = "Positive Medium";
                            else
                                category = "Positive High";

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Normalized value for {arrayName}: {normalizedValue:F4}, Category: {category}");

                            // Step H.11: Calculate the XYZ coordinates using the padded top 3 centroids
                            double x = paddedCentroids[0];
                            double y = paddedCentroids[1];
                            double z = paddedCentroids[2];

                            // Step H.12: Normalize XYZ coordinates relative to 0,0,0 origin
                            double maxAbsCoordinate = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));
                            if (maxAbsCoordinate > 1e-6) // Use tolerance for comparison
                            {
                                x /= maxAbsCoordinate;
                                y /= maxAbsCoordinate;
                                z /= maxAbsCoordinate;
                            }

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Normalized XYZ coordinates for {arrayName}: ({x:F4}, {y:F4}, {z:F4})");

                            // Step H.13: Store the results in resultsStore
                            resultsStore[$"{arrayName}_Category"] = category;
                            resultsStore[$"{arrayName}_NormalizedValue"] = normalizedValue;
                            resultsStore[$"{arrayName}_NormalizedX"] = x;
                            resultsStore[$"{arrayName}_NormalizedY"] = y;
                            resultsStore[$"{arrayName}_NormalizedZ"] = z;

                        }
                        catch (Exception learnEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error during K-means Learn for {arrayName}: {learnEx.Message}");
                            // Store error indicators if learning failed
                            resultsStore[$"{arrayName}_Category"] = "ClusteringLearnError";
                            resultsStore[$"{arrayName}_NormalizedValue"] = 0.0;
                            resultsStore[$"{arrayName}_NormalizedX"] = 0.0;
                            resultsStore[$"{arrayName}_NormalizedY"] = 0.0;
                            resultsStore[$"{arrayName}_NormalizedZ"] = 0.0;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error processing K-means for {arrayName}: {ex.Message}");
                        // Store error indicators
                        resultsStore[$"{arrayName}_Category"] = "ProcessingError";
                        resultsStore[$"{arrayName}_NormalizedValue"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedX"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedY"] = 0.0;
                        resultsStore[$"{arrayName}_NormalizedZ"] = 0.0;
                    }
                }

                // Helper function to calculate trajectory stability
                double CalculateTrajectoryStability(List<double[]> trajectoryPoints)
                {
                    if (trajectoryPoints == null || trajectoryPoints.Count < 2) // Added null check
                        return 0.5; // Default stability for insufficient points

                    // Calculate consistency of direction along trajectory
                    double averageAngleChange = 0;
                    int angleCount = 0; // Track how many angle calculations were successful

                    for (int i = 1; i < trajectoryPoints.Count - 1; i++)
                    {
                        // Added null/length checks for points
                        if (trajectoryPoints[i - 1] == null || trajectoryPoints[i - 1].Length < 3 ||
                           trajectoryPoints[i] == null || trajectoryPoints[i].Length < 3 ||
                           trajectoryPoints[i + 1] == null || trajectoryPoints[i + 1].Length < 3)
                        {
                            continue; // Skip calculation if points are invalid
                        }


                        double[] prevVector = new double[3];
                        double[] nextVector = new double[3];

                        // Get vector from previous to current point
                        for (int j = 0; j < 3; j++)
                            prevVector[j] = trajectoryPoints[i][j] - trajectoryPoints[i - 1][j];

                        // Get vector from current to next point
                        for (int j = 0; j < 3; j++)
                            nextVector[j] = trajectoryPoints[i + 1][j] - trajectoryPoints[i][j];

                        // Calculate angle between vectors (dot product / product of magnitudes)
                        double dotProduct = 0;
                        double prevMagSq = 0;
                        double nextMagSq = 0;

                        for (int j = 0; j < 3; j++)
                        {
                            dotProduct += prevVector[j] * nextVector[j];
                            prevMagSq += prevVector[j] * prevVector[j];
                            nextMagSq += nextVector[j] * nextVector[j];
                        }

                        double prevMag = Math.Sqrt(prevMagSq);
                        double nextMag = Math.Sqrt(nextMagSq);


                        if (prevMag > 1e-9 && nextMag > 1e-9) // Use tolerance for non-zero magnitude
                        {
                            double cosAngle = dotProduct / (prevMag * nextMag);
                            // Clamp to valid range for acos
                            cosAngle = Math.Max(-1.0, Math.Min(1.0, cosAngle));
                            double angle = Math.Acos(cosAngle);
                            averageAngleChange += angle;
                            angleCount++;
                        }
                    }

                    if (angleCount > 0)
                        averageAngleChange /= angleCount; // Divide by count of successful angle calculations
                    else
                        return 0.5; // Default if no angles could be calculated

                    // Convert to stability score (0-1): lower angle changes = more stable
                    // Map angle [0, PI] to stability [1, 0]
                    double stabilityScore = 1.0 - (averageAngleChange / Math.PI);
                    return stabilityScore;
                }


                //==========================================================================
                // Workflow Coordination
                //==========================================================================
                // This function coordinates the sequential execution of the eight processing stages.
                string ExecuteProductionWorkflow(CoreMlOutcomeRecord record, int custId, Session session)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting multi-stage workflow for customer {custId}.");

                    // Step 1: Begin Data Acquisition and Initial Feature Analysis
                    string analysisResult = Stage1_DataAcquisitionAndAnalysis(record, custId);
                    unitResultsStore["DataAcquisitionResult"] = analysisResult; // Use more general key

                    // Step 2: Begin Feature Tensor Generation and Trajectory Mapping
                    string tensorMappingResult = Stage2_FeatureTensorAndMapping(analysisResult, custId);
                    unitResultsStore["FeatureTensorMappingResult"] = tensorMappingResult; // Use more general key

                    // Step 3: Begin Processed Feature Definition Creation
                    string processedFeatureResult = Stage3_ProcessedFeatureDefinition(tensorMappingResult, session, custId);
                    unitResultsStore["ProcessedFeatureResult"] = processedFeatureResult; // Use more general key

                    // Step 4: Begin Feature Quality Assessment
                    string qualityAssessmentResult = Stage4_FeatureQualityAssessment(processedFeatureResult, custId);
                    unitResultsStore["QualityAssessmentResult"] = qualityAssessmentResult; // Use more general key

                    // Step 5: Begin Combined Feature Evaluation
                    float combinedEvaluationScore = Stage5_CombinedFeatureEvaluation(qualityAssessmentResult, custId);
                    unitResultsStore["CombinedEvaluationScore"] = combinedEvaluationScore; // Use more general key

                    // Step 6: Begin Fractal Optimization Analysis
                    string optimizationAnalysisResult = Stage6_FractalOptimizationAnalysis(qualityAssessmentResult, combinedEvaluationScore, custId); // Pass QA result and evaluation score
                    unitResultsStore["OptimizationAnalysisResult"] = optimizationAnalysisResult; // Use more general key

                    // Step 7: Begin Tensor Network Training with Curvature Embedding (Includes Actual TF.NET)
                    string trainingOutcomeResult = Stage7_TensorNetworkTraining(optimizationAnalysisResult, custId, session, unitResultsStore); // Pass optimization analysis result
                    unitResultsStore["TensorNetworkTrainingOutcome"] = trainingOutcomeResult; // Use more general key

                    // Step 8: Begin Future Performance Projection
                    string performanceProjectionResult = Stage8_FutureProjection(trainingOutcomeResult, combinedEvaluationScore, custId); // Base projection on training outcome and evaluation score
                    unitResultsStore["PerformanceProjectionResult"] = performanceProjectionResult; // Use more general key


                    // Final workflow result combines key outputs - Use the projected score as the final outcome
                    float finalScore = unitResultsStore.TryGetValue("ProjectedPerformanceScore", out var projectedScoreVal) // Retrieve the projected score
                        ? Convert.ToSingle(projectedScoreVal) : combinedEvaluationScore; // Fallback to evaluation score if projection failed or not stored


                    unitResultsStore["ModelAProcessingOutcome"] = finalScore; // Store the final score for Unit D

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Workflow completed for customer {custId} with final score {finalScore:F4}");

                    return $"Workflow_Complete_Cust_{custId}_FinalScore_{finalScore:F4}"; // More general return string
                }

                //==========================================================================
                // Step 1: Data Acquisition & Analysis
                //==========================================================================
                string Stage1_DataAcquisitionAndAnalysis(CoreMlOutcomeRecord record, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 1 - Acquiring data and analyzing initial features for customer {custId}.");

                    // Step 1.1: Retrieve primary datasets from RuntimeProcessingContext
                    var productInventory = RuntimeProcessingContext.RetrieveContextValue("All_Simulated_Product_Inventory") as List<dynamic>;
                    var serviceOfferings = RuntimeProcessingContext.RetrieveContextValue("All_Simulated_Service_Offerings") as List<dynamic>;


                    // Process Product Data if available
                    if (productInventory != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 1 - Processing Product Data ({productInventory.Count} items).");
                        // Step 1.2: Extract product properties into separate arrays for analysis
                        var quantityAvailable = new List<int>();
                        var productMonetaryValue = new List<double>();
                        var productCostContribution = new List<double>();

                        foreach (var product in productInventory)
                        {
                            // FIX: Use dynamic access directly and convert to expected type
                            try
                            {
                                quantityAvailable.Add(Convert.ToInt32(product.QuantityAvailable));
                                productMonetaryValue.Add(Convert.ToDouble(product.MonetaryValue));
                                productCostContribution.Add(Convert.ToDouble(product.CostContributionValue));
                            }
                            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException rbEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] RuntimeBinder Error accessing product properties: {rbEx.Message}");
                                // Handle missing properties or invalid types if necessary, e.g., add default values or log specific product info
                                // For this example, we'll just log and skip this product or use default values added during list initialization
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Unexpected Error accessing product properties: {ex.Message}");
                                // Handle other potential exceptions during conversion
                            }
                        }

                        // Step 1.3: Output extracted product data for diagnostic purposes
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product QuantityAvailable: [{string.Join(", ", quantityAvailable)}]");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product MonetaryValue: [{string.Join(", ", productMonetaryValue)}]");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product CostContributionValue: [{string.Join(", ", productCostContribution)}]");

                        // Step 1.4: Process each product property array with K-means clustering for initial categorization
                        ProcessArrayWithKMeans(quantityAvailable.Select(x => (double)x).ToArray(), "Product QuantityAvailable", unitResultsStore);
                        ProcessArrayWithKMeans(productMonetaryValue.ToArray(), "Product MonetaryValue", unitResultsStore);
                        ProcessArrayWithKMeans(productCostContribution.ToArray(), "Product CostContributionValue", unitResultsStore);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product inventory data not found in RuntimeProcessingContext. Skipping product analysis.");
                    }

                    // Process Service Data if available
                    if (serviceOfferings != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 1 - Processing Service Data ({serviceOfferings.Count} items).");
                        // Step 1.5: Extract service properties into separate arrays for analysis
                        var fulfillmentQuantity = new List<int>();
                        var serviceMonetaryValue = new List<double>();
                        var serviceCostContribution = new List<double>();

                        foreach (var service in serviceOfferings)
                        {
                            // FIX: Use dynamic access directly and convert to expected type
                            try
                            {
                                fulfillmentQuantity.Add(Convert.ToInt32(service.FulfillmentQuantity));
                                serviceMonetaryValue.Add(Convert.ToDouble(service.MonetaryValue));
                                serviceCostContribution.Add(Convert.ToDouble(service.CostContributionValue));
                            }
                            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException rbEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] RuntimeBinder Error accessing service properties: {rbEx.Message}");
                                // Handle missing properties or invalid types if necessary
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Unexpected Error accessing service properties: {ex.Message}");
                                // Handle other potential exceptions
                            }
                        }

                        // Step 1.6: Output extracted service data for diagnostic purposes
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service FulfillmentQuantity: [{string.Join(", ", fulfillmentQuantity)}]");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service MonetaryValue: [{string.Join(", ", serviceMonetaryValue)}]");
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service CostContributionValue: [{string.Join(", ", serviceCostContribution)}]");

                        // Step 1.7: Process each service property array with K-means clustering for initial categorization
                        ProcessArrayWithKMeans(fulfillmentQuantity.Select(x => (double)x).ToArray(), "Service FulfillmentQuantity", unitResultsStore);
                        ProcessArrayWithKMeans(serviceMonetaryValue.ToArray(), "Service MonetaryValue", unitResultsStore);
                        ProcessArrayWithKMeans(serviceCostContribution.ToArray(), "Service CostContributionValue", unitResultsStore);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service offerings data not found in RuntimeProcessingContext. Skipping service analysis.");
                    }

                    // TODO: Incorporate additional data sources or initial analysis steps here.

                    // Step 1.8: Generate a result string summarizing the initial analysis state.
                    string result = $"InitialAnalysis_Cust_{custId}_Record_{record.RecordIdentifier}";

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 1 - Data acquisition and initial analysis completed: {result}");
                    return result;
                }

                //==========================================================================
                // Step 2: Feature Tensor Generation & Mapping
                //==========================================================================
                string Stage2_FeatureTensorAndMapping(string analysisResult, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 2 - Generating feature tensors and mapping trajectories for customer {custId}.");

                    // Step 2.1: Retrieve initial analysis results (coordinates) from the shared store.
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 2 - Retrieving coordinates from Step 1 analysis.");

                    //------------------------------------------
                    // PRODUCT COORDINATES (Derived from Step 1 K-means analysis)
                    //------------------------------------------
                    string prodQtyCategory = unitResultsStore.TryGetValue("Product QuantityAvailable_Category", out var pqc) ? pqc.ToString() : "Unknown";
                    double prodQtyX = unitResultsStore.TryGetValue("Product QuantityAvailable_NormalizedX", out var pqx) ? Convert.ToDouble(pqx) : 0;
                    double prodQtyY = unitResultsStore.TryGetValue("Product QuantityAvailable_NormalizedY", out var pqy) ? Convert.ToDouble(pqy) : 0;
                    double prodQtyZ = unitResultsStore.TryGetValue("Product QuantityAvailable_NormalizedZ", out var pqz) ? Convert.ToDouble(pqz) : 0;

                    string prodMonCategory = unitResultsStore.TryGetValue("Product MonetaryValue_Category", out var pmc) ? pmc.ToString() : "Unknown";
                    double prodMonX = unitResultsStore.TryGetValue("Product MonetaryValue_NormalizedX", out var pmx) ? Convert.ToDouble(pmx) : 0;
                    double prodMonY = unitResultsStore.TryGetValue("Product MonetaryValue_NormalizedY", out var pmy) ? Convert.ToDouble(pmy) : 0;
                    double prodMonZ = unitResultsStore.TryGetValue("Product MonetaryValue_NormalizedZ", out var pmz) ? Convert.ToDouble(pmz) : 0;

                    string prodCostCategory = unitResultsStore.TryGetValue("Product CostContributionValue_Category", out var pcc) ? pcc.ToString() : "Unknown";
                    double prodCostX = unitResultsStore.TryGetValue("Product CostContributionValue_NormalizedX", out var pcx) ? Convert.ToDouble(pcx) : 0;
                    double prodCostY = unitResultsStore.TryGetValue("Product CostContributionValue_NormalizedY", out var pcy) ? Convert.ToDouble(pcy) : 0;
                    double prodCostZ = unitResultsStore.TryGetValue("Product CostContributionValue_NormalizedZ", out var pcz) ? Convert.ToDouble(pcz) : 0;

                    //------------------------------------------
                    // SERVICE COORDINATES (Derived from Step 1 K-means analysis)
                    //------------------------------------------
                    // FIX: Retrieve service coordinate values from unitResultsStore
                    string servFulfillCategory = unitResultsStore.TryGetValue("Service FulfillmentQuantity_Category", out var sfc) ? sfc.ToString() : "Unknown";
                    double servFulfillX = unitResultsStore.TryGetValue("Service FulfillmentQuantity_NormalizedX", out var sfx) ? Convert.ToDouble(sfx) : 0;
                    double servFulfillY = unitResultsStore.TryGetValue("Service FulfillmentQuantity_NormalizedY", out var sfy) ? Convert.ToDouble(sfy) : 0;
                    double servFulfillZ = unitResultsStore.TryGetValue("Service FulfillmentQuantity_NormalizedZ", out var sfz) ? Convert.ToDouble(sfz) : 0;

                    string servMonCategory = unitResultsStore.TryGetValue("Service MonetaryValue_Category", out var smc) ? smc.ToString() : "Unknown";
                    double servMonX = unitResultsStore.TryGetValue("Service MonetaryValue_NormalizedX", out var smx) ? Convert.ToDouble(smx) : 0;
                    double servMonY = unitResultsStore.TryGetValue("Service MonetaryValue_NormalizedY", out var smy) ? Convert.ToDouble(smy) : 0;
                    double servMonZ = unitResultsStore.TryGetValue("Service MonetaryValue_NormalizedZ", out var smz) ? Convert.ToDouble(smz) : 0;

                    string servCostCategory = unitResultsStore.TryGetValue("Service CostContributionValue_Category", out var scc) ? scc.ToString() : "Unknown";
                    double servCostX = unitResultsStore.TryGetValue("Service CostContributionValue_NormalizedX", out var scx) ? Convert.ToDouble(scx) : 0;
                    double servCostY = unitResultsStore.TryGetValue("Service CostContributionValue_NormalizedY", out var scy) ? Convert.ToDouble(scy) : 0;
                    double servCostZ = unitResultsStore.TryGetValue("Service CostContributionValue_NormalizedZ", out var scz) ? Convert.ToDouble(scz) : 0;


                    // TODO: Incorporate additional data sources or initial analysis steps here.

                    // Step 2.2: Calculate overall feature tensors, magnitudes, and initial trajectories.
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 2 - Calculating tensors, magnitudes, and trajectories.");

                    //------------------------------------------
                    // PRODUCT TENSOR CALCULATIONS
                    //------------------------------------------
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] ----- PRODUCT TENSOR AND MAGNITUDE CALCULATIONS -----");

                    double prodOverallTensorX = (prodQtyX + prodMonX + prodCostX) / 3.0; // Use double for division
                    double prodOverallTensorY = (prodQtyY + prodMonY + prodCostY) / 3.0;
                    double prodOverallTensorZ = (prodQtyZ + prodMonZ + prodCostZ) / 3.0;
                    double prodOverallMagnitude = Math.Sqrt(prodOverallTensorX * prodOverallTensorX + prodOverallTensorY * prodOverallTensorY + prodOverallTensorZ * prodOverallTensorZ);

                    double[] prodTrajectory = new double[3] { 0, 0, 0 };
                    if (prodOverallMagnitude > 1e-9) // Use tolerance for comparison with zero
                    {
                        prodTrajectory[0] = prodOverallTensorX / prodOverallMagnitude;
                        prodTrajectory[1] = prodOverallTensorY / prodOverallMagnitude;
                        prodTrajectory[2] = prodOverallTensorZ / prodOverallMagnitude;
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Overall Tensor: ({prodOverallTensorX:F4}, {prodOverallTensorY:F4}, {prodOverallTensorZ:F4})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Overall Magnitude: {prodOverallMagnitude:F4}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Trajectory: ({prodTrajectory[0]:F4}, {prodTrajectory[1]:F4}, {prodTrajectory[2]:F4})");

                    //------------------------------------------
                    // SERVICE TENSOR CALCULATIONS
                    //------------------------------------------
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] ----- SERVICE TENSOR AND MAGNITUDE CALCULATIONS -----");

                    double servOverallTensorX = (servFulfillX + servMonX + servCostX) / 3.0; // Use double for division
                    double servOverallTensorY = (servFulfillY + servMonY + servCostY) / 3.0;
                    double servOverallTensorZ = (servFulfillZ + servMonZ + servCostZ) / 3.0;
                    double servOverallMagnitude = Math.Sqrt(servOverallTensorX * servOverallTensorX + servOverallTensorY * servOverallTensorY + servOverallTensorZ * servOverallTensorZ);

                    double[] servTrajectory = new double[3] { 0, 0, 0 };
                    if (servOverallMagnitude > 1e-9) // Use tolerance for comparison with zero
                    {
                        servTrajectory[0] = servOverallTensorX / servOverallMagnitude;
                        servTrajectory[1] = servOverallTensorY / servOverallMagnitude;
                        servTrajectory[2] = servOverallTensorZ / servOverallMagnitude;
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Overall Tensor: ({servOverallTensorX:F4}, {servOverallTensorY:F4}, {servOverallTensorZ:F4})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Overall Magnitude: {servOverallMagnitude:F4}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Trajectory: ({servTrajectory[0]:F4}, {servTrajectory[1]:F4}, {servTrajectory[2]:F4})");

                    // TODO: Calculate tensors for additional categories if added.

                    //==========================================================================
                    // Trajectory Plot Generation & Analysis
                    //==========================================================================
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] ----- TRAJECTORY PLOT GENERATION & ANALYSIS -----");

                    // Set parameters for recursive trajectory plotting
                    const int MAX_RECURSION_DEPTH = 35;
                    const double CONTINUE_PAST_PLANE = -2.0;

                    // Create data structures to store trajectory plot points and intensities
                    List<double[]> productTrajectoryPoints = new List<double[]>();
                    List<double> productPointIntensities = new List<double>();
                    List<double[]> serviceTrajectoryPoints = new List<double[]>();
                    List<double> servicePointIntensities = new List<double>();

                    // Starting positions for trajectories (at origin of overall tensor)
                    double[] productCurrentPosition = new double[] { prodOverallTensorX, prodOverallTensorY, prodOverallTensorZ };
                    double[] serviceCurrentPosition = new double[] { servOverallTensorX, servOverallTensorY, servOverallTensorZ };

                    // Calculate scaling factor for magnitude visualization
                    double recursionFactor = 0.95;

                    // Invert trajectories to ensure they move toward negative X and Y
                    double[] InvertTrajectoryIfNeeded(double[] trajectory)
                    {
                        // Check if trajectory moves toward negative X and Y
                        bool movesTowardNegativeX = trajectory != null && trajectory.Length > 0 && trajectory[0] < -1e-6; // Use tolerance
                        bool movesTowardNegativeY = trajectory != null && trajectory.Length > 1 && trajectory[1] < -1e-6; // Use tolerance

                        // If not moving toward negative for both X and Y, invert the trajectory
                        if (!movesTowardNegativeX || !movesTowardNegativeY)
                        {
                            if (trajectory == null || trajectory.Length < 3) return new double[] { 0, 0, 0 }; // Return default if invalid input


                            double[] invertedTrajectory = new double[3];
                            // Invert if not sufficiently negative
                            invertedTrajectory[0] = movesTowardNegativeX ? trajectory[0] : -Math.Abs(trajectory[0]);
                            invertedTrajectory[1] = movesTowardNegativeY ? trajectory[1] : -Math.Abs(trajectory[1]);
                            invertedTrajectory[2] = trajectory.Length > 2 ? trajectory[2] : 0; // Keep Z as is, handle short array

                            // Normalize again
                            double magnitude = Math.Sqrt(
                                invertedTrajectory[0] * invertedTrajectory[0] +
                                invertedTrajectory[1] * invertedTrajectory[1] +
                                invertedTrajectory[2] * invertedTrajectory[2]
                            );

                            if (magnitude > 1e-9) // Use tolerance for comparison
                            {
                                invertedTrajectory[0] /= magnitude;
                                invertedTrajectory[1] /= magnitude;
                                invertedTrajectory[2] /= magnitude;
                            }

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Inverted trajectory from ({trajectory[0]:F4}, {trajectory[1]:F4}, {trajectory[2]:F4}) to ({invertedTrajectory[0]:F4}, {invertedTrajectory[1]:F4}, {invertedTrajectory[2]:F4})");
                            return invertedTrajectory;
                        }
                        if (trajectory != null && trajectory.Length >= 3) return (double[])trajectory.Clone(); // Return clone if no inversion needed
                        return new double[] { 0, 0, 0 }; // Default if invalid input
                    }

                    // Get adjusted trajectories (ensure they move toward negative X and Y)
                    double[] productTrajectoryAdjusted = InvertTrajectoryIfNeeded(prodTrajectory); // Renamed to avoid conflict
                    double[] serviceTrajectoryAdjusted = InvertTrajectoryIfNeeded(servTrajectory); // Renamed to avoid conflict

                    // Function to recursively plot trajectory
                    void RecursivePlotTrajectory(double[] currentPosition, double[] trajectory, double magnitude,
                                                List<double[]> points, List<double> intensities, int depth,
                                                string trajectoryName)
                    {
                        if (currentPosition == null || trajectory == null || currentPosition.Length < 3 || trajectory.Length < 3) return; // Add null/length check


                        // Base case - stop recursion at maximum depth
                        if (depth >= MAX_RECURSION_DEPTH)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {trajectoryName} recursion stopped at max depth {depth}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {trajectoryName} final position: ({currentPosition[0]:F6}, {currentPosition[1]:F6}, {currentPosition[2]:F4})");
                            // Add the final point to ensure the endpoint in negative space is captured
                            points.Add((double[])currentPosition.Clone());
                            double finalPointIntensity = magnitude * Math.Pow(recursionFactor, depth); // Renamed variable
                            intensities.Add(finalPointIntensity);
                            return;
                        }

                        // Stop if we've gone far enough past both planes
                        if (currentPosition[0] < CONTINUE_PAST_PLANE && currentPosition[1] < CONTINUE_PAST_PLANE)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {trajectoryName} recursion stopped - Reached target negative threshold at depth {depth}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {trajectoryName} final position: ({currentPosition[0]:F6}, {currentPosition[1]:F6}, {currentPosition[2]:F4})");
                            // Add the final point before stopping
                            points.Add((double[])currentPosition.Clone());
                            double finalPointIntensity = magnitude * Math.Pow(recursionFactor, depth); // Renamed variable
                            intensities.Add(finalPointIntensity);
                            return;
                        }

                        // Add current position to trajectory points
                        points.Add((double[])currentPosition.Clone());

                        // Calculate intensity based on magnitude and recursion depth
                        double currentPointIntensity = magnitude * Math.Pow(recursionFactor, depth); // Renamed variable
                        intensities.Add(currentPointIntensity);

                        // Note progress through planes
                        bool beyondXPlane = currentPosition[0] < -1e-6; // Use tolerance
                        bool beyondYPlane = currentPosition[1] < -1e-6; // Use tolerance
                        bool beyondBothPlanes = beyondXPlane && beyondYPlane;

                        // Log position for every 4th point or when crossing planes
                        if (depth % 4 == 0 || beyondBothPlanes ||
                            (depth > 0 && points.Count > 1 && ( // Ensure we have a previous point to compare
                                (points[points.Count - 2][0] >= -1e-6 && beyondXPlane) || // Crossed X=0 with tolerance
                                (points[points.Count - 2][1] >= -1e-6 && beyondYPlane)     // Crossed Y=0 with tolerance
                            )))
                        {
                            string positionInfo = "";
                            if (beyondXPlane) positionInfo += " BEYOND-X-PLANE";
                            if (beyondYPlane) positionInfo += " BEYOND-Y-PLANE";

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {trajectoryName} point {depth}: " +
                                                              $"Position=({currentPosition[0]:F6}, {currentPosition[1]:F6}, {currentPosition[2]:F4}), " +
                                                              $"Intensity={currentPointIntensity:F4}{positionInfo}"); // Use renamed variable
                        }

                        // Calculate next position along trajectory
                        // Use aggressive step size to ensure we reach negative territory
                        double stepMultiplier = 1.0;

                        // Larger steps early in recursion, smaller as we get deeper
                        if (depth < 10)
                        {
                            stepMultiplier = 2.0; // Much larger initial steps
                        }
                        else if (!beyondBothPlanes && depth < MAX_RECURSION_DEPTH - 5) // Ensure we try to reach negative territory unless near end
                        {
                            stepMultiplier = 1.5; // Still need to reach negative territory
                        }
                        else
                        {
                            stepMultiplier = 1.0; // Standard step once in negative territory or near end
                        }

                        // Base step size calculation
                        double stepSize = magnitude * Math.Pow(recursionFactor, depth) * 0.4 * stepMultiplier;

                        // Calculate next position
                        double[] nextPosition = new double[3];
                        for (int i = 0; i < 3; i++)
                        {
                            nextPosition[i] = currentPosition[i] + trajectory[i] * stepSize;
                        }

                        // Recursively plot next point
                        RecursivePlotTrajectory(nextPosition, trajectory, magnitude, points, intensities, depth + 1, trajectoryName);
                    }


                    // Generate recursive plots for product trajectory
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Generating Product trajectory recursive plot");
                    RecursivePlotTrajectory(productCurrentPosition, productTrajectoryAdjusted, prodOverallMagnitude,
                                           productTrajectoryPoints, productPointIntensities, 0, "PRODUCT");

                    // Generate recursive plots for service trajectory
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Generating Service trajectory recursive plot");
                    RecursivePlotTrajectory(serviceCurrentPosition, serviceTrajectoryAdjusted, servOverallMagnitude,
                                           serviceTrajectoryPoints, servicePointIntensities, 0, "SERVICE");

                    // TODO: Generate plots for additional categories if added.

                    //==========================================================================
                    // Trajectory Intersection Analysis
                    //==========================================================================

                    // Calculate plane intersection points for both trajectories
                    // Use a tolerance for checking sign changes when crossing planes
                    double[] productXPlaneIntersection = CalculatePlaneIntersection(productTrajectoryPoints, 0, 1e-6); // X=0 plane
                    double[] productYPlaneIntersection = CalculatePlaneIntersection(productTrajectoryPoints, 1, 1e-6); // Y=0 plane
                    double[] serviceXPlaneIntersection = CalculatePlaneIntersection(serviceTrajectoryPoints, 0, 1e-6); // X=0 plane
                    double[] serviceYPlaneIntersection = CalculatePlaneIntersection(serviceTrajectoryPoints, 1, 1e-6); // Y=0 plane;


                    // Helper function to calculate plane intersection point
                    double[] CalculatePlaneIntersection(List<double[]> trajectoryPoints, int planeAxis, double tolerance)
                    {
                        // Need at least two points to find an intersection
                        if (trajectoryPoints == null || trajectoryPoints.Count < 2) // Added null check
                            return null;

                        // Find the first pair of points that cross the plane (considering tolerance)
                        for (int i = 1; i < trajectoryPoints.Count; i++)
                        {
                            // Added null/length checks for points
                            if (trajectoryPoints[i - 1] == null || trajectoryPoints[i - 1].Length < Math.Max(3, planeAxis + 1) ||
                               trajectoryPoints[i] == null || trajectoryPoints[i].Length < Math.Max(3, planeAxis + 1))
                            {
                                continue; // Skip if points are invalid for the calculation
                            }

                            double v1 = trajectoryPoints[i - 1][planeAxis];
                            double v2 = trajectoryPoints[i][planeAxis];

                            // Check if points are on opposite sides of the plane
                            // Use tolerance: one point is >= -tol and the other is <= +tol
                            // This handles cases where a point might be exactly zero or very close
                            bool crossedZero = (v1 >= -tolerance && v2 <= tolerance) || (v1 <= tolerance && v2 >= -tolerance);

                            if (crossedZero && !((v1 >= -tolerance && v1 <= tolerance) && (v2 >= -tolerance && v2 <= tolerance))) // Ensure they aren't both near zero
                            {
                                // Handle the case where one point is exactly zero or very close
                                if (Math.Abs(v1) < tolerance) return (double[])trajectoryPoints[i - 1].Clone();
                                if (Math.Abs(v2) < tolerance) return (double[])trajectoryPoints[i].Clone();


                                // Calculate the interpolation factor (t) where the plane is crossed
                                // Use absolute values for interpolation based on distance from zero
                                double t = Math.Abs(v1) / (Math.Abs(v1) + Math.Abs(v2));

                                // Interpolate the exact intersection point
                                double[] intersection = new double[3];
                                for (int j = 0; j < 3; j++)
                                {
                                    // Added bounds check for j
                                    if (j < trajectoryPoints[i - 1].Length && j < trajectoryPoints[i].Length)
                                    {
                                        intersection[j] = trajectoryPoints[i - 1][j] * (1 - t) + trajectoryPoints[i][j] * t;
                                    }
                                    else
                                    {
                                        intersection[j] = 0; // Default to 0 if coordinate doesn't exist
                                    }
                                }

                                // Explicitly set the plane axis coordinate to zero for precision
                                if (planeAxis >= 0 && planeAxis < intersection.Length)
                                {
                                    intersection[planeAxis] = 0.0;
                                }


                                return intersection;
                            }
                        }

                        return null; // No intersection found
                    }


                    //==========================================================================
                    // Feature Coordinate Extraction
                    //==========================================================================

                    // Find positive and negative coordinate pairs for both trajectories
                    // Use tolerance for checking signs
                    double[] FindPositiveCoordinate(List<double[]> points, double tolerance)
                    {
                        if (points == null || points.Count == 0) return new double[] { 0, 0, 0 }; // Added null/empty check

                        foreach (var point in points)
                        {
                            if (point != null && point.Length >= 2 && point[0] > tolerance && point[1] > tolerance) // Added null/length check
                                return (double[])point.Clone();
                        }
                        // Default to first point if no suitable point found
                        int firstIndex = points.Count > 0 ? 0 : -1;
                        if (firstIndex != -1 && points[firstIndex] != null && points[firstIndex].Length >= 3) return (double[])points[firstIndex].Clone(); // Return clone if valid
                        return new double[] { 0, 0, 0 }; // Default if first point invalid or list empty

                    }

                    double[] FindNegativeCoordinate(List<double[]> points, double tolerance)
                    {
                        if (points == null || points.Count == 0) return new double[] { 0, 0, 0 }; // Added null/empty check

                        foreach (var point in points)
                        {
                            if (point != null && point.Length >= 2 && point[0] < -tolerance && point[1] < -tolerance) // Added null/length check
                                return (double[])point.Clone();
                        }

                        // Default to last point if no suitable point found
                        int lastIndex = points.Count > 0 ? points.Count - 1 : 0;
                        if (points[lastIndex] != null && points[lastIndex].Length >= 3) return (double[])points[lastIndex].Clone(); // Return clone if valid
                        return new double[] { 0, 0, 0 }; // Default if last point invalid or list empty
                    }

                    // Calculate velocities based on trajectory and magnitude
                    double CalculateVelocity(double[] trajectory, double magnitude)
                    {
                        // Velocity is magnitude scaled by trajectory length (which should be 1 if normalized)
                        // This definition might be simplified; perhaps just use magnitude directly?
                        // Let's use magnitude as a simpler measure of velocity
                        return magnitude;
                    }


                    //------------------------------------------
                    // PRODUCT TRAJECTORY VARIABLES
                    //------------------------------------------
                    double[] productVector = (productTrajectoryAdjusted != null && productTrajectoryAdjusted.Length >= 3) ? (double[])productTrajectoryAdjusted.Clone() : new double[] { 0, 0, 0 }; // Use adjusted trajectory, handle null/length
                    double productVelocity = CalculateVelocity(productVector, prodOverallMagnitude);
                    double[] productPositiveCoordinate = FindPositiveCoordinate(productTrajectoryPoints, 1e-6); // Use tolerance
                    double[] productNegativeCoordinate = FindNegativeCoordinate(productTrajectoryPoints, 1e-6); // Use tolerance


                    //------------------------------------------
                    // SERVICE TRAJECTORY VARIABLES
                    //------------------------------------------
                    double[] serviceVector = (serviceTrajectoryAdjusted != null && serviceTrajectoryAdjusted.Length >= 3) ? (double[])serviceTrajectoryAdjusted.Clone() : new double[] { 0, 0, 0 }; // Use adjusted trajectory, handle null/length
                    double serviceVelocity = CalculateVelocity(serviceVector, servOverallMagnitude);
                    double[] servicePositiveCoordinate = FindPositiveCoordinate(serviceTrajectoryPoints, 1e-6); // Use tolerance
                    double[] serviceNegativeCoordinate = FindNegativeCoordinate(serviceTrajectoryPoints, 1e-6); // Use tolerance


                    // TODO: Extract coordinates for additional categories if added.

                    //==========================================================================
                    // Trajectory Analysis Logging
                    //==========================================================================

                    // Log plane intersection points and key trajectory data
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] ----- PLANE INTERSECTION ANALYSIS -----");

                    if (productXPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product X-Plane Intersection: " +
                                                          $"(0.000000, {productXPlaneIntersection[1]:F6}, {productXPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product trajectory does not intersect X-Plane");
                    }

                    if (productYPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Y-Plane Intersection: " +
                                                          $"({productYPlaneIntersection[0]:F6}, 0.000000, {productYPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product trajectory does not intersect Y-Plane");
                    }

                    if (serviceXPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service X-Plane Intersection: " +
                                                          $"(0.000000, {serviceXPlaneIntersection[1]:F6}, {serviceXPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service trajectory does not intersect X-Plane");
                    }

                    if (serviceYPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Y-Plane Intersection: " +
                                                          $"({serviceYPlaneIntersection[0]:F6}, 0.000000, {serviceYPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service trajectory does not intersect Y-Plane");
                    }

                    // Log key trajectory data
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] ----- KEY TRAJECTORY DATA -----");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Vector: ({productVector[0]:F6}, {productVector[1]:F6}, {productVector[2]:F6})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Velocity: {productVelocity:F6}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Positive Coordinate: ({productPositiveCoordinate[0]:F6}, {productPositiveCoordinate[1]:F6}, {productPositiveCoordinate[2]:F6})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product Negative Coordinate: ({productNegativeCoordinate[0]:F6}, {productNegativeCoordinate[1]:F6}, {productNegativeCoordinate[2]:F6})");

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Vector: ({serviceVector[0]:F6}, {serviceVector[1]:F6}, {serviceVector[2]:F6})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Velocity: {serviceVelocity:F6}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Positive Coordinate: ({servicePositiveCoordinate[0]:F6}, {servicePositiveCoordinate[1]:F6}, {servicePositiveCoordinate[2]:F6})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service Negative Coordinate: ({serviceNegativeCoordinate[0]:F6}, {serviceNegativeCoordinate[1]:F6}, {serviceNegativeCoordinate[2]:F6})");

                    //==========================================================================
                    // Trajectory Statistics
                    //==========================================================================

                    // Count points in negative quadrants using tolerance
                    int productNegativeXCount = CountNegativePoints(productTrajectoryPoints, 0, 1e-6);
                    int productNegativeYCount = CountNegativePoints(productTrajectoryPoints, 1, 1e-6);
                    int productNegativeBothCount = CountNegativeBothPoints(productTrajectoryPoints, 1e-6);

                    int serviceNegativeXCount = CountNegativePoints(serviceTrajectoryPoints, 0, 1e-6);
                    int serviceNegativeYCount = CountNegativePoints(serviceTrajectoryPoints, 1, 1e-6);
                    int serviceNegativeBothCount = CountNegativeBothPoints(serviceTrajectoryPoints, 1e-6);

                    // Helper functions to count negative points using tolerance
                    int CountNegativePoints(List<double[]> points, int axis, double tolerance)
                    {
                        if (points == null) return 0; // Added null check
                        int count = 0;
                        foreach (var point in points)
                        {
                            if (point != null && point.Length > axis && point[axis] < -tolerance) // Added null/length check
                                count++;
                        }
                        return count;
                    }

                    int CountNegativeBothPoints(List<double[]> points, double tolerance)
                    {
                        if (points == null) return 0; // Added null check
                        int count = 0;
                        foreach (var point in points)
                        {
                            if (point != null && point.Length >= 2 && point[0] < -tolerance && point[1] < -tolerance) // Added null/length check
                                count++;
                        }
                        return count;
                    }

                    // Log negative point counts
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product negative X count: {productNegativeXCount}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product negative Y count: {productNegativeYCount}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product negative both count: {productNegativeBothCount}");

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service negative X count: {serviceNegativeXCount}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service negative Y count: {serviceNegativeYCount}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service negative both count: {serviceNegativeBothCount}");

                    //==========================================================================
                    // Store Analysis Results
                    //==========================================================================

                    // Store trajectory plot data in unitResultsStore
                    unitResultsStore["Product_TrajectoryPoints"] = productTrajectoryPoints;
                    unitResultsStore["Product_PointIntensities"] = productPointIntensities;
                    unitResultsStore["Service_TrajectoryPoints"] = serviceTrajectoryPoints;
                    unitResultsStore["Service_PointIntensities"] = servicePointIntensities;

                    // Store plane intersections
                    unitResultsStore["Product_XPlaneIntersection"] = productXPlaneIntersection;
                    unitResultsStore["Product_YPlaneIntersection"] = productYPlaneIntersection;
                    unitResultsStore["Service_XPlaneIntersection"] = serviceXPlaneIntersection;
                    unitResultsStore["Service_YPlaneIntersection"] = serviceYPlaneIntersection;

                    // Store key trajectory data
                    unitResultsStore["Product_Vector"] = productVector;
                    unitResultsStore["Product_Velocity"] = productVelocity;
                    unitResultsStore["Product_PositiveCoordinate"] = productPositiveCoordinate;
                    unitResultsStore["Product_NegativeCoordinate"] = productNegativeCoordinate;

                    unitResultsStore["Service_Vector"] = serviceVector;
                    unitResultsStore["Service_Velocity"] = serviceVelocity;
                    unitResultsStore["Service_PositiveCoordinate"] = servicePositiveCoordinate;
                    unitResultsStore["Service_NegativeCoordinate"] = serviceNegativeCoordinate;

                    // Store trajectory statistics
                    unitResultsStore["Product_NegativeXCount"] = productNegativeXCount;
                    unitResultsStore["Product_NegativeYCount"] = productNegativeYCount;
                    unitResultsStore["Product_NegativeBothCount"] = productNegativeBothCount;
                    unitResultsStore["Service_NegativeXCount"] = serviceNegativeXCount;
                    unitResultsStore["Service_NegativeYCount"] = serviceNegativeYCount;
                    unitResultsStore["Service_NegativeBothCount"] = serviceNegativeBothCount;

                    // TODO: Store results for additional categories if added.

                    // Log summary
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Product trajectory plot: {productTrajectoryPoints?.Count ?? 0} points, {productNegativeBothCount} in negative X-Y quadrant"); // Added null check
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Service trajectory plot: {serviceTrajectoryPoints?.Count ?? 0} points, {serviceNegativeBothCount} in negative X-Y quadrant"); // Added null check

                    // Step 2.3: Generate a result string summarizing the tensor generation and mapping state.
                    string result = $"FeatureTensorsAndMapping_Cust_{custId}_BasedOn_{analysisResult.Replace("InitialAnalysis_", "")}";

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 2 - Feature tensor generation and mapping completed: {result}");
                    return result;
                }

                //==========================================================================
                // Step 3: Processed Feature Definition Creation
                //==========================================================================
                string Stage3_ProcessedFeatureDefinition(string tensorMappingResult, Session session, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 3 - Creating processed feature definition for customer {custId}.");

                    //------------------------------------------
                    // RETRIEVE FEATURE DATA (from Step 2)
                    //------------------------------------------
                    // Retrieve velocity, trajectory and intersection data for QA assessment
                    double productVelocity = unitResultsStore.TryGetValue("Product_Velocity", out var pv)
                        ? Convert.ToDouble(pv) : 0.5;
                    double serviceVelocity = unitResultsStore.TryGetValue("Service_Velocity", out var sv)
                        ? Convert.ToDouble(sv) : 0.5;

                    // Retrieve plane intersection data
                    double[] productXPlaneIntersection = unitResultsStore.TryGetValue("Product_XPlaneIntersection", out var pxi)
                        ? pxi as double[] : null;
                    double[] productYPlaneIntersection = unitResultsStore.TryGetValue("Product_YPlaneIntersection", out var pyi)
                        ? pyi as double[] : null;
                    double[] serviceXPlaneIntersection = unitResultsStore.TryGetValue("Service_XPlaneIntersection", out var sxi)
                        ? sxi as double[] : null;
                    double[] serviceYPlaneIntersection = unitResultsStore.TryGetValue("Service_YPlaneIntersection", out var syi)
                        ? syi as double[] : null;

                    // Get negative coordinate counts to assess trajectory behavior
                    int productNegativeBothCount = unitResultsStore.TryGetValue("Product_NegativeBothCount", out var pnbc)
                        ? Convert.ToInt32(pnbc) : 0;
                    int serviceNegativeBothCount = unitResultsStore.TryGetValue("Service_NegativeBothCount", out var snbc)
                        ? Convert.ToInt32(snbc) : 0;

                    // TODO: Retrieve feature data for additional categories if added.

                    //------------------------------------------
                    // APPLY FEATURE PROCESSING LOGIC
                    //------------------------------------------
                    // Apply advanced processing based on tensor magnitudes and trajectory characteristics.
                    string processingLevel = "Standard";
                    if (productVelocity > 0.8 || serviceVelocity > 0.8)
                    {
                        processingLevel = "Premium";
                    }
                    else if (productVelocity > 0.5 || serviceVelocity > 0.5)
                    {
                        processingLevel = "Enhanced";
                    }

                    // Add intelligence based on plane intersections and negative quadrant presence.
                    string processingModifier = "";

                    // Check if both trajectories have substantial negative quadrant presence
                    if (productNegativeBothCount > 3 && serviceNegativeBothCount > 3)
                    {
                        processingModifier = "DeepNegative";
                    }
                    // Check if trajectories cross planes in similar positions
                    else if (productXPlaneIntersection != null && serviceXPlaneIntersection != null &&
                             productYPlaneIntersection != null && serviceYPlaneIntersection != null)
                    {
                        // Calculate similarity of X-plane intersections based on Y and Z coordinates
                        double xPlaneDistanceY = Math.Abs(productXPlaneIntersection[1] - serviceXPlaneIntersection[1]);
                        double xPlaneDistanceZ = Math.Abs(productXPlaneIntersection[2] - serviceXPlaneIntersection[2]);

                        // Calculate similarity of Y-plane intersections based on X and Z coordinates
                        double yPlaneDistanceX = Math.Abs(productYPlaneIntersection[0] - serviceYPlaneIntersection[0]);
                        double yPlaneDistanceZ = Math.Abs(productYPlaneIntersection[2] - serviceYPlaneIntersection[2]);

                        // Average the distances to get overall intersection similarity
                        double avgIntersectionDistance = (xPlaneDistanceY + xPlaneDistanceZ + yPlaneDistanceX + yPlaneDistanceZ) / 4.0;

                        if (avgIntersectionDistance < 0.2)
                        {
                            processingModifier = "Convergent";
                        }
                        else if (avgIntersectionDistance < 0.5)
                        {
                            processingModifier = "Aligned";
                        }
                        else
                        {
                            processingModifier = "Divergent";
                        }
                    }
                    // If only one trajectory has substantial negative presence
                    else if (productNegativeBothCount > 3 || serviceNegativeBothCount > 3)
                    {
                        processingModifier = productNegativeBothCount > serviceNegativeBothCount ?
                            "ProductNegativeDominant" : "ServiceNegativeDominant";
                    }

                    // TODO: Extend processing logic to incorporate additional categories.

                    // Step 3.1: Construct the final processed feature definition string.
                    string result = $"ProcessedFeatures_Cust_{custId}_Level_{processingLevel}";
                    if (!string.IsNullOrEmpty(processingModifier))
                    {
                        result += $"_{processingModifier}";
                    }

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 3 - Processed feature definition created: {result}");
                    return result;
                }

                //==========================================================================
                // Step 4: Feature Quality Assessment
                //==========================================================================
                string Stage4_FeatureQualityAssessment(string processedFeatureResult, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 4 - Assessing feature quality for customer {custId}.");

                    //------------------------------------------
                    // RETRIEVE ASSESSMENT DATA (from Step 2 & 3)
                    //------------------------------------------
                    // Retrieve velocity, trajectory and intersection data for QA assessment
                    double productVelocity = unitResultsStore.TryGetValue("Product_Velocity", out var pv)
                        ? Convert.ToDouble(pv) : 0.5;
                    double serviceVelocity = unitResultsStore.TryGetValue("Service_Velocity", out var sv)
                        ? Convert.ToDouble(sv) : 0.5;

                    // Access trajectory points for verification (from Step 2)
                    var productTrajectoryPoints = unitResultsStore.TryGetValue("Product_TrajectoryPoints", out var ptp)
                        ? ptp as List<double[]> : new List<double[]>();
                    var serviceTrajectoryPoints = unitResultsStore.TryGetValue("Service_TrajectoryPoints", out var stp)
                        ? stp as List<double[]> : new List<double[]>();

                    // Get plane intersections (from Step 2)
                    double[] productXPlaneIntersection = unitResultsStore.TryGetValue("Product_XPlaneIntersection", out var pxi)
                        ? pxi as double[] : null;
                    double[] productYPlaneIntersection = unitResultsStore.TryGetValue("Product_YPlaneIntersection", out var pyi)
                        ? pyi as double[] : null;

                    // TODO: Retrieve assessment data for additional categories if added.

                    //------------------------------------------
                    // QUALITY ASSESSMENT CALCULATION
                    //------------------------------------------
                    double velocityComponent = (productVelocity + serviceVelocity) / 2.0;
                    double trajectoryStability = 0.5; // Default value
                    double intersectionQuality = 0.5; // Default value

                    // Calculate trajectory stability if we have points
                    if (productTrajectoryPoints != null && productTrajectoryPoints.Count > 1)
                    {
                        trajectoryStability = CalculateTrajectoryStability(productTrajectoryPoints);
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] QA product trajectory stability: {trajectoryStability:F4}");
                    }

                    // Calculate intersection quality if we have intersections
                    if (productXPlaneIntersection != null && productYPlaneIntersection != null)
                    {
                        // Higher quality if X and Y intersections have similar Z values
                        double zDifference = Math.Abs(productXPlaneIntersection[2] - productYPlaneIntersection[2]);
                        intersectionQuality = 1.0 - Math.Min(1.0, zDifference); // 1.0 for identical Z, lower for different Z
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] QA intersection quality: {intersectionQuality:F4}");
                    }

                    // Combine factors with weights: 40% velocity, 30% stability, 30% intersection
                    double qaScore = velocityComponent * 0.4 + trajectoryStability * 0.3 + intersectionQuality * 0.3;
                    qaScore = Math.Min(qaScore, 1.0); // Cap at 1.0

                    int qaLevel = (int)(qaScore * 3) + 1; // QA levels 1-4 (adjust based on desired range and granularity)
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] QA final score: {qaScore:F4}, level: {qaLevel}");

                    // Step 4.1: Generate a result string summarizing the QA assessment.
                    string result = $"QualityAssessment_Passed_Level_{qaLevel}_V{velocityComponent:F2}_S{trajectoryStability:F2}_I{intersectionQuality:F2}";

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 4 - Feature quality assessment completed: {result}");
                    return result;
                }


                //==========================================================================
                // Step 5: Combined Feature Evaluation
                //==========================================================================
                float Stage5_CombinedFeatureEvaluation(string qualityAssessmentResult, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 5 - Evaluating combined features for customer {custId}.");

                    //------------------------------------------
                    // RETRIEVE EVALUATION DATA (from Step 2 & 4)
                    //------------------------------------------
                    // Retrieve velocity and negative coordinate counts (from Step 2)
                    double productVelocity = unitResultsStore.TryGetValue("Product_Velocity", out var pv)
                        ? Convert.ToDouble(pv) : 0.5;
                    double serviceVelocity = unitResultsStore.TryGetValue("Service_Velocity", out var sv)
                        ? Convert.ToDouble(sv) : 0.5;

                    int productNegativeBothCount = unitResultsStore.TryGetValue("Product_NegativeBothCount", out var pnbc)
                        ? Convert.ToInt32(pnbc) : 0;
                    int serviceNegativeBothCount = unitResultsStore.TryGetValue("Service_NegativeBothCount", out var snbc)
                        ? Convert.ToInt32(snbc) : 0;

                    // Calculate total negative coordinate coverage
                    int totalNegativePoints = productNegativeBothCount + serviceNegativeBothCount;

                    // Retrieve feature vectors for alignment assessment (from Step 2)
                    double[] productVector = unitResultsStore.TryGetValue("Product_Vector", out var pvec)
                        ? pvec as double[] : new double[] { 0, 0, 0 };
                    double[] serviceVector = unitResultsStore.TryGetValue("Service_Vector", out var svec)
                        ? svec as double[] : new double[] { 0, 0, 0 };


                    // TODO: Retrieve evaluation data for additional categories if added.

                    //------------------------------------------
                    // ALIGNMENT ASSESSMENT
                    //------------------------------------------
                    // Calculate alignment score between product and service vectors using cosine similarity.
                    double alignmentScore = 0.5; // Default if magnitudes are zero
                    double productMagSq = productVector != null ? productVector[0] * productVector[0] + productVector[1] * productVector[1] + productVector[2] * productVector[2] : 0; // Handle null
                    double serviceMagSq = serviceVector != null ? serviceVector[0] * serviceVector[0] + serviceVector[1] * serviceVector[1] + serviceVector[2] * serviceVector[2] : 0; // Handle null

                    double productMag = Math.Sqrt(productMagSq);
                    double serviceMag = Math.Sqrt(serviceMagSq);


                    if (productMag > 1e-9 && serviceMag > 1e-9) // Use tolerance for non-zero magnitude
                    {
                        double dotProduct = 0;
                        if (productVector != null && serviceVector != null) // Add null checks
                        {
                            for (int i = 0; i < Math.Min(productVector.Length, serviceVector.Length); i++) // Use min length
                            {
                                dotProduct += productVector[i] * serviceVector[i];
                            }
                        }
                        alignmentScore = dotProduct / (productMag * serviceMag);
                        // Clamp alignment score to [-1, 1]
                        alignmentScore = Math.Max(-1.0, Math.Min(1.0, alignmentScore));
                        // Normalize alignment score to [0, 1] (closer to 1 means better alignment)
                        alignmentScore = (alignmentScore + 1.0) / 2.0;
                    }


                    //------------------------------------------
                    // FINAL EVALUATION SCORE CALCULATION
                    //------------------------------------------
                    // Calculate final evaluation score based on multiple factors from previous steps.
                    float baseScore = 0.75f + (custId % 10) / 10.0f; // Base score influenced by customer ID
                    float velocityBonus = (float)((productVelocity + serviceVelocity) / 4); // Bonus based on feature velocity magnitude (Max 0.5)
                    float alignmentBonus = (float)(alignmentScore / 5); // Bonus based on product-service alignment (Max 0.2)
                    float negativeBonus = (float)(Math.Min(totalNegativePoints, 10) / 33.33); // Bonus for significant negative trajectory presence (Max ~0.3 for 10+ points)

                    float result = Math.Min(baseScore + velocityBonus + alignmentBonus + negativeBonus, 1.0f); // Cap the final score at 1.0

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 5 - Combined feature evaluation calculation.");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Base Score: {baseScore:F4}");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Velocity Bonus: {velocityBonus:F4} (Product: {productVelocity:F4}, Service: {serviceVelocity:F4})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Alignment Bonus: {alignmentBonus:F4} (Alignment Score: {alignmentScore:F4})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Negative Trajectory Bonus: {negativeBonus:F4} (Total Negative Points: {totalNegativePoints})");
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Final Score: {result:F4}");

                    return result; // Return the final evaluation score
                }




                //==========================================================================
                // Step 6: Fractal Optimization Analysis
                //==========================================================================


                string Stage6_FractalOptimizationAnalysis(string evaluationResult, float evaluationScore, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 6 - Performing fractal optimization analysis for customer {custId}.");

                    //------------------------------------------
                    // RETRIEVE FEATURE DATA (from Step 2)
                    //------------------------------------------
                    // Retrieve velocity variables for products and services (from Step 2)
                    double productVelocity = unitResultsStore.TryGetValue("Product_Velocity", out var pv)
                        ? Convert.ToDouble(pv) : 0.5;
                    double serviceVelocity = unitResultsStore.TryGetValue("Service_Velocity", out var sv)
                        ? Convert.ToDouble(sv) : 0.5;

                    // Retrieve plane intersection points for the X=0 and Y=0 planes (from Step 2)
                    double[] productXPlaneIntersection = unitResultsStore.TryGetValue("Product_XPlaneIntersection", out var pxi)
                        ? pxi as double[] : null;
                    double[] productYPlaneIntersection = unitResultsStore.TryGetValue("Product_YPlaneIntersection", out var pyi)
                        ? pyi as double[] : null;
                    double[] serviceXPlaneIntersection = unitResultsStore.TryGetValue("Service_XPlaneIntersection", out var sxi)
                        ? sxi as double[] : null;
                    double[] serviceYPlaneIntersection = unitResultsStore.TryGetValue("Service_YPlaneIntersection", out var syi)
                        ? syi as double[] : null;

                    // TODO: Retrieve data for additional categories if added.

                    //------------------------------------------
                    // LOG PRODUCT AND SERVICE PLANE INTERSECTIONS
                    //------------------------------------------
                    System.Diagnostics.Debug.WriteLine("========== PRODUCT INTERSECTIONS ==========");
                    if (productXPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Product X-Plane Intersection: (0.0, {productXPlaneIntersection[1]:F6}, {productXPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Product X-Plane Intersection: null");
                    }

                    if (productYPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Product Y-Plane Intersection: ({productYPlaneIntersection[0]:F6}, 0.0, {productYPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Product Y-Plane Intersection: null");
                    }

                    System.Diagnostics.Debug.WriteLine("========== SERVICE INTERSECTIONS ==========");
                    if (serviceXPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Service X-Plane Intersection: (0.0, {serviceXPlaneIntersection[1]:F6}, {serviceXPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Service X-Plane Intersection: null");
                    }

                    if (serviceYPlaneIntersection != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Service Y-Plane Intersection: ({serviceYPlaneIntersection[0]:F6}, 0.0, {serviceYPlaneIntersection[2]:F6})");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Service Y-Plane Intersection: null");
                    }

                    //------------------------------------------
                    // MANDELBULB ALGORITHM CONSTANTS
                    //------------------------------------------
                    const int Power = 8;               // Power parameter for the fractal
                    const float EscapeThreshold = 2.0f; // Escape threshold
                    const int MaxIterations = 30;      // Maximum iterations for fractal generation

                    //------------------------------------------
                    // CALCULATE VELOCITY AT INTERSECTIONS
                    //------------------------------------------
                    // Calculate velocity at each plane intersection
                    float productXPlaneVelocity = (float)(productXPlaneIntersection != null ? productVelocity : 0.0);
                    float productYPlaneVelocity = (float)(productYPlaneIntersection != null ? productVelocity : 0.0);
                    float serviceXPlaneVelocity = (float)(serviceXPlaneIntersection != null ? serviceVelocity : 0.0);
                    float serviceYPlaneVelocity = (float)(serviceYPlaneIntersection != null ? serviceVelocity : 0.0);

                    System.Diagnostics.Debug.WriteLine("========== INTERSECTION VELOCITIES ==========");
                    System.Diagnostics.Debug.WriteLine($"Product X-Plane Velocity: {productXPlaneVelocity:F4}");
                    System.Diagnostics.Debug.WriteLine($"Product Y-Plane Velocity: {productYPlaneVelocity:F4}");
                    System.Diagnostics.Debug.WriteLine($"Service X-Plane Velocity: {serviceXPlaneVelocity:F4}");
                    System.Diagnostics.Debug.WriteLine($"Service Y-Plane Velocity: {serviceYPlaneVelocity:F4}");

                    //------------------------------------------
                    // FRACTAL GENERATION AND VELOCITY DIFFUSION
                    //------------------------------------------
                    // Define velocity source points (the plane intersections) in 3D space
                    List<(Vector3 position, float velocity, string source)> velocitySources = new List<(Vector3, float, string)>();

                    if (productXPlaneIntersection != null && productXPlaneIntersection.Length >= 3) // Added length check
                    {
                        velocitySources.Add((
                            new Vector3(0.0f, (float)productXPlaneIntersection[1], (float)productXPlaneIntersection[2]),
                            productXPlaneVelocity,
                            "ProductX"));
                    }

                    if (productYPlaneIntersection != null && productYPlaneIntersection.Length >= 3) // Added length check
                    {
                        velocitySources.Add((
                            new Vector3((float)productYPlaneIntersection[0], 0.0f, (float)productYPlaneIntersection[2]),
                            productYPlaneVelocity,
                            "ProductY"));
                    }

                    if (serviceXPlaneIntersection != null && serviceXPlaneIntersection.Length >= 3) // Added length check
                    {
                        velocitySources.Add((
                            new Vector3(0.0f, (float)serviceXPlaneIntersection[1], (float)serviceXPlaneIntersection[2]),
                            serviceXPlaneVelocity,
                            "ServiceX"));
                    }

                    if (serviceYPlaneIntersection != null && serviceYPlaneIntersection.Length >= 3) // Added length check
                    {
                        velocitySources.Add((
                            new Vector3((float)serviceYPlaneIntersection[0], 0.0f, (float)serviceYPlaneIntersection[2]),
                            serviceYPlaneVelocity,
                            "ServiceY"));
                    }

                    System.Diagnostics.Debug.WriteLine("========== VELOCITY SOURCES ==========");
                    foreach (var source in velocitySources)
                    {
                        System.Diagnostics.Debug.WriteLine($"{source.source} Source Position: ({source.position.X:F4}, {source.position.Y:F4}, {source.position.Z:F4}), Velocity: {source.velocity:F4}");
                    }

                    // Generate 5 sample points within the fractal space
                    Vector3[] samplePoints = new Vector3[5];

                    // Sample 1: Near the Product X-Plane intersection if available, otherwise default
                    samplePoints[0] = (productXPlaneIntersection != null && productXPlaneIntersection.Length >= 3) ? // Added length check
                        new Vector3(0.1f, (float)productXPlaneIntersection[1], (float)productXPlaneIntersection[2]) :
                        new Vector3(0.1f, 0.1f, 0.1f);

                    // Sample 2: Near the Product Y-Plane intersection if available, otherwise default
                    samplePoints[1] = (productYPlaneIntersection != null && productYPlaneIntersection.Length >= 3) ? // Added length check
                        new Vector3((float)productYPlaneIntersection[0], 0.1f, (float)productYPlaneIntersection[2]) :
                        new Vector3(0.5f, 0.0f, 0.0f);

                    // Sample 3: Near the Service X-Plane intersection if available, otherwise default
                    samplePoints[2] = (serviceXPlaneIntersection != null && serviceXPlaneIntersection.Length >= 3) ? // Added length check
                        new Vector3(0.1f, (float)serviceXPlaneIntersection[1], (float)serviceXPlaneIntersection[2]) :
                        new Vector3(0.0f, 0.8f, 0.0f);

                    // Sample 4: Near the Service Y-Plane intersection if available, otherwise default
                    samplePoints[3] = (serviceYPlaneIntersection != null && serviceYPlaneIntersection.Length >= 3) ? // Added length check
                        new Vector3((float)serviceYPlaneIntersection[0], 0.1f, (float)serviceYPlaneIntersection[2]) :
                        new Vector3(0.3f, 0.3f, 0.3f);

                    // Sample 5: Average of all available intersections, or default
                    if (velocitySources.Count > 0)
                    {
                        Vector3 sum = Vector3.Zero;
                        foreach (var source in velocitySources)
                        {
                            sum += source.position;
                        }
                        samplePoints[4] = sum / velocitySources.Count;
                    }
                    else
                    {
                        samplePoints[4] = new Vector3(1.0f, 1.0f, 1.0f);
                    }

                    System.Diagnostics.Debug.WriteLine("========== SAMPLE POINTS ==========");
                    for (int i = 0; i < 5; i++)
                    {
                        System.Diagnostics.Debug.WriteLine($"Sample {i + 1} Coordinates: ({samplePoints[i].X:F4}, {samplePoints[i].Y:F4}, {samplePoints[i].Z:F4})");
                    }

                    // Arrays to store results for each sample
                    Vector3[] sampleValues = new Vector3[5];
                    int[] sampleIterations = new int[5];
                    float[] sampleVelocities = new float[5];
                    Dictionary<int, Dictionary<string, float>> sampleContributions = new Dictionary<int, Dictionary<string, float>>();

                    // Initialize sample contributions tracking
                    for (int i = 0; i < 5; i++)
                    {
                        sampleContributions[i] = new Dictionary<string, float>();
                        foreach (var source in velocitySources)
                        {
                            sampleContributions[i][source.source] = 0.0f;
                        }
                    }

                    // Generate the fractal and track velocity diffusion at each sample point
                    for (int sampleIndex = 0; sampleIndex < 5; sampleIndex++)
                    {
                        // Initialize the fractal computation for this sample point
                        Vector3 c = samplePoints[sampleIndex];
                        Vector3 z = Vector3.Zero;
                        int iterations = 0;
                        float diffusedVelocity = 0.0f; // Will accumulate diffused velocity

                        System.Diagnostics.Debug.WriteLine($"========== PROCESSING SAMPLE {sampleIndex + 1} ==========");
                        System.Diagnostics.Debug.WriteLine($"Starting point: ({c.X:F4}, {c.Y:F4}, {c.Z:F4})");

                        // Iterate the fractal formula for this sample point
                        for (iterations = 0; iterations < MaxIterations; iterations++)
                        {
                            float rSq = z.LengthSquared(); // Use squared length for efficiency

                            // If we've escaped, we're done with this sample
                            if (rSq > EscapeThreshold * EscapeThreshold) // Compare squared length
                            {
                                System.Diagnostics.Debug.WriteLine($"Escaped at iteration {iterations + 1}");
                                break;
                            }
                            float r = MathF.Sqrt(rSq);

                            System.Diagnostics.Debug.WriteLine($"Iteration {iterations + 1}, z=({z.X:F6}, {z.Y:F6}, {z.Z:F6}), r={r:F6}");

                            // Calculate the diffused velocity from each velocity source
                            // The diffusion decreases with distance and iterations
                            foreach (var source in velocitySources)
                            {
                                // Calculate distance from current z position to the velocity source
                                float distanceSq = Vector3.DistanceSquared(z, source.position); // Use squared distance
                                float distance = MathF.Sqrt(distanceSq);


                                // Apply velocity diffusion along fractal edges
                                // Velocity decreases with distance and iterations
                                if (distance < 2.0f) // Only sources within a reasonable distance contribute
                                {
                                    float contribution = source.velocity *
                                                         MathF.Exp(-distance * 2.0f) * // Exponential falloff with distance
                                                         MathF.Exp(-iterations * 0.1f); // Exponential falloff with iterations

                                    diffusedVelocity += contribution;
                                    if (sampleContributions[sampleIndex].ContainsKey(source.source)) // Check if source exists
                                    {
                                        sampleContributions[sampleIndex][source.source] += contribution;
                                    }
                                    else
                                    {
                                        sampleContributions[sampleIndex][source.source] = contribution;
                                    }


                                    System.Diagnostics.Debug.WriteLine($"  Contribution from {source.source}: {contribution:F6} (distance: {distance:F4})");
                                }
                            }

                            // Standard Mandelbulb formula calculation
                            float theta = (r < 1e-6f) ? 0 : MathF.Acos(z.Z / r);
                            float phi = MathF.Atan2(z.Y, z.X);

                            float newR = MathF.Pow(r, Power);
                            float newTheta = Power * theta;
                            float newPhi = Power * phi;

                            // Calculate the next z value
                            z = new Vector3(
                                newR * MathF.Sin(newTheta) * MathF.Cos(newPhi),
                                newR * MathF.Sin(newTheta) * MathF.Sin(newPhi),
                                newR * MathF.Cos(newTheta)) + c;
                        }

                        // Store the results for this sample
                        sampleValues[sampleIndex] = z;
                        sampleIterations[sampleIndex] = iterations;
                        sampleVelocities[sampleIndex] = diffusedVelocity;

                        System.Diagnostics.Debug.WriteLine($"Final Sample {sampleIndex + 1} Results:");
                        System.Diagnostics.Debug.WriteLine($"  Final z value: ({z.X:F6}, {z.Y:F6}, {z.Z:F6})");
                        System.Diagnostics.Debug.WriteLine($"  Iterations: {iterations}");
                        System.Diagnostics.Debug.WriteLine($"  Total diffused velocity: {diffusedVelocity:F6}");
                        System.Diagnostics.Debug.WriteLine($"  Contributions breakdown:");
                        foreach (var source in velocitySources)
                        {
                            if (sampleContributions[sampleIndex].ContainsKey(source.source))
                                System.Diagnostics.Debug.WriteLine($"    {source.source}: {sampleContributions[sampleIndex][source.source]:F6}");
                        }
                    }

                    //------------------------------------------
                    // STORE ANALYSIS RESULTS
                    //------------------------------------------
                    // Store intersection velocities
                    unitResultsStore["ProductXPlaneVelocity"] = productXPlaneVelocity;
                    unitResultsStore["ProductYPlaneVelocity"] = productYPlaneVelocity;
                    unitResultsStore["ServiceXPlaneVelocity"] = serviceXPlaneVelocity;
                    unitResultsStore["ServiceYPlaneVelocity"] = serviceYPlaneVelocity;

                    // Store each sample's data
                    for (int i = 0; i < 5; i++)
                    {
                        unitResultsStore[$"Sample{i + 1}Coordinate"] = samplePoints[i];
                        unitResultsStore[$"Sample{i + 1}Value"] = sampleValues[i];
                        unitResultsStore[$"Sample{i + 1}Iterations"] = sampleIterations[i];
                        unitResultsStore[$"Sample{i + 1}Velocity"] = sampleVelocities[i];

                        // Store individual contributions
                        foreach (var source in velocitySources)
                        {
                            unitResultsStore[$"Sample{i + 1}_{source.source}Contribution"] =
                                sampleContributions[i].ContainsKey(source.source) ?
                                sampleContributions[i][source.source] : 0.0f;
                        }
                    }

                    // Step 6.1: Generate a result string summarizing the optimization analysis.
                    System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();
                    resultBuilder.Append($"OptimizationAnalysis_Cust_{custId}");

                    // Add velocity information
                    resultBuilder.Append("_V[");
                    // Only append if velocity is non-zero (using tolerance)
                    if (productXPlaneVelocity > 1e-6f) resultBuilder.Append($"PX:{productXPlaneVelocity:F3},"); else if (productXPlaneIntersection != null) resultBuilder.Append($"PX:0.000,"); // Add 0 if intersection exists but velocity is effectively zero
                    if (productYPlaneVelocity > 1e-6f) resultBuilder.Append($"PY:{productYPlaneVelocity:F3},"); else if (productYPlaneIntersection != null) resultBuilder.Append($"PY:0.000,");
                    if (serviceXPlaneVelocity > 1e-6f) resultBuilder.Append($"SX:{serviceXPlaneVelocity:F3},"); else if (serviceXPlaneIntersection != null) resultBuilder.Append($"SX:0.000,");
                    if (serviceYPlaneVelocity > 1e-6f) resultBuilder.Append($"SY:{serviceYPlaneVelocity:F3}"); else if (serviceYPlaneIntersection != null) resultBuilder.Append($"SY:0.000");
                    // Remove trailing comma if any
                    if (resultBuilder.Length > "_V[".Length && resultBuilder[resultBuilder.Length - 1] == ',') resultBuilder.Length--; // Check length before removing
                    resultBuilder.Append("]");


                    // Add sample information
                    resultBuilder.Append("_S[");
                    for (int i = 0; i < 5; i++)
                    {
                        string status = sampleIterations[i] >= MaxIterations ? "InSet" : $"Escaped({sampleIterations[i]})";
                        resultBuilder.Append($"P{i + 1}:{sampleVelocities[i]:F4}_S{status}"); // Appended 'S' to status, Renamed Sample to P for Point
                        if (i < 4) resultBuilder.Append(",");
                    }
                    resultBuilder.Append("]");

                    string result = resultBuilder.ToString();

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 6 - Fractal optimization analysis completed: {result}");
                    return result;
                }













                //==========================================================================
                // Step 7: Tensor Network Training
                //==========================================================================

                string Stage7_TensorNetworkTraining(string optimizationResult, int custId, Session mlSession, ConcurrentDictionary<string, object> resultsStore)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 7 - Training tensor network for customer {custId} using Actual TF.NET Model A.");

                    // Disable eager execution before defining any TensorFlow operations
                    tf.compat.v1.disable_eager_execution();

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Disabled eager execution for TensorFlow operations.");

                    //------------------------------------------
                    // Retrieve Required Data (from previous steps and context)
                    //------------------------------------------
                    // FIX: Corrected the retrieval keys to match the keys used for storing in SequentialInitialProcessingUnitC
                    byte[] modelWeightsBytes = RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_SerializedModelData") as byte[];
                    byte[] modelBiasBytes = RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_AncillaryData") as byte[];
                    float[] eigenvalues = unitResultsStore.TryGetValue("MarketCurvatureEigenvalues", out var eigVals) ? eigVals as float[] : new float[] { 1.0f, 1.0f, 1.0f };

                    int numEpochs = 100;
                    List<float> trainingLosses = new List<float>();
                    List<float> trainingErrors = new List<float>();

                    //------------------------------------------
                    // Sample Training Data Creation
                    //------------------------------------------
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 7 - Creating sample training data.");

                    // Define numerical sample data
                    float[][] numericalSamples = new float[][] {
                        new float[] { 0.3f, 0.7f, 0.1f, 0.85f },
                        new float[] { 0.5f, 0.2f, 0.9f, 0.35f },
                        new float[] { 0.8f, 0.6f, 0.4f, 0.55f },
                        new float[] { 0.1f, 0.8f, 0.6f, 0.25f },
                        new float[] { 0.7f, 0.3f, 0.2f, 0.95f },
                        new float[] { 0.4f, 0.5f, 0.7f, 0.65f },
                        new float[] { 0.2f, 0.9f, 0.3f, 0.15f },
                        new float[] { 0.6f, 0.1f, 0.8f, 0.75f },
                        new float[] { 0.35f, 0.65f, 0.15f, 0.80f },
                        new float[] { 0.55f, 0.25f, 0.85f, 0.30f },
                        new float[] { 0.75f, 0.55f, 0.45f, 0.60f },
                        new float[] { 0.15f, 0.75f, 0.55f, 0.20f },
                        new float[] { 0.65f, 0.35f, 0.25f, 0.90f },
                        new float[] { 0.45f, 0.45f, 0.65f, 0.70f },
                        new float[] { 0.25f, 0.85f, 0.35f, 0.10f },
                        new float[] { 0.50f, 0.15f, 0.75f, 0.80f }
                    };

                    // Define expected labels
                    float[] numericalLabels = new float[numericalSamples.Length];
                    for (int i = 0; i < numericalSamples.Length; i++)
                    {
                        if (numericalSamples[i] == null || numericalSamples[i].Length < 4)
                        {
                            numericalLabels[i] = 0.0f;
                            continue;
                        }
                        float x = numericalSamples[i][0];
                        float y = numericalSamples[i][1];
                        float z = numericalSamples[i][2];
                        float p = numericalSamples[i][3];

                        // Use a more complex formula that includes non-linear terms
                        numericalLabels[i] = x * (float)Math.Cos(p) +
                                            y * (float)Math.Sin(p) +
                                            z * (float)Math.Cos(p / 2f) +
                                            x * y * z * 0.1f; // Add non-linear interaction term
                    }

                    string[] wordSamples = new string[] {
                        "market growth potential high", "customer satisfaction excellent", "product quality superior",
                        "service delivery timely", "price competitiveness average", "brand recognition strong",
                        "operational efficiency optimal", "supply chain resilient", "market segment expanding",
                        "customer retention excellent", "product innovation substantial", "service response immediate",
                        "price positioning competitive", "brand loyalty increasing", "operational costs decreasing",
                        "supply reliability consistent"
                    };

                    float[][] wordEmbeddings = TransformWordsToEmbeddings(wordSamples);
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Created {numericalSamples.Length} numerical samples and {wordSamples.Length} word-based samples.");

                    // Prepare data arrays for TensorFlow
                    float[,] numericalData = ConvertJaggedToMultidimensional(numericalSamples);
                    float[,] wordData = ConvertJaggedToMultidimensional(wordEmbeddings);
                    float[,] targetValues = new float[numericalLabels.Length, 1];
                    for (int i = 0; i < numericalLabels.Length; i++)
                    {
                        targetValues[i, 0] = numericalLabels[i];
                    }

                    // Training configuration
                    int batchSize = 4;
                    int dataSize = numericalData.GetLength(0);
                    int actualBatchSize = Math.Min(batchSize, dataSize);

                    if (actualBatchSize <= 0 && dataSize > 0)
                    {
                        actualBatchSize = dataSize;
                    }
                    else if (dataSize == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Warning: No training data available. Skipping training.");
                        resultsStore["ModelAProcessingWarning"] = "No training data available.";
                        resultsStore["ModelAProcessingOutcome"] = 0.1f;
                        return $"TensorNetworkTrainingSkipped_Cust_{custId}_NoData";
                    }

                    if (actualBatchSize <= 0)
                    {
                        actualBatchSize = 1;
                    }

                    int numBatches = (actualBatchSize > 0) ? (int)Math.Ceiling((double)dataSize / actualBatchSize) : 0;
                    int[] indices = Enumerable.Range(0, dataSize).ToArray();

                    // Expression strings
                    string initialExpression = "1+1";
                    string regexPattern = ConvertExpressionToRegex(initialExpression);
                    string nDimensionalExpression = ConvertRegexToNDimensionalExpression(regexPattern);

                    if (modelWeightsBytes != null && modelBiasBytes != null && modelWeightsBytes.Length > 0 && modelBiasBytes.Length > 0)
                    {
                        // Get the context manager object
                        var graphContext = mlSession.graph.as_default();
                        try // Start the try block for graph context management
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 7 - Initializing Model A Architecture.");

                            // Deserialize initial model parameters
                            float[] unitCWeightsArray = DeserializeFloatArray(modelWeightsBytes);
                            float[] unitCBiasArray = DeserializeFloatArray(modelBiasBytes);

                            // Infer input feature counts
                            int numericalFeatureCount = numericalSamples[0].Length;
                            int wordFeatureCount = wordEmbeddings[0].Length;
                            int totalInputFeatureCount = numericalFeatureCount + wordFeatureCount;

                            // Set layer size
                            int unitCLayerSize = unitCWeightsArray.Length > 0 ? unitCWeightsArray.Length : 10;
                            if (unitCLayerSize <= 0) unitCLayerSize = 10; // Ensure size is at least 10 if 0 or negative


                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A architecture parameters:");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}]  - Total Input Feature Count: {totalInputFeatureCount}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}]  - Hidden Layer Size (derived from Unit C or default): {unitCLayerSize}");

                            // Generate weights with attention to outermost vertices
                            float[,] modelAWeights1Data = GenerateWeightsFromExpression(nDimensionalExpression, totalInputFeatureCount, unitCLayerSize);
                            float[,] modelAWeights2Data = GenerateWeightsFromExpression(nDimensionalExpression, unitCLayerSize, 1);

                            // Now define your TensorFlow operations within this try block
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Using session graph context for defining operations.");

                            // Define placeholders - fixed shape format
                            var numericalInput = tf.placeholder(tf.float32, shape: new int[] { -1, numericalFeatureCount }, name: "numerical_input_A");
                            var wordInput = tf.placeholder(tf.float32, shape: new int[] { -1, wordFeatureCount }, name: "word_input_A");
                            var targetOutput = tf.placeholder(tf.float32, shape: new int[] { -1, 1 }, name: "target_output_A");

                            // Concatenate inputs
                            var combinedInput = tf.concat(new[] { numericalInput, wordInput }, axis: 1, name: "combined_input_A");

                            // Create weights and biases variables using constant initializers
                            var weights1 = tf.Variable(tf.constant(modelAWeights1Data, dtype: tf.float32), name: "weights1_A");
                            var bias1 = tf.Variable(tf.zeros(unitCLayerSize, dtype: tf.float32), name: "bias1_A");
                            var weights2 = tf.Variable(tf.constant(modelAWeights2Data, dtype: tf.float32), name: "weights2_A");
                            var bias2 = tf.Variable(tf.zeros(1, dtype: tf.float32), name: "bias2_A");


                            // Simple feedforward network structure
                            var hidden = tf.nn.relu(tf.add(tf.matmul(combinedInput, weights1), bias1), name: "hidden_A");
                            var predictions = tf.add(tf.matmul(hidden, weights2), bias2, name: "predictions_A");

                            // Loss and optimizer
                            var loss = tf.reduce_mean(tf.square(tf.subtract(predictions, targetOutput)), name: "mse_loss_A");
                            var optimizer = tf.train.AdamOptimizer(0.001f);
                            var trainOp = optimizer.minimize(loss);

                            // Evaluation metric
                            var meanAbsError = tf.reduce_mean(tf.abs(tf.subtract(predictions, targetOutput)), name: "mae_A");

                            // Initialize variables
                            var initOp = tf.global_variables_initializer();
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] TensorFlow operations defined within session graph context.");

                            // Create training data feed dictionary
                            var trainingDataFeed = new FeedItem[] {
                                new FeedItem(numericalInput, numericalData),
                                new FeedItem(wordInput, wordData),
                                new FeedItem(targetOutput, targetValues)
                            };

                            // Run the initialization operation using the session
                            mlSession.run(initOp);
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A - Actual TensorFlow.NET variables initialized.");


                            // Main training loop
                            for (int epoch = 0; epoch < numEpochs; epoch++)
                            {
                                // Shuffle indices for this epoch
                                ShuffleArray(indices);
                                float epochLoss = 0.0f;

                                for (int batch = 0; batch < numBatches; batch++)
                                {
                                    int startIdx = batch * actualBatchSize;
                                    int endIdx = Math.Min(startIdx + actualBatchSize, dataSize);
                                    int batchCount = endIdx - startIdx;

                                    if (batchCount <= 0) continue;

                                    float[,] batchNumerical = ExtractBatch(numericalData, indices, startIdx, batchCount);
                                    float[,] batchWord = ExtractBatch(wordData, indices, startIdx, batchCount);
                                    float[,] batchTarget = ExtractBatch(targetValues, indices, startIdx, batchCount);

                                    // Feed dictionary for this batch
                                    var batchFeed = new FeedItem[] {
                                        new FeedItem(numericalInput, batchNumerical),
                                        new FeedItem(wordInput, batchWord),
                                        new FeedItem(targetOutput, batchTarget)
                                    };

                                    // Run training operation with loss using the session
                                    var results = mlSession.run(new ITensorOrOperation[] { loss, trainOp }, batchFeed);
                                    // Extract tensor value properly
                                    float batchLoss = (float)((Tensor)results[0]).numpy()[0];
                                    epochLoss += batchLoss;

                                    if (batch % 5 == 0 || batch == numBatches - 1)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Epoch {epoch + 1}/{numEpochs}, Batch {batch + 1}/{numBatches}, Batch Loss: {batchLoss:F6}");
                                    }
                                }

                                if (numBatches > 0)
                                {
                                    epochLoss /= numBatches;
                                }
                                else
                                {
                                    epochLoss = float.NaN;
                                }
                                trainingLosses.Add(epochLoss);

                                // Evaluation on interval
                                if (epoch % 10 == 0 || epoch == numEpochs - 1)
                                {
                                    // Run evaluation using the session
                                    var evalResults = mlSession.run(new ITensorOrOperation[] { meanAbsError }, trainingDataFeed);
                                    // Extract tensor value properly
                                    float currentError = (float)((Tensor)evalResults[0]).numpy()[0];
                                    trainingErrors.Add(currentError);

                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Epoch {epoch + 1}/{numEpochs}, Average Loss: {(float.IsNaN(epochLoss) ? "N/A" : epochLoss.ToString("F6"))}, Mean Absolute Error: {currentError:F6}");
                                }
                            }

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A training completed");

                            // Final evaluation and predictions using the session
                            var finalResults = mlSession.run(new ITensorOrOperation[] { meanAbsError, predictions }, trainingDataFeed);
                            // Extract tensor value properly
                            float finalError = (float)((Tensor)finalResults[0]).numpy()[0];
                            Tensor finalPredictionsTensor = (Tensor)finalResults[1];

                            // Extract prediction data
                            float[] finalPredictionsFlat = finalPredictionsTensor.ToArray<float>();
                            int[] finalPredictionsDims = finalPredictionsTensor.shape.dims.Select(d => (int)d).ToArray();

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A Final Predictions Shape: {string.Join(",", finalPredictionsDims)}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A Final Predictions (First few): [{string.Join(", ", finalPredictionsFlat.Take(Math.Min(finalPredictionsFlat.Length, 10)))}...]");

                            // Extract weights and biases using the session
                            var finalParams = mlSession.run(new ITensorOrOperation[] {
                                (ITensorOrOperation)weights1,
                                (ITensorOrOperation)bias1,
                                (ITensorOrOperation)weights2,
                                (ITensorOrOperation)bias2
                            });

                            // Convert all results to float arrays for storage
                            var finalWeights1 = ((Tensor)finalParams[0]).ToArray<float>();
                            var finalBias1 = ((Tensor)finalParams[1]).ToArray<float>();
                            var finalWeights2 = ((Tensor)finalParams[2]).ToArray<float>();
                            var finalBias2 = ((Tensor)finalParams[3]).ToArray<float>();

                            // Store results
                            resultsStore["ModelAPredictionsFlat"] = finalPredictionsFlat;
                            resultsStore["ModelAPredictionsShape"] = finalPredictionsDims;
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Model A Final Mean Absolute Error: {finalError:F6}");

                            // Serialize model parameters
                            byte[] trainedWeights1Bytes = SerializeFloatArray(finalWeights1);
                            byte[] trainedBias1Bytes = SerializeFloatArray(finalBias1);
                            byte[] trainedWeights2Bytes = SerializeFloatArray(finalWeights2);
                            byte[] trainedBias2Bytes = SerializeFloatArray(finalBias2);

                            // Combine all parameters
                            var byteArraysToCombine = new List<byte[]>();
                            if (trainedWeights1Bytes != null) byteArraysToCombine.Add(trainedWeights1Bytes);
                            if (trainedBias1Bytes != null) byteArraysToCombine.Add(trainedBias1Bytes);
                            if (trainedWeights2Bytes != null) byteArraysToCombine.Add(trainedWeights2Bytes);
                            if (trainedBias2Bytes != null) byteArraysToCombine.Add(trainedBias2Bytes);

                            byte[] combinedModelAData = byteArraysToCombine.SelectMany(arr => arr).ToArray();

                            // Store key training results
                            resultsStore["ModelAProcessingOutcome"] = 1.0f - finalError; // Map MAE to a score (0-1)
                            resultsStore["ModelATrainingError"] = finalError;
                            resultsStore["ModelATrainingLosses"] = trainingLosses.ToArray();
                            resultsStore["ModelATrainingErrors"] = trainingErrors.ToArray();
                            resultsStore["ModelACombinedParameters"] = combinedModelAData;

                            // Create and store metadata
                            var modelMetadata = new Dictionary<string, object> {
                                { "EmbeddedExpression", initialExpression },
                                { "NDimensionalExpression", nDimensionalExpression },
                                { "TrainingEpochs", numEpochs },
                                { "FinalMeanAbsoluteError", finalError },
                                { "TotalInputFeatureCount", totalInputFeatureCount },
                                { "HiddenLayerSize", unitCLayerSize },
                                { "TrainingSampleCount", dataSize },
                                { "CreationTimestamp", DateTime.UtcNow.ToString() },
                                { "CurvatureEigenvalues", eigenvalues },
                                { "HasOutermostVertexFocus", true },
                                { "UsesNDimensionalIterations", true }
                            };

                            string metadataJson = SerializeMetadata(modelMetadata);
                            byte[] metadataBytes = System.Text.Encoding.UTF8.GetBytes(metadataJson);
                            resultsStore["ModelAMetadata"] = metadataBytes;

                            // Save to runtime context
                            RuntimeProcessingContext.StoreContextValue("model_a_params_combined", combinedModelAData);
                            RuntimeProcessingContext.StoreContextValue("model_a_metadata", metadataBytes);
                            RuntimeProcessingContext.StoreContextValue("model_a_expression", initialExpression);
                            RuntimeProcessingContext.StoreContextValue("model_a_expression_nd", nDimensionalExpression);

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 7 - Model A trained and saved to RuntimeProcessingContext and Results Store.");

                            string result = $"TensorNetworkTrained_Cust_{custId}_MAE{finalError:F4}_Expr({initialExpression.Replace('+', 'p')})";
                            return result;
                        } // End of try block for graph context
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Error during Step 7 - Tensor Network Training: {ex.Message}");
                            resultsStore["ModelAProcessingError"] = "Model A Training Error: " + ex.Message;
                            resultsStore["ModelAProcessingOutcome"] = 0.0f;

                            string errorMessageForReturn = ex.Message;
                            if (errorMessageForReturn.Length > 50) errorMessageForReturn = errorMessageForReturn.Substring(0, 50) + "...";
                            return $"TensorNetworkTrainingError_Cust_{custId}_{errorMessageForReturn.Replace(" ", "_").Replace(":", "_")}";
                        }
                        finally // Add the finally block to ensure context cleanup
                        {
                            // Explicitly dispose the context manager if it implements IDisposable
                            if (graphContext is IDisposable disposableContext)
                            {
                                disposableContext.Dispose();
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Graph context disposed.");
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Step 7 - Missing initial model parameters from Unit C for Model A training. Skipping training.");
                        resultsStore["ModelAProcessingWarning"] = "Missing initial parameters from Unit C for training.";
                        resultsStore["ModelAProcessingOutcome"] = 0.1f;
                        return $"TensorNetworkTrainingSkipped_Cust_{custId}_MissingData";
                    }
                }

























                //==========================================================================
                // Step 8: Future Performance Projection
                //==========================================================================
                string Stage8_FutureProjection(string trainingOutcomeResult, float evaluationScore, int custId)
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 8 - Generating future performance projection for customer {custId}.");

                    //------------------------------------------
                    // Retrieve Results (from Step 5 & 7)
                    //------------------------------------------
                    // Retrieve the evaluation score from Step 5
                    float combinedFeatureEvaluationScore = unitResultsStore.TryGetValue("CombinedEvaluationScore", out var evalScore)
                        ? Convert.ToSingle(evalScore) : evaluationScore; // Use Step 5 score

                    // Retrieve the Model A training outcome score (mapping of MAE) from Step 7
                    float modelATrainingOutcomeScore = unitResultsStore.TryGetValue("ModelAProcessingOutcome", out var maeScore)
                        ? Convert.ToSingle(maeScore) : 0.0f; // Default to 0 if training outcome isn't stored

                    // Retrieve the actual training error from Step 7
                    float modelATrainingError = unitResultsStore.TryGetValue("ModelATrainingError", out var maeError)
                        ? Convert.ToSingle(maeError) : 1.0f; // Default error is high if not available

                    // Get combined parameters from Model A (from Step 7)
                    byte[] modelACombinedParams = unitResultsStore.TryGetValue("ModelACombinedParameters", out var maParams)
                        ? maParams as byte[] : null;

                    //------------------------------------------
                    // Perform Projection Calculation
                    //------------------------------------------
                    // Simple simulation logic based on previous stage outcomes
                    string projectionOutcome = "Stable";
                    // Base projection on the evaluation score, adjusted by training outcome
                    float projectedScore = (combinedFeatureEvaluationScore + modelATrainingOutcomeScore) / 2.0f; // Average the scores


                    // Adjust projection based on training error and parameter complexity
                    if (modelATrainingError < 0.05f)
                    {
                        projectionOutcome = "StrongGrowth";
                        projectedScore = Math.Min(projectedScore + 0.1f, 1.0f); // Add bonus for low training error
                    }
                    else if (modelATrainingError > 0.2f)
                    {
                        projectionOutcome = "PotentialChallenges";
                        projectedScore = Math.Max(projectedScore - 0.05f, 0.0f); // Deduct for high training error
                    }

                    // Consider parameter size as a proxy for model complexity/stability - might indicate overfitting if error is high
                    if (modelACombinedParams != null && modelACombinedParams.Length > 1000) // Arbitrary threshold for complexity
                    {
                        projectionOutcome += "_ComplexModel";
                        // If error is low, complexity is good; if error is high, complexity is bad (potential overfitting)
                        if (modelATrainingError < 0.1f)
                        {
                            projectedScore = Math.Min(projectedScore + 0.03f, 1.0f); // Small bonus for effective complexity
                        }
                        else if (modelATrainingError > 0.3f)
                        {
                            projectedScore = Math.Max(projectedScore - 0.03f, 0.0f); // Small penalty for ineffective complexity
                        }
                    }

                    // Ensure score is within valid bounds [0, 1]
                    projectedScore = Math.Max(0.0f, Math.Min(1.0f, projectedScore));


                    // Set the final score for workflow orchestrator (used by ExecuteProductionWorkflow)
                    unitResultsStore["ProjectedPerformanceScore"] = projectedScore;

                    // Step 8.1: Generate a result string summarizing the projection.
                    string result = $"PerformanceProjection_Cust_{custId}_Outcome_{projectionOutcome}_Score_{projectedScore:F4}_TrainError_{modelATrainingError:F4}";

                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Step 8 - Future performance projection completed: {result}");
                    return result;
                }


                //==========================================================================
                // Workflow Execution
                //==========================================================================
                // Execute the entire workflow through the orchestrator function.
                var workflowResult = ExecuteProductionWorkflow(outcomeRecord, customerIdentifier, mlSession);
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Workflow completed with result: {workflowResult}");

                // Simulate some non-ML work duration at the end of the unit
                await Task.Delay(150);
            }
            catch (Exception ex)
            {
                //==========================================================================
                // Error Handling
                //==========================================================================
                // Catch any exceptions during the execution of this processing unit.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error in Parallel Processing Unit A: {ex.Message}");
                // Store error state in the results dictionary for downstream units or logging.
                unitResultsStore["ModelAProcessingError"] = ex.Message;
                unitResultsStore["ModelAProcessingOutcome"] = 0.0f; // Indicate failure with a low score

                // Re-throw the exception so the main orchestrator method can catch it and handle the overall workflow failure.
                throw;
            }
            finally
            {
                //==========================================================================
                // Cleanup
                //==========================================================================
                // Log the finish of the process unit.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit A finished.");
            }
        }






        /// <summary>
        /// Processes data simulating Model B (ParallelProcessingUnitB).
        /// This method is designed to run in parallel with ParallelProcessingUnitA (Actual Model A).
        /// It performs another actual TensorFlow.NET operation based on the core outcome record data
        /// (potentially using the model data generated by Unit C) and stores its results in a shared thread-safe dictionary.
        /// </summary>
        /// <param name="outcomeRecord">The core CoreMlOutcomeRecord object established by SequentialInitialProcessingUnitC.</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        /// <param name="mlSession">A dedicated actual TensorFlow.NET Session environment for this parallel task.</param>
        /// <param name="unitResultsStore">A thread-safe dictionary to store results for SequentialFinalProcessingUnitD.</param>
        private async Task ParallelProcessingUnitB(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier, Session mlSession, ConcurrentDictionary<string, object> unitResultsStore)
        {
            // This method simulates the other parallel processing branch (Actual Model B).
            // Operational Process Dependency: Called by InitiateMlOutcomeGeneration (Step 5) as part of Task.WhenAll.
            // Depends on the 'outcomeRecord' object (established/updated by Step 4), the provided actual 'mlSession', and the 'unitResultsStore' dictionary.
            // Subsequent Usage: Its output (stored in 'unitResultsStore') is a dependency for SequentialFinalProcessingUnitD (Step 6). Runs concurrently with ParallelProcessingUnitA.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Parallel Processing Unit B (Actual Model B) for customer {customerIdentifier}.");

            // Use the provided session's graph implicitly.
            // Removed `using (mlSession.graph.as_default())` to fix CS1674, assuming ops implicitly attach or run using the session's graph.
            // If ops need explicit graph association, they might need `tf.Graph().as_default()` managed manually or via different TF.NET patterns.
            // For this example, let's assume ops within the method will correctly use the session's graph context without explicit `as_default()` in a `using`.

            try
            {
                /// <summary>
                /// Operational Step 5.3 (Actual Parallel Processing - Model B)
                /// </summary>
                // Placeholder logic for Actual Model B processing using TensorFlow.NET.
                // This example uses the model parameters saved by Unit C to perform a simple aggregation.
                // Operational Process Dependency: Depends on input parameters (outcomeRecord, mlSession). Relies on Unit C having saved model data to the record.
                // Subsequent Usage: The results computed here are stored in 'unitResultsStore' for SequentialFinalProcessingUnitD.

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: Attempting to perform actual TF.NET operation.");

                // Retrieve serialized model data from the outcome record (generated by Unit C)
                var weightsBytes = outcomeRecord.SerializedSimulatedModelData; // Weights saved as "ModelData"
                var biasBytes = outcomeRecord.AncillaryBinaryDataPayload;      // Bias saved as "AncillaryData"

                Tensor? aggregationResult = null;

                if (weightsBytes != null && biasBytes != null)
                {
                    try
                    {
                        var weightsArray = DeserializeFloatArray(weightsBytes);
                        var biasArray = DeserializeFloatArray(biasBytes);

                        // Infer shapes based on the deserialized data
                        int featureCount = weightsArray.Length; // Assuming weights are [feature_count] or [feature_count, 1]

                        if (featureCount == 0 || biasArray.Length == 0 || biasArray.Length != 1)
                        {
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: Deserialized model data has unexpected dimensions. Skipping aggregation.");
                        }
                        else
                        {
                            // Use the provided session's graph explicitly for defining ops within this scope
                            using (IDisposable graphScope = (IDisposable)mlSession.graph.as_default()) // Explicitly cast to IDisposable
                            {
                                // Create constants from the deserialized parameters. These ops should attach to mlSession.graph
                                // Assume weights were saved as [feature_count, 1] from Unit C
                                var weightsConstant = tf.constant(weightsArray.reshape(featureCount, 1), dtype: tf.float32, name: "loaded_weights_B");
                                var biasConstant = tf.constant(biasArray.reshape(1), dtype: tf.float32, name: "loaded_bias_B");


                                // Define a simple aggregation operation using the loaded parameters (e.g., sum of weights * bias). This op should attach to mlSession.graph
                                var unitB_op = tf.reduce_sum(tf.multiply(weightsConstant, biasConstant), name: "unitB_aggregation");

                                // Run the operation in the dedicated session
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: Running actual TF.NET operation with loaded parameters.");
                                // Correct Session.run arguments. fetches = unitB_op (ITensorOrOperation[])
                                var results = mlSession.run(fetches: new ITensorOrOperation[] { unitB_op }); // No feed items needed for this simple op
                                aggregationResult = results[0] as Tensor;

                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: Actual TF.NET operation completed.");
                            }
                        }
                    }
                    catch (Exception loadOpEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: Error loading or running TF.NET operation: {loadOpEx.Message}.");
                        unitResultsStore["ModelBProcessingWarning"] = "Error during TF.NET operation in Unit B: " + loadOpEx.Message;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: ParallelProcessingUnitB: No model data found in outcome record. Skipping actual TF.NET operation.");
                    unitResultsStore["ModelBProcessingWarning"] = "No model data found in outcome record for Unit B.";
                }


                // Store the result (actual tensor value or placeholder) in the shared results dictionary.
                // Operational Process Dependency: Writes to the 'unitResultsStore' dictionary.
                // Subsequent Usage: SequentialFinalProcessingUnitD reads from this dictionary.
                if (aggregationResult != null)
                {
                    // Convert tensor result to something serializable/storable, e.g., a scalar float
                    // Use ToArray<float>()[0] or similar for scalar
                    var resultScalar = aggregationResult.ToArray<float>().FirstOrDefault(); // Get the first (and only) element
                    unitResultsStore["ModelBProcessingOutcome"] = resultScalar; // Store the scalar result
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit B stored actual TF.NET result: {resultScalar}.");
                }
                else
                {
                    unitResultsStore["ModelBProcessingOutcome"] = "Skipped TF.NET op"; // Indicate no actual op ran
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit B stored placeholder result (TF.NET op skipped).");
                }

                await Task.Delay(200); // Simulate some non-ML work duration

            }
            /// <summary>
            /// Operational Step 5.4 (Error Handling - Model B)
            /// </summary>
            // Catch any exceptions during Actual Model B processing.
            // Operational Process Dependency: Catches errors specific to this unit's actual TF.NET logic or data handling.
            // Subsequent Usage: Logs the error and stores an error indicator/message in the results dictionary. Re-throws so Task.WhenAll can detect the failure.
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error in Parallel Processing Unit B: {ex.Message}");
                unitResultsStore["ModelBProcessingError"] = ex.Message; // Store error state in results
                throw; // Re-throw to be caught by Task.WhenAll in the orchestrator
            }
            finally
            {
                // Log the finish of the process unit.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit B (Actual Model B) finished.");
            }
            // mlSession.graph is managed by the Session object, which is disposed in the controller's finally block.
        }

        /// <summary>
        /// Processes data simulating Model D (SequentialFinalProcessingUnitD).
        /// This is the *final sequential* processing step in the workflow.
        /// It runs after ParallelProcessingUnitA and ParallelProcessingUnitB have completed.
        /// It is intended to combine or use the results from the parallel tasks (available in the provided dictionaries)
        /// and perform final updates to the core CoreMlOutcomeRecord.
        /// </summary>
        /// <param name="outcomeRecord">The core CoreMlOutcomeRecord object (potentially updated by SequentialInitialProcessingUnitC).</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        /// <param name="unitAResults">Thread-safe dictionary containing results from ParallelProcessingUnitA.</param>
        /// <param name="unitBResults">Thread-safe dictionary containing results from ParallelProcessingUnitB.</param>
        private async Task SequentialFinalProcessingUnitD(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier, ConcurrentDictionary<string, object> unitAResults, ConcurrentDictionary<string, object> unitBResults)
        {
            // This method handles the final processing steps (Actual Model D conceptually).
            // Operational Process Dependency: Called by InitiateMlOutcomeGeneration (Step 6) after Task.WhenAll completes.
            // Depends on the 'outcomeRecord' object (established/updated by SequentialInitialProcessingUnitC).
            // Crucially depends on 'unitAResults' and 'unitBResults' containing the outputs of the parallel tasks (Step 5).
            // Depends on InMemoryTestDataSet for saving the final state of the outcome record.
            // Subsequent Usage: Finalizes the state of the CoreMlOutcomeRecord before it's retrieved and returned by the main controller method.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Sequential Final Processing Unit D (Actual Model D Concept) for customer {customerIdentifier}.");

            try
            {
                /// <summary>
                /// Operational Step 6.1 (Processing Results from Parallel Units - Model D)
                /// </summary>
                // Read and interpret the results stored in unitAResults and unitBResults.
                // Perform calculations or aggregation based on those results.
                // Update the main 'outcomeRecord' object, potentially setting final vector values or other properties.
                // Operational Process Dependency: Reads from 'unitAResults' and 'unitBResults'. Operates on the 'outcomeRecord' object reference.
                // Subsequent Usage: The updated 'outcomeRecord' object is saved to the simulated database.

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Checking outcomes from parallel processing units...");

                float? unitAResultScalar = null;
                if (unitAResults.TryGetValue("ModelAProcessingOutcome", out var unitAResultValue))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received outcome from Unit A: {unitAResultValue}");
                    if (unitAResultValue is float floatValA) unitAResultScalar = floatValA;
                }
                else if (unitAResults.TryGetValue("ModelAProcessingError", out var unitAErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Error from Unit A: {unitAErrorMessage}");
                    outcomeRecord.CategoricalClassificationDescription += " (Unit A Error)"; // Example error handling update
                }
                else if (unitAResults.TryGetValue("ModelAProcessingWarning", out var unitAWarningMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Warning from Unit A: {unitAWarningMessage}");
                    outcomeRecord.CategoricalClassificationDescription += " (Unit A Warn)"; // Example warning handling update
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: No specific outcome, error, or warning found for Unit A.");
                }


                float? unitBResultScalar = null;
                if (unitBResults.TryGetValue("ModelBProcessingOutcome", out var unitBResultValue))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received outcome from Unit B: {unitBResultValue}");
                    if (unitBResultValue is float floatValB) unitBResultScalar = floatValB;
                }
                else if (unitBResults.TryGetValue("ModelBProcessingError", out var unitBErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Error from Unit B: {unitBErrorMessage}");
                    outcomeRecord.CategoricalClassificationDescription += " (Unit B Error)"; // Example error handling update
                }
                else if (unitBResults.TryGetValue("ModelBProcessingWarning", out var unitBWarningMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Warning from Unit B: {unitBWarningMessage}");
                    outcomeRecord.CategoricalClassificationDescription += " (Unit B Warn)"; // Example warning handling update
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: No specific outcome, error, or warning found for Unit B.");
                }

                await Task.Delay(100); // Simulate finalization work duration

                // Update the main core outcome record object based on combined actual/placeholder results.
                // Operational Process Dependency: Modifies the 'outcomeRecord' object instance that was passed in.
                // Subsequent Usage: This modified object is then saved to the simulated database in the next step.
                // Use ?? 0.0f to ensure operands are floats
                var combinedResult = (unitAResultScalar ?? 0.0f) + (unitBResultScalar ?? 0.0f);

                outcomeRecord.DerivedProductFeatureVector = $"Final_P_Combined_A:{unitAResultScalar ?? 0.0f}_B:{unitBResultScalar ?? 0.0f}";
                outcomeRecord.DerivedServiceBenefitVector = $"Final_S_Combined_Sum:{combinedResult:F4}";
                outcomeRecord.OutcomeGenerationTimestamp = DateTime.UtcNow; // Update timestamp on finalization

                // Example: Update categorical info based on combined result magnitude
                if (combinedResult > 1.0f)
                {
                    outcomeRecord.CategoricalClassificationIdentifier = 100;
                    outcomeRecord.CategoricalClassificationDescription = "High Combined Result";
                }
                else if (combinedResult > 0.5f)
                {
                    outcomeRecord.CategoricalClassificationIdentifier = 50;
                    outcomeRecord.CategoricalClassificationDescription = "Medium Combined Result";
                }
                else
                {
                    outcomeRecord.CategoricalClassificationIdentifier = 10;
                    outcomeRecord.CategoricalClassificationDescription = "Low Combined Result";
                }

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD updated main CoreMlOutcomeRecord object with final vectors, timestamp, and categorical info based on combined parallel results.");


                /// <summary>
                /// Operational Step 6.2 (Save Final State to Simulated Persistence)
                /// </summary>
                // Update the record in the simulated database with the final state of the core outcome object.
                // Operational Process Dependency: Modifies the specific entry in InMemoryTestDataSet.SimulatedCoreOutcomes.
                // Subsequent Usage: Makes the final result available for the main controller method to retrieve and return (Step 7).
                var recordIndex = InMemoryTestDataSet.SimulatedCoreOutcomes.FindIndex(r => r.AssociatedCustomerIdentifier == customerIdentifier);
                if (recordIndex >= 0)
                {
                    InMemoryTestDataSet.SimulatedCoreOutcomes[recordIndex] = outcomeRecord; // Update the static list entry with the final state
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Updated simulated persistent storage for CoreMlOutcomeRecord with customer identifier {customerIdentifier}.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error - CoreMlOutcomeRecord with customer identifier {customerIdentifier} not found in simulated storage during final update attempt!");
                    // This indicates a severe issue where the record established in Step 4 was lost or modified unexpectedly.
                }

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Final Processing Unit D (Actual Model D Concept) completed its processing.");
            }
            /// <summary>
            /// Operational Step 6.3 (Error Handling)
            /// </summary>
            // Catch any exceptions during SequentialFinalProcessingUnitD's execution.
            // Operational Process Dependency: Catches errors during result processing or simulated database saving.
            // Subsequent Usage: Logs the error and re-throws it so the main InitiateMlOutcomeGeneration method can handle it.
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error in Sequential Final Processing Unit D: {ex.Message}");
                throw; // Re-throw to be caught by the main try-catch
            }
            finally
            {
                // Log the finish of the process unit.
                // Operational Process Dependency: Indicates the completion of the final sequential step before workflow cleanup and return in the main method.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Final Processing Unit D (Actual Model D Concept) finished.");
            }
        }


        // Helper method to serialize a float array to a byte array
        private byte[] SerializeFloatArray(float[] data)
        {
            if (data == null) return new byte[0];
            var byteList = new List<byte>();
            foreach (var f in data)
            {
                byteList.AddRange(BitConverter.GetBytes(f));
            }
            return byteList.ToArray();
        }

        // Helper method to deserialize a byte array back to a float array
        private float[] DeserializeFloatArray(byte[] data)
        {
            if (data == null || data.Length == 0) return new float[0];
            if (data.Length % 4 != 0) // Size of float is 4 bytes
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Byte array length ({data.Length}) is not a multiple of 4 for deserialization.");
                return new float[0];
            }
            var floatArray = new float[data.Length / 4];
            // Specify System.Buffer
            System.Buffer.BlockCopy(data, 0, floatArray, 0, data.Length);
            return floatArray;
        }


        // GET endpoint to retrieve a CoreMlOutcomeRecord by its unique identifier.
        // This endpoint operates on the same simulated persistence layer but is separate from the main ML workflow.
        [HttpGet("{recordIdentifier}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoreMlOutcomeRecord))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CoreMlOutcomeRecord> GetOutcomeRecordByIdentifier(int recordIdentifier)
        {
            // Operational Step: Retrieve a single record from simulated persistence by its unique identifier.
            // Operational Process Dependency: Reads from InMemoryTestDataSet.SimulatedCoreOutcomes.
            // Subsequent Usage: Returns the found record or a 404 Not Found response.
            var outcomeRecord = InMemoryTestDataSet.SimulatedCoreOutcomes.FirstOrDefault(r => r.RecordIdentifier == recordIdentifier);

            if (outcomeRecord == null)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] GET request for record ID {recordIdentifier}: Record not found.");
                return NotFound(); // Record not found
            }

            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH::ss.fff}] GET request for record ID {recordIdentifier}: Record found.");
            return Ok(outcomeRecord); // Return found record
        }

        // GET endpoint to retrieve all CoreMlOutcomeRecords from simulated persistence.
        // This endpoint operates on the same simulated persistence layer but is separate from the main ML workflow.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoreMlOutcomeRecord>))]
        public ActionResult<IEnumerable<CoreMlOutcomeRecord>> GetAllOutcomeRecords()
        {
            // Operational Step: Retrieve all records from simulated persistence.
            // Operational Process Dependency: Reads from InMemoryTestDataSet.SimulatedCoreOutcomes.
            // Subsequent Usage: Returns the list of all records currently in the simulated storage.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] GET request for all records: Returning {InMemoryTestDataSet.SimulatedCoreOutcomes.Count} records.");
            return Ok(InMemoryTestDataSet.SimulatedCoreOutcomes); // Return all records
        }
    }

    // Extension method to reshape a 1D array into a multi-dimensional array
    // Note: TensorFlow.NET's Tensor.reshape() is for Tensor objects, not .NET arrays.
    // This helper is needed to convert the deserialized float[] into the shape expected by tf.constant().
    public static class ArrayExtensions
    {
        public static Array reshape<T>(this T[] flatArray, params int[] dimensions)
        {
            if (flatArray == null) throw new ArgumentNullException(nameof(flatArray));
            if (dimensions == null) throw new ArgumentNullException(nameof(dimensions));

            int totalSize = dimensions.Aggregate(1, (acc, val) => acc * val);
            if (totalSize != flatArray.Length)
            {
                throw new ArgumentException($"Total size of new dimensions ({totalSize}) must match the size of the array ({flatArray.Length}).", nameof(dimensions));
            }

            // For simplicity, this helper only supports 2D reshaping to [N, 1] for this specific use case.
            if (dimensions.Length == 2 && dimensions[1] == 1)
            {
                T[,] reshaped = new T[dimensions[0], dimensions[1]];
                // Specify System.Buffer
                System.Buffer.BlockCopy(flatArray.ToArray(), 0, reshaped, 0, System.Buffer.ByteLength(flatArray.ToArray()));
                return reshaped;
            }
            else if (dimensions.Length == 1)
            {
                // Return the 1D array itself if reshaping to 1D
                return flatArray;
            }
            else
            {
                // Throw for unsupported shapes
                throw new NotSupportedException($"Reshaping to {string.Join(",", dimensions)} is not supported by this helper. Only 1D and [N, 1] 2D arrays are supported.");
            }
        }
    }

}