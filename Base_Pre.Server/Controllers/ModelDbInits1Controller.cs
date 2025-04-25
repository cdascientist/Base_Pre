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

// Essential framework and utility imports for creating the web controller,
// managing concurrent data structures, handling asynchronous operations,
// performing I/O, using LINQ for data querying, and reflection/dynamic features.
// These form the foundational dependencies for the application's execution environment.

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
        public byte[]? SerializedSimulatedModelData { get; set; } // Binary data representing the trained simulated model
        public byte[]? AncillaryBinaryDataPayload { get; set; } // Additional binary data payload
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

    #region Simulated Machine Intelligence Components

    // This region contains mock or simulated classes representing components
    // used in a machine learning process, particularly inspired by TensorFlow concepts.
    // These serve as functional dependencies for the processing units that perform ML simulation.

    /// <summary>
    /// Configuration parameters for the simulated machine learning training process.
    /// Defines how the mock training loop behaves.
    /// </summary>
    public class MlSimulationTrainingParameters
    {
        // Properties define training settings.
        // Operational Process Dependency: Could be used by simulation logic in processing units.
        public int IterationCount { get; set; }
        public float InitialGradientAdjustmentRate { get; set; }
        public float ConvergenceStabilityThreshold { get; set; }
        public int StableIterationsRequired { get; set; }
        public float MinimumAdjustmentRate { get; set; }
    }

    /// <summary>
    /// Represents a node (data or operation) in the simulated computation graph.
    /// Analogous to a TensorFlow Tensor.
    /// </summary>
    public class SimulatedMlTensor
    {
        // Properties define the simulated tensor node.
        // Operational Process Dependency: Created by SimulatedMlEngine methods (e.g., inputOutputPlaceholder, trainableParameterVariable).
        // Passed between SimulatedMlEngine operations.
        // Used in SimulatedMlInputFeedEntry and processed by SimulatedMlSession.
        public string DescriptiveIdentifier { get; } // Arbitrary self-descriptive name
        public object ShapeRepresentation { get; } // Represents the shape (e.g., int[])

        public SimulatedMlTensor(string identifier, object shape)
        {
            DescriptiveIdentifier = identifier;
            ShapeRepresentation = shape;
        }
    }

    /// <summary>
    /// Represents a key-value pair for feeding input data into the simulated ML session.
    /// Links a placeholder tensor to actual data.
    /// </summary>
    public class SimulatedMlInputFeedEntry
    {
        // Properties define one entry in the feed dictionary.
        // Operational Process Dependency: Created by processing units to provide input data
        // for SimulatedMlSession.RunExecutionStep or SimulatedMlSession.RunTensorQuery.
        // Depends on a SimulatedMlTensor and the data value (SimulatedNdArray or similar).
        public object Key { get; } // The SimulatedMlTensor placeholder
        public object Value { get; } // The input data (e.g., SimulatedNdArray)

        public SimulatedMlInputFeedEntry(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// Represents a multi-dimensional array of floating-point numbers in the simulation.
    /// Analogous to a NumPy ndarray or TensorFlow Tensor data value.
    /// </summary>
    public class SimulatedNdArray
    {
        // Holds the raw data for the simulated array.
        // Operational Process Dependency: Returned by SimulatedMlSession.RunTensorQuery
        // when querying variable tensors (like weights, biases).
        // Used as data values in SimulatedMlInputFeedEntry.
        private float[] _internalFloatData;

        public SimulatedNdArray(float[] data)
        {
            _internalFloatData = data;
        }

        public T[] RetrieveAsArray<T>()
        {
            // Converts the internal float data array to a specific type.
            // Operational Process Dependency: Called by processing units (specifically SequentialInitialProcessingUnitC)
            // after retrieving results from SimulatedMlSession to access the raw numerical data for serialization or processing.
            if (typeof(T) == typeof(float))
            {
                return _internalFloatData as T[];
            }

            throw new InvalidOperationException("Unsupported array type requested");
        }
    }

    /// <summary>
    /// Simulates a TensorFlow Session, the environment for executing the computation graph.
    /// Manages the simulated graph and provides methods to run operations or query tensors.
    /// </summary>
    public class SimulatedMlSession : IDisposable
    {
        // Simulates the core ML session object.
        // Operational Process Dependency: Instances are created by InitiateMlOutcomeGeneration
        // and SequentialInitialProcessingUnitC (for Model C).
        // Passed to ParallelProcessingUnitA (Model A) and ParallelProcessingUnitB (Model B).
        // Provides methods (RunExecutionStep, RunTensorQuery) used by processing units to execute simulated ML operations.
        // Must be Disposed of after use (handled in the main controller method's finally block).

        private readonly Random _randomDataSource = new Random(); // Source for simulated random numbers

        public SimulatedMlSession()
        {
            System.Diagnostics.Debug.WriteLine("Simulated ML Session Environment created.");
        }

        public void Dispose()
        {
            // Placeholder for releasing simulated resources.
            // Operational Process Dependency: Called by the main controller method's finally block
            // to clean up session resources after the workflow completes.
            System.Diagnostics.Debug.WriteLine("Simulated ML Session Environment disposed.");
        }

        public void RunExecutionStep(object operationNode, IEnumerable<SimulatedMlInputFeedEntry> simulationInputData = null)
        {
            // Simulates running a TensorFlow operation (e.g., a training step, initialization).
            // Does not return a value representing computation output, only executes the operation.
            // Operational Process Dependency: Called by processing units to execute simulated training steps or initialization.
            // Depends on the 'operationNode' (e.g., result of SimulatedGradientAdjustmentMechanism.MinimizeProcess) and potentially 'simulationInputData'.
            System.Diagnostics.Debug.WriteLine($"Simulated ML Session Run: Executing operational node: {(operationNode as SimulatedMlTensor)?.DescriptiveIdentifier ?? operationNode?.GetType().Name ?? "unspecified_operation"}");
        }

        /// <summary>
        /// Simulates fetching the value of a tensor node in the computation graph.
        /// </summary>
        /// <param name="tensorNode">The SimulatedMlTensor node whose value to fetch.</param>
        /// <param name="simulationInputData">Optional input data for placeholders.</param>
        /// <returns>A simulated value (e.g., SimulatedNdArray or float).</returns>
        public object RunTensorQuery(object tensorNode, IEnumerable<SimulatedMlInputFeedEntry> simulationInputData = null)
        {
            // Simulates fetching the value of a tensor.
            // Operational Process Dependency: Called by processing units to get results (like weights, bias, loss) after simulated training.
            // Depends on the 'tensorNode' (e.g., a SimulatedMlTensor) and potentially 'simulationInputData'.
            // The returned object (SimulatedNdArray or float) is used for further processing, serialization, or logging.
            // Provides data that SequentialInitialProcessingUnitC serializes into CoreMlOutcomeRecord.SerializedSimulatedModelData or AncillaryBinaryDataPayload.

            System.Diagnostics.Debug.WriteLine($"Simulated ML Session Run: Fetching value for tensor: {((SimulatedMlTensor)tensorNode)?.DescriptiveIdentifier ?? "unspecified_tensor_object"}");

            if (tensorNode is SimulatedMlTensor descriptiveTensor)
            {
                // Return appropriate mock data based on the tensor's descriptive identifier
                if (descriptiveTensor.DescriptiveIdentifier == "Simulated_ML_Weight_Variable" || descriptiveTensor.DescriptiveIdentifier.Contains("Weight"))
                {
                    // Simulate weights array based on shape
                    if (descriptiveTensor.ShapeRepresentation is int[] shape && shape.Length == 2)
                    {
                        float[] weights = new float[shape[0] * shape[1]];
                        for (int i = 0; i < weights.Length; i++)
                        {
                            weights[i] = (float)(_randomDataSource.NextDouble() * 0.02 - 0.01); // Simulate random initialization
                        }
                        return new SimulatedNdArray(weights); // Return a simulated ND array
                    }
                }
                else if (descriptiveTensor.DescriptiveIdentifier == "Simulated_ML_Bias_Variable" || descriptiveTensor.DescriptiveIdentifier.Contains("Bias"))
                {
                    // Simulate bias array based on shape
                    if (descriptiveTensor.ShapeRepresentation is int[] shape && shape.Length == 1)
                    {
                        float[] biases = new float[shape[0]];
                        for (int i = 0; i < biases.Length; i++)
                        {
                            biases[i] = 0.1f; // Sample bias value
                        }
                        return new SimulatedNdArray(biases); // Return a simulated ND array
                    }
                }
                else if (descriptiveTensor.DescriptiveIdentifier == "Simulated_ML_Predictions_Output" || descriptiveTensor.DescriptiveIdentifier.Contains("Predictions"))
                {
                    // Simulate a prediction value (as a single-element array in NDArray)
                    return new SimulatedNdArray(new float[] { (float)_randomDataSource.NextDouble() });
                }
                else if (descriptiveTensor.DescriptiveIdentifier == "Simulated_ML_Loss_Scalar") // Explicitly check for the loss tensor's identifier
                {
                    // Return a simulated float scalar loss value
                    return (float)(0.01f + (_randomDataSource.NextDouble() * 0.01));
                }
                else if (descriptiveTensor.DescriptiveIdentifier.Contains("Magnitude") || descriptiveTensor.DescriptiveIdentifier.Contains("Sqrt")) // Generic check for magnitude/sqrt results
                {
                    // Simulate a scalar result for magnitude/sqrt
                    return new SimulatedNdArray(new float[] { 1.5f });
                }
                else if (descriptiveTensor.DescriptiveIdentifier.Contains("ReduceMean")) // Add explicit check for reduce_mean output if needed
                {
                    // Simulate a scalar result for reduce mean
                    return (float)(0.5 + (_randomDataSource.NextDouble() * 0.1));
                }
                else
                {
                    // FIX for CS0161: If it is a SimulatedMlTensor but doesn't match any specific known name,
                    // we must return something. A default float scalar seems appropriate based on original code intent.
                    System.Diagnostics.Debug.WriteLine($"Simulated ML Session Run: Unrecognized SimulatedMlTensor name '{descriptiveTensor.DescriptiveIdentifier}', returning default float scalar.");
                    return 0.0f;
                }
            }
            else
            {
                // FIX for CS0161: If the input object is not even a SimulatedMlTensor,
                // we must return something.
                System.Diagnostics.Debug.WriteLine($"Simulated ML Session Run: Input object is not a SimulatedMlTensor, returning default float scalar.");
                return 0.0f; // Default return for non-Tensor input
            }
        }
    }

    /// <summary>
    /// Mock component simulating a machine learning optimizer (like Gradient Descent).
    /// Used to define the training step operation.
    /// </summary>
    public class SimulatedGradientAdjustmentMechanism
    {
        // Simulates an optimizer component.
        // Operational Process Dependency: Created by SimulatedMlEngine.trainingModule.SteepestDescentAdjustmentMechanism.
        // Its MinimizeProcess method is called by processing units to get the simulated training operation.
        private readonly float _adjustmentStepMagnitude;

        public SimulatedGradientAdjustmentMechanism(float adjustmentRate)
        {
            _adjustmentStepMagnitude = adjustmentRate;
        }

        public object MinimizeProcess(object targetLoss)
        {
            // Simulates returning an operation object representing the training step.
            // Operational Process Dependency: Called by processing units to define the simulated training step.
            // Depends on the 'targetLoss' tensor. The returned object is then passed to SimulatedMlSession.RunExecutionStep.
            return new object(); // Represents the simulated training operation
        }
    }

    /// <summary>
    /// Mock component simulating random number generation functions for ML initialization.
    /// Used for simulating weight initialization.
    /// </summary>
    public class SimulatedRandomDistributionSource
    {
        // Simulates TF random functions.
        // Operational Process Dependency: Accessed via SimulatedMlEngine.randomDataSource.
        // Its NormalDistributionTensor method is used by processing units to simulate initializing variable Tensors (like weights).
        private readonly Random _internalRandom = new Random();

        public object NormalDistributionTensor(object shapeRepresentation, float meanValue = 0.0f, float standardDeviation = 0.01f)
        {
            // Simulates returning a variable tensor initialized from a normal distribution.
            // Operational Process Dependency: Used when creating simulated variable Tensors (e.g., weights).
            // Depends on the 'shapeRepresentation'. The returned object (a SimulatedMlTensor) is then typically passed to SimulatedMlEngine.trainableParameterVariable.
            return new SimulatedMlTensor("Simulated_Weight_Init_Tensor", shapeRepresentation);
        }
    }

    /// <summary>
    /// Static facade simulating core TensorFlow library functionality.
    /// Provides static access to simulated ML operations and components.
    /// </summary>
    public static class SimulatedMlEngine
    {
        // Simulates the static TF library entry point.
        // Operational Process Dependency: All simulated ML operations within processing units are accessed via this static class.
        // This is a static dependency for any method simulating ML usage.

        public static readonly object SimulatedFloatPrecisionType = new object(); // Simulates a data type representation
        public static SimulatedRandomDistributionSource randomDataSource = new SimulatedRandomDistributionSource(); // Provides access to simulated random functions

        public static class compatibilityLayer
        {
            public static class versionOne
            {
                // Simulates compatibility functions for a specific version.
                public static void disableImmediateExecutionMode()
                {
                    // Simulates disabling eager execution mode.
                    // Operational Process Dependency: Called at the start of simulated ML setup in processing units.
                    System.Diagnostics.Debug.WriteLine("Simulated ML Engine: Immediate Execution Mode disabled.");
                }
            }
        }

        public static class trainingModule
        {
            // Simulates training related functions.
            public static SimulatedGradientAdjustmentMechanism SteepestDescentAdjustmentMechanism(float adjustmentRate)
            {
                // Simulates creating an optimizer instance.
                // Operational Process Dependency: Called by processing units to get a simulated optimizer.
                // Depends on the 'adjustmentRate'. The returned object (SimulatedGradientAdjustmentMechanism) is then used to get the training operation.
                return new SimulatedGradientAdjustmentMechanism(adjustmentRate);
            }
        }

        public static object ComputationGraphBuilder()
        {
            // Simulates creating a computation graph container.
            // Operational Process Dependency: Called at the start of simulated ML setup in processing units.
            return new object(); // Represents the simulated graph
        }

        public static SimulatedMlTensor inputOutputPlaceholder(object dataType, object shapeRepresentation, string identifier = "")
        {
            // Simulates creating a placeholder node for inputs or outputs.
            // Operational Process Dependency: Called by processing units to define input/output nodes in the simulated graph.
            // Depends on 'dataType' and 'shapeRepresentation'. The returned SimulatedMlTensor is used in defining the model graph and in SimulatedMlInputFeedEntry.
            return new SimulatedMlTensor(identifier, shapeRepresentation);
        }

        public static SimulatedMlTensor trainableParameterVariable(object initialValue, string identifier = "")
        {
            // Simulates creating a variable node for trainable parameters (like weights, biases).
            // Operational Process Dependency: Called by processing units to define trainable variables.
            // Depends on the 'initialValue' (often derived from SimulatedRandomDistributionSource or zeroFilledTensorCreator). The returned SimulatedMlTensor is part of the graph and can be queried/updated.
            object shape = null;
            if (initialValue is SimulatedMlTensor valueTensor)
            {
                shape = valueTensor.ShapeRepresentation; // Infer shape from initial tensor
            }
            return new SimulatedMlTensor(identifier, shape);
        }

        public static SimulatedMlTensor tensorAdditionOperation(SimulatedMlTensor tensorA, SimulatedMlTensor tensorB)
        {
            // Simulates adding two tensors element-wise.
            // Operational Process Dependency: Called by processing units as part of defining the simulated model graph (ee.g., output = matrix_multiplication + bias).
            // Depends on input SimulatedMlTensors 'tensorA' and 'tensorB'. Returns a new SimulatedMlTensor representing the result node.
            return new SimulatedMlTensor($"Addition_{tensorA.DescriptiveIdentifier}_{tensorB.DescriptiveIdentifier}", null); // Simplified shape handling
        }

        public static SimulatedMlTensor matrixMultiplicationOperation(SimulatedMlTensor tensorA, SimulatedMlTensor tensorB)
        {
            // Simulates matrix multiplication of two tensors.
            // Operational Process Dependency: Called by processing units as part of defining the simulated model graph (e.g., matrix_multiplication(input, weights)).
            // Depends on input SimulatedMlTensors 'tensorA' and 'tensorB'. Returns a new SimulatedMlTensor representing the result node. Includes basic shape simulation.
            if (tensorA.ShapeRepresentation is int[] shapeA && tensorB.ShapeRepresentation is int[] shapeB && shapeA.Length == 2 && shapeB.Length == 2 && shapeA[1] == shapeB[0])
            {
                // Simulate output shape: [batch_size/shapeA[0], shapeB[1]]
                // Note: For batch size placeholder (-1), output shape is also [-1, shapeB[1]]
                int outputDim0 = shapeA[0]; // Keep batch size placeholder or actual size
                int outputDim1 = shapeB[1];
                return new SimulatedMlTensor($"MatrixMult_{tensorA.DescriptiveIdentifier}_{tensorB.DescriptiveIdentifier}", new int[] { outputDim0, outputDim1 });
            }
            System.Diagnostics.Debug.WriteLine($"Simulated ML Engine: Warning - Matmul operation inputs have incompatible or invalid shapes for simulation.");
            return new SimulatedMlTensor($"MatrixMult_{tensorA.DescriptiveIdentifier}_{tensorB.DescriptiveIdentifier}_ShapeError", null); // Return with null shape on error
        }

        public static object meanReductionOperation(object tensorToReduce, float scalingMultiplier = 1.0f)
        {
            // Simulates computing the mean of tensor elements, typically reducing dimensionality.
            // Operational Process Dependency: Used in defining the simulated loss function.
            // Depends on the input 'tensorToReduce'. Returns a SimulatedMlTensor representing the scalar mean (or reduced shape).
            // Note: Loss is often a scalar, so a shape of {1} is simulated.
            return new SimulatedMlTensor("ReduceMean_Result_Scalar", new int[] { 1 });
        }

        public static object elementWiseSquareOperation(object tensorToSquare)
        {
            // Simulates squaring elements of a tensor.
            // Operational Process Dependency: Used in defining the simulated loss function (e.g., squared error).
            // Depends on the input 'tensorToSquare'. Returns a SimulatedMlTensor with the same shape.
            return new SimulatedMlTensor("SquareOperation_Result", null); // Shape remains the same (simplified)
        }

        public static object parameterInitializationOperation()
        {
            // Simulates the operation to initialize all variable nodes in the graph.
            // Operational Process Dependency: Called by processing units using SimulatedMlSession.RunExecutionStep before starting training.
            return new object(); // Represents the simulated initialization operation
        }

        public static SimulatedMlTensor zeroFilledTensorCreator(object shapeRepresentation)
        {
            // Simulates creating a tensor node filled with zeros.
            // Operational Process Dependency: Used when creating simulated variable Tensors (like biases).
            // Depends on the 'shapeRepresentation'. Returns a new SimulatedMlTensor.
            return new SimulatedMlTensor("ZeroFilled_Tensor", shapeRepresentation);
        }
    }

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
            new { Identifier = 3, ItemDesignation = "Product C Gamma", Categorization = "Type 1 Assembly", QuantityAvailable = 15, MonetaryValue = 199.99, CostContributionValue = 0.20 }
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
            new { Identifier = 1, ServiceNameDescriptor = "Service A Initial Offering", Categorization = "Tier 1 Support", FulfillmentQuantity = 5, MonetaryValue = 299.99, CostContributionValue = 0.30 },
            new { Identifier = 2, ServiceNameDescriptor = "Service B Advanced Provision", Categorization = "Tier 2 Consulting", FulfillmentQuantity = 10, MonetaryValue = 399.99, CostContributionValue = 0.35 },
            new { Identifier = 3, ServiceNameDescriptor = "Service C Premium Engagement", Categorization = "Tier 3 Managed", FulfillmentQuantity = 8, MonetaryValue = 599.99, CostContributionValue = 0.40 }
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
        private readonly ConcurrentDictionary<int, SimulatedMlSession> _activeMlSessions; // Tracks active simulated ML sessions by unique ID
        private readonly ConcurrentOperationManager _operationConcurrencyManager; // Instance of the concurrency manager

        /// <summary>
        /// Constructor for the ML process orchestration controller.
        /// Initializes session tracking and the concurrency manager.
        /// Populates the runtime processing context with initial simulated data.
        /// </summary>
        public MlProcessOrchestrationController()
        {
            // Initializes the concurrent dictionary for tracking simulated ML sessions and the concurrency manager.
            _activeMlSessions = new ConcurrentDictionary<int, SimulatedMlSession>();
            _operationConcurrencyManager = new ConcurrentOperationManager();

            // Initialize runtime memory with simulated data from the test dataset.
            // This makes simulated product/service data easily accessible to processing logic via RuntimeProcessingContext.
            // Operational Process Dependency: Data loaded here can be retrieved via RuntimeProcessingContext.RetrieveContextValue
            // by processing units that need lookups based on IDs from InitialOperationalStageData.
            RuntimeProcessingContext.StoreContextValue("All_Simulated_Service_Offerings", InMemoryTestDataSet.SampleServiceOfferings);
            RuntimeProcessingContext.StoreContextValue("All_Simulated_Product_Inventory", InMemoryTestDataSet.SampleProductInventory);
        }

        /// <summary>
        /// This endpoint initiates the machine learning outcome generation process for a specific customer.
        /// It orchestrates a sequence involving initial setup, parallel processing, and final aggregation,
        /// using simulated services and data persistence.
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
            // Operational Process Dependency: Used for logging and uniquely identifying simulated ML sessions created for this request in the _activeMlSessions dictionary.
            var requestSequenceIdentifier = Interlocked.Increment(ref _requestSessionSequenceCounter);
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Starting ML Outcome Generation Workflow Session {requestSequenceIdentifier} for customer {customerIdentifier.Value}");

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

                // Create dedicated simulated ML sessions for the parallel processing units (Units A and B).
                // These are functional dependencies for ParallelProcessingUnitA and ParallelProcessingUnitB.
                var modelAProcessingSession = new SimulatedMlSession(); // Simulated ML session for Unit A
                var modelBProcessingSession = new SimulatedMlSession(); // Simulated ML session for Unit B

                /// <summary>
                /// Operational Step 3: Register Simulated ML Sessions for Management
                /// </summary>
                // Register the created simulated ML sessions in the controller's session manager using unique IDs derived from the requestSequenceIdentifier.
                // Operational Process Dependency: This is necessary for proper resource disposal in the 'finally' block at the end of the workflow.
                _activeMlSessions.TryAdd(requestSequenceIdentifier * 2, modelAProcessingSession);     // Even numbered ID for Unit A session
                _activeMlSessions.TryAdd(requestSequenceIdentifier * 2 + 1, modelBProcessingSession); // Odd numbered ID for Unit B session

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Executing Sequential Initial Processing Unit (C).");

                /// <summary>
                /// Operational Step 4 (Sequential): Execute Initial Processing Unit (C)
                /// </summary>
                // Execute SequentialInitialProcessingUnitC (simulating "ProcessFactoryOne"). This step runs sequentially first.
                // This unit is responsible for creating or retrieving the core CoreMlOutcomeRecord for the customer and establishing associated dependency records in simulated persistence.
                // It also simulates the initial ML training (Model C) and saves the resulting simulated model data.
                // Operational Process Dependency: Requires the initial 'currentOutcomeRecord' container, the customerIdentifier, and the requestSequenceIdentifier for context and logging.
                // Internally depends on InMemoryTestDataSet and RuntimeProcessingContext.
                // Subsequent Usage: The successful completion of this unit is a dependency for the parallel processing units (A and B) as they operate on the established core record.
                // It populates InMemoryTestDataSet and RuntimeProcessingContext with the initial CoreMlOutcomeRecord and related data.
                await SequentialInitialProcessingUnitC(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier);

                // Retrieve the CoreMlOutcomeRecord object from simulated persistence *after* SequentialInitialProcessingUnitC has potentially created or updated it.
                // This ensures the orchestrator method has the latest state of the record before passing it to parallel units.
                // Operational Process Dependency: Depends on SequentialInitialProcessingUnitC successfully creating/finding and adding/updating the record in InMemoryTestDataSet.
                // Subsequent Usage: The 'outcomeRecordAfterStepOne' reference is used to check if Step 4 was successful and is then assigned to 'currentOutcomeRecord' to be passed to parallel units.
                var outcomeRecordAfterStepOne = InMemoryTestDataSet.SimulatedCoreOutcomes
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
                // Execute ParallelProcessingUnitA (simulating "ProcessFactoryTwo") and ParallelProcessingUnitB (simulating "ProcessFactoryThree") concurrently using Task.WhenAll.
                // Operational Process Dependency: Both units depend on the core 'currentOutcomeRecord' object established by SequentialInitialProcessingUnitC (Step 4).
                // They also depend on their respective allocated simulated ML sessions (modelAProcessingSession, modelBProcessingSession) and thread-safe result dictionaries (modelAConcurrentResults, modelBConcurrentResults).
                // Subsequent Usage: The main workflow waits here until both parallel tasks complete. Their outputs are stored in 'modelAConcurrentResults' and 'modelBConcurrentResults'.
                await Task.WhenAll(
                    ParallelProcessingUnitA(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier, modelAProcessingSession, modelAConcurrentResults),
                    ParallelProcessingUnitB(currentOutcomeRecord, customerIdentifier.Value, requestSequenceIdentifier, modelBProcessingSession, modelBConcurrentResults)
                );

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Units A and B completed. Starting Sequential Final Processing Unit (D).");

                /// <summary>
                /// Operational Step 6 (Sequential): Execute Final Processing Unit (D)
                /// </summary>
                // Execute SequentialFinalProcessingUnitD (simulating "ProcessFactoryFour"). This step runs sequentially after the parallel units have completed.
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
                var finalOutcomeRecord = InMemoryTestDataSet.SimulatedCoreOutcomes
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
                // Cleanup: Remove and dispose the simulated ML sessions created for this request from the manager.
                // Operational Process Dependency: Depends on the sessions being added to the _activeMlSessions dictionary in Step 3.
                // Subsequent Usage: Releases simulated resources associated with this request, important for managing state in a real application.
                if (_activeMlSessions.TryRemove(requestSequenceIdentifier * 2, out var modelAProcessingSession))
                    modelAProcessingSession?.Dispose();
                if (_activeMlSessions.TryRemove(requestSequenceIdentifier * 2 + 1, out var modelBProcessingSession))
                    modelBProcessingSession?.Dispose();

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Associated simulated ML session resources cleaned up.");
            }
        }




        /// <summary>
        /// Processes data for Model C (SequentialInitialProcessingUnitC).
        /// This is the *first sequential* processing step in the workflow.
        /// It is responsible for ensuring the core CoreMlOutcomeRecord exists for the customer,
        /// creating it and associated dependency records in simulated persistence if necessary, or loading existing ones.
        /// It simulates a simple machine learning training process using mock components and saves the resulting simulated model data.
        /// </summary>
        /// <param name="outcomeRecord">A reference to the CoreMlOutcomeRecord container/instance to work with.</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        private async Task SequentialInitialProcessingUnitC(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier)
        {
            // This method handles the initial setup, data record establishment, and Simulated Model C logic.
            // Operational Process Dependency: Called exclusively by InitiateMlOutcomeGeneration (Step 4).
            // Depends on InMemoryTestDataSet for data persistence simulation.
            // Depends on RuntimeProcessingContext for runtime data sharing within the request.
            // Depends on SimulatedMachineIntelligenceComponents (mock ML classes) for ML simulation.
            // Subsequent Usage: Its successful completion and data updates are dependencies for ParallelProcessingUnitA, ParallelProcessingUnitB, and SequentialFinalProcessingUnitD.

            // Set a flag in runtime memory indicating this process is active.
            // Subsequent Usage: Could theoretically be checked by other parts of the system for monitoring or coordination within the request.
            RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_ActiveStatus", true);
            bool isActiveStart = (bool)RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_ActiveStatus"); // Verification log
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialProcessingUnitC ActiveStatus property value: {isActiveStart}");
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Sequential Initial Processing Unit C (Simulated Model C).");

            try
            {
                /// <summary>
                /// Operational Step 4.1: Find or Create Core Outcome Record and Associated Dependencies
                /// </summary>
                // Check if a CoreMlOutcomeRecord already exists for the given customer in the simulated database.
                // Operational Process Dependency: Reads from InMemoryTestDataSet.SimulatedCoreOutcomes.
                // Subsequent Usage: Determines whether to perform 'new record' or 'existing record' setup and simulated processing.
                var retrievedOrNewOutcomeRecord = InMemoryTestDataSet.SimulatedCoreOutcomes
                                .FirstOrDefault(r => r.AssociatedCustomerIdentifier == customerIdentifier);

                if (retrievedOrNewOutcomeRecord == null)
                {
                                 // --- If NO MODEL FOR INSTANCE FOUND ---
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: No existing CoreMlOutcomeRecord found for Customer Identifier {customerIdentifier}. Initializing new record and associated dependencies.");

                                // --- Operations for NEW Core Record Initialization ---

                                // Create a new CoreMlOutcomeRecord instance.
                                // Operational Process Dependency: Creates the primary data entity for this customer's workflow if it didn't exist.
                                var nextAvailableRecordIdentifier = InMemoryTestDataSet.SimulatedCoreOutcomes
                                                    .Count > 0 ? InMemoryTestDataSet.SimulatedCoreOutcomes.Max(r => r.RecordIdentifier) : 0;

                                retrievedOrNewOutcomeRecord = new CoreMlOutcomeRecord
                                {
                                    AssociatedCustomerIdentifier = customerIdentifier,
                                    OutcomeGenerationTimestamp = DateTime.UtcNow,
                                    RecordIdentifier = nextAvailableRecordIdentifier + 1, // Assign simulated new ID
                                    CategoricalClassificationIdentifier = null, // Initial state
                                    CategoricalClassificationDescription = null, // Initial state
                                    SerializedSimulatedModelData = null, // Will be populated by simulated training
                                    AncillaryBinaryDataPayload = null, // Will be populated by simulated training (e.g., bias data)
                                    DerivedProductFeatureVector = null, // Will be populated later (likely SequentialFinalProcessingUnitD)
                                    DerivedServiceBenefitVector = null // Will be populated later (likely SequentialFinalProcessingUnitD)
                                };

                                // Add the newly created CoreMlOutcomeRecord to the simulated database.
                                // Operational Process Dependency: Modifies InMemoryTestDataSet.SimulatedCoreOutcomes.
                                // Subsequent Usage: Makes this record retrievable by the main controller method (for checking existence and final result) and other processing units via InMemoryTestDataSet.
                                InMemoryTestDataSet.SimulatedCoreOutcomes.Add(retrievedOrNewOutcomeRecord);
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created new CoreMlOutcomeRecord with Identifier {retrievedOrNewOutcomeRecord.RecordIdentifier} for customer {customerIdentifier}");

                                // Create or find associated dependency records (AssociatedCustomerContext, OperationalWorkOrderRecord, MlInitialOperationEvent, MlOutcomeValidationRecord, InitialOperationalStageData).
                                // Operational Process Dependency: Checks and potentially modifies static lists in InMemoryTestDataSet for these types. Linked by CustomerLinkIdentifier (and potentially RelatedOrderIdentifier).
                                // Subsequent Usage: These records provide related context, though their data isn't deeply used in the placeholder ML logic. References are stored in RuntimeProcessingContext for potential later access within the request.

                                // Create or find AssociatedCustomerContext
                                var associatedCustomer = InMemoryTestDataSet.SimulatedCustomerContexts.FirstOrDefault(c => c.CustomerLinkIdentifier == customerIdentifier);
                                if (associatedCustomer == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new AssociatedCustomerContext record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedCustomerContexts.Count > 0 ? InMemoryTestDataSet.SimulatedCustomerContexts.Max(c => c.ContextIdentifier) : 0; associatedCustomer = new AssociatedCustomerContext { ContextIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, CustomerPrimaryGivenName = $"Simulated FN {customerIdentifier}", CustomerFamilyName = $"Simulated LN {customerIdentifier}", CustomerContactPhoneNumber = $"555-cust-{customerIdentifier}", CustomerStreetAddress = $"Simulated Address {customerIdentifier}", AffiliatedCompanyName = $"Simulated Company {customerIdentifier}" }; InMemoryTestDataSet.SimulatedCustomerContexts.Add(associatedCustomer); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created AssociatedCustomerContext record with Identifier {associatedCustomer.ContextIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing AssociatedCustomerContext record found for Customer {customerIdentifier}"); }

                                // Create or find OperationalWorkOrderRecord
                                var associatedWorkOrder = InMemoryTestDataSet.SimulatedWorkOrders.FirstOrDefault(o => o.CustomerLinkIdentifier == customerIdentifier);
                                if (associatedWorkOrder == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new OperationalWorkOrderRecord for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedWorkOrders.Count > 0 ? InMemoryTestDataSet.SimulatedWorkOrders.Max(o => o.OrderRecordIdentifier) : 0; associatedWorkOrder = new OperationalWorkOrderRecord { OrderRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, SpecificOrderIdentifier = customerIdentifier + 9000 }; InMemoryTestDataSet.SimulatedWorkOrders.Add(associatedWorkOrder); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created OperationalWorkOrderRecord with Identifier {associatedWorkOrder.OrderRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing OperationalWorkOrderRecord found for Customer {customerIdentifier}"); }

                                // Create or find MlInitialOperationEvent
                                var operationalEventRecord = InMemoryTestDataSet.SimulatedOperationalEvents.FirstOrDefault(e => e.CustomerLinkIdentifier == customerIdentifier);
                                if (operationalEventRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlInitialOperationEvent record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedOperationalEvents.Count > 0 ? InMemoryTestDataSet.SimulatedOperationalEvents.Max(e => e.EventIdentifier) : 0; operationalEventRecord = new MlInitialOperationEvent { EventIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, EventPayloadData = new byte[] { (byte)customerIdentifier, 0xAA } }; InMemoryTestDataSet.SimulatedOperationalEvents.Add(operationalEventRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlInitialOperationEvent record with Identifier {operationalEventRecord.EventIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlInitialOperationEvent record found for Customer {customerIdentifier}"); }

                                // Create or find MlOutcomeValidationRecord
                                var validationRecord = InMemoryTestDataSet.SimulatedOutcomeValidations.FirstOrDefault(v => v.CustomerLinkIdentifier == customerIdentifier);
                                if (validationRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlOutcomeValidationRecord for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedOutcomeValidations.Count > 0 ? InMemoryTestDataSet.SimulatedOutcomeValidations.Max(v => v.ValidationRecordIdentifier) : 0; validationRecord = new MlOutcomeValidationRecord { ValidationRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, ValidationResultData = new byte[] { (byte)customerIdentifier, 0xBB } }; InMemoryTestDataSet.SimulatedOutcomeValidations.Add(validationRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlOutcomeValidationRecord record with Identifier {validationRecord.ValidationRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlOutcomeValidationRecord record found for Customer {customerIdentifier}"); }

                                // Create or find InitialOperationalStageData
                                var initialStageDataRecord = InMemoryTestDataSet.SimulatedInitialOperationalStages.FirstOrDefault(s => s.CustomerLinkIdentifier == customerIdentifier);
                                if (initialStageDataRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new InitialOperationalStageData record for Customer {customerIdentifier}"); var maxId = InMemoryTestDataSet.SimulatedInitialOperationalStages.Count > 0 ? InMemoryTestDataSet.SimulatedInitialOperationalStages.Max(s => s.StageIdentifier) : 0; initialStageDataRecord = new InitialOperationalStageData { StageIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, ProcessOperationalIdentifier = customerIdentifier + 7000, CustomerServiceOperationIdentifier = customerIdentifier + 6000, SalesProcessIdentifier = customerIdentifier + 5000, LinkedSubServiceA = 1, LinkedSubServiceB = 2, LinkedSubServiceC = 3, LinkedSubProductA = 1, LinkedSubProductB = 2, LinkedSubProductC = 3, StageSpecificData = $"Simulated Stage Data for Customer {customerIdentifier}", StageProductVectorSnapshot = $"Stage1_P_Simulated:{customerIdentifier}", StageServiceVectorSnapshot = $"Stage1_S_Simulated:{customerIdentifier}" }; InMemoryTestDataSet.SimulatedInitialOperationalStages.Add(initialStageDataRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created InitialOperationalStageData record with Identifier {initialStageDataRecord.StageIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing InitialOperationalStageData record found for Customer {customerIdentifier}"); }


                                // Store references to the core and dependency records in Runtime Processing Context.
                                // Operational Process Dependency: Writes to RuntimeProcessingContext.
                                // Subsequent Usage: Allows ParallelProcessingUnitA, ParallelProcessingUnitB, and SequentialFinalProcessingUnitD to potentially access these exact record instances easily without re-querying simulated persistence.
                                RuntimeProcessingContext.StoreContextValue("AssociatedCustomerContextRecord", associatedCustomer);
                                RuntimeProcessingContext.StoreContextValue("OperationalWorkOrderRecord", associatedWorkOrder);
                                RuntimeProcessingContext.StoreContextValue("MlInitialOperationEventRecord", operationalEventRecord);
                                RuntimeProcessingContext.StoreContextValue("MlOutcomeValidationRecord", validationRecord);
                                RuntimeProcessingContext.StoreContextValue("InitialOperationalStageDataRecord", initialStageDataRecord);
                                RuntimeProcessingContext.StoreContextValue("CurrentCoreOutcomeRecord", retrievedOrNewOutcomeRecord); // Store reference to the newly created record
                                RuntimeProcessingContext.StoreContextValue("CurrentCustomerIdentifier", customerIdentifier);

                                // Log verification of Runtime Processing Context contents.
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - AssociatedCustomerContext Identifier: {associatedCustomer?.ContextIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - OperationalWorkOrderRecord Identifier: {associatedWorkOrder?.OrderRecordIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - MlInitialOperationEventRecord Identifier: {operationalEventRecord?.EventIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - MlOutcomeValidationRecord Identifier: {validationRecord?.ValidationRecordIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - InitialOperationalStageDataRecord Identifier: {initialStageDataRecord?.StageIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - CurrentCoreOutcomeRecord Identifier: {retrievedOrNewOutcomeRecord?.RecordIdentifier}");


                                /// <summary>
                                /// Operational Step 4.2 (Simulated ML Training - NEW Record)
                                /// </summary>
                                // Simulate a machine learning training process using the mock ML components for the newly initialized record (Simulated Model C).
                                // Operational Process Dependency: Depends on the mock SimulatedMachineIntelligenceComponents library. Operates on simulated input derived from the new CoreMlOutcomeRecord.
                                // Output: Serialized simulated model data (simulated weights/biases) which is stored in retrievedOrNewOutcomeRecord.SerializedSimulatedModelData/AncillaryBinaryDataPayload and RuntimeProcessingContext.
                                // Subsequent Usage: The stored simulated model data is available for potential use by later processing stages or for persistence.
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Simulated Model C Training for NEW outcome record.");

                                int simulatedTrainingIterations = 100; // Training configuration parameter
                                float adjustmentStepMagnitude = 0.0001f; // Training configuration parameter

                                // Initialize simulated ML environment (Graph and Session).
                                // Operational Process Dependency: Calls methods in the mock SimulatedMlEngine library.
                                SimulatedMlEngine.compatibilityLayer.versionOne.disableImmediateExecutionMode(); // Simulate disabling eager execution
                                var currentGraph = SimulatedMlEngine.ComputationGraphBuilder(); // Simulate creating a graph
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Computation Graph created.");

                                // Create a simulated ML session within a 'using' block for automatic disposal within this scope.
                                // Operational Process Dependency: Uses the mock SimulatedMlSession class. This session is distinct from the ones passed to parallel units.
                                // Subsequent Usage: Used to run simulated operations and fetch tensor values for Model C.
                                using (var currentSimulationSession = new SimulatedMlSession())
                                {
                                    // Define input and output shapes for the simulated model based on the number of features derived from the core record.
                                    // Operational Process Dependency: Defines the structure of the simulated computation graph.
                                    var featureCount = 5; // Simulated features: RecordIdentifier, CustomerIdentifier, Timestamp, CategoricalIdentifier, CategoricalDescription (as bool/flag)
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated feature count: {featureCount}");

                                    // Simulate creating input and output placeholder nodes.
                                    // Operational Process Dependency: Uses SimulatedMlEngine.inputOutputPlaceholder. Creates SimulatedMlTensor objects.
                                    // Subsequent Usage: These SimulatedMlTensors are used when defining the model graph and are targets for feeding data via SimulatedMlInputFeedEntry during session runs.
                                    var inputPlaceholder = SimulatedMlEngine.inputOutputPlaceholder(SimulatedMlEngine.SimulatedFloatPrecisionType, new[] { -1, featureCount }, identifier: "Simulated_ML_Input_Features"); // Input placeholder
                                    var outputPlaceholder = SimulatedMlEngine.inputOutputPlaceholder(SimulatedMlEngine.SimulatedFloatPrecisionType, new[] { -1, 1 }, identifier: "Simulated_ML_Output_Target"); // Output placeholder
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Placeholders created: Input, Output.");

                                    // Simulate creating model variable nodes (weights and biases).
                                    // Operational Process Dependency: Uses SimulatedMlEngine.trainableParameterVariable and SimulatedMlEngine.randomDataSource/zeroFilledTensorCreator. Creates SimulatedMlTensor objects.
                                    // Subsequent Usage: These SimulatedMlTensors are part of the model graph and their values are updated during simulated training.
                                    var Simulated_ML_Weight_Variable = SimulatedMlEngine.trainableParameterVariable(SimulatedMlEngine.randomDataSource.NormalDistributionTensor(new[] { featureCount, 1 }, meanValue: 0.0f, standardDeviation: 0.01f), identifier: "Simulated_ML_Weight_Variable"); // Simulated weights variable
                                    var Simulated_ML_Bias_Variable = SimulatedMlEngine.trainableParameterVariable(SimulatedMlEngine.zeroFilledTensorCreator(new[] { 1 }), identifier: "Simulated_ML_Bias_Variable"); // Simulated bias variable
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Variables created: Weight, Bias.");

                                    // Define the simulated model's prediction operation using the graph builder.
                                    // Operational Process Dependency: Uses SimulatedMlEngine.matrixMultiplicationOperation and SimulatedMlEngine.tensorAdditionOperation. Creates a SimulatedMlTensor representing the output node.
                                    // Subsequent Usage: This prediction tensor is part of the simulated graph definition.
                                    var Simulated_ML_Predictions_Output = SimulatedMlEngine.tensorAdditionOperation(SimulatedMlEngine.matrixMultiplicationOperation(inputPlaceholder, Simulated_ML_Weight_Variable), Simulated_ML_Bias_Variable);
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Model graph defined (Predictions = MatMul(Input, Weight) + Bias).");

                                    // Define the simulated loss function and optimizer operation.
                                    // Operational Process Dependency: Uses a mock SimulatedMlTensor for 'loss' (representing the output of a loss calculation) and SimulatedMlEngine.trainingModule.SteepestDescentAdjustmentMechanism.
                                    // Subsequent Usage: 'trainingExecutionOperation' is the operation run during the simulated training loop.
                                    var Simulated_ML_Loss_Scalar = new SimulatedMlTensor("Simulated_ML_Loss_Scalar", new int[] { 1 }); // Simulated scalar loss tensor node
                                    var adjustmentMechanism = SimulatedMlEngine.trainingModule.SteepestDescentAdjustmentMechanism(adjustmentStepMagnitude); // Simulate optimizer creation
                                    var trainingExecutionOperation = adjustmentMechanism.MinimizeProcess(Simulated_ML_Loss_Scalar); // Simulate getting the training operation
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Loss function and Adjustment Mechanism defined (Steepest Descent with rate {adjustmentStepMagnitude}).");

                                    // Prepare simulated input data for training (using data from the new record).
                                    // Operational Process Dependency: Uses values from the retrievedOrNewOutcomeRecord.
                                    // Subsequent Usage: This data *would* be fed into the inputPlaceholder and outputPlaceholder via SimulatedMlInputFeedEntry during simulated training runs.
                                    var recordIdentifier = retrievedOrNewOutcomeRecord.RecordIdentifier;
                                    var recordCustomerIdentifier = retrievedOrNewOutcomeRecord.AssociatedCustomerIdentifier;
                                    var recordCreationTimestamp = retrievedOrNewOutcomeRecord.OutcomeGenerationTimestamp;
                                    var recordCategoricalIdentifier = retrievedOrNewOutcomeRecord.CategoricalClassificationIdentifier;
                                    var recordCategoricalName = retrievedOrNewOutcomeRecord.CategoricalClassificationDescription;
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Input data prepared from new record.");

                                    // Simulate initializing variables in the ML session.
                                    // Operational Process Dependency: Calls SimulatedMlSession.RunExecutionStep with the parameterInitializationOperation.
                                    // Subsequent Usage: Prepares the simulated variable tensors (Simulated_ML_Weight_Variable, Simulated_ML_Bias_Variable) with their initial values before simulated training starts.
                                    currentSimulationSession.RunExecutionStep(SimulatedMlEngine.parameterInitializationOperation());
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Global Parameters initialized.");

                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Starting Simulated Training Loop for {Math.Min(simulatedTrainingIterations, 20)} iterations.");

                                    // Simulate the training loop for a limited number of iterations.
                                    // Operational Process Dependency: Repeatedly calls SimulatedMlSession.RunExecutionStep for the training operation and SimulatedMlSession.RunTensorQuery to get the simulated loss.
                                    // Subsequent Usage: Simulates updating the model variable values (weights, bias) internally within the currentSimulationSession instance.
                                    float previousIterationLoss = float.MaxValue; // Track loss for simulated convergence check
                                    int stableIterationCount = 0; // Count stable iterations

                                    // Truncated simulation of training loop for processing time demonstration
                                    for (int iteration = 0; iteration < Math.Min(simulatedTrainingIterations, 20); iteration++) // Simulate training iterations
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Iteration {iteration + 1}/{Math.Min(simulatedTrainingIterations, 20)} starting...");

                                        // Simulate creating the feed dictionary for the current iteration.
                                        // Operational Process Dependency: Uses the inputPlaceholder and outputPlaceholder SimulatedMlTensors and simulated input/output data. Creates SimulatedMlInputFeedEntry list.
                                        // Subsequent Usage: Passed to SimulatedMlSession.RunExecutionStep/RunTensorQuery.
                                        var simulationInputData = new List<SimulatedMlInputFeedEntry>
                                        {
                                            new SimulatedMlInputFeedEntry(inputPlaceholder, new float[1, featureCount]), // Simulated input data array
                                            new SimulatedMlInputFeedEntry(outputPlaceholder, new float[1, 1]) // Simulated output data array
                                        };
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Input Feed created for iteration {iteration + 1}.");

                                        // Execute the simulated training operation for the current iteration.
                                        // Operational Process Dependency: Calls SimulatedMlSession.RunExecutionStep with the trainingExecutionOperation and simulationInputData.
                                        // Subsequent Usage: Simulates the process of updating model weights/biases.
                                        currentSimulationSession.RunExecutionStep(trainingExecutionOperation, simulationInputData);
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Training Execution operation executed for iteration {iteration + 1}.");

                                        // Get the simulated loss value for the current iteration.
                                        // Operational Process Dependency: Calls SimulatedMlSession.RunTensorQuery on the Simulated_ML_Loss_Scalar tensor with simulationInputData. Returns a float.
                                        // Subsequent Usage: Used for logging and potentially simulated convergence checking.
                                        var currentIterationLoss = Convert.ToSingle(currentSimulationSession.RunTensorQuery(Simulated_ML_Loss_Scalar, simulationInputData)); // Get simulated loss

                                        if (iteration % 5 == 0 || iteration == Math.Min(simulatedTrainingIterations, 20) - 1) // Log initial, every 5th, and final iteration loss
                                        {
                                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Iteration {iteration + 1}, Simulated Loss: {currentIterationLoss:E4}");
                                        }

                                        await Task.Delay(50); // Small delay to simulate computation time per iteration

                                        previousIterationLoss = currentIterationLoss; // Update previous loss for next iteration check
                                    }

                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Simulated Training completed after {Math.Min(simulatedTrainingIterations, 20)} iterations.");
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) - Final Simulated Loss: {previousIterationLoss:E4}");

                                    /// <summary>
                                    /// Operational Step 4.3 (Simulated Model Serialization and Storage - NEW Record)
                                    /// </summary>
                                    // Simulate serializing the trained model parameters (weights and biases) into a binary format.
                                    // Operational Process Dependency: Calls SimulatedMlSession.RunTensorQuery to fetch the final Simulated_ML_Weight_Variable and Simulated_ML_Bias_Variable values (returns SimulatedNdArray). Uses BinaryWriter/MemoryStream for serialization.
                                    // Output: A byte array containing the simulated model data.
                                    // Subsequent Usage: This byte array is stored in the retrievedOrNewOutcomeRecord.SerializedSimulatedModelData/AncillaryBinaryDataPayload properties and in RuntimeProcessingContext.
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Simulated Model C (NEW) parameter serialization.");
                                    using (var serializationStream = new MemoryStream()) // Stream for writing binary data
                                    using (var binaryDataWriter = new BinaryWriter(serializationStream)) // Writer for basic data types
                                    {
                                        // Get final weights from session and write to stream.
                                        var finalWeightValues = currentSimulationSession.RunTensorQuery(Simulated_ML_Weight_Variable); // Get simulated weights
                                        var weightArray = ((SimulatedNdArray)finalWeightValues).RetrieveAsArray<float>(); // Get raw data
                                        binaryDataWriter.Write(weightArray.Length); // Write size
                                        foreach (var w in weightArray) { binaryDataWriter.Write(w); } // Write each float
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated weights serialized successfully ({weightArray.Length} floats).");

                                        // Get final bias from session and write to stream.
                                        var finalBiasValues = currentSimulationSession.RunTensorQuery(Simulated_ML_Bias_Variable); // Get simulated bias
                                        var biasArray = ((SimulatedNdArray)finalBiasValues).RetrieveAsArray<float>(); // Get raw data
                                        binaryDataWriter.Write(biasArray.Length); // Write size
                                        foreach (var bias in biasArray) { binaryDataWriter.Write(bias); } // Write each float
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated bias serialized successfully ({biasArray.Length} floats).");

                                        // Store the combined serialized data in the CoreMlOutcomeRecord instance.
                                        // Operational Process Dependency: Updates the retrievedOrNewOutcomeRecord object's binary data properties.
                                        // Subsequent Usage: This data will be saved to InMemoryTestDataSet and stored in RuntimeProcessingContext.
                                        retrievedOrNewOutcomeRecord.SerializedSimulatedModelData = serializationStream.ToArray(); // Get the byte array from the stream
                                                                                                                                  // Optionally store bias data separately if needed, e.g., in AncillaryBinaryDataPayload
                                        retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = biasArray.SelectMany(BitConverter.GetBytes).ToArray(); // Example: Store bias separately as well
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated model parameters serialized to byte arrays (ModelData size: {retrievedOrNewOutcomeRecord.SerializedSimulatedModelData.Length}, AncillaryData size: {retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload.Length}).");
                                    }
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated parameter serialization completed.");

                                    // Update the record in the simulated database with the new model data.
                                    // Operational Process Dependency: Modifies the specific entry in InMemoryTestDataSet.SimulatedCoreOutcomes.
                                    // Subsequent Usage: Makes the trained simulated model data persistent (in simulation) for later retrieval or use in future workflows for this customer.
                                    var recordIndex = InMemoryTestDataSet.SimulatedCoreOutcomes.FindIndex(r => r.RecordIdentifier == retrievedOrNewOutcomeRecord.RecordIdentifier);
                                    if (recordIndex >= 0)
                                    {
                                        InMemoryTestDataSet.SimulatedCoreOutcomes[recordIndex] = retrievedOrNewOutcomeRecord; // Update the static list entry with the modified object
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated parameter data saved successfully in simulated persistent storage.");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error - CoreMlOutcomeRecord with Identifier {retrievedOrNewOutcomeRecord.RecordIdentifier} not found in simulated storage after creation attempt!");
                                        // This indicates an issue with the list manipulation logic above.
                                    }


                                    // Store the serialized simulated model data in Runtime Processing Context.
                                    // Operational Process Dependency: Writes to RuntimeProcessingContext.
                                    // Subsequent Usage: Makes the simulated model data easily accessible to other processing units (A, B, D) *within the same request* without needing to re-query simulated persistence.
                                    RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_SerializedModelData", retrievedOrNewOutcomeRecord.SerializedSimulatedModelData);
                                    RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_AncillaryData", retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload); // Store ancillary data too
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) simulated model parameter data stored in Runtime Processing Context.");


                                    // Verification logs for Runtime Processing Context contents.
                                    var contextCustomerIdentifier = RuntimeProcessingContext.RetrieveContextValue("CurrentCustomerIdentifier");
                                    var contextProcessOneData = RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_SerializedModelData") as byte[];
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - Customer Identifier: {contextCustomerIdentifier}");
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - Serialized Model Data Size: {contextProcessOneData?.Length ?? 0} bytes");
                                }
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (NEW) Simulated ML Session Environment disposed.");

                }
                else // Existing record found
                {
                            // --- If model found in Database ---

                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing CoreMlOutcomeRecord found for Customer Identifier {customerIdentifier} (ID: {retrievedOrNewOutcomeRecord.RecordIdentifier}).");

                            // --- Operations for EXISTING Core Record Processing ---

                            // Store existing model data and relevant identifiers in Runtime Processing Context.
                            // Operational Process Dependency: Reads from the found retrievedOrNewOutcomeRecord object and writes to RuntimeProcessingContext.
                            // Subsequent Usage: Makes existing data easily accessible to other processing units within the request.
                            RuntimeProcessingContext.StoreContextValue("CurrentCustomerIdentifier", retrievedOrNewOutcomeRecord.AssociatedCustomerIdentifier);
                            RuntimeProcessingContext.StoreContextValue("OutcomeGenerationTimestamp", retrievedOrNewOutcomeRecord.OutcomeGenerationTimestamp); // Store relevant fields
                            RuntimeProcessingContext.StoreContextValue("CoreOutcomeRecordIdentifier", retrievedOrNewOutcomeRecord.RecordIdentifier); // Store relevant fields
                            RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_SerializedModelData", retrievedOrNewOutcomeRecord.SerializedSimulatedModelData); // Store existing binary data
                            RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_AncillaryData", retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload); // Store existing ancillary data too
                            RuntimeProcessingContext.StoreContextValue("CurrentCoreOutcomeRecord", retrievedOrNewOutcomeRecord); // Store reference to the existing record
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing CoreMlOutcomeRecord data loaded into Runtime Processing Context.");


                            // Retrieve or create associated dependency records (AssociatedCustomerContext, OperationalWorkOrderRecord, MlInitialOperationEvent, MlOutcomeValidationRecord, InitialOperationalStageData).
                            // Operational Process Dependency: Checks and potentially modifies static lists in InMemoryTestDataSet. Linked by CustomerLinkIdentifier.
                            // Subsequent Usage: Same as for the new record case; references are stored in RuntimeProcessingContext.

                            // Create or find AssociatedCustomerContext
                            var associatedCustomer = InMemoryTestDataSet.SimulatedCustomerContexts.FirstOrDefault(c => c.CustomerLinkIdentifier == customerIdentifier);
                            if (associatedCustomer == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new AssociatedCustomerContext record for Customer {customerIdentifier} (existing Core record)"); var maxId = InMemoryTestDataSet.SimulatedCustomerContexts.Count > 0 ? InMemoryTestDataSet.SimulatedCustomerContexts.Max(c => c.ContextIdentifier) : 0; associatedCustomer = new AssociatedCustomerContext { ContextIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, CustomerPrimaryGivenName = $"Simulated FN {customerIdentifier}", CustomerFamilyName = $"Simulated LN {customerIdentifier}", CustomerContactPhoneNumber = $"555-cust-{customerIdentifier}", CustomerStreetAddress = $"123 Main St Sim {customerIdentifier}", AffiliatedCompanyName = $"Simulated Company {customerIdentifier}" }; InMemoryTestDataSet.SimulatedCustomerContexts.Add(associatedCustomer); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created AssociatedCustomerContext record with Identifier {associatedCustomer.ContextIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing AssociatedCustomerContext record found for Customer {customerIdentifier}"); }

                            // Create or find OperationalWorkOrderRecord
                            var associatedWorkOrder = InMemoryTestDataSet.SimulatedWorkOrders.FirstOrDefault(o => o.CustomerLinkIdentifier == customerIdentifier);
                            if (associatedWorkOrder == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new OperationalWorkOrderRecord for Customer {customerIdentifier} (existing Core record)"); var maxId = InMemoryTestDataSet.SimulatedWorkOrders.Count > 0 ? InMemoryTestDataSet.SimulatedWorkOrders.Max(o => o.OrderRecordIdentifier) : 0; associatedWorkOrder = new OperationalWorkOrderRecord { OrderRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, SpecificOrderIdentifier = customerIdentifier + 9000 }; InMemoryTestDataSet.SimulatedWorkOrders.Add(associatedWorkOrder); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created OperationalWorkOrderRecord with Identifier {associatedWorkOrder.OrderRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing OperationalWorkOrderRecord found for Customer {customerIdentifier}"); }

                            // Create or find MlInitialOperationEvent
                            var operationalEventRecord = InMemoryTestDataSet.SimulatedOperationalEvents.FirstOrDefault(e => e.CustomerLinkIdentifier == customerIdentifier);
                            if (operationalEventRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlInitialOperationEvent record for Customer {customerIdentifier} (existing Core record)"); var maxId = InMemoryTestDataSet.SimulatedOperationalEvents.Count > 0 ? InMemoryTestDataSet.SimulatedOperationalEvents.Max(e => e.EventIdentifier) : 0; operationalEventRecord = new MlInitialOperationEvent { EventIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, EventPayloadData = new byte[] { (byte)customerIdentifier, 0xAC } }; InMemoryTestDataSet.SimulatedOperationalEvents.Add(operationalEventRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlInitialOperationEvent record with Identifier {operationalEventRecord.EventIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlInitialOperationEvent record found for Customer {customerIdentifier}"); }

                            // Create or find MlOutcomeValidationRecord
                            var validationRecord = InMemoryTestDataSet.SimulatedOutcomeValidations.FirstOrDefault(v => v.CustomerLinkIdentifier == customerIdentifier);
                            if (validationRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new MlOutcomeValidationRecord for Customer {customerIdentifier} (existing Core record)"); var maxId = InMemoryTestDataSet.SimulatedOutcomeValidations.Count > 0 ? InMemoryTestDataSet.SimulatedOutcomeValidations.Max(v => v.ValidationRecordIdentifier) : 0; validationRecord = new MlOutcomeValidationRecord { ValidationRecordIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, ValidationResultData = new byte[] { (byte)customerIdentifier, 0xBC } }; InMemoryTestDataSet.SimulatedOutcomeValidations.Add(validationRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created MlOutcomeValidationRecord record with Identifier {validationRecord.ValidationRecordIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing MlOutcomeValidationRecord record found for Customer {customerIdentifier}"); }

                            // Create or find InitialOperationalStageData
                            var initialStageDataRecord = InMemoryTestDataSet.SimulatedInitialOperationalStages.FirstOrDefault(s => s.CustomerLinkIdentifier == customerIdentifier);
                            if (initialStageDataRecord == null) { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Creating new InitialOperationalStageData record for Customer {customerIdentifier} (existing Core record)"); var maxId = InMemoryTestDataSet.SimulatedInitialOperationalStages.Count > 0 ? InMemoryTestDataSet.SimulatedInitialOperationalStages.Max(s => s.StageIdentifier) : 0; initialStageDataRecord = new InitialOperationalStageData { StageIdentifier = maxId + 1, CustomerLinkIdentifier = customerIdentifier, RelatedOrderIdentifier = customerIdentifier + 9000, InternalOperationIdentifier = customerIdentifier + 8000, ProcessOperationalIdentifier = customerIdentifier + 7000, CustomerServiceOperationIdentifier = customerIdentifier + 6000, SalesProcessIdentifier = customerIdentifier + 5000, LinkedSubServiceA = 1, LinkedSubServiceB = 2, LinkedSubServiceC = 3, LinkedSubProductA = 1, LinkedSubProductB = 2, LinkedSubProductC = 3, StageSpecificData = $"Simulated Stage Data for Customer {customerIdentifier}", StageProductVectorSnapshot = $"Stage1_P_Simulated:{customerIdentifier}", StageServiceVectorSnapshot = $"Stage1_S_Simulated:{customerIdentifier}" }; InMemoryTestDataSet.SimulatedInitialOperationalStages.Add(initialStageDataRecord); System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Created InitialOperationalStageData record with Identifier {initialStageDataRecord.StageIdentifier}"); } else { System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Existing InitialOperationalStageData record found for Customer {customerIdentifier}"); }


                            // Store references to the dependency records in Runtime Processing Context.
                            // Operational Process Dependency: Writes to RuntimeProcessingContext.
                            // Subsequent Usage: Allows other processing units to potentially access these exact record instances easily.
                            RuntimeProcessingContext.StoreContextValue("AssociatedCustomerContextRecord", associatedCustomer);
                            RuntimeProcessingContext.StoreContextValue("OperationalWorkOrderRecord", associatedWorkOrder);
                            RuntimeProcessingContext.StoreContextValue("MlInitialOperationEventRecord", operationalEventRecord);
                            RuntimeProcessingContext.StoreContextValue("MlOutcomeValidationRecord", validationRecord);
                            RuntimeProcessingContext.StoreContextValue("InitialOperationalStageDataRecord", initialStageDataRecord);


                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Associated dependency records (CustomerContext, WorkOrder, OperationEvent, OutcomeValidation, InitialStageData) processed for existing core record.");


                            // Log verification of Runtime Processing Context contents.
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - Customer Identifier: {customerIdentifier}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - AssociatedCustomerContext Identifier: {associatedCustomer?.ContextIdentifier}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - OperationalWorkOrderRecord Identifier: {associatedWorkOrder?.OrderRecordIdentifier}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - MlInitialOperationEventRecord Identifier: {operationalEventRecord?.EventIdentifier}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - MlOutcomeValidationRecord Identifier: {validationRecord?.ValidationRecordIdentifier}");
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - InitialOperationalStageDataRecord Identifier: {initialStageDataRecord?.StageIdentifier}");


                            /// <summary>
                            /// Operational Step 4.2 (Simulated ML Training - EXISTING Record)
                            /// </summary>
                            // Simulate a machine learning re-training or inference process for the existing record (Simulated Model C).
                            // Operational Process Dependency: Depends on the mock SimulatedMachineIntelligenceComponents library. Operates on simulated input derived from the existing CoreMlOutcomeRecord.
                            // In a real scenario, this would load the existing simulated model data (retrievedOrNewOutcomeRecord.SerializedSimulatedModelData/AncillaryBinaryDataPayload) and restore variables before training/inference.
                            // Output: Updated serialized simulated model data which is stored back in the record and RuntimeProcessingContext.
                            // Subsequent Usage: The updated simulated model data is available for potential use by later processing stages or for persistence.
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Simulated Model C Training for EXISTING outcome record.");

                            int simulatedTrainingIterations = 100; // Training configuration parameter
                            float adjustmentStepMagnitude = 0.0001f; // Training configuration parameter

                            // Initialize simulated ML environment (Graph and Session).
                            // Operational Process Dependency: Calls methods in the mock SimulatedMlEngine library.
                            SimulatedMlEngine.compatibilityLayer.versionOne.disableImmediateExecutionMode(); // Simulate disabling eager execution
                            var currentGraph = SimulatedMlEngine.ComputationGraphBuilder(); // Simulate creating a graph
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Computation Graph created.");

                            // Create a simulated ML session within a 'using' block for automatic disposal within this scope.
                            // Operational Process Dependency: Uses the mock SimulatedMlSession class.
                            // Subsequent Usage: Used to run simulated operations and fetch tensor values for Model C.
                            using (var currentSimulationSession = new SimulatedMlSession())
                            {
                                // Define input and output shapes for the simulated model.
                                // Operational Process Dependency: Defines the structure of the simulated computation graph.
                                var featureCount = 5; // Simulated features: Identifier, CustomerIdentifier, Timestamp, CategoricalIdentifier, CategoricalDescription (as bool/flag)
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated feature count: {featureCount}");
                                // Simulate creating input and output placeholder nodes.
                                // Operational Process Dependency: Uses SimulatedMlEngine.inputOutputPlaceholder. Creates SimulatedMlTensor objects.
                                // Subsequent Usage: Used when defining the model graph and for feeding data.
                                var inputPlaceholder = SimulatedMlEngine.inputOutputPlaceholder(SimulatedMlEngine.SimulatedFloatPrecisionType, new[] { -1, featureCount }, identifier: "Simulated_ML_Input_Features");
                                var outputPlaceholder = SimulatedMlEngine.inputOutputPlaceholder(SimulatedMlEngine.SimulatedFloatPrecisionType, new[] { -1, 1 }, identifier: "Simulated_ML_Output_Target");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Placeholders created: Input, Output.");

                                // Simulate creating model variable nodes (weights and biases).
                                // Operational Process Dependency: Uses SimulatedMlEngine.trainableParameterVariable.
                                // In a real scenario, you'd load the existing serialized model data here and restore variables instead of random initialization.
                                // Subsequent Usage: These SimulatedMlTensors are part of the graph and their values are potentially updated during training.
                                var Simulated_ML_Weight_Variable = SimulatedMlEngine.trainableParameterVariable(SimulatedMlEngine.randomDataSource.NormalDistributionTensor(new[] { featureCount, 1 }, meanValue: 0.0f, standardDeviation: 0.01f), identifier: "Simulated_ML_Weight_Variable"); // Placeholder for loading weights
                                var Simulated_ML_Bias_Variable = SimulatedMlEngine.trainableParameterVariable(SimulatedMlEngine.zeroFilledTensorCreator(new[] { 1 }), identifier: "Simulated_ML_Bias_Variable"); // Placeholder for loading bias
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Variables created (placeholders for loading). Loading existing parameters would happen here in a real implementation.");


                                // Define the simulated model's prediction operation.
                                // Operational Process Dependency: Uses SimulatedMlEngine.matrixMultiplicationOperation and SimulatedMlEngine.tensorAdditionOperation. Creates a SimulatedMlTensor.
                                // Subsequent Usage: This prediction tensor is part of the simulated graph definition.
                                var Simulated_ML_Predictions_Output = SimulatedMlEngine.tensorAdditionOperation(SimulatedMlEngine.matrixMultiplicationOperation(inputPlaceholder, Simulated_ML_Weight_Variable), Simulated_ML_Bias_Variable);
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Model graph defined.");


                                // Define the simulated loss function and optimizer.
                                // Operational Process Dependency: Uses a mock SimulatedMlTensor for 'loss' and SimulatedMlEngine.trainingModule.SteepestDescentAdjustmentMechanism.
                                // Subsequent Usage: 'trainingExecutionOperation' is the operation run during the simulated training loop.
                                var Simulated_ML_Loss_Scalar = new SimulatedMlTensor("Simulated_ML_Loss_Scalar", new int[] { 1 }); // Simulated loss scalar tensor
                                var adjustmentMechanism = SimulatedMlEngine.trainingModule.SteepestDescentAdjustmentMechanism(adjustmentStepMagnitude); // Simulate optimizer creation
                                var trainingExecutionOperation = adjustmentMechanism.MinimizeProcess(Simulated_ML_Loss_Scalar); // Simulate getting the training operation
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Loss function and Adjustment Mechanism defined.");


                                // Prepare simulated input data.
                                // Operational Process Dependency: Uses values from the retrievedOrNewOutcomeRecord.
                                // Subsequent Usage: This data *would* be fed into the inputPlaceholder and outputPlaceholder.
                                var recordIdentifier = retrievedOrNewOutcomeRecord.RecordIdentifier;
                                var recordCustomerIdentifier = retrievedOrNewOutcomeRecord.AssociatedCustomerIdentifier;
                                var recordCreationTimestamp = retrievedOrNewOutcomeRecord.OutcomeGenerationTimestamp;
                                var recordCategoricalIdentifier = retrievedOrNewOutcomeRecord.CategoricalClassificationIdentifier;
                                var recordCategoricalName = retrievedOrNewOutcomeRecord.CategoricalClassificationDescription;
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Input data prepared from existing record.");


                                // Simulate initializing variables or restoring them from saved data.
                                // Operational Process Dependency: Calls SimulatedMlSession.RunExecutionStep (simulating initialization or restore).
                                // Subsequent Usage: Prepares variables with either initial or loaded values before training.
                                currentSimulationSession.RunExecutionStep(SimulatedMlEngine.parameterInitializationOperation()); // This would simulate a restore operation using the loaded data in real TF
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Global Parameters initialized (simulated). Parameter restore from loaded data would happen here in real implementation.");


                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Starting Simulated Training Loop for {Math.Min(simulatedTrainingIterations, 20)} iterations.");


                                // Simulate the training loop.
                                // Operational Process Dependency: Repeatedly calls SimulatedMlSession.RunExecutionStep and SimulatedMlSession.RunTensorQuery.
                                // Subsequent Usage: Simulates updating the model variable values (weights, bias) internally.
                                float previousIterationLoss = float.MaxValue; // Track loss for simulated convergence check
                                int stableIterationCount = 0; // Count stable iterations

                                // Truncated simulation for brevity
                                for (int iteration = 0; iteration < Math.Min(simulatedTrainingIterations, 20); iteration++)
                                {
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Iteration {iteration + 1}/{Math.Min(simulatedTrainingIterations, 20)} starting...");

                                    // Simulate creating the feed dictionary.
                                    // Operational Process Dependency: Uses placeholders and simulated data.
                                    // Subsequent Usage: Passed to SimulatedMlSession.RunExecutionStep/RunTensorQuery.
                                    var simulationInputData = new List<SimulatedMlInputFeedEntry>
                                    {
                                        new SimulatedMlInputFeedEntry(inputPlaceholder, new float[1, featureCount]),
                                        new SimulatedMlInputFeedEntry(outputPlaceholder, new float[1, 1])
                                    };
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Input Feed created for iteration {iteration + 1}.");


                                    // Execute the simulated training operation.
                                    // Operational Process Dependency: Calls SimulatedMlSession.RunExecutionStep.
                                    // Subsequent Usage: Simulates updating model weights/biases.
                                    currentSimulationSession.RunExecutionStep(trainingExecutionOperation, simulationInputData);
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Training Execution operation executed for iteration {iteration + 1}.");


                                    // Get the simulated loss.
                                    // Operational Process Dependency: Calls SimulatedMlSession.RunTensorQuery. Returns a float.
                                    // Subsequent Usage: Used for logging.
                                    var currentIterationLoss = Convert.ToSingle(currentSimulationSession.RunTensorQuery(Simulated_ML_Loss_Scalar, simulationInputData)); // Get simulated loss

                                    if (iteration % 5 == 0 || iteration == Math.Min(simulatedTrainingIterations, 20) - 1) // Log initial, every 5th, and final iteration
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Iteration {iteration + 1}, Simulated Loss: {currentIterationLoss:E4}");
                                    }

                                    await Task.Delay(50); // Simulate computation time

                                    previousIterationLoss = currentIterationLoss; // Update previous loss
                                }

                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Simulated Training completed after {Math.Min(simulatedTrainingIterations, 20)} iterations.");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) - Final Simulated Loss: {previousIterationLoss:E4}");


                                /// <summary>
                                /// Operational Step 4.3 (Simulated Model Serialization and Storage - EXISTING Record)
                                /// </summary>
                                // Simulate serializing the re-trained or inferred model parameters.
                                // Operational Process Dependency: Calls SimulatedMlSession.RunTensorQuery to fetch updated Simulated_ML_Weight_Variable and Simulated_ML_Bias_Variable values. Uses BinaryWriter/MemoryStream.
                                // Output: A byte array with the updated simulated model data.
                                // Subsequent Usage: This byte array overwrites the existing data in retrievedOrNewOutcomeRecord.SerializedSimulatedModelData/AncillaryBinaryDataPayload and is stored in RuntimeProcessingContext.
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Simulated Model C (EXISTING) parameter serialization.");
                                using (var serializationStream = new MemoryStream()) // Stream for writing binary data
                                using (var binaryDataWriter = new BinaryWriter(serializationStream)) // Writer for basic data types
                                {
                                    // Get updated weights from session and write to stream.
                                    var finalWeightValues = currentSimulationSession.RunTensorQuery(Simulated_ML_Weight_Variable); // Get simulated weights
                                    var weightArray = ((SimulatedNdArray)finalWeightValues).RetrieveAsArray<float>(); // Get raw data
                                    binaryDataWriter.Write(weightArray.Length); // Write size
                                    foreach (var w in weightArray) { binaryDataWriter.Write(w); } // Write each float
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated weights serialized successfully ({weightArray.Length} floats).");

                                    // Get updated bias from session and write to stream.
                                    var finalBiasValues = currentSimulationSession.RunTensorQuery(Simulated_ML_Bias_Variable); // Get simulated bias
                                    var biasArray = ((SimulatedNdArray)finalBiasValues).RetrieveAsArray<float>(); // Get raw data
                                    binaryDataWriter.Write(biasArray.Length); // Write size
                                    foreach (var bias in biasArray) { binaryDataWriter.Write(bias); } // Write each float
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated bias serialized successfully ({biasArray.Length} floats).");

                                    // Store the combined serialized data in the CoreMlOutcomeRecord instance.
                                    // Operational Process Dependency: Updates the retrievedOrNewOutcomeRecord object's binary data properties.
                                    // Subsequent Usage: This data will be saved to InMemoryTestDataSet and stored in RuntimeProcessingContext.
                                    retrievedOrNewOutcomeRecord.SerializedSimulatedModelData = serializationStream.ToArray(); // Get the byte array from the stream
                                    // Optionally store bias data separately if needed
                                    retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload = biasArray.SelectMany(BitConverter.GetBytes).ToArray(); // Example: Store bias separately

                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated model parameters serialized to byte arrays (ModelData size: {retrievedOrNewOutcomeRecord.SerializedSimulatedModelData.Length}, AncillaryData size: {retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload.Length}).");
                                }
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated parameter serialization completed.");

                                // Update the record in the simulated database with the updated model data.
                                // Operational Process Dependency: Modifies the specific entry in InMemoryTestDataSet.SimulatedCoreOutcomes.
                                // Subsequent Usage: Makes the re-trained simulated model data persistent (in simulation).
                                var recordIndex = InMemoryTestDataSet.SimulatedCoreOutcomes.FindIndex(r => r.RecordIdentifier == retrievedOrNewOutcomeRecord.RecordIdentifier);
                                if (recordIndex >= 0)
                                {
                                    InMemoryTestDataSet.SimulatedCoreOutcomes[recordIndex] = retrievedOrNewOutcomeRecord; // Update the static list entry
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated parameter data saved successfully in simulated persistent storage.");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error - CoreMlOutcomeRecord with Identifier {retrievedOrNewOutcomeRecord.RecordIdentifier} not found in simulated storage after update attempt!");
                                    // This indicates an issue with the list manipulation logic above.
                                }


                                // Store the updated serialized simulated model data in Runtime Processing Context.
                                // Operational Process Dependency: Writes to RuntimeProcessingContext.
                                // Subsequent Usage: Makes the updated simulated model data easily accessible to other processing units *within the same request*.
                                RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_SerializedModelData", retrievedOrNewOutcomeRecord.SerializedSimulatedModelData);
                                RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_AncillaryData", retrievedOrNewOutcomeRecord.AncillaryBinaryDataPayload); // Store ancillary data too
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) simulated model parameter data stored in Runtime Processing Context.");


                                // Verification logs for Runtime Processing Context contents.
                                var contextCustomerIdentifier = RuntimeProcessingContext.RetrieveContextValue("CurrentCustomerIdentifier");
                                var contextProcessOneData = RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_SerializedModelData") as byte[];
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Verification (RuntimeContext) - Customer Identifier: {contextCustomerIdentifier}");
                                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Verification (RuntimeContext) - Serialized Model Data Size: {contextProcessOneData?.Length ?? 0} bytes");
                            }
                            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Model C (EXISTING) Simulated ML Session Environment disposed.");
                        }
            }
            /// <summary>
            /// Operational Step 4.4 (Error Handling and Unit Cleanup)
            /// </summary>
            // Catch any exceptions thrown during SequentialInitialProcessingUnitC's execution.
            // Operational Process Dependency: Catches errors from simulated data access (InMemoryTestDataSet, RuntimeProcessingContext) or ML simulation (mock ML components).
            // Subsequent Usage: Logs the error and re-throws it so the main InitiateMlOutcomeGeneration method can catch and handle it (e.g., return a server error).
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error in Sequential Initial Processing Unit C: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Stack Trace: {ex.StackTrace}");
                throw; // Re-throw the exception
            }
            finally
            {
                // Set the runtime memory flag back to false indicating the process unit is complete.
                // Operational Process Dependency: Writes to RuntimeProcessingContext.
                // Subsequent Usage: Could be checked externally for monitoring. Marks the logical end of this specific processing unit's execution within the request workflow.
                RuntimeProcessingContext.StoreContextValue("SequentialProcessingUnitC_ActiveStatus", false);
                bool isActiveAfterExecution = (bool)RuntimeProcessingContext.RetrieveContextValue("SequentialProcessingUnitC_ActiveStatus");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialProcessingUnitC ActiveStatus property value after execution: {isActiveAfterExecution}");
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Initial Processing Unit C (Simulated Model C) finished.");
            }
        }

        /// <summary>
        /// Processes data simulating Model A (ParallelProcessingUnitA).
        /// This method is designed to run in parallel with ParallelProcessingUnitB (Simulated Model B).
        /// It simulates a separate processing task that operates on the core outcome record data and stores its results in a shared thread-safe dictionary.
        /// </summary>
        /// <param name="outcomeRecord">The core CoreMlOutcomeRecord object established by SequentialInitialProcessingUnitC.</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        /// <param name="mlSimulationSession">A dedicated Simulated ML Session environment for this parallel task.</param>
        /// <param name="unitResultsStore">A thread-safe dictionary to store results for SequentialFinalProcessingUnitD.</param>
        private async Task ParallelProcessingUnitA(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier, SimulatedMlSession mlSimulationSession, ConcurrentDictionary<string, object> unitResultsStore)
        {
            // This method simulates one of the parallel processing branches (Simulated Model A).
            // Operational Process Dependency: Called by InitiateMlOutcomeGeneration (Step 5) as part of Task.WhenAll.
            // Depends on the 'outcomeRecord' object (established by Step 4), the provided 'mlSimulationSession', and the 'unitResultsStore' dictionary.
            // Subsequent Usage: Its output (stored in 'unitResultsStore') is a dependency for SequentialFinalProcessingUnitD (Step 6). Runs concurrently with ParallelProcessingUnitB.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Parallel Processing Unit A (Simulated Model A) for customer {customerIdentifier}.");

            try
            {
                /// <summary>
                /// Operational Step 5.1 (Simulated Parallel Processing - Model A)
                /// </summary>
                // Placeholder logic for Simulated Model A processing.
                // In a real scenario, this would involve:
                // - Loading relevant data (potentially using the 'outcomeRecord' object, accessing dependency records via RuntimeProcessingContext, or querying InMemoryTestDataSet).
                // - Performing ML operations using the 'mlSimulationSession'.
                // - Computing results relevant to Simulated Model A.
                // Operational Process Dependency: Depends on input parameters (outcomeRecord, mlSimulationSession). May implicitly depend on data in RuntimeProcessingContext or InMemoryTestDataSet.
                // Subsequent Usage: The results computed here are stored in 'unitResultsStore' for SequentialFinalProcessingUnitD.
                await Task.Delay(200); // Simulate work duration for Unit A

                // Store some placeholder result in the shared results dictionary.
                // Operational Process Dependency: Writes to the 'unitResultsStore' dictionary.
                // Subsequent Usage: SequentialFinalProcessingUnitD reads from this dictionary.
                unitResultsStore["ModelAProcessingOutcome"] = "Simulated Model A processed successfully.";
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit A (Simulated Model A) completed its simulated processing.");
            }
            /// <summary>
            /// Operational Step 5.2 (Error Handling - Model A)
            /// </summary>
            // Catch any exceptions during Simulated Model A processing.
            // Operational Process Dependency: Catches errors specific to this unit's simulated logic.
            // Subsequent Usage: Logs the error and stores an error indicator/message in the results dictionary. Re-throws so Task.WhenAll in the main method can detect the failure.
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Error in Parallel Processing Unit A: {ex.Message}");
                unitResultsStore["ModelAProcessingError"] = ex.Message; // Store error state in results
                throw; // Re-throw to be caught by Task.WhenAll in the orchestrator
            }
            finally
            {
                // Log the finish of the process unit.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit A (Simulated Model A) finished.");
            }
        }

        /// <summary>
        /// Processes data simulating Model B (ParallelProcessingUnitB).
        /// This method is designed to run in parallel with ParallelProcessingUnitA (Simulated Model A).
        /// It simulates another separate processing task that operates on the core outcome record data and stores its results in a shared thread-safe dictionary.
        /// </summary>
        /// <param name="outcomeRecord">The core CoreMlOutcomeRecord object established by SequentialInitialProcessingUnitC.</param>
        /// <param name="customerIdentifier">The customer identifier.</param>
        /// <param name="requestSequenceIdentifier">The request session identifier.</param>
        /// <param name="mlSimulationSession">A dedicated Simulated ML Session environment for this parallel task.</param>
        /// <param name="unitResultsStore">A thread-safe dictionary to store results for SequentialFinalProcessingUnitD.</param>
        private async Task ParallelProcessingUnitB(CoreMlOutcomeRecord outcomeRecord, int customerIdentifier, int requestSequenceIdentifier, SimulatedMlSession mlSimulationSession, ConcurrentDictionary<string, object> unitResultsStore)
        {
            // This method simulates the other parallel processing branch (Simulated Model B).
            // Operational Process Dependency: Called by InitiateMlOutcomeGeneration (Step 5) as part of Task.WhenAll.
            // Depends on the 'outcomeRecord' object (established by Step 4), the provided 'mlSimulationSession', and the 'unitResultsStore' dictionary.
            // Subsequent Usage: Its output (stored in 'unitResultsStore') is a dependency for SequentialFinalProcessingUnitD (Step 6). Runs concurrently with ParallelProcessingUnitA.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Parallel Processing Unit B (Simulated Model B) for customer {customerIdentifier}.");

            try
            {
                /// <summary>
                /// Operational Step 5.3 (Simulated Parallel Processing - Model B)
                /// </summary>
                // Placeholder logic for Simulated Model B processing.
                // Similar to ParallelProcessingUnitA, this would load data, perform ML using 'mlSimulationSession', and compute results.
                // Operational Process Dependency: Depends on input parameters (outcomeRecord, mlSimulationSession). May implicitly depend on data in RuntimeProcessingContext or InMemoryTestDataSet.
                // Subsequent Usage: The results computed here are stored in 'unitResultsStore' for SequentialFinalProcessingUnitD.
                await Task.Delay(300); // Simulate work duration for Unit B (slightly longer)

                // Store some placeholder result in the shared results dictionary.
                // Operational Process Dependency: Writes to the 'unitResultsStore' dictionary.
                // Subsequent Usage: SequentialFinalProcessingUnitD reads from this dictionary.
                unitResultsStore["ModelBProcessingOutcome"] = "Simulated Model B processed successfully.";
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit B (Simulated Model B) completed its simulated processing.");
            }
            /// <summary>
            /// Operational Step 5.4 (Error Handling - Model B)
            /// </summary>
            // Catch any exceptions during Simulated Model B processing.
            // Operational Process Dependency: Catches errors specific to this unit's simulated logic.
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
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Parallel Processing Unit B (Simulated Model B) finished.");
            }
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
            // This method handles the final processing steps (Simulated Model D).
            // Operational Process Dependency: Called by InitiateMlOutcomeGeneration (Step 6) after Task.WhenAll completes.
            // Depends on the 'outcomeRecord' object (established/updated by SequentialInitialProcessingUnitC).
            // Crucially depends on 'unitAResults' and 'unitBResults' containing the outputs of the parallel tasks (Step 5).
            // Depends on InMemoryTestDataSet for saving the final state of the CoreMlOutcomeRecord.
            // Subsequent Usage: Finalizes the state of the CoreMlOutcomeRecord before it's retrieved and returned by the main controller method.
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Starting Sequential Final Processing Unit D (Simulated Model D) for customer {customerIdentifier}.");

            try
            {
                /// <summary>
                /// Operational Step 6.1 (Simulated Final Processing - Model D)
                /// </summary>
                // Placeholder logic for Simulated Model D processing.
                // In a real scenario, this would:
                // - Read and interpret the results stored in unitAResults and unitBResults.
                // - Perform calculations or aggregation based on those results.
                // - Update the main 'outcomeRecord' object, potentially setting final vector values or other properties.
                // Operational Process Dependency: Reads from 'unitAResults' and 'unitBResults'. Operates on the 'outcomeRecord' object reference.
                // Subsequent Usage: The updated 'outcomeRecord' object is saved to the simulated database.
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Checking outcomes from parallel processing units...");

                // Log results or errors from parallel tasks.
                if (unitAResults.TryGetValue("ModelAProcessingOutcome", out var unitAResultValue))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received outcome from Unit A: {unitAResultValue}");
                }
                else if (unitAResults.TryGetValue("ModelAProcessingError", out var unitAErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Error from Unit A: {unitAErrorMessage}");
                    // Handle error - potentially update outcomeRecord status, log warning etc.
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: No specific outcome or error found for Unit A.");
                }


                if (unitBResults.TryGetValue("ModelBProcessingOutcome", out var unitBResultValue))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received outcome from Unit B: {unitBResultValue}");
                }
                else if (unitBResults.TryGetValue("ModelBProcessingError", out var unitBErrorMessage))
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: Received Error from Unit B: {unitBErrorMessage}");
                    // Handle error - potentially update outcomeRecord status, log warning etc.
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD: No specific outcome or error found for Unit B.");
                }

                await Task.Delay(100); // Simulate finalization work duration

                // Update the main core outcome record object based on combined simulated results (placeholder logic).
                // Operational Process Dependency: Modifies the 'outcomeRecord' object instance that was passed in.
                // Subsequent Usage: This modified object is then saved to the simulated database in the next step.
                outcomeRecord.DerivedProductFeatureVector = $"Final_P_Combined_{customerIdentifier}_A_B_Outcomes";
                outcomeRecord.DerivedServiceBenefitVector = $"Final_S_Combined_{customerIdentifier}_A_B_Outcomes";
                outcomeRecord.OutcomeGenerationTimestamp = DateTime.UtcNow; // Update timestamp on finalization
                                                                            // Optionally, update categorical info based on processing outcomes
                outcomeRecord.CategoricalClassificationIdentifier = (outcomeRecord.CategoricalClassificationIdentifier ?? 0) + 1;
                outcomeRecord.CategoricalClassificationDescription = "Finalized by Unit D";


                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: SequentialFinalProcessingUnitD updated main CoreMlOutcomeRecord object with final vectors and timestamp.");

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

                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Final Processing Unit D (Simulated Model D) completed its simulated processing.");
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
                System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] Workflow Session {requestSequenceIdentifier}: Sequential Final Processing Unit D (Simulated Model D) finished.");
            }
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

            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] GET request for record ID {recordIdentifier}: Record found.");
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
}