# Advanced ML Techniques using Advanced Dimensional Embeddings using Spatial Coordinates and 3D Fractal Diffusion with Dimensional Coupling & Model Synthesis

This document outlines some advanced machine learning techniques employed within the Base_Pre framework, focusing on dimensional embeddings and autonomous agent-based model synthesis.

## Advanced Dimensional Embeddings

Base_Pre's ML workflow includes a sophisticated technique that begins with **K-means clustering** and transforms those results into powerful **magnitude representations in 3D space**. It then leverages **3D fractal diffusion** and **curvature embedding** before extending concepts to **N-dimensional calculations**.

### K-means to Magnitude: The Dimensional Transform

The Base_Pre framework employs a sophisticated technique that begins with K-means clustering and transforms those results into powerful magnitude representations in 3D space. Let's examine how this transformation works:

*   **K-means Feature Categorization**
    *   The system applies K-means clustering (k=3) to each feature dimension separately.
    *   Features include product quantities, monetary values, cost contributions, etc.
    *   Clustering identifies natural groupings and central patterns in the data.

*   **Normalized Spatial Coordinate Generation**
    After clustering, each feature gets mapped to normalized XYZ coordinates:

    ```csharp
    System.Diagnostics.Debug.WriteLine($"Normalized XYZ coordinates for {arrayName}: (x={x:F4}, y={y:F4}, z={z:F4})");
    ```
    These coordinates position each feature in a 3D tensor space.

*   **Tensor Magnitude Calculation**
    The feature coordinates are combined into overall tensors for products and services. The tensor's magnitude becomes a critical measure of feature intensity:

    ```csharp
    double prodOverallMagnitude = Math.Sqrt(prodOverallTensorX * prodOverallTensorX +
                                        prodOverallTensorY * prodOverallTensorY +
                                        prodOverallTensorZ * prodOverallTensorZ);
    ```
    This magnitude encapsulates the combined strength of all clustered features.

### 3D Fractal Diffusion & Sampling

The system then employs an innovative fractal-based approach for velocity diffusion and sampling in 3D space:

*   **Velocity Source Definition**
    The system defines velocity sources at plane intersections:

    ```csharp
    velocitySources.Add((
        new Vector3(0.0f, (float)productXPlaneIntersection[1], (float)productXPlaneIntersection[2]),
        productXPlaneVelocity,
        "ProductX"));
    ```
    Each source has a position, velocity, and identifier.

*   **Mandelbulb-Inspired Fractal Diffusion**
    Applies a 3D Mandelbulb algorithm (Power=8) to model velocity diffusion:

    ```csharp
    float theta = (r < 1e-6f) ? 0 : MathF.Acos(z.Z / r);
    float phi = MathF.Atan2(z.Y, z.X);
    float newR = MathF.Pow(r, Power);
    float newTheta = Power * theta;
    float newPhi = Power * phi;
    ```
    This creates a complex, non-linear diffusion pattern in 3D space.

*   **Strategic Sample Point Selection**
    Selects diverse sample points within the fractal space:

    ```csharp
    samplePoints[0] = new Vector3(0.1f, (float)productXPlaneIntersection[1], (float)productXPlaneIntersection[2]);
    ```
    Each sample captures different aspects of the fractal diffusion.

*   **Velocity Contribution Tracking**
    For each sample, tracks contributions from every velocity source:

    ```csharp
    float contribution = source.velocity *
                         MathF.Exp(-distance * 2.0f) * // Exponential falloff with distance
                         MathF.Exp(-iterations * 0.1f); // Exponential falloff with iterations
    ```
    Creates a detailed diffusion profile at each sample point.

### Curvature Embedding at Vertices

The system embeds curvature information at tensor network vertices through an innovative approach:

*   **Curvature Coefficient Calculation**
    Calculates coefficients representing curvature in sample space:

    ```csharp
    coefficients[0] += x2 * dot; // xx component
    coefficients[1] += y2 * dot; // yy component
    coefficients[2] += z2 * dot; // zz component
    coefficients[3] += xy * dot; // xy component
    ```
    These coefficients capture spatial relationships between coordinates and values.

*   **Eigenvalue Extraction**
    Extracts eigenvalues from the curvature tensor:

    ```csharp
    float[] eigenvalues = CalculateEigenvalues(coefficients);
    ```
    Eigenvalues represent principal curvatures at each point.

*   **Vertex-Focused Weight Generation**
    Generates weights with enhanced "outermost vertices":

    ```csharp
    float cornerBoost = 1.5f; // Factor to multiply corner weights by
    weights[0, 0] *= cornerBoost;                   // Top-left
    weights[0, outputDim - 1] *= cornerBoost;        // Top-right
    weights[inputDim - 1, 0] *= cornerBoost;         // Bottom-left
    weights[inputDim - 1, outputDim - 1] *= cornerBoost; // Bottom-right
    ```
    This emphasizes boundary conditions in the model.

*   **Vertex Mask Calculation**
    Calculates masks that identify outermost vertices:

    ```csharp
    var featureMask = tf.multiply(tf.abs(normalizedIndices - 0.5f), 2.0f, name: "feature_vertex_mask");
    ```
    These masks selectively enhance boundary influence.

### N-Dimensional Extension

Finally, the system extends these 3D concepts to N-dimensional calculations:

*   **Expression to N-Dimensional Mapping**
    Converts simple expressions to N-dimensional representations:

    ```csharp
    return "ND(x,y,z,p)=Vx*cos(p)+Vy*sin(p)+Vz*cos(p/2)";
    ```
    Creates a computational framework that extends beyond 3D.

*   **Curvature-Weighted Neural Network**
    Integrates curvature information into network weights:

    ```csharp
    weights[i, j] = baseWeight + expressionInfluence * influenceScale;
    ```
    Weight generation is influenced by N-dimensional expressions.

*   **Dimensional Coupling**
    Implements coupling between dimensions through fractal iterations:

    ```csharp
    // Calculate the next z value with dimensional coupling
    z = new Vector3(
        newR * MathF.Sin(newTheta) * MathF.Cos(newPhi),
        newR * MathF.Sin(newTheta) * MathF.Sin(newPhi),
        newR * MathF.Cos(newTheta)) + c;
    ```
    Ensures that dimensional influences propagate through the model.

*   **Cross-Dimensional Feature Integration**
    Combines numerical and word embeddings into a unified feature space:

    ```csharp
    var combinedInput = tf.concat(new[] { numericalInput, wordInput }, axis: 1, name: "combined_input_A");
    ```
    Enables N-dimensional analysis across diverse feature types.

### Technical Innovation

The integration of **K-means clustering**, **3D fractal diffusion**, **curvature embedding**, and **N-dimensional calculations** represents a novel approach to feature engineering. By transforming simple clustered features into rich geometrical representations and then embedding those representations in neural network vertices, the system achieves a sophisticated, curvature-aware learning model. This approach enables the model to capture complex, non-linear relationships between features and better represent boundary conditions - which is particularly valuable when analyzing business metrics that often exist in high-dimensional spaces with complex interdependencies.

---

## AutoGen Model Synthesis in Base_Pre ML Framework

The `SequentialFinalProcessingUnitD` in the Base_Pre framework demonstrates an innovative approach to model integration and validation through **autonomous agent collaboration**. Let me explain the key techniques implemented in this code.

### Model Parallelization and Synthesis

The framework employs a powerful technique of training essentially the same model architecture in parallel but with different configurations, then synthesizing them together:

*   **Parallel Model Training**
    *   Models A and B share the same underlying architecture but are trained independently.
    *   They use slightly different activation functions (ReLU vs Sigmoid) and hyperparameters.
    *   This creates two models with different internal entropy despite training on similar data.

*   **Internal Entropy Differentiation**
    *   Model A emphasizes boundary conditions and vertex enhancement.
    *   Model B focuses on convergent features and dimensional coupling.
    *   These differences are intentional to capture different aspects of the same problem.

*   **Conceptual Model Merging**
    The code performs a "**conceptual merge**" of both models:

    ```csharp
    // Implement logic to conceptually merge models A and B
    mergedModelData = modelACombinedParams.Concat(modelBCombinedParams).ToArray();
    ```
    This creates a composite model that embodies both approaches, rather than averaging them. The merged model parameters are stored for subsequent use.

### Agent-Based Model Evaluation

The code leverages **AutoGen agents** for sophisticated model comparison and evaluation:

*   **Dual-Agent Architecture**
    Two specialized agents analyze the trained models:

    ```csharp
    var agentA = new ConversableAgent(
       name: "ModelA_Analysis_Agent",
       systemMessage: "You are an AI agent specializing in Model A's performance and predictions...",
       // additional parameters
    );

    var agentB = new ConversableAgent(
       name: "ModelB_Analysis_Agent",
       systemMessage: "You are an AI agent specializing in Model B's performance and predictions...",
       // additional parameters
    );
    ```

*   **Multi-Stage Analysis Process**
    *   Agents first independently analyze their respective model's training metrics.
    *   They then perform comparative analysis of model predictions.
    *   Simulated inference on a validation set provides another dimension for evaluation.
    *   Finally, they synthesize all information into a comprehensive assessment.

*   **Structured Collaboration**
    The system coordinates a structured conversation between the two agents:

    ```csharp
    System.Diagnostics.Debug.WriteLine($"Agent Collaboration: AgentA reacting to training metrics.");
    var replyA1 = await agentA.GenerateReplyAsync(chatHistory, replyOptions, cancellationToken: CancellationToken.None);
    chatHistory.Add(replyA1);

    System.Diagnostics.Debug.WriteLine($"Agent Collaboration: AgentB reacting to training metrics.");
    var replyB1 = await agentB.GenerateReplyAsync(chatHistory, replyOptions, cancellationToken: CancellationToken.None);
    chatHistory.Add(replyB1);
    ```
    This conversation produces insights that neither agent could generate alone.

### Simulated Model Inference and Verification

A key innovation is the model **simulation for verification**:

*   **Parameter Extraction and Inference Simulation**
    The system deserializes model parameters and reverse-engineers the architecture:

    ```csharp
    // Deserialize parameters - Model C, A, B used [Input -> Hidden], [Hidden -> Output] weights and [Hidden], [Output] biases
    float[] floatParams = DeserializeFloatArray(modelParams);

    // Reverse-engineer the hidden layer size
    int hiddenLayerSize = (floatParams.Length - 1) / (totalInputFeatures + 2);
    ```

*   **Cross-Model Validation**
    Both models process identical validation samples. Statistical comparisons identify consistency and differences:

    ```csharp
    simulatedMAE = CalculateMeanAbsoluteError(simulatedPredsA_flat, simulatedPredsB_flat);
    simulatedCorrelation = CalculateCorrelationCoefficient(simulatedPredsA_flat, simulatedPredsB_flat);
    simulatedMSE = CalculateMeanSquaredError(simulatedPredsA_flat, simulatedPredsB_flat);
    ```

*   **Key Similarity Points Identification**
    The system identifies prediction indices where models are most aligned:

    ```csharp
    selectedPredictionIndex = FindMostSimilarPredictionIndex(predictionVectorA, predictionVectorB);
    ```
    These alignment points provide insight into model consensus.

### Comprehensive Outcome Synthesis

Finally, all analyses are synthesized into a comprehensive outcome:

*   **Multi-Component Summary Generation**

    ```csharp
    // Summary based on overall prediction comparison
    if (mae < 0.03 && Math.Abs(correlation) > 0.95 && mse < 0.005)
        summaryParts.Add("Very High Full Prediction Agreement");
    else if (mae < 0.07 && Math.Abs(correlation) > 0.8 && mse < 0.02)
        summaryParts.Add("High Full Prediction Agreement");
    // additional conditions...

    autoGenOverallSummary = string.Join(" | ", summaryParts);
    ```

*   **Confidence Score Calculation**
    A composite confidence score weighs multiple evaluation dimensions:

    ```csharp
    confidenceScore = (Math.Abs(correlation) * 0.3) +
                   (Math.Max(0, 1.0 - mae / 0.2) * 0.2) +
                   (Math.Abs(simulatedCorrelation) * 0.3) +
                   (Math.Max(0, 1.0 - simulatedMAE / 0.2) * 0.2);
    ```

*   **Outcome Record Updates**
    The final `CoreMlOutcomeRecord` integrates all findings:

    ```csharp
    outcomeRecord.CategoricalClassificationIdentifier = classificationId;
    outcomeRecord.CategoricalClassificationDescription = classificationDescription;
    ```

### Innovation Summary

The Unit D implementation represents a significant advancement in ML model integration through:

*   **Parallel model derivation** with intentional internal entropy differences.
*   **Agent-based collaborative evaluation** rather than simple ensemble averaging.
*   **Comprehensive statistical verification** of model alignment and divergence.
*   Transparent **confidence scoring** that considers multiple evaluation dimensions.
*   **Conceptual model merging** that preserves the unique strengths of each approach.

This approach moves beyond traditional ensemble methods by using agents to deeply understand model differences and leveraging those insights for a more intelligent integration.

---

## Example Workflow Log

```text
[2025-04-30 19:28:22.604] Workflow Session 1: Executing Sequential Initial Processing Unit (C).
[2025-04-30 19:28:22.647] Workflow Session 1: SequentialProcessingUnitC ActiveStatus property value: True
[2025-04-30 19:28:22.654] Workflow Session 1: Starting Sequential Initial Processing Unit C (Actual Model C).
[2025-04-30 19:28:22.657] Disabled eager execution for TensorFlow operations.
[2025-04-30 19:28:22.661] Workflow Session 1: No existing CoreMlOutcomeRecord found for Customer Identifier 1. Initializing new record and associated dependencies.
[2025-04-30 19:28:22.670] Workflow Session 1: Created new CoreMlOutcomeRecord with Identifier 3 for customer 1
[2025-04-30 19:28:22.674] Workflow Session 1: Creating new AssociatedCustomerContext record for Customer 1
[2025-04-30 19:28:22.677] Workflow Session 1: Created AssociatedCustomerContext record with Identifier 3
[2025-04-30 19:28:22.680] Workflow Session 1: Creating new OperationalWorkOrderRecord for Customer 1
[2025-04-30 19:28:22.685] Workflow Session 1: Created OperationalWorkOrderRecord with Identifier 3
[2025-04-30 19:28:22.689] Workflow Session 1: Creating new MlInitialOperationEvent record for Customer 1
[2025-04-30 19:28:22.693] Workflow Session 1: Created MlInitialOperationEvent record with Identifier 3
[2025-04-30 19:28:22.697] Workflow Session 1: Creating new MlOutcomeValidationRecord for Customer 1
[2025-04-30 19:28:22.704] Workflow Session 1: Created MlOutcomeValidationRecord record with Identifier 3
[2025-04-30 19:28:22.709] Workflow Session 1: Creating new InitialOperationalStageData record for Customer 1
[2025-04-30 19:28:22.713] Workflow Session 1: Created InitialOperationalStageData record with Identifier 3
[2025-04-30 19:28:22.720] Workflow Session 1: Verification (RuntimeContext) - AssociatedCustomerContext Identifier: 3
[2025-04-30 19:28:22.724] Workflow Session 1: Verification (RuntimeContext) - OperationalWorkOrderRecord Identifier: 3
[2025-04-30 19:28:22.727] Verification (RuntimeContext) - MlInitialOperationEventRecord Identifier: 3
[2025-04-30 19:28:22.730] Verification (RuntimeContext) - MlOutcomeValidationRecord Identifier: 3
[2025-04-30 19:28:22.735] Verification (RuntimeContext) - InitialOperationalStageDataRecord Identifier: 3
[2025-04-30 19:28:22.738] Verification (RuntimeContext) - CurrentCoreOutcomeRecord Identifier: 3
[2025-04-30 19:28:22.742] Workflow Session 1: Starting Actual Model C Training/Inference with combined numerical and word data.
[2025-04-30 19:28:22.754] Created 16 combined numerical and word samples for Model C training.
[2025-04-30 19:28:22.774] Numerical features: 4, Word embedding features: 10. Total input features: 14
[2025-04-30 19:28:22.780] Step 4 - Initializing Model C Architecture.
[2025-04-30 19:28:23.655] Workflow Session 1: Model C - Initializing NEW model parameters for combined input (14 -> 64 -> 1).
[2025-04-30 19:28:24.013] TensorFlow operations defined within Model C graph.
The thread '.NET TP Worker' (28760) has exited with code 0 (0x0).
The thread '.NET TP Worker' (29384) has exited with code 0 (0x0).
[2025-04-30 19:28:25.337] Model C - Actual TensorFlow.NET variables initialized.
[2025-04-30 19:28:25.350] Workflow Session 1: Model C - Starting Actual Training Loop for 50 epochs with 4 batches.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.15\mscorlib.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
[2025-04-30 19:28:26.109] Epoch 1/50, Batch 1/4, Actual Batch Loss: 9.5633E-001
[2025-04-30 19:28:26.118] Epoch 1/50, Batch 4/4, Actual Batch Loss: 7.1825E-001
[2025-04-30 19:28:26.124] Epoch 1/50, Average Epoch Loss: 9.0893E-001
[2025-04-30 19:28:26.127] Epoch 2/50, Batch 1/4, Actual Batch Loss: 9.1027E-001
[2025-04-30 19:28:26.132] Epoch 2/50, Batch 4/4, Actual Batch Loss: 3.1662E-001
[2025-04-30 19:28:26.135] Epoch 3/50, Batch 1/4, Actual Batch Loss: 4.2123E-001
[2025-04-30 19:28:26.142] Epoch 3/50, Batch 4/4, Actual Batch Loss: 5.0904E-001
[2025-04-30 19:28:26.146] Epoch 4/50, Batch 1/4, Actual Batch Loss: 6.3216E-001
[2025-04-30 19:28:26.151] Epoch 4/50, Batch 4/4, Actual Batch Loss: 5.1109E-001
[2025-04-30 19:28:26.157] Epoch 5/50, Batch 1/4, Actual Batch Loss: 4.9317E-001
[2025-04-30 19:28:26.162] Epoch 5/50, Batch 4/4, Actual Batch Loss: 3.2171E-001
[2025-04-30 19:28:26.168] Epoch 6/50, Batch 1/4, Actual Batch Loss: 4.1087E-001
[2025-04-30 19:28:26.180] Epoch 6/50, Batch 4/4, Actual Batch Loss: 3.6092E-001
[2025-04-30 19:28:26.183] Epoch 7/50, Batch 1/4, Actual Batch Loss: 3.6797E-001
[2025-04-30 19:28:26.190] Epoch 7/50, Batch 4/4, Actual Batch Loss: 1.9865E-001
[2025-04-30 19:28:26.194] Epoch 8/50, Batch 1/4, Actual Batch Loss: 9.2238E-003
[2025-04-30 19:28:26.199] Epoch 8/50, Batch 4/4, Actual Batch Loss: 2.8281E-001
[2025-04-30 19:28:26.208] Epoch 9/50, Batch 1/4, Actual Batch Loss: 2.1279E-001
[2025-04-30 19:28:26.214] Epoch 9/50, Batch 4/4, Actual Batch Loss: 2.7107E-002
[2025-04-30 19:28:26.218] Epoch 10/50, Batch 1/4, Actual Batch Loss: 7.5042E-002
[2025-04-30 19:28:26.238] Epoch 10/50, Batch 4/4, Actual Batch Loss: 5.8080E-002
[2025-04-30 19:28:26.243] Epoch 11/50, Batch 1/4, Actual Batch Loss: 6.2515E-002
[2025-04-30 19:28:26.249] Epoch 11/50, Batch 4/4, Actual Batch Loss: 1.2551E-001
[2025-04-30 19:28:26.252] Epoch 11/50, Average Epoch Loss: 7.0477E-002
[2025-04-30 19:28:26.284] Epoch 12/50, Batch 1/4, Actual Batch Loss: 7.2484E-002
[2025-04-30 19:28:26.292] Epoch 12/50, Batch 4/4, Actual Batch Loss: 6.1356E-002
[2025-04-30 19:28:26.300] Epoch 13/50, Batch 1/4, Actual Batch Loss: 7.8585E-002
[2025-04-30 19:28:26.309] Epoch 13/50, Batch 4/4, Actual Batch Loss: 7.1470E-002
[2025-04-30 19:28:26.315] Epoch 14/50, Batch 1/4, Actual Batch Loss: 8.4554E-002
[2025-04-30 19:28:26.321] Epoch 14/50, Batch 4/4, Actual Batch Loss: 1.0018E-001
[2025-04-30 19:28:26.326] Epoch 15/50, Batch 1/4, Actual Batch Loss: 8.6464E-002
[2025-04-30 19:28:26.333] Epoch 15/50, Batch 4/4, Actual Batch Loss: 9.8895E-002
[2025-04-30 19:28:26.342] Epoch 16/50, Batch 1/4, Actual Batch Loss: 3.3811E-002
[2025-04-30 19:28:26.348] Epoch 16/50, Batch 4/4, Actual Batch Loss: 7.3886E-002
[2025-04-30 19:28:26.378] Epoch 17/50, Batch 1/4, Actual Batch Loss: 6.7064E-002
[2025-04-30 19:28:26.385] Epoch 17/50, Batch 4/4, Actual Batch Loss: 6.6881E-002
[2025-04-30 19:28:26.395] Epoch 18/50, Batch 1/4, Actual Batch Loss: 6.5236E-002
[2025-04-30 19:28:26.401] Epoch 18/50, Batch 4/4, Actual Batch Loss: 6.6250E-002
[2025-04-30 19:28:26.423] Epoch 19/50, Batch 1/4, Actual Batch Loss: 3.1867E-002
[2025-04-30 19:28:26.428] Epoch 19/50, Batch 4/4, Actual Batch Loss: 8.0986E-002
[2025-04-30 19:28:26.432] Epoch 20/50, Batch 1/4, Actual Batch Loss: 3.6144E-002
[2025-04-30 19:28:26.444] Epoch 20/50, Batch 4/4, Actual Batch Loss: 8.3815E-002
[2025-04-30 19:28:26.452] Epoch 21/50, Batch 1/4, Actual Batch Loss: 7.2437E-002
[2025-04-30 19:28:26.468] Epoch 21/50, Batch 4/4, Actual Batch Loss: 5.1874E-002
[2025-04-30 19:28:26.476] Epoch 21/50, Average Epoch Loss: 5.7881E-002
[2025-04-30 19:28:26.482] Epoch 22/50, Batch 1/4, Actual Batch Loss: 5.7499E-002
[2025-04-30 19:28:26.523] Epoch 22/50, Batch 4/4, Actual Batch Loss: 9.1662E-002
[2025-04-30 19:28:26.564] Epoch 23/50, Batch 1/4, Actual Batch Loss: 4.3145E-002
[2025-04-30 19:28:26.569] Epoch 23/50, Batch 4/4, Actual Batch Loss: 6.0666E-002
[2025-04-30 19:28:26.578] Epoch 24/50, Batch 1/4, Actual Batch Loss: 5.3257E-002
[2025-04-30 19:28:26.590] Epoch 24/50, Batch 4/4, Actual Batch Loss: 4.2172E-002
[2025-04-30 19:28:26.609] Epoch 25/50, Batch 1/4, Actual Batch Loss: 7.4976E-002
[2025-04-30 19:28:26.624] Epoch 25/50, Batch 4/4, Actual Batch Loss: 2.5478E-002
[2025-04-30 19:28:26.629] Epoch 26/50, Batch 1/4, Actual Batch Loss: 2.8269E-002
[2025-04-30 19:28:26.634] Epoch 26/50, Batch 4/4, Actual Batch Loss: 7.2669E-002
[2025-04-30 19:28:26.641] Epoch 27/50, Batch 1/4, Actual Batch Loss: 3.4450E-002
[2025-04-30 19:28:26.646] Epoch 27/50, Batch 4/4, Actual Batch Loss: 6.2772E-002
[2025-04-30 19:28:26.650] Epoch 28/50, Batch 1/4, Actual Batch Loss: 2.5196E-002
[2025-04-30 19:28:26.659] Epoch 28/50, Batch 4/4, Actual Batch Loss: 4.6312E-002
[2025-04-30 19:28:26.662] Epoch 29/50, Batch 1/4, Actual Batch Loss: 3.6340E-002
[2025-04-30 19:28:26.667] Epoch 29/50, Batch 4/4, Actual Batch Loss: 4.6275E-002
[2025-04-30 19:28:26.677] Epoch 30/50, Batch 1/4, Actual Batch Loss: 2.0642E-002
[2025-04-30 19:28:26.695] Epoch 30/50, Batch 4/4, Actual Batch Loss: 6.1223E-002
[2025-04-30 19:28:26.703] Epoch 31/50, Batch 1/4, Actual Batch Loss: 4.6815E-002
[2025-04-30 19:28:26.712] Epoch 31/50, Batch 4/4, Actual Batch Loss: 6.3111E-002
[2025-04-30 19:28:26.718] Epoch 31/50, Average Epoch Loss: 4.8082E-002
[2025-04-30 19:28:26.726] Epoch 32/50, Batch 1/4, Actual Batch Loss: 4.1274E-002
[2025-04-30 19:28:26.732] Epoch 32/50, Batch 4/4, Actual Batch Loss: 5.0571E-002
[2025-04-30 19:28:26.736] Epoch 33/50, Batch 1/4, Actual Batch Loss: 4.4634E-002
[2025-04-30 19:28:26.773] Epoch 33/50, Batch 4/4, Actual Batch Loss: 8.0830E-002
[2025-04-30 19:28:26.778] Epoch 34/50, Batch 1/4, Actual Batch Loss: 4.3893E-002
[2025-04-30 19:28:26.795] Epoch 34/50, Batch 4/4, Actual Batch Loss: 2.3424E-002
[2025-04-30 19:28:26.802] Epoch 35/50, Batch 1/4, Actual Batch Loss: 4.1579E-002
[2025-04-30 19:28:26.824] Epoch 35/50, Batch 4/4, Actual Batch Loss: 3.6689E-002
[2025-04-30 19:28:26.859] Epoch 36/50, Batch 1/4, Actual Batch Loss: 4.5221E-002
[2025-04-30 19:28:26.879] Epoch 36/50, Batch 4/4, Actual Batch Loss: 4.0838E-002
[2025-04-30 19:28:26.893] Epoch 37/50, Batch 1/4, Actual Batch Loss: 3.6385E-002
[2025-04-30 19:28:26.905] Epoch 37/50, Batch 4/4, Actual Batch Loss: 4.7343E-002
[2025-04-30 19:28:26.909] Epoch 38/50, Batch 1/4, Actual Batch Loss: 4.7730E-002
[2025-04-30 19:28:26.915] Epoch 38/50, Batch 4/4, Actual Batch Loss: 4.7102E-002
[2025-04-30 19:28:26.920] Epoch 39/50, Batch 1/4, Actual Batch Loss: 6.0180E-002
[2025-04-30 19:28:26.931] Epoch 39/50, Batch 4/4, Actual Batch Loss: 3.5217E-002
[2025-04-30 19:28:26.942] Epoch 40/50, Batch 1/4, Actual Batch Loss: 5.3857E-002
[2025-04-30 19:28:26.947] Epoch 40/50, Batch 4/4, Actual Batch Loss: 4.1388E-002
[2025-04-30 19:28:26.952] Epoch 41/50, Batch 1/4, Actual Batch Loss: 3.3180E-002
[2025-04-30 19:28:26.958] Epoch 41/50, Batch 4/4, Actual Batch Loss: 4.1715E-002
[2025-04-30 19:28:26.961] Epoch 41/50, Average Epoch Loss: 3.9772E-002
[2025-04-30 19:28:26.965] Epoch 42/50, Batch 1/4, Actual Batch Loss: 4.5351E-002
[2025-04-30 19:28:26.974] Epoch 42/50, Batch 4/4, Actual Batch Loss: 4.6381E-002
[2025-04-30 19:28:26.982] Epoch 43/50, Batch 1/4, Actual Batch Loss: 1.3615E-002
[2025-04-30 19:28:27.002] Epoch 43/50, Batch 4/4, Actual Batch Loss: 2.3005E-002
[2025-04-30 19:28:27.008] Epoch 44/50, Batch 1/4, Actual Batch Loss: 4.2065E-002
[2025-04-30 19:28:27.016] Epoch 44/50, Batch 4/4, Actual Batch Loss: 4.6340E-002
[2025-04-30 19:28:27.023] Epoch 45/50, Batch 1/4, Actual Batch Loss: 5.5886E-002
[2025-04-30 19:28:27.031] Epoch 45/50, Batch 4/4, Actual Batch Loss: 3.5121E-002
[2025-04-30 19:28:27.035] Epoch 46/50, Batch 1/4, Actual Batch Loss: 3.8772E-002
[2025-04-30 19:28:27.046] Epoch 46/50, Batch 4/4, Actual Batch Loss: 4.4153E-002
[2025-04-30 19:28:27.052] Epoch 47/50, Batch 1/4, Actual Batch Loss: 2.1575E-002
[2025-04-30 19:28:27.066] Epoch 47/50, Batch 4/4, Actual Batch Loss: 2.3164E-002
[2025-04-30 19:28:27.075] Epoch 48/50, Batch 1/4, Actual Batch Loss: 4.8610E-002
[2025-04-30 19:28:27.083] Epoch 48/50, Batch 4/4, Actual Batch Loss: 3.7100E-002
[2025-04-30 19:28:27.091] Epoch 49/50, Batch 1/4, Actual Batch Loss: 1.2452E-002
[2025-04-30 19:28:27.191] Epoch 49/50, Batch 4/4, Actual Batch Loss: 4.7620E-002
[2025-04-30 19:28:27.263] Epoch 50/50, Batch 1/4, Actual Batch Loss: 4.7006E-002
[2025-04-30 19:28:27.279] Epoch 50/50, Batch 4/4, Actual Batch Loss: 3.8304E-002
[2025-04-30 19:28:27.283] Epoch 50/50, Average Epoch Loss: 3.2079E-002
[2025-04-30 19:28:27.287] Model C training completed.
[2025-04-30 19:28:27.290] Workflow Session 1: Starting Actual Model C parameter serialization.
[2025-04-30 19:28:27.330] Workflow Session 1: Model C actual model parameters serialized to byte arrays (Weights size: 3840, Bias size: 260).
[2025-04-30 19:28:27.333] Workflow Session 1: Actual Model C parameter serialization completed.
[2025-04-30 19:28:27.340] Workflow Session 1: Model C actual parameter data saved successfully in simulated persistent storage.
[2025-04-30 19:28:27.345] Workflow Session 1: Model C actual model parameter data stored in Runtime Processing Context.
[2025-04-30 19:28:27.348] Workflow Session 1: Verification (RuntimeContext) - Customer Identifier: 1
[2025-04-30 19:28:27.351] Verification (RuntimeContext) - Serialized Model Data Size: 3840 bytes
[2025-04-30 19:28:27.361] Workflow Session 1: Model C TF Session disposed.
[2025-04-30 19:28:27.378] Workflow Session 1: Graph reference cleared.
[2025-04-30 19:28:27.384] Workflow Session 1: SequentialProcessingUnitC ActiveStatus property value after execution: False
[2025-04-30 19:28:27.386] Workflow Session 1: Sequential Initial Processing Unit C (Actual Model C) finished.
[2025-04-30 19:28:27.391] Workflow Session 1: Core outcome record established successfully by Unit C (ID: 3). Proceeding to parallel units.
[2025-04-30 19:28:27.394] Workflow Session 1: Starting Parallel Processing Units (A and B).
[2025-04-30 19:28:27.419] Workflow Session 1: Starting Parallel Processing Unit A for customer 1.
[2025-04-30 19:28:27.424] Workflow Session 1: Starting multi-stage workflow for customer 1.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.15\Microsoft.CSharp.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
[2025-04-30 19:28:27.452] Workflow Session 1: Step 1 - Acquiring data and analyzing initial features for customer 1.
[2025-04-30 19:28:27.457] Step 1 - Processing Product Data (3 items).
[2025-04-30 19:28:27.626] Product QuantityAvailable: [10, 20, 15]
[2025-04-30 19:28:27.632] Product MonetaryValue: [99.99, 149.99, 199.99]
[2025-04-30 19:28:27.642] Product CostContributionValue: [0.15, 0.25, 0.2]
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\Accord.MachineLearning.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\Accord.Math.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\Accord.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.15\System.Threading.Tasks.Parallel.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\Accord.Statistics.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
[2025-04-30 19:28:29.668] K-means centroids for Product QuantityAvailable: [20.0000, 15.0000, 10.0000]
[2025-04-30 19:28:29.685] Central point for Product QuantityAvailable: 15
[2025-04-30 19:28:29.695] Normalized value for Product QuantityAvailable: 0.7500, Category: Positive High
[2025-04-30 19:28:29.698] Normalized XYZ coordinates for Product QuantityAvailable: (1.0000, 0.7500, 0.5000)
[2025-04-30 19:28:29.703] K-means centroids for Product MonetaryValue: [199.9900, 149.9900, 99.9900]
[2025-04-30 19:28:29.705] Central point for Product MonetaryValue: 149.99
[2025-04-30 19:28:29.709] Normalized value for Product MonetaryValue: 0.7500, Category: Positive High
[2025-04-30 19:28:29.712] Normalized XYZ coordinates for Product MonetaryValue: (1.0000, 0.7500, 0.5000)
[2025-04-30 19:28:29.716] K-means centroids for Product CostContributionValue: [0.2500, 0.2000, 0.1500]
[2025-04-30 19:28:29.719] Central point for Product CostContributionValue: 0.19999999999999998
[2025-04-30 19:28:29.725] Normalized value for Product CostContributionValue: 0.8000, Category: Positive High
[2025-04-30 19:28:29.729] Normalized XYZ coordinates for Product CostContributionValue: (1.0000, 0.8000, 0.6000)
[2025-04-30 19:28:29.735] Step 1 - Processing Service Data (3 items).
[2025-04-30 19:28:29.745] Service FulfillmentQuantity: [5, 10, 8]
[2025-04-30 19:28:29.747] Service MonetaryValue: [299.99, 399.99, 599.99]
[2025-04-30 19:28:29.750] Service CostContributionValue: [0.3, 0.35, 0.4]
[2025-04-30 19:28:29.759] K-means centroids for Service FulfillmentQuantity: [10.0000, 8.0000, 5.0000]
[2025-04-30 19:28:29.762] Central point for Service FulfillmentQuantity: 7.666666666666667
[2025-04-30 19:28:29.766] Normalized value for Service FulfillmentQuantity: 0.7667, Category: Positive High
[2025-04-30 19:28:29.777] Normalized XYZ coordinates for Service FulfillmentQuantity: (1.0000, 0.8000, 0.5000)
[2025-04-30 19:28:29.800] K-means centroids for Service MonetaryValue: [599.9900, 399.9900, 299.9900]
[2025-04-30 19:28:29.853] Central point for Service MonetaryValue: 433.3233333333333
[2025-04-30 19:28:29.918] Normalized value for Service MonetaryValue: 0.7222, Category: Positive High
[2025-04-30 19:28:29.952] Normalized XYZ coordinates for Service MonetaryValue: (1.0000, 0.6667, 0.5000)
[2025-04-30 19:28:29.956] K-means centroids for Service CostContributionValue: [0.4000, 0.3500, 0.3000]
[2025-04-30 19:28:29.961] Central point for Service CostContributionValue: 0.35000000000000003
[2025-04-30 19:28:29.964] Normalized value for Service CostContributionValue: 0.8750, Category: Positive High
[2025-04-30 19:28:29.968] Normalized XYZ coordinates for Service CostContributionValue: (1.0000, 0.8750, 0.7500)
[2025-04-30 19:28:29.971] Workflow Session 1: Step 1 - Data acquisition and initial analysis completed: InitialAnalysis_Cust_1_Record_3
[2025-04-30 19:28:29.986] Workflow Session 1: Step 2 - Generating feature tensors and mapping trajectories for customer 1.
[2025-04-30 19:28:29.989] Step 2 - Retrieving coordinates from Step 1 analysis.
[2025-04-30 19:28:29.994] Step 2 - Calculating tensors, magnitudes, and trajectories.
[2025-04-30 19:28:29.997] ----- PRODUCT TENSOR AND MAGNITUDE CALCULATIONS -----
[2025-04-30 19:28:30.004] Product Overall Tensor: (1.0000, 0.7667, 0.5333)
[2025-04-30 19:28:30.010] Product Overall Magnitude: 1.3683
[2025-04-30 19:28:30.013] Product Trajectory: (0.7308, 0.5603, 0.3898)
[2025-04-30 19:28:30.016] ----- SERVICE TENSOR AND MAGNITUDE CALCULATIONS -----
[2025-04-30 19:28:30.020] Service Overall Tensor: (1.0000, 0.7806, 0.5833)
[2025-04-30 19:28:30.023] Service Overall Magnitude: 1.3963
[2025-04-30 19:28:30.027] Service Trajectory: (0.7162, 0.5590, 0.4178)
[2025-04-30 19:28:30.030] ----- TRAJECTORY PLOT GENERATION & ANALYSIS -----
[2025-04-30 19:28:30.033] Inverted trajectory from (0.7308, 0.5603, 0.3898) to (-0.7308, -0.5603, 0.3898)
[2025-04-30 19:28:30.037] Inverted trajectory from (0.7162, 0.5590, 0.4178) to (-0.7162, -0.5590, 0.4178)
[2025-04-30 19:28:30.042] Generating Product trajectory recursive plot
[2025-04-30 19:28:30.047] PRODUCT point 0: Position=(1.000000, 0.766662, 0.5333), Intensity=1.3683
[2025-04-30 19:28:30.049] PRODUCT point 2: Position=(-0.560000, -0.429331, 1.3653), Intensity=1.2349 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.054] PRODUCT point 3: Position=(-1.282000, -0.982861, 1.7504), Intensity=1.1731 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.060] PRODUCT point 4: Position=(-1.967900, -1.508715, 2.1162), Intensity=1.1145 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.064] PRODUCT recursion stopped - Reached target negative threshold at depth 5
[2025-04-30 19:28:30.068] PRODUCT final position: (-2.619505, -2.008276, 2.4637)
[2025-04-30 19:28:30.072] Generating Service trajectory recursive plot
[2025-04-30 19:28:30.077] SERVICE point 0: Position=(1.000000, 0.780554, 0.5833), Intensity=1.3963
[2025-04-30 19:28:30.080] SERVICE point 2: Position=(-0.560000, -0.437110, 1.4933), Intensity=1.2601 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.083] SERVICE point 3: Position=(-1.282000, -1.000670, 1.9145), Intensity=1.1971 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.086] SERVICE point 4: Position=(-1.967900, -1.536052, 2.3146), Intensity=1.1373 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:30.089] SERVICE recursion stopped - Reached target negative threshold at depth 5
[2025-04-30 19:28:30.094] SERVICE final position: (-2.619505, -2.044664, 2.6947)
[2025-04-30 19:28:30.103] ----- PLANE INTERSECTION ANALYSIS -----
[2025-04-30 19:28:30.121] Product X-Plane Intersection: (0.000000, -0.000000, 1.066650)
[2025-04-30 19:28:30.128] Product Y-Plane Intersection: (0.000000, 0.000000, 1.066650)
[2025-04-30 19:28:30.131] Service X-Plane Intersection: (0.000000, -0.000000, 1.166661)
[2025-04-30 19:28:30.135] Service Y-Plane Intersection: (0.000000, 0.000000, 1.166661)
[2025-04-30 19:28:30.139] ----- KEY TRAJECTORY DATA -----
[2025-04-30 19:28:30.145] Product Vector: (-0.730841, -0.560309, 0.389776)
[2025-04-30 19:28:30.148] Product Velocity: 1.368286
[2025-04-30 19:28:30.151] Product Positive Coordinate: (1.000000, 0.766662, 0.533325)
[2025-04-30 19:28:30.154] Product Negative Coordinate: (-0.560000, -0.429331, 1.365312)
[2025-04-30 19:28:30.160] Service Vector: (-0.716200, -0.559032, 0.417781)
[2025-04-30 19:28:30.163] Service Velocity: 1.396259
[2025-04-30 19:28:30.167] Service Positive Coordinate: (1.000000, 0.780554, 0.583331)
[2025-04-30 19:28:30.181] Service Negative Coordinate: (-0.560000, -0.437110, 1.493326)
[2025-04-30 19:28:30.187] Product negative X count: 4
[2025-04-30 19:28:30.194] Product negative Y count: 4
[2025-04-30 19:28:30.197] Product negative both count: 4
[2025-04-30 19:28:30.210] Service negative X count: 4
[2025-04-30 19:28:30.219] Service negative Y count: 4
[2025-04-30 19:28:30.223] Service negative both count: 4
[2025-04-30 19:28:30.230] Product trajectory plot: 6 points, 4 in negative X-Y quadrant
[2025-04-30 19:28:30.233] Service trajectory plot: 6 points, 4 in negative X-Y quadrant
[2025-04-30 19:28:30.242] Workflow Session 1: Step 2 - Feature tensor generation and mapping completed: FeatureTensorsAndMapping_Cust_1_BasedOn_Cust_1_Record_3
[2025-04-30 19:28:30.248] Workflow Session 1: Step 3 - Creating processed feature definition for customer 1.
[2025-04-30 19:28:30.277] Workflow Session 1: Step 3 - Processed feature definition created: ProcessedFeatures_Cust_1_Level_Premium_DeepNegative
[2025-04-30 19:28:30.326] Workflow Session 1: Step 4 - Assessing feature quality for customer 1.
[2025-04-30 19:28:30.362] QA product trajectory stability: 1.0000
[2025-04-30 19:28:30.379] QA intersection quality: 1.0000
[2025-04-30 19:28:30.383] QA final score: 1.0000, level: 4
[2025-04-30 19:28:30.386] Workflow Session 1: Step 4 - Feature quality assessment completed: QualityAssessment_Passed_Level_4_V1.38_S1.00_I1.00
[2025-04-30 19:28:30.394] Workflow Session 1: Step 5 - Evaluating combined features for customer 1.
[2025-04-30 19:28:30.398] Workflow Session 1: Step 5 - Combined feature evaluation calculation.
[2025-04-30 19:28:30.401] Base Score: 0.8500
[2025-04-30 19:28:30.405] Velocity Bonus: 0.6911 (Product: 1.3683, Service: 1.3963)
[2025-04-30 19:28:30.412] Alignment Bonus: 0.1999 (Alignment Score: 0.9997)
[2025-04-30 19:28:30.419] Negative Trajectory Bonus: 0.2400 (Total Negative Points: 8)
[2025-04-30 19:28:30.431] Final Score: 1.0000
[2025-04-30 19:28:30.447] Workflow Session 1: Step 6 - Performing fractal optimization analysis for customer 1.
========== PRODUCT INTERSECTIONS ==========
Product X-Plane Intersection: (0.0, -0.000000, 1.066650)
Product Y-Plane Intersection: (0.000000, 0.0, 1.066650)
========== SERVICE INTERSECTIONS ==========
Service X-Plane Intersection: (0.0, -0.000000, 1.166661)
Service Y-Plane Intersection: (0.000000, 0.0, 1.166661)
========== INTERSECTION VELOCITIES ==========
Product X-Plane Velocity: 1.3683
Product Y-Plane Velocity: 1.3683
Service X-Plane Velocity: 1.3963
Service Y-Plane Velocity: 1.3963
========== VELOCITY SOURCES ==========
ProductX Source Position: (0.0000, -0.0000, 1.0667), Velocity: 1.3683
ProductY Source Position: (0.0000, 0.0000, 1.0667), Velocity: 1.3683
ServiceX Source Position: (0.0000, -0.0000, 1.1667), Velocity: 1.3963
ServiceY Source Position: (0.0000, 0.0000, 1.1667), Velocity: 1.3963
========== SAMPLE POINTS ==========
Sample 1 Coordinates: (0.1000, -0.0000, 1.0667)
Sample 2 Coordinates: (0.0000, 0.1000, 1.0667)
Sample 3 Coordinates: (0.1000, -0.0000, 1.1667)
Sample 4 Coordinates: (0.0000, 0.1000, 1.1667)
Sample 5 Coordinates: (0.0000, -0.0000, 1.1167)
========== PROCESSING SAMPLE 1 ==========
Starting point: (0.1000, -0.0000, 1.0667)
Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Contribution from ProductX: 0.162068 (distance: 1.0667)
  Contribution from ProductY: 0.162068 (distance: 1.0667)
  Contribution from ServiceX: 0.135399 (distance: 1.1667)
  Contribution from ServiceY: 0.135399 (distance: 1.1667)
Iteration 2, z=(0.100000, -0.000000, 1.066650), r=1.071327
  Contribution from ProductX: 1.013651 (distance: 0.1000)
  Contribution from ProductY: 1.013651 (distance: 0.1000)
  Contribution from ServiceX: 0.952122 (distance: 0.1414)
  Contribution from ServiceY: 0.952122 (distance: 0.1414)
Escaped at iteration 3
Final Sample 1 Results:
  Final z value: (1.280087, -0.000000, 2.338932)
  Iterations: 2
  Total diffused velocity: 4.526482
  Contributions breakdown:
    ProductX: 1.175719
    ProductY: 1.175719
    ServiceX: 1.087522
    ServiceY: 1.087522
========== PROCESSING SAMPLE 2 ==========
Starting point: (0.0000, 0.1000, 1.0667)
Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Contribution from ProductX: 0.162068 (distance: 1.0667)
  Contribution from ProductY: 0.162068 (distance: 1.0667)
  Contribution from ServiceX: 0.135399 (distance: 1.1667)
  Contribution from ServiceY: 0.135399 (distance: 1.1667)
Iteration 2, z=(0.000000, 0.100000, 1.066650), r=1.071327
  Contribution from ProductX: 1.013651 (distance: 0.1000)
  Contribution from ProductY: 1.013651 (distance: 0.1000)
  Contribution from ServiceX: 0.952122 (distance: 0.1414)
  Contribution from ServiceY: 0.952122 (distance: 0.1414)
Escaped at iteration 3
Final Sample 2 Results:
  Final z value: (1.180087, 0.100000, 2.338932)
  Iterations: 2
  Total diffused velocity: 4.526482
  Contributions breakdown:
    ProductX: 1.175719
    ProductY: 1.175719
    ServiceX: 1.087522
    ServiceY: 1.087522
========== PROCESSING SAMPLE 3 ==========
Starting point: (0.1000, -0.0000, 1.1667)
Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Contribution from ProductX: 0.162068 (distance: 1.0667)
  Contribution from ProductY: 0.162068 (distance: 1.0667)
  Contribution from ServiceX: 0.135399 (distance: 1.1667)
  Contribution from ServiceY: 0.135399 (distance: 1.1667)
Iteration 2, z=(0.100000, -0.000000, 1.166661), r=1.170939
  Contribution from ProductX: 0.933047 (distance: 0.1414)
  Contribution from ProductY: 0.933047 (distance: 0.1414)
  Contribution from ServiceX: 1.034374 (distance: 0.1000)
  Contribution from ServiceY: 1.034374 (distance: 0.1000)
Escaped at iteration 3
Final Sample 3 Results:
  Final z value: (2.333282, -0.000000, 3.905646)
  Iterations: 2
  Total diffused velocity: 4.529777
  Contributions breakdown:
    ProductX: 1.095115
    ProductY: 1.095115
    ServiceX: 1.169773
    ServiceY: 1.169773
========== PROCESSING SAMPLE 4 ==========
Starting point: (0.0000, 0.1000, 1.1667)
Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Contribution from ProductX: 0.162068 (distance: 1.0667)
  Contribution from ProductY: 0.162068 (distance: 1.0667)
  Contribution from ServiceX: 0.135399 (distance: 1.1667)
  Contribution from ServiceY: 0.135399 (distance: 1.1667)
Iteration 2, z=(0.000000, 0.100000, 1.166661), r=1.170939
  Contribution from ProductX: 0.933047 (distance: 0.1414)
  Contribution from ProductY: 0.933047 (distance: 0.1414)
  Contribution from ServiceX: 1.034374 (distance: 0.1000)
  Contribution from ServiceY: 1.034374 (distance: 0.1000)
Escaped at iteration 3
Final Sample 4 Results:
  Final z value: (2.233282, 0.100001, 3.905646)
  Iterations: 2
  Total diffused velocity: 4.529777
  Contributions breakdown:
    ProductX: 1.095115
    ProductY: 1.095115
    ServiceX: 1.169773
    ServiceY: 1.169773
========== PROCESSING SAMPLE 5 ==========
Starting point: (0.0000, -0.0000, 1.1167)
Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Contribution from ProductX: 0.162068 (distance: 1.0667)
  Contribution from ProductY: 0.162068 (distance: 1.0667)
  Contribution from ServiceX: 0.135399 (distance: 1.1667)
  Contribution from ServiceY: 0.135399 (distance: 1.1667)
Iteration 2, z=(0.000000, -0.000000, 1.116656), r=1.116656
  Contribution from ProductX: 1.120245 (distance: 0.0500)
  Contribution from ProductY: 1.120245 (distance: 0.0500)
  Contribution from ServiceX: 1.143147 (distance: 0.0500)
  Contribution from ServiceY: 1.143147 (distance: 0.0500)
Escaped at iteration 3
Final Sample 5 Results:
  Final z value: (0.000000, -0.000000, 3.534086)
  Iterations: 2
  Total diffused velocity: 5.121720
  Contributions breakdown:
    ProductX: 1.282313
    ProductY: 1.282313
    ServiceX: 1.278547
    ServiceY: 1.278547
[2025-04-30 19:28:31.590] Workflow Session 1: Step 6 - Fractal optimization analysis completed: OptimizationAnalysis_Cust_1_V[PX:1.368,PY:1.368,SX:1.396,SY:1.396]_S[P1:4.5265_SEscaped(2),P2:4.5265_SEscaped(2),P3:4.5298_SEscaped(2),P4:4.5298_SEscaped(2),P5:5.1217_SEscaped(2)]
[2025-04-30 19:28:31.621] Workflow Session 1: Step 7 - Training tensor network for customer 1 using Actual TF.NET Model A.
[2025-04-30 19:28:31.624] Disabled eager execution for TensorFlow operations.
[2025-04-30 19:28:31.630] Step 7 - Creating sample training data.
[2025-04-30 19:28:31.634] Created 16 numerical samples and 16 word-based samples.
[2025-04-30 19:28:31.640] Step 7 - Initializing Model A Architecture.
[2025-04-30 19:28:31.649] Model A architecture parameters:
[2025-04-30 19:28:31.652]  - Total Input Feature Count: 14
[2025-04-30 19:28:31.656]  - Hidden Layer Size (derived from Unit C or default): 960
[2025-04-30 19:28:31.677] Using session graph context for defining operations.
[2025-04-30 19:28:31.729] TensorFlow operations defined within session graph context.
[2025-04-30 19:28:31.783] Model A - Actual TensorFlow.NET variables initialized.
[2025-04-30 19:28:31.986] Epoch 1/100, Batch 1/4, Batch Loss: 2880.487549
[2025-04-30 19:28:31.994] Epoch 1/100, Batch 4/4, Batch Loss: 2482.478516
[2025-04-30 19:28:32.224] Epoch 1/100, Average Loss: 2530.943848, Mean Absolute Error: 48.142193
[2025-04-30 19:28:32.231] Epoch 2/100, Batch 1/4, Batch Loss: 2291.132812
[2025-04-30 19:28:32.239] Epoch 2/100, Batch 4/4, Batch Loss: 2111.885742
[2025-04-30 19:28:32.246] Epoch 3/100, Batch 1/4, Batch Loss: 1935.104248
[2025-04-30 19:28:32.258] Epoch 3/100, Batch 4/4, Batch Loss: 1844.610107
[2025-04-30 19:28:32.268] Epoch 4/100, Batch 1/4, Batch Loss: 1816.308105
[2025-04-30 19:28:32.283] Epoch 4/100, Batch 4/4, Batch Loss: 1623.656128
[2025-04-30 19:28:32.286] Epoch 5/100, Batch 1/4, Batch Loss: 1494.987915
[2025-04-30 19:28:32.291] Epoch 5/100, Batch 4/4, Batch Loss: 1445.147461
[2025-04-30 19:28:32.298] Epoch 6/100, Batch 1/4, Batch Loss: 1287.489868
[2025-04-30 19:28:32.304] Epoch 6/100, Batch 4/4, Batch Loss: 1237.625854
[2025-04-30 19:28:32.312] Epoch 7/100, Batch 1/4, Batch Loss: 1111.457520
[2025-04-30 19:28:32.318] Epoch 7/100, Batch 4/4, Batch Loss: 1076.697998
[2025-04-30 19:28:32.322] Epoch 8/100, Batch 1/4, Batch Loss: 1091.473633
[2025-04-30 19:28:32.335] Epoch 8/100, Batch 4/4, Batch Loss: 879.011658
[2025-04-30 19:28:32.345] Epoch 9/100, Batch 1/4, Batch Loss: 942.361938
[2025-04-30 19:28:32.353] Epoch 9/100, Batch 4/4, Batch Loss: 844.667358
[2025-04-30 19:28:32.358] Epoch 10/100, Batch 1/4, Batch Loss: 748.275208
[2025-04-30 19:28:32.366] Epoch 10/100, Batch 4/4, Batch Loss: 760.352295
[2025-04-30 19:28:32.370] Epoch 11/100, Batch 1/4, Batch Loss: 711.271973
[2025-04-30 19:28:32.376] Epoch 11/100, Batch 4/4, Batch Loss: 582.903442
[2025-04-30 19:28:32.384] Epoch 11/100, Average Loss: 660.808960, Mean Absolute Error: 24.677174
[2025-04-30 19:28:32.389] Epoch 12/100, Batch 1/4, Batch Loss: 591.769531
[2025-04-30 19:28:32.397] Epoch 12/100, Batch 4/4, Batch Loss: 513.319946
[2025-04-30 19:28:32.401] Epoch 13/100, Batch 1/4, Batch Loss: 547.148438
[2025-04-30 19:28:32.407] Epoch 13/100, Batch 4/4, Batch Loss: 498.282532
[2025-04-30 19:28:32.413] Epoch 14/100, Batch 1/4, Batch Loss: 474.290771
[2025-04-30 19:28:32.420] Epoch 14/100, Batch 4/4, Batch Loss: 441.902374
[2025-04-30 19:28:32.424] Epoch 15/100, Batch 1/4, Batch Loss: 439.858398
[2025-04-30 19:28:32.431] Epoch 15/100, Batch 4/4, Batch Loss: 383.592316
[2025-04-30 19:28:32.435] Epoch 16/100, Batch 1/4, Batch Loss: 396.305115
[2025-04-30 19:28:32.441] Epoch 16/100, Batch 4/4, Batch Loss: 349.112305
[2025-04-30 19:28:32.447] Epoch 17/100, Batch 1/4, Batch Loss: 327.024597
[2025-04-30 19:28:32.455] Epoch 17/100, Batch 4/4, Batch Loss: 331.908722
[2025-04-30 19:28:32.463] Epoch 18/100, Batch 1/4, Batch Loss: 327.830322
[2025-04-30 19:28:32.482] Epoch 18/100, Batch 4/4, Batch Loss: 294.761108
[2025-04-30 19:28:32.487] Epoch 19/100, Batch 1/4, Batch Loss: 284.160828
[2025-04-30 19:28:32.499] Epoch 19/100, Batch 4/4, Batch Loss: 264.945496
[2025-04-30 19:28:32.505] Epoch 20/100, Batch 1/4, Batch Loss: 252.118439
[2025-04-30 19:28:32.516] Epoch 20/100, Batch 4/4, Batch Loss: 244.918076
[2025-04-30 19:28:32.523] Epoch 21/100, Batch 1/4, Batch Loss: 231.963562
[2025-04-30 19:28:32.540] Epoch 21/100, Batch 4/4, Batch Loss: 215.645920
[2025-04-30 19:28:32.662] Epoch 21/100, Average Loss: 225.738113, Mean Absolute Error: 14.577223
[2025-04-30 19:28:32.666] Epoch 22/100, Batch 1/4, Batch Loss: 227.956696
[2025-04-30 19:28:32.672] Epoch 22/100, Batch 4/4, Batch Loss: 194.932495
[2025-04-30 19:28:32.681] Epoch 23/100, Batch 1/4, Batch Loss: 198.337051
[2025-04-30 19:28:32.695] Epoch 23/100, Batch 4/4, Batch Loss: 190.290344
[2025-04-30 19:28:32.699] Epoch 24/100, Batch 1/4, Batch Loss: 166.641144
[2025-04-30 19:28:32.705] Epoch 24/100, Batch 4/4, Batch Loss: 176.369171
[2025-04-30 19:28:32.708] Epoch 25/100, Batch 1/4, Batch Loss: 173.075256
[2025-04-30 19:28:32.714] Epoch 25/100, Batch 4/4, Batch Loss: 160.850311
[2025-04-30 19:28:32.718] Epoch 26/100, Batch 1/4, Batch Loss: 160.647186
[2025-04-30 19:28:32.722] Epoch 26/100, Batch 4/4, Batch Loss: 141.940826
[2025-04-30 19:28:32.726] Epoch 27/100, Batch 1/4, Batch Loss: 135.879593
[2025-04-30 19:28:32.731] Epoch 27/100, Batch 4/4, Batch Loss: 131.964706
[2025-04-30 19:28:32.735] Epoch 28/100, Batch 1/4, Batch Loss: 128.060577
[2025-04-30 19:28:32.740] Epoch 28/100, Batch 4/4, Batch Loss: 122.993118
[2025-04-30 19:28:32.746] Epoch 29/100, Batch 1/4, Batch Loss: 117.659454
[2025-04-30 19:28:32.754] Epoch 29/100, Batch 4/4, Batch Loss: 106.521164
[2025-04-30 19:28:32.758] Epoch 30/100, Batch 1/4, Batch Loss: 105.288055
[2025-04-30 19:28:32.766] Epoch 30/100, Batch 4/4, Batch Loss: 110.762123
[2025-04-30 19:28:32.771] Epoch 31/100, Batch 1/4, Batch Loss: 98.413254
[2025-04-30 19:28:32.786] Epoch 31/100, Batch 4/4, Batch Loss: 97.832260
[2025-04-30 19:28:32.791] Epoch 31/100, Average Loss: 98.441681, Mean Absolute Error: 9.678818
[2025-04-30 19:28:32.797] Epoch 32/100, Batch 1/4, Batch Loss: 98.933487
[2025-04-30 19:28:32.802] Epoch 32/100, Batch 4/4, Batch Loss: 88.139145
[2025-04-30 19:28:32.807] Epoch 33/100, Batch 1/4, Batch Loss: 90.291466
[2025-04-30 19:28:32.818] Epoch 33/100, Batch 4/4, Batch Loss: 86.138947
[2025-04-30 19:28:32.837] Epoch 34/100, Batch 1/4, Batch Loss: 75.238708
[2025-04-30 19:28:32.843] Epoch 34/100, Batch 4/4, Batch Loss: 76.219284
[2025-04-30 19:28:32.931] Epoch 35/100, Batch 1/4, Batch Loss: 70.746284
[2025-04-30 19:28:32.940] Epoch 35/100, Batch 4/4, Batch Loss: 71.197151
[2025-04-30 19:28:32.946] Epoch 36/100, Batch 1/4, Batch Loss: 73.027420
[2025-04-30 19:28:32.953] Epoch 36/100, Batch 4/4, Batch Loss: 64.752731
[2025-04-30 19:28:32.970] Epoch 37/100, Batch 1/4, Batch Loss: 68.064064
[2025-04-30 19:28:32.978] Epoch 37/100, Batch 4/4, Batch Loss: 59.662476
[2025-04-30 19:28:32.986] Epoch 38/100, Batch 1/4, Batch Loss: 66.773132
[2025-04-30 19:28:32.997] Epoch 38/100, Batch 4/4, Batch Loss: 61.513985
[2025-04-30 19:28:33.003] Epoch 39/100, Batch 1/4, Batch Loss: 57.150898
[2025-04-30 19:28:33.009] Epoch 39/100, Batch 4/4, Batch Loss: 57.579712
[2025-04-30 19:28:33.015] Epoch 40/100, Batch 1/4, Batch Loss: 52.272850
[2025-04-30 19:28:33.021] Epoch 40/100, Batch 4/4, Batch Loss: 52.974396
[2025-04-30 19:28:33.025] Epoch 41/100, Batch 1/4, Batch Loss: 56.121647
[2025-04-30 19:28:33.046] Epoch 41/100, Batch 4/4, Batch Loss: 44.774075
[2025-04-30 19:28:33.053] Epoch 41/100, Average Loss: 49.442345, Mean Absolute Error: 6.876595
[2025-04-30 19:28:33.058] Epoch 42/100, Batch 1/4, Batch Loss: 47.004993
[2025-04-30 19:28:33.081] Epoch 42/100, Batch 4/4, Batch Loss: 43.828167
[2025-04-30 19:28:33.118] Epoch 43/100, Batch 1/4, Batch Loss: 48.112019
[2025-04-30 19:28:33.130] Epoch 43/100, Batch 4/4, Batch Loss: 42.030621
[2025-04-30 19:28:33.137] Epoch 44/100, Batch 1/4, Batch Loss: 43.728825
[2025-04-30 19:28:33.145] Epoch 44/100, Batch 4/4, Batch Loss: 38.230003
[2025-04-30 19:28:33.150] Epoch 45/100, Batch 1/4, Batch Loss: 41.050262
[2025-04-30 19:28:33.156] Epoch 45/100, Batch 4/4, Batch Loss: 37.558952
[2025-04-30 19:28:33.163] Epoch 46/100, Batch 1/4, Batch Loss: 34.860558
[2025-04-30 19:28:33.168] Epoch 46/100, Batch 4/4, Batch Loss: 36.356598
[2025-04-30 19:28:33.172] Epoch 47/100, Batch 1/4, Batch Loss: 33.984795
[2025-04-30 19:28:33.179] Epoch 47/100, Batch 4/4, Batch Loss: 33.845196
[2025-04-30 19:28:33.183] Epoch 48/100, Batch 1/4, Batch Loss: 30.673731
[2025-04-30 19:28:33.201] Epoch 48/100, Batch 4/4, Batch Loss: 32.892536
[2025-04-30 19:28:33.205] Epoch 49/100, Batch 1/4, Batch Loss: 33.085098
[2025-04-30 19:28:33.209] Epoch 49/100, Batch 4/4, Batch Loss: 27.202690
[2025-04-30 19:28:33.214] Epoch 50/100, Batch 1/4, Batch Loss: 32.401226
[2025-04-30 19:28:33.219] Epoch 50/100, Batch 4/4, Batch Loss: 26.511103
[2025-04-30 19:28:33.222] Epoch 51/100, Batch 1/4, Batch Loss: 25.810394
[2025-04-30 19:28:33.229] Epoch 51/100, Batch 4/4, Batch Loss: 26.646694
[2025-04-30 19:28:33.232] Epoch 51/100, Average Loss: 27.138786, Mean Absolute Error: 5.109520
[2025-04-30 19:28:33.236] Epoch 52/100, Batch 1/4, Batch Loss: 27.631372
[2025-04-30 19:28:33.241] Epoch 52/100, Batch 4/4, Batch Loss: 25.157881
[2025-04-30 19:28:33.246] Epoch 53/100, Batch 1/4, Batch Loss: 24.258455
[2025-04-30 19:28:33.252] Epoch 53/100, Batch 4/4, Batch Loss: 25.038366
[2025-04-30 19:28:33.258] Epoch 54/100, Batch 1/4, Batch Loss: 22.056629
[2025-04-30 19:28:33.268] Epoch 54/100, Batch 4/4, Batch Loss: 24.823826
[2025-04-30 19:28:33.272] Epoch 55/100, Batch 1/4, Batch Loss: 23.397381
[2025-04-30 19:28:33.276] Epoch 55/100, Batch 4/4, Batch Loss: 20.506641
[2025-04-30 19:28:33.282] Epoch 56/100, Batch 1/4, Batch Loss: 23.042355
[2025-04-30 19:28:33.287] Epoch 56/100, Batch 4/4, Batch Loss: 18.733490
[2025-04-30 19:28:33.290] Epoch 57/100, Batch 1/4, Batch Loss: 19.506779
[2025-04-30 19:28:33.298] Epoch 57/100, Batch 4/4, Batch Loss: 20.628786
[2025-04-30 19:28:33.302] Epoch 58/100, Batch 1/4, Batch Loss: 19.867359
[2025-04-30 19:28:33.308] Epoch 58/100, Batch 4/4, Batch Loss: 20.188946
[2025-04-30 19:28:33.315] Epoch 59/100, Batch 1/4, Batch Loss: 18.586159
[2025-04-30 19:28:33.356] Epoch 59/100, Batch 4/4, Batch Loss: 18.279095
[2025-04-30 19:28:33.361] Epoch 60/100, Batch 1/4, Batch Loss: 18.126432
[2025-04-30 19:28:33.367] Epoch 60/100, Batch 4/4, Batch Loss: 14.813745
[2025-04-30 19:28:33.398] Epoch 61/100, Batch 1/4, Batch Loss: 17.074863
[2025-04-30 19:28:33.407] Epoch 61/100, Batch 4/4, Batch Loss: 15.257046
[2025-04-30 19:28:33.413] Epoch 61/100, Average Loss: 15.905288, Mean Absolute Error: 3.914403
[2025-04-30 19:28:33.421] Epoch 62/100, Batch 1/4, Batch Loss: 13.550612
[2025-04-30 19:28:33.426] Epoch 62/100, Batch 4/4, Batch Loss: 14.690270
[2025-04-30 19:28:33.432] Epoch 63/100, Batch 1/4, Batch Loss: 13.491112
[2025-04-30 19:28:33.438] Epoch 63/100, Batch 4/4, Batch Loss: 12.829569
[2025-04-30 19:28:33.445] Epoch 64/100, Batch 1/4, Batch Loss: 11.498376
[2025-04-30 19:28:33.452] Epoch 64/100, Batch 4/4, Batch Loss: 13.190695
[2025-04-30 19:28:33.471] Epoch 65/100, Batch 1/4, Batch Loss: 12.683626
[2025-04-30 19:28:33.480] Epoch 65/100, Batch 4/4, Batch Loss: 13.925201
[2025-04-30 19:28:33.485] Epoch 66/100, Batch 1/4, Batch Loss: 12.967624
[2025-04-30 19:28:33.490] Epoch 66/100, Batch 4/4, Batch Loss: 12.734449
[2025-04-30 19:28:33.495] Epoch 67/100, Batch 1/4, Batch Loss: 12.172451
[2025-04-30 19:28:33.502] Epoch 67/100, Batch 4/4, Batch Loss: 11.100880
[2025-04-30 19:28:33.505] Epoch 68/100, Batch 1/4, Batch Loss: 11.280186
[2025-04-30 19:28:33.510] Epoch 68/100, Batch 4/4, Batch Loss: 11.161272
[2025-04-30 19:28:33.516] Epoch 69/100, Batch 1/4, Batch Loss: 10.110331
[2025-04-30 19:28:33.520] Epoch 69/100, Batch 4/4, Batch Loss: 12.222969
[2025-04-30 19:28:33.524] Epoch 70/100, Batch 1/4, Batch Loss: 9.871469
[2025-04-30 19:28:33.531] Epoch 70/100, Batch 4/4, Batch Loss: 10.248388
[2025-04-30 19:28:33.535] Epoch 71/100, Batch 1/4, Batch Loss: 9.924362
[2025-04-30 19:28:33.547] Epoch 71/100, Batch 4/4, Batch Loss: 9.141717
[2025-04-30 19:28:33.555] Epoch 71/100, Average Loss: 9.761502, Mean Absolute Error: 3.068060
[2025-04-30 19:28:33.559] Epoch 72/100, Batch 1/4, Batch Loss: 9.306902
[2025-04-30 19:28:33.566] Epoch 72/100, Batch 4/4, Batch Loss: 9.441669
[2025-04-30 19:28:33.570] Epoch 73/100, Batch 1/4, Batch Loss: 8.879135
[2025-04-30 19:28:33.579] Epoch 73/100, Batch 4/4, Batch Loss: 8.646286
[2025-04-30 19:28:33.583] Epoch 74/100, Batch 1/4, Batch Loss: 9.024102
[2025-04-30 19:28:33.626] Epoch 74/100, Batch 4/4, Batch Loss: 7.333499
[2025-04-30 19:28:33.771] Epoch 75/100, Batch 1/4, Batch Loss: 7.987123
[2025-04-30 19:28:33.781] Epoch 75/100, Batch 4/4, Batch Loss: 7.875538
[2025-04-30 19:28:33.786] Epoch 76/100, Batch 1/4, Batch Loss: 8.494083
[2025-04-30 19:28:33.804] Epoch 76/100, Batch 4/4, Batch Loss: 7.125979
[2025-04-30 19:28:33.808] Epoch 77/100, Batch 1/4, Batch Loss: 8.821074
[2025-04-30 19:28:33.815] Epoch 77/100, Batch 4/4, Batch Loss: 7.452907
[2025-04-30 19:28:33.819] Epoch 78/100, Batch 1/4, Batch Loss: 6.453465
[2025-04-30 19:28:33.825] Epoch 78/100, Batch 4/4, Batch Loss: 6.951849
[2025-04-30 19:28:33.831] Epoch 79/100, Batch 1/4, Batch Loss: 6.418497
[2025-04-30 19:28:33.837] Epoch 79/100, Batch 4/4, Batch Loss: 5.804357
[2025-04-30 19:28:33.842] Epoch 80/100, Batch 1/4, Batch Loss: 6.374564
[2025-04-30 19:28:33.850] Epoch 80/100, Batch 4/4, Batch Loss: 6.673484
[2025-04-30 19:28:33.868] Epoch 81/100, Batch 1/4, Batch Loss: 6.490993
[2025-04-30 19:28:33.875] Epoch 81/100, Batch 4/4, Batch Loss: 6.845324
[2025-04-30 19:28:33.889] Epoch 81/100, Average Loss: 6.176466, Mean Absolute Error: 2.440105
[2025-04-30 19:28:33.907] Epoch 82/100, Batch 1/4, Batch Loss: 5.783945
[2025-04-30 19:28:33.914] Epoch 82/100, Batch 4/4, Batch Loss: 6.099806
[2025-04-30 19:28:33.925] Epoch 83/100, Batch 1/4, Batch Loss: 5.844983
[2025-04-30 19:28:33.932] Epoch 83/100, Batch 4/4, Batch Loss: 4.800236
[2025-04-30 19:28:33.938] Epoch 84/100, Batch 1/4, Batch Loss: 5.438835
[2025-04-30 19:28:33.947] Epoch 84/100, Batch 4/4, Batch Loss: 4.404550
[2025-04-30 19:28:33.955] Epoch 85/100, Batch 1/4, Batch Loss: 4.583347
[2025-04-30 19:28:33.964] Epoch 85/100, Batch 4/4, Batch Loss: 5.128870
[2025-04-30 19:28:33.990] Epoch 86/100, Batch 1/4, Batch Loss: 5.558029
[2025-04-30 19:28:34.000] Epoch 86/100, Batch 4/4, Batch Loss: 5.482007
[2025-04-30 19:28:34.013] Epoch 87/100, Batch 1/4, Batch Loss: 4.600142
[2025-04-30 19:28:34.020] Epoch 87/100, Batch 4/4, Batch Loss: 4.738224
[2025-04-30 19:28:34.024] Epoch 88/100, Batch 1/4, Batch Loss: 5.007065
[2025-04-30 19:28:34.031] Epoch 88/100, Batch 4/4, Batch Loss: 4.771507
[2025-04-30 19:28:34.035] Epoch 89/100, Batch 1/4, Batch Loss: 5.203652
[2025-04-30 19:28:34.040] Epoch 89/100, Batch 4/4, Batch Loss: 4.652297
[2025-04-30 19:28:34.047] Epoch 90/100, Batch 1/4, Batch Loss: 4.266953
[2025-04-30 19:28:34.052] Epoch 90/100, Batch 4/4, Batch Loss: 3.837036
[2025-04-30 19:28:34.056] Epoch 91/100, Batch 1/4, Batch Loss: 4.300825
[2025-04-30 19:28:34.063] Epoch 91/100, Batch 4/4, Batch Loss: 4.008062
[2025-04-30 19:28:34.075] Epoch 91/100, Average Loss: 4.005413, Mean Absolute Error: 1.962025
[2025-04-30 19:28:34.081] Epoch 92/100, Batch 1/4, Batch Loss: 4.447447
[2025-04-30 19:28:34.086] Epoch 92/100, Batch 4/4, Batch Loss: 3.484192
[2025-04-30 19:28:34.092] Epoch 93/100, Batch 1/4, Batch Loss: 3.518643
[2025-04-30 19:28:34.108] Epoch 93/100, Batch 4/4, Batch Loss: 3.433716
[2025-04-30 19:28:34.116] Epoch 94/100, Batch 1/4, Batch Loss: 3.609279
[2025-04-30 19:28:34.124] Epoch 94/100, Batch 4/4, Batch Loss: 3.517057
[2025-04-30 19:28:34.150] Epoch 95/100, Batch 1/4, Batch Loss: 3.835632
[2025-04-30 19:28:34.160] Epoch 95/100, Batch 4/4, Batch Loss: 3.146121
[2025-04-30 19:28:34.166] Epoch 96/100, Batch 1/4, Batch Loss: 3.686440
[2025-04-30 19:28:34.171] Epoch 96/100, Batch 4/4, Batch Loss: 3.122259
[2025-04-30 19:28:34.176] Epoch 97/100, Batch 1/4, Batch Loss: 2.850266
[2025-04-30 19:28:34.192] Epoch 97/100, Batch 4/4, Batch Loss: 3.228275
[2025-04-30 19:28:34.197] Epoch 98/100, Batch 1/4, Batch Loss: 3.277637
[2025-04-30 19:28:34.203] Epoch 98/100, Batch 4/4, Batch Loss: 2.885861
[2025-04-30 19:28:34.207] Epoch 99/100, Batch 1/4, Batch Loss: 3.273483
[2025-04-30 19:28:34.214] Epoch 99/100, Batch 4/4, Batch Loss: 2.864765
[2025-04-30 19:28:34.218] Epoch 100/100, Batch 1/4, Batch Loss: 3.043433
[2025-04-30 19:28:34.223] Epoch 100/100, Batch 4/4, Batch Loss: 2.859061
[2025-04-30 19:28:34.227] Epoch 100/100, Average Loss: 2.764549, Mean Absolute Error: 1.627609
[2025-04-30 19:28:34.232] Model A training completed
[2025-04-30 19:28:34.284] Model A Final Predictions Shape: 16,1
[2025-04-30 19:28:34.289] Model A Final Predictions (First few): [2.750352, 2.7034044, 2.9580672, 2.5592737, 2.736664, 2.6734867, 2.2127323, 3.0338564, 2.6606913, 2.6618104...]
[2025-04-30 19:28:34.339] Model A Final Mean Absolute Error: 1.627609
[2025-04-30 19:28:34.369] Step 7 - Model A trained and saved to RuntimeProcessingContext and Results Store.
[2025-04-30 19:28:34.377] Workflow Session 1: Step 8 - Generating future performance projection for customer 1.
[2025-04-30 19:28:34.426] Workflow Session 1: Step 8 - Future performance projection completed: PerformanceProjection_Cust_1_Outcome_PotentialChallenges_ComplexModel_Score_0.1062_TrainError_1.6276
[2025-04-30 19:28:34.432] Workflow Session 1: Workflow completed for customer 1 with final score 0.1062
[2025-04-30 19:28:34.439] Workflow Session 1: Workflow completed with result: Workflow_Complete_Cust_1_FinalScore_0.1062
[2025-04-30 19:28:34.467] Workflow Session 1: Starting Parallel Processing Unit B for customer 1.
[2025-04-30 19:28:34.472] Workflow Session 1: Starting multi-stage workflow (Unit B) for customer 1.
[2025-04-30 19:28:34.487] Workflow Session 1: Step 1 (Unit B) - Acquiring data and analyzing initial features for customer 1.
[2025-04-30 19:28:34.490] Step 1 (Unit B) - Processing Product Data (3 items).
[2025-04-30 19:28:34.494] Unit B Product QuantityAvailable: [10, 20, 15]
[2025-04-30 19:28:34.499] Unit B Product MonetaryValue: [99.99, 149.99, 199.99]
[2025-04-30 19:28:34.502] Unit B Product CostContributionValue: [0.15, 0.25, 0.2]
[2025-04-30 19:28:34.536] K-means centroids for Product QuantityAvailable (Unit B): [20.0000, 15.0000, 10.0000]
[2025-04-30 19:28:34.539] Central point for Product QuantityAvailable (Unit B): 15
[2025-04-30 19:28:34.542] Normalized value for Product QuantityAvailable (Unit B): 0.7500, Category: Positive High
[2025-04-30 19:28:34.547] Normalized XYZ coordinates for Product QuantityAvailable (Unit B): (1.0000, 0.7500, 0.5000)
[2025-04-30 19:28:34.551] K-means centroids for Product MonetaryValue (Unit B): [199.9900, 149.9900, 99.9900]
[2025-04-30 19:28:34.553] Central point for Product MonetaryValue (Unit B): 149.99
[2025-04-30 19:28:34.556] Normalized value for Product MonetaryValue (Unit B): 0.7500, Category: Positive High
[2025-04-30 19:28:34.560] Normalized XYZ coordinates for Product MonetaryValue (Unit B): (1.0000, 0.7500, 0.5000)
[2025-04-30 19:28:34.566] K-means centroids for Product CostContributionValue (Unit B): [0.2500, 0.2000, 0.1500]
[2025-04-30 19:28:34.568] Central point for Product CostContributionValue (Unit B): 0.19999999999999998
[2025-04-30 19:28:34.571] Normalized value for Product CostContributionValue (Unit B): 0.8000, Category: Positive High
[2025-04-30 19:28:34.574] Normalized XYZ coordinates for Product CostContributionValue (Unit B): (1.0000, 0.8000, 0.6000)
[2025-04-30 19:28:34.581] Step 1 (Unit B) - Processing Service Data (3 items).
[2025-04-30 19:28:34.585] Unit B Service FulfillmentQuantity: [5, 10, 8]
[2025-04-30 19:28:34.589] Unit B Service MonetaryValue: [299.99, 399.99, 599.99]
[2025-04-30 19:28:34.593] Unit B Service CostContributionValue: [0.3, 0.35, 0.4]
[2025-04-30 19:28:34.601] Workflow Session 1: Parallel Processing Unit A finished.
[2025-04-30 19:28:34.609] K-means centroids for Service FulfillmentQuantity (Unit B): [10.0000, 8.0000, 5.0000]
[2025-04-30 19:28:34.614] Central point for Service FulfillmentQuantity (Unit B): 7.666666666666667
[2025-04-30 19:28:34.617] Normalized value for Service FulfillmentQuantity (Unit B): 0.7667, Category: Positive High
[2025-04-30 19:28:34.620] Normalized XYZ coordinates for Service FulfillmentQuantity (Unit B): (1.0000, 0.8000, 0.5000)
[2025-04-30 19:28:34.623] K-means centroids for Service MonetaryValue (Unit B): [599.9900, 399.9900, 299.9900]
[2025-04-30 19:28:34.628] Central point for Service MonetaryValue (Unit B): 433.3233333333333
[2025-04-30 19:28:34.633] Normalized value for Service MonetaryValue (Unit B): 0.7222, Category: Positive High
[2025-04-30 19:28:34.636] Normalized XYZ coordinates for Service MonetaryValue (Unit B): (1.0000, 0.6667, 0.5000)
[2025-04-30 19:28:34.642] K-means centroids for Service CostContributionValue (Unit B): [0.4000, 0.3500, 0.3000]
[2025-04-30 19:28:34.665] Central point for Service CostContributionValue (Unit B): 0.35000000000000003
[2025-04-30 19:28:34.668] Normalized value for Service CostContributionValue (Unit B): 0.8750, Category: Positive High
[2025-04-30 19:28:34.673] Normalized XYZ coordinates for Service CostContributionValue (Unit B): (1.0000, 0.8750, 0.7500)
[2025-04-30 19:28:34.681] Workflow Session 1: Step 1 (Unit B) - Data acquisition and initial analysis completed: InitialAnalysis_B_Cust_1_Record_3
[2025-04-30 19:28:34.702] Workflow Session 1: Step 2 (Unit B) - Generating feature tensors and mapping trajectories for customer 1.
[2025-04-30 19:28:34.706] Step 2 (Unit B) - Retrieving coordinates from Step 1 analysis.
[2025-04-30 19:28:34.710] Step 2 (Unit B) - Calculating tensors, magnitudes, and trajectories.
[2025-04-30 19:28:34.717] ----- PRODUCT TENSOR AND MAGNITUDE CALCULATIONS (Unit B) -----
[2025-04-30 19:28:34.723] Product Overall Tensor (Unit B): (1.0000, 0.7667, 0.5333)
[2025-04-30 19:28:34.746] Product Overall Magnitude (Unit B): 1.3683
[2025-04-30 19:28:34.758] Product Trajectory (Unit B): (0.7308, 0.5603, 0.3898)
[2025-04-30 19:28:34.768] ----- SERVICE TENSOR AND MAGNITUDE CALCULATIONS (Unit B) -----
[2025-04-30 19:28:34.772] Service Overall Tensor (Unit B): (1.0000, 0.7806, 0.5833)
[2025-04-30 19:28:34.781] Service Overall Magnitude (Unit B): 1.3963
[2025-04-30 19:28:34.786] Service Trajectory (Unit B): (0.7162, 0.5590, 0.4178)
[2025-04-30 19:28:34.790] ----- TRAJECTORY PLOT GENERATION & ANALYSIS (Unit B) -----
[2025-04-30 19:28:34.794] Unit B Inverted trajectory from (0.7308, 0.5603, 0.3898) to (-0.7308, -0.5603, 0.3898)
[2025-04-30 19:28:34.800] Unit B Inverted trajectory from (0.7162, 0.5590, 0.4178) to (-0.7162, -0.5590, 0.4178)
[2025-04-30 19:28:34.806] Generating Product trajectory recursive plot (Unit B)
[2025-04-30 19:28:34.815] Unit B PRODUCT_B point 0: Position=(1.000000, 0.766662, 0.5333), Intensity=1.3683
[2025-04-30 19:28:34.827] Unit B PRODUCT_B point 2: Position=(-0.425000, -0.325832, 1.2933), Intensity=1.1083 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.837] Unit B PRODUCT_B point 3: Position=(-1.032500, -0.791579, 1.6173), Intensity=0.9975 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.841] Unit B PRODUCT_B point 4: Position=(-1.579250, -1.210752, 1.9089), Intensity=0.8977 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.843] Unit B PRODUCT_B recursion stopped - Reached target negative threshold at depth 5
[2025-04-30 19:28:34.852] Unit B PRODUCT_B final position: (-2.071325, -1.588007, 2.1713)
[2025-04-30 19:28:34.857] Generating Service trajectory recursive plot (Unit B)
[2025-04-30 19:28:34.868] Unit B SERVICE_B point 0: Position=(1.000000, 0.780554, 0.5833), Intensity=1.3963
[2025-04-30 19:28:34.877] Unit B SERVICE_B point 2: Position=(-0.425000, -0.331735, 1.4146), Intensity=1.1310 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.882] Unit B SERVICE_B point 3: Position=(-1.032500, -0.805922, 1.7689), Intensity=1.0179 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.884] Unit B SERVICE_B point 4: Position=(-1.579250, -1.232689, 2.0879), Intensity=0.9161 BEYOND-X-PLANE BEYOND-Y-PLANE
[2025-04-30 19:28:34.887] Unit B SERVICE_B recursion stopped - Reached target negative threshold at depth 5
[2025-04-30 19:28:34.890] Unit B SERVICE_B final position: (-2.071325, -1.616780, 2.3749)
[2025-04-30 19:28:34.935] ----- PLANE INTERSECTION ANALYSIS (Unit B) -----
[2025-04-30 19:28:34.938] Product X-Plane Intersection (Unit B): (0.000000, 0.000000, 1.066650)
[2025-04-30 19:28:34.947] Product Y-Plane Intersection (Unit B): (-0.000000, 0.000000, 1.066650)
[2025-04-30 19:28:34.950] Service X-Plane Intersection (Unit B): (0.000000, -0.000000, 1.166661)
[2025-04-30 19:28:34.955] Service Y-Plane Intersection (Unit B): (0.000000, 0.000000, 1.166661)
[2025-04-30 19:28:34.963] ----- KEY TRAJECTORY DATA (Unit B) -----
[2025-04-30 19:28:34.968] Product Vector (Unit B): (-0.730841, -0.560309, 0.389776)
[2025-04-30 19:28:34.975] Product Velocity (Unit B): 1.368286
[2025-04-30 19:28:34.983] Product Positive Coordinate (Unit B): (1.000000, 0.766662, 0.533325)
[2025-04-30 19:28:35.058] Product Negative Coordinate (Unit B): (-0.425000, -0.325832, 1.293313)
[2025-04-30 19:28:35.065] Service Vector (Unit B): (-0.716200, -0.559032, 0.417781)
[2025-04-30 19:28:35.069] Service Velocity (Unit B): 1.396259
[2025-04-30 19:28:35.072] Service Positive Coordinate (Unit B): (1.000000, 0.780554, 0.583331)
[2025-04-30 19:28:35.075] Service Negative Coordinate (Unit B): (-0.425000, -0.331735, 1.414577)
[2025-04-30 19:28:35.081] Product negative X count (Unit B): 4
[2025-04-30 19:28:35.085] Product negative Y count (Unit B): 4
[2025-04-30 19:28:35.092] Product negative both count (Unit B): 4
[2025-04-30 19:28:35.098] Service negative X count (Unit B): 4
[2025-04-30 19:28:35.107] Service negative Y count (Unit B): 4
[2025-04-30 19:28:35.111] Service negative both count (Unit B): 4
[2025-04-30 19:28:35.141] Product trajectory plot (Unit B): 6 points, 4 in negative X-Y quadrant
[2025-04-30 19:28:35.165] Service trajectory plot (Unit B): 6 points, 4 in negative X-Y quadrant
[2025-04-30 19:28:35.266] Workflow Session 1: Step 2 (Unit B) - Feature tensor generation and mapping completed: FeatureTensorsAndMapping_B_Cust_1_BasedOn_Cust_1_Record_3
[2025-04-30 19:28:35.277] Workflow Session 1: Step 3 (Unit B) - Creating processed feature definition for customer 1.
[2025-04-30 19:28:35.291] Workflow Session 1: Step 3 (Unit B) - Processed feature definition created: ProcessedFeatures_B_Cust_1_Level_PremiumB_ConvergentB
[2025-04-30 19:28:35.303] Workflow Session 1: Step 4 (Unit B) - Assessing feature quality for customer 1.
[2025-04-30 19:28:35.311] QA product trajectory stability (Unit B): 1.0000
[2025-04-30 19:28:35.318] QA intersection quality (Unit B): 1.0000
[2025-04-30 19:28:35.321] QA final score (Unit B): 1.0000, level: 5
[2025-04-30 19:28:35.324] Workflow Session 1: Step 4 (Unit B) - Feature quality assessment completed: QualityAssessment_B_Passed_Level_5_V1.38_S1.00_I1.00
[2025-04-30 19:28:35.331] Workflow Session 1: Step 5 (Unit B) - Evaluating combined features for customer 1.
[2025-04-30 19:28:35.335] Workflow Session 1: Step 5 (Unit B) - Combined feature evaluation calculation.
[2025-04-30 19:28:35.338] Base Score: 0.7167
[2025-04-30 19:28:35.350] Velocity Bonus: 0.7899 (Product B: 1.3683, Service B: 1.3963)
[2025-04-30 19:28:35.360] Alignment Bonus: 0.2499 (Alignment Score B: 0.9997)
[2025-04-30 19:28:35.369] Negative Trajectory Bonus (Unit B): 0.2000 (Total Negative Points B: 8)
[2025-04-30 19:28:35.373] Final Score (Unit B): 1.0000
[2025-04-30 19:28:35.387] Workflow Session 1: Step 6 (Unit B) - Performing fractal optimization analysis for customer 1.
========== PRODUCT INTERSECTIONS (Unit B) ==========
Product X-Plane Intersection (Unit B): (0.0, 0.000000, 1.066650)
Product Y-Plane Intersection (Unit B): (-0.000000, 0.0, 1.066650)
========== SERVICE INTERSECTIONS (Unit B) ==========
Service X-Plane Intersection (Unit B): (0.0, -0.000000, 1.166661)
Service Y-Plane Intersection (Unit B): (0.000000, 0.0, 1.166661)
========== INTERSECTION VELOCITIES (Unit B) ==========
Product X-Plane Velocity (Unit B): 1.3683
Product Y-Plane Velocity (Unit B): 1.3683
Service X-Plane Velocity (Unit B): 1.3963
Service Y-Plane Velocity (Unit B): 1.3963
========== VELOCITY SOURCES (Unit B) ==========
ProductX_B Source Position (Unit B): (0.0000, 0.0000, 1.0667), Velocity: 1.5051
ProductY_B Source Position (Unit B): (-0.0000, 0.0000, 1.0667), Velocity: 1.5051
ServiceX_B Source Position (Unit B): (0.0000, -0.0000, 1.1667), Velocity: 1.5359
ServiceY_B Source Position (Unit B): (0.0000, 0.0000, 1.1667), Velocity: 1.5359
========== SAMPLE POINTS (Unit B) ==========
Sample 1 Coordinates (Unit B): (-0.0500, 0.0000, 1.0667)
Sample 2 Coordinates (Unit B): (-0.0000, -0.0500, 1.0667)
Sample 3 Coordinates (Unit B): (-0.0500, -0.0000, 1.1667)
Sample 4 Coordinates (Unit B): (0.0000, -0.0500, 1.1667)
Sample 5 Coordinates (Unit B): (-0.0000, 0.0000, 1.1167)
Sample 6 Coordinates (Unit B): (0.0000, 0.0000, 0.0000)
========== PROCESSING SAMPLE 1 (Unit B) ==========
Starting point (Unit B): (-0.0500, 0.0000, 1.0667)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(-0.050000, 0.000000, 1.066650), r=1.067821
  Unit B Contribution from ProductX_B: 1.269812 (distance: 0.0500)
  Unit B Contribution from ProductY_B: 1.269812 (distance: 0.0500)
  Unit B Contribution from ServiceX_B: 1.159330 (distance: 0.1118)
  Unit B Contribution from ServiceY_B: 1.159330 (distance: 0.1118)
Unit B Escaped at iteration 3
Final Sample 1 Results (Unit B):
  Final z value (Unit B): (0.361185, 0.000000, 2.490974)
  Iterations (Unit B): 2
  Total diffused velocity (Unit B): 5.675781
  Contributions breakdown (Unit B):
    ProductX_B: 1.490479
    ProductY_B: 1.490479
    ServiceX_B: 1.347411
    ServiceY_B: 1.347411
========== PROCESSING SAMPLE 2 (Unit B) ==========
Starting point (Unit B): (-0.0000, -0.0500, 1.0667)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(-0.000000, -0.050000, 1.066650), r=1.067821
  Unit B Contribution from ProductX_B: 1.269812 (distance: 0.0500)
  Unit B Contribution from ProductY_B: 1.269812 (distance: 0.0500)
  Unit B Contribution from ServiceX_B: 1.159330 (distance: 0.1118)
  Unit B Contribution from ServiceY_B: 1.159330 (distance: 0.1118)
Unit B Escaped at iteration 3
Final Sample 2 Results (Unit B):
  Final z value (Unit B): (-0.411185, -0.050000, 2.490974)
  Iterations (Unit B): 2
  Total diffused velocity (Unit B): 5.675781
  Contributions breakdown (Unit B):
    ProductX_B: 1.490479
    ProductY_B: 1.490479
    ServiceX_B: 1.347411
    ServiceY_B: 1.347411
========== PROCESSING SAMPLE 3 (Unit B) ==========
Starting point (Unit B): (-0.0500, -0.0000, 1.1667)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(-0.050000, -0.000000, 1.166661), r=1.167732
  Unit B Contribution from ProductX_B: 1.136104 (distance: 0.1118)
  Unit B Contribution from ProductY_B: 1.136104 (distance: 0.1118)
  Unit B Contribution from ServiceX_B: 1.295772 (distance: 0.0500)
  Unit B Contribution from ServiceY_B: 1.295772 (distance: 0.0500)
Unit B Escaped at iteration 3
Final Sample 3 Results (Unit B):
  Final z value (Unit B): (0.594444, -0.000000, 3.618870)
  Iterations (Unit B): 2
  Total diffused velocity (Unit B): 5.681248
  Contributions breakdown (Unit B):
    ProductX_B: 1.356771
    ProductY_B: 1.356771
    ServiceX_B: 1.483853
    ServiceY_B: 1.483853
========== PROCESSING SAMPLE 4 (Unit B) ==========
Starting point (Unit B): (0.0000, -0.0500, 1.1667)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(0.000000, -0.050000, 1.166661), r=1.167732
  Unit B Contribution from ProductX_B: 1.136104 (distance: 0.1118)
  Unit B Contribution from ProductY_B: 1.136104 (distance: 0.1118)
  Unit B Contribution from ServiceX_B: 1.295772 (distance: 0.0500)
  Unit B Contribution from ServiceY_B: 1.295772 (distance: 0.0500)
Unit B Escaped at iteration 3
Final Sample 4 Results (Unit B):
  Final z value (Unit B): (-0.644444, -0.050000, 3.618870)
  Iterations (Unit B): 2
  Total diffused velocity (Unit B): 5.681248
  Contributions breakdown (Unit B):
    ProductX_B: 1.356771
    ProductY_B: 1.356771
    ServiceX_B: 1.483853
    ServiceY_B: 1.483853
========== PROCESSING SAMPLE 5 (Unit B) ==========
Starting point (Unit B): (-0.0000, 0.0000, 1.1167)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(-0.000000, 0.000000, 1.116656), r=1.116656
  Unit B Contribution from ProductX_B: 1.269800 (distance: 0.0500)
  Unit B Contribution from ProductY_B: 1.269800 (distance: 0.0500)
  Unit B Contribution from ServiceX_B: 1.295759 (distance: 0.0500)
  Unit B Contribution from ServiceY_B: 1.295759 (distance: 0.0500)
Unit B Escaped at iteration 3
Final Sample 5 Results (Unit B):
  Final z value (Unit B): (-0.000000, 0.000000, 3.055377)
  Iterations (Unit B): 2
  Total diffused velocity (Unit B): 5.948613
  Contributions breakdown (Unit B):
    ProductX_B: 1.490467
    ProductY_B: 1.490467
    ServiceX_B: 1.483840
    ServiceY_B: 1.483840
========== PROCESSING SAMPLE 6 (Unit B) ==========
Starting point (Unit B): (0.0000, 0.0000, 0.0000)
Unit B Iteration 1, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.220667 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.188081 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.188081 (distance: 1.1667)
Unit B Iteration 2, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.203701 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.203701 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.173621 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.173621 (distance: 1.1667)
Unit B Iteration 3, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.188040 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.188040 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.160272 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.160272 (distance: 1.1667)
Unit B Iteration 4, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.173583 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.173583 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.147950 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.147950 (distance: 1.1667)
Unit B Iteration 5, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.160237 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.160237 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.136575 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.136575 (distance: 1.1667)
Unit B Iteration 6, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.147917 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.147917 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.126074 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.126074 (distance: 1.1667)
Unit B Iteration 7, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.136545 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.136545 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.116381 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.116381 (distance: 1.1667)
Unit B Iteration 8, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.126047 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.126047 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.107433 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.107433 (distance: 1.1667)
Unit B Iteration 9, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.116356 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.116356 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.099174 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.099174 (distance: 1.1667)
Unit B Iteration 10, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.107410 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.107410 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.091549 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.091549 (distance: 1.1667)
Unit B Iteration 11, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.099152 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.099152 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.084510 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.084510 (distance: 1.1667)
Unit B Iteration 12, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.091529 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.091529 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.078013 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.078013 (distance: 1.1667)
Unit B Iteration 13, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.084492 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.084492 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.072015 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.072015 (distance: 1.1667)
Unit B Iteration 14, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.077996 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.077996 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.066478 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.066478 (distance: 1.1667)
Unit B Iteration 15, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.071999 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.071999 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.061367 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.061367 (distance: 1.1667)
Unit B Iteration 16, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.066464 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.066464 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.056649 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.056649 (distance: 1.1667)
Unit B Iteration 17, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.061354 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.061354 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.052293 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.052293 (distance: 1.1667)
Unit B Iteration 18, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.056637 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.056637 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.048273 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.048273 (distance: 1.1667)
Unit B Iteration 19, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.052282 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.052282 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.044562 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.044562 (distance: 1.1667)
Unit B Iteration 20, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.048262 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.048262 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.041136 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.041136 (distance: 1.1667)
Unit B Iteration 21, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.044552 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.044552 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.037973 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.037973 (distance: 1.1667)
Unit B Iteration 22, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.041127 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.041127 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.035053 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.035053 (distance: 1.1667)
Unit B Iteration 23, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.037965 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.037965 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.032358 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.032358 (distance: 1.1667)
Unit B Iteration 24, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.035046 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.035046 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.029871 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.029871 (distance: 1.1667)
Unit B Iteration 25, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.032351 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.032351 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.027574 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.027574 (distance: 1.1667)
Unit B Iteration 26, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.029864 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.029864 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.025454 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.025454 (distance: 1.1667)
Unit B Iteration 27, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.027568 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.027568 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.023497 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.023497 (distance: 1.1667)
Unit B Iteration 28, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.025448 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.025448 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.021690 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.021690 (distance: 1.1667)
Unit B Iteration 29, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.023492 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.023492 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.020023 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.020023 (distance: 1.1667)
Unit B Iteration 30, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.021686 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.021686 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.018483 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.018483 (distance: 1.1667)
Unit B Iteration 31, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.020018 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.020018 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.017062 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.017062 (distance: 1.1667)
Unit B Iteration 32, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.018479 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.018479 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.015750 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.015750 (distance: 1.1667)
Unit B Iteration 33, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.017059 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.017059 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.014540 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.014540 (distance: 1.1667)
Unit B Iteration 34, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.015747 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.015747 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.013422 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.013422 (distance: 1.1667)
Unit B Iteration 35, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.014536 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.014536 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.012390 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.012390 (distance: 1.1667)
Unit B Iteration 36, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.013419 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.013419 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.011437 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.011437 (distance: 1.1667)
Unit B Iteration 37, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.012387 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.012387 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.010558 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.010558 (distance: 1.1667)
Unit B Iteration 38, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.011435 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.011435 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.009746 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.009746 (distance: 1.1667)
Unit B Iteration 39, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.010556 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.010556 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.008997 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.008997 (distance: 1.1667)
Unit B Iteration 40, z=(0.000000, 0.000000, 0.000000), r=0.000000
  Unit B Contribution from ProductX_B: 0.009744 (distance: 1.0667)
  Unit B Contribution from ProductY_B: 0.009744 (distance: 1.0667)
  Unit B Contribution from ServiceX_B: 0.008305 (distance: 1.1667)
  Unit B Contribution from ServiceY_B: 0.008305 (distance: 1.1667)
Final Sample 6 Results (Unit B):
  Final z value (Unit B): (0.000000, 0.000000, 0.000000)
  Iterations (Unit B): 40
  Total diffused velocity (Unit B): 10.199472
  Contributions breakdown (Unit B):
    ProductX_B: 2.753148
    ProductY_B: 2.753148
    ServiceX_B: 2.346588
    ServiceY_B: 2.346588
[2025-04-30 19:28:37.602] Workflow Session 1: Step 6 (Unit B) - Fractal optimization analysis completed: OptimizationAnalysis_B_Cust_1_V[PX_B:1.368,PY_B:1.368,SX_B:1.396,SY_B:1.396]_S[P1_B:5.6758_SEscaped(2),P2_B:5.6758_SEscaped(2),P3_B:5.6812_SEscaped(2),P4_B:5.6812_SEscaped(2),P5_B:5.9486_SEscaped(2),P6_B:10.1995_SInSet]
[2025-04-30 19:28:37.688] Workflow Session 1: Step 7 (Unit B) - Training tensor network for customer 1 using Actual TF.NET Model B.
[2025-04-30 19:28:37.691] Disabled eager execution for TensorFlow operations for Unit B.
[2025-04-30 19:28:37.693] Step 7 (Unit B) - Creating sample training data.
[2025-04-30 19:28:37.697] Created 16 numerical samples and 16 word-based samples (Unit B).
[2025-04-30 19:28:37.704] Step 7 (Unit B) - Initializing Model B Architecture.
[2025-04-30 19:28:37.707] Model B architecture parameters:
[2025-04-30 19:28:37.710]  - Total Input Feature Count: 14
[2025-04-30 19:28:37.713]  - Hidden Layer Size (Model B): 70
[2025-04-30 19:28:37.719] Using session graph context (Unit B) for defining operations.
[2025-04-30 19:28:37.811] TensorFlow operations defined within session graph context (Unit B).
[2025-04-30 19:28:37.913] Model B - Actual TensorFlow.NET variables initialized.
[2025-04-30 19:28:38.230] Epoch 1/80, Batch 1/3, Batch Loss (Unit B): 0.105378
[2025-04-30 19:28:38.238] Epoch 1/80, Batch 3/3, Batch Loss (Unit B): 0.138235
[2025-04-30 19:28:38.274] Epoch 1/80, Average Loss (Unit B): 0.094634, Mean Absolute Error (Unit B): 0.204843
[2025-04-30 19:28:38.278] Epoch 2/80, Batch 1/3, Batch Loss (Unit B): 0.123030
[2025-04-30 19:28:38.285] Epoch 2/80, Batch 3/3, Batch Loss (Unit B): 0.056405
[2025-04-30 19:28:38.289] Epoch 3/80, Batch 1/3, Batch Loss (Unit B): 0.078420
[2025-04-30 19:28:38.293] Epoch 3/80, Batch 3/3, Batch Loss (Unit B): 0.172994
[2025-04-30 19:28:38.297] Epoch 4/80, Batch 1/3, Batch Loss (Unit B): 0.049852
[2025-04-30 19:28:38.303] Epoch 4/80, Batch 3/3, Batch Loss (Unit B): 0.151458
[2025-04-30 19:28:38.307] Epoch 5/80, Batch 1/3, Batch Loss (Unit B): 0.130349
[2025-04-30 19:28:38.312] Epoch 5/80, Batch 3/3, Batch Loss (Unit B): 0.025955
[2025-04-30 19:28:38.319] Epoch 6/80, Batch 1/3, Batch Loss (Unit B): 0.100851
[2025-04-30 19:28:38.339] Epoch 6/80, Batch 3/3, Batch Loss (Unit B): 0.015571
[2025-04-30 19:28:38.447] Epoch 7/80, Batch 1/3, Batch Loss (Unit B): 0.073358
[2025-04-30 19:28:38.519] Epoch 7/80, Batch 3/3, Batch Loss (Unit B): 0.023763
[2025-04-30 19:28:38.525] Epoch 8/80, Batch 1/3, Batch Loss (Unit B): 0.090242
[2025-04-30 19:28:38.532] Epoch 8/80, Batch 3/3, Batch Loss (Unit B): 0.093964
[2025-04-30 19:28:38.537] Epoch 9/80, Batch 1/3, Batch Loss (Unit B): 0.102877
[2025-04-30 19:28:38.542] Epoch 9/80, Batch 3/3, Batch Loss (Unit B): 0.114243
[2025-04-30 19:28:38.547] Epoch 10/80, Batch 1/3, Batch Loss (Unit B): 0.078046
[2025-04-30 19:28:38.553] Epoch 10/80, Batch 3/3, Batch Loss (Unit B): 0.078022
[2025-04-30 19:28:38.556] Epoch 11/80, Batch 1/3, Batch Loss (Unit B): 0.054334
[2025-04-30 19:28:38.562] Epoch 11/80, Batch 3/3, Batch Loss (Unit B): 0.090120
[2025-04-30 19:28:38.568] Epoch 11/80, Average Loss (Unit B): 0.074993, Mean Absolute Error (Unit B): 0.207108
[2025-04-30 19:28:38.572] Epoch 12/80, Batch 1/3, Batch Loss (Unit B): 0.095785
[2025-04-30 19:28:38.611] Epoch 12/80, Batch 3/3, Batch Loss (Unit B): 0.064575
[2025-04-30 19:28:38.620] Epoch 13/80, Batch 1/3, Batch Loss (Unit B): 0.084101
[2025-04-30 19:28:38.626] Epoch 13/80, Batch 3/3, Batch Loss (Unit B): 0.050554
[2025-04-30 19:28:38.631] Epoch 14/80, Batch 1/3, Batch Loss (Unit B): 0.113143
[2025-04-30 19:28:38.642] Epoch 14/80, Batch 3/3, Batch Loss (Unit B): 0.016219
[2025-04-30 19:28:38.655] Epoch 15/80, Batch 1/3, Batch Loss (Unit B): 0.025027
[2025-04-30 19:28:38.662] Epoch 15/80, Batch 3/3, Batch Loss (Unit B): 0.126769
[2025-04-30 19:28:38.669] Epoch 16/80, Batch 1/3, Batch Loss (Unit B): 0.096301
[2025-04-30 19:28:38.674] Epoch 16/80, Batch 3/3, Batch Loss (Unit B): 0.085942
[2025-04-30 19:28:38.681] Epoch 17/80, Batch 1/3, Batch Loss (Unit B): 0.031091
[2025-04-30 19:28:38.688] Epoch 17/80, Batch 3/3, Batch Loss (Unit B): 0.184547
[2025-04-30 19:28:38.697] Epoch 18/80, Batch 1/3, Batch Loss (Unit B): 0.031953
[2025-04-30 19:28:38.721] Epoch 18/80, Batch 3/3, Batch Loss (Unit B): 0.116892
[2025-04-30 19:28:38.726] Epoch 19/80, Batch 1/3, Batch Loss (Unit B): 0.022379
[2025-04-30 19:28:38.735] Epoch 19/80, Batch 3/3, Batch Loss (Unit B): 0.070357
[2025-04-30 19:28:38.743] Epoch 20/80, Batch 1/3, Batch Loss (Unit B): 0.061489
[2025-04-30 19:28:38.753] Epoch 20/80, Batch 3/3, Batch Loss (Unit B): 0.012799
[2025-04-30 19:28:38.758] Epoch 21/80, Batch 1/3, Batch Loss (Unit B): 0.040932
[2025-04-30 19:28:38.763] Epoch 21/80, Batch 3/3, Batch Loss (Unit B): 0.069463
[2025-04-30 19:28:38.769] Epoch 21/80, Average Loss (Unit B): 0.066999, Mean Absolute Error (Unit B): 0.191415
[2025-04-30 19:28:38.773] Epoch 22/80, Batch 1/3, Batch Loss (Unit B): 0.037665
[2025-04-30 19:28:38.777] Epoch 22/80, Batch 3/3, Batch Loss (Unit B): 0.090632
[2025-04-30 19:28:38.781] Epoch 23/80, Batch 1/3, Batch Loss (Unit B): 0.040413
[2025-04-30 19:28:38.797] Epoch 23/80, Batch 3/3, Batch Loss (Unit B): 0.088290
[2025-04-30 19:28:38.803] Epoch 24/80, Batch 1/3, Batch Loss (Unit B): 0.053705
[2025-04-30 19:28:38.807] Epoch 24/80, Batch 3/3, Batch Loss (Unit B): 0.086460
[2025-04-30 19:28:38.811] Epoch 25/80, Batch 1/3, Batch Loss (Unit B): 0.019884
[2025-04-30 19:28:38.815] Epoch 25/80, Batch 3/3, Batch Loss (Unit B): 0.070696
[2025-04-30 19:28:38.819] Epoch 26/80, Batch 1/3, Batch Loss (Unit B): 0.050009
[2025-04-30 19:28:38.824] Epoch 26/80, Batch 3/3, Batch Loss (Unit B): 0.033049
[2025-04-30 19:28:38.828] Epoch 27/80, Batch 1/3, Batch Loss (Unit B): 0.078200
[2025-04-30 19:28:38.833] Epoch 27/80, Batch 3/3, Batch Loss (Unit B): 0.016186
[2025-04-30 19:28:38.838] Epoch 28/80, Batch 1/3, Batch Loss (Unit B): 0.059776
[2025-04-30 19:28:38.843] Epoch 28/80, Batch 3/3, Batch Loss (Unit B): 0.096790
[2025-04-30 19:28:38.860] Epoch 29/80, Batch 1/3, Batch Loss (Unit B): 0.070878
[2025-04-30 19:28:38.865] Epoch 29/80, Batch 3/3, Batch Loss (Unit B): 0.011319
[2025-04-30 19:28:38.878] Epoch 30/80, Batch 1/3, Batch Loss (Unit B): 0.109732
[2025-04-30 19:28:38.884] Epoch 30/80, Batch 3/3, Batch Loss (Unit B): 0.058781
[2025-04-30 19:28:38.890] Epoch 31/80, Batch 1/3, Batch Loss (Unit B): 0.062234
[2025-04-30 19:28:38.895] Epoch 31/80, Batch 3/3, Batch Loss (Unit B): 0.078862
[2025-04-30 19:28:38.898] Epoch 31/80, Average Loss (Unit B): 0.062729, Mean Absolute Error (Unit B): 0.175357
[2025-04-30 19:28:38.903] Epoch 32/80, Batch 1/3, Batch Loss (Unit B): 0.110751
[2025-04-30 19:28:38.908] Epoch 32/80, Batch 3/3, Batch Loss (Unit B): 0.047446
[2025-04-30 19:28:38.912] Epoch 33/80, Batch 1/3, Batch Loss (Unit B): 0.034191
[2025-04-30 19:28:38.919] Epoch 33/80, Batch 3/3, Batch Loss (Unit B): 0.096497
[2025-04-30 19:28:38.923] Epoch 34/80, Batch 1/3, Batch Loss (Unit B): 0.073023
[2025-04-30 19:28:38.928] Epoch 34/80, Batch 3/3, Batch Loss (Unit B): 0.008592
[2025-04-30 19:28:38.943] Epoch 35/80, Batch 1/3, Batch Loss (Unit B): 0.031383
[2025-04-30 19:28:38.953] Epoch 35/80, Batch 3/3, Batch Loss (Unit B): 0.025770
[2025-04-30 19:28:38.970] Epoch 36/80, Batch 1/3, Batch Loss (Unit B): 0.012412
[2025-04-30 19:28:38.996] Epoch 36/80, Batch 3/3, Batch Loss (Unit B): 0.062531
[2025-04-30 19:28:39.040] Epoch 37/80, Batch 1/3, Batch Loss (Unit B): 0.079575
[2025-04-30 19:28:39.071] Epoch 37/80, Batch 3/3, Batch Loss (Unit B): 0.018024
[2025-04-30 19:28:39.075] Epoch 38/80, Batch 1/3, Batch Loss (Unit B): 0.015377
[2025-04-30 19:28:39.080] Epoch 38/80, Batch 3/3, Batch Loss (Unit B): 0.099843
[2025-04-30 19:28:39.086] Epoch 39/80, Batch 1/3, Batch Loss (Unit B): 0.021698
[2025-04-30 19:28:39.092] Epoch 39/80, Batch 3/3, Batch Loss (Unit B): 0.069487
[2025-04-30 19:28:39.099] Epoch 40/80, Batch 1/3, Batch Loss (Unit B): 0.062229
[2025-04-30 19:28:39.106] Epoch 40/80, Batch 3/3, Batch Loss (Unit B): 0.097985
[2025-04-30 19:28:39.111] Epoch 41/80, Batch 1/3, Batch Loss (Unit B): 0.118229
[2025-04-30 19:28:39.121] Epoch 41/80, Batch 3/3, Batch Loss (Unit B): 0.016523
[2025-04-30 19:28:39.127] Epoch 41/80, Average Loss (Unit B): 0.050715, Mean Absolute Error (Unit B): 0.171851
[2025-04-30 19:28:39.130] Epoch 42/80, Batch 1/3, Batch Loss (Unit B): 0.049008
[2025-04-30 19:28:39.142] Epoch 42/80, Batch 3/3, Batch Loss (Unit B): 0.025887
[2025-04-30 19:28:39.153] Epoch 43/80, Batch 1/3, Batch Loss (Unit B): 0.044278
[2025-04-30 19:28:39.160] Epoch 43/80, Batch 3/3, Batch Loss (Unit B): 0.095103
[2025-04-30 19:28:39.173] Epoch 44/80, Batch 1/3, Batch Loss (Unit B): 0.095754
[2025-04-30 19:28:39.178] Epoch 44/80, Batch 3/3, Batch Loss (Unit B): 0.012137
[2025-04-30 19:28:39.182] Epoch 45/80, Batch 1/3, Batch Loss (Unit B): 0.031658
[2025-04-30 19:28:39.197] Epoch 45/80, Batch 3/3, Batch Loss (Unit B): 0.050009
[2025-04-30 19:28:39.209] Epoch 46/80, Batch 1/3, Batch Loss (Unit B): 0.034950
[2025-04-30 19:28:39.214] Epoch 46/80, Batch 3/3, Batch Loss (Unit B): 0.080277
[2025-04-30 19:28:39.229] Epoch 47/80, Batch 1/3, Batch Loss (Unit B): 0.038218
[2025-04-30 19:28:39.238] Epoch 47/80, Batch 3/3, Batch Loss (Unit B): 0.090612
[2025-04-30 19:28:39.241] Epoch 48/80, Batch 1/3, Batch Loss (Unit B): 0.044109
[2025-04-30 19:28:39.265] Epoch 48/80, Batch 3/3, Batch Loss (Unit B): 0.045764
[2025-04-30 19:28:39.276] Epoch 49/80, Batch 1/3, Batch Loss (Unit B): 0.048501
[2025-04-30 19:28:39.319] Epoch 49/80, Batch 3/3, Batch Loss (Unit B): 0.046633
[2025-04-30 19:28:39.325] Epoch 50/80, Batch 1/3, Batch Loss (Unit B): 0.068859
[2025-04-30 19:28:39.330] Epoch 50/80, Batch 3/3, Batch Loss (Unit B): 0.014534
[2025-04-30 19:28:39.336] Epoch 51/80, Batch 1/3, Batch Loss (Unit B): 0.065734
[2025-04-30 19:28:39.342] Epoch 51/80, Batch 3/3, Batch Loss (Unit B): 0.015067
[2025-04-30 19:28:39.347] Epoch 51/80, Average Loss (Unit B): 0.046679, Mean Absolute Error (Unit B): 0.162785
[2025-04-30 19:28:39.353] Epoch 52/80, Batch 1/3, Batch Loss (Unit B): 0.035824
[2025-04-30 19:28:39.360] Epoch 52/80, Batch 3/3, Batch Loss (Unit B): 0.087296
[2025-04-30 19:28:39.368] Epoch 53/80, Batch 1/3, Batch Loss (Unit B): 0.016614
[2025-04-30 19:28:39.374] Epoch 53/80, Batch 3/3, Batch Loss (Unit B): 0.121582
[2025-04-30 19:28:39.378] Epoch 54/80, Batch 1/3, Batch Loss (Unit B): 0.057494
[2025-04-30 19:28:39.394] Epoch 54/80, Batch 3/3, Batch Loss (Unit B): 0.019175
[2025-04-30 19:28:39.398] Epoch 55/80, Batch 1/3, Batch Loss (Unit B): 0.011571
[2025-04-30 19:28:39.405] Epoch 55/80, Batch 3/3, Batch Loss (Unit B): 0.040510
[2025-04-30 19:28:39.409] Epoch 56/80, Batch 1/3, Batch Loss (Unit B): 0.019892
[2025-04-30 19:28:39.413] Epoch 56/80, Batch 3/3, Batch Loss (Unit B): 0.080625
[2025-04-30 19:28:39.421] Epoch 57/80, Batch 1/3, Batch Loss (Unit B): 0.033528
[2025-04-30 19:28:39.439] Epoch 57/80, Batch 3/3, Batch Loss (Unit B): 0.051858
[2025-04-30 19:28:39.454] Epoch 58/80, Batch 1/3, Batch Loss (Unit B): 0.047077
[2025-04-30 19:28:39.460] Epoch 58/80, Batch 3/3, Batch Loss (Unit B): 0.111546
[2025-04-30 19:28:39.468] Epoch 59/80, Batch 1/3, Batch Loss (Unit B): 0.091477
[2025-04-30 19:28:39.473] Epoch 59/80, Batch 3/3, Batch Loss (Unit B): 0.006671
[2025-04-30 19:28:39.477] Epoch 60/80, Batch 1/3, Batch Loss (Unit B): 0.058746
[2025-04-30 19:28:39.499] Epoch 60/80, Batch 3/3, Batch Loss (Unit B): 0.046798
[2025-04-30 19:28:39.505] Epoch 61/80, Batch 1/3, Batch Loss (Unit B): 0.058419
[2025-04-30 19:28:39.510] Epoch 61/80, Batch 3/3, Batch Loss (Unit B): 0.019792
[2025-04-30 19:28:39.514] Epoch 61/80, Average Loss (Unit B): 0.043773, Mean Absolute Error (Unit B): 0.160944
[2025-04-30 19:28:39.519] Epoch 62/80, Batch 1/3, Batch Loss (Unit B): 0.050210
[2025-04-30 19:28:39.524] Epoch 62/80, Batch 3/3, Batch Loss (Unit B): 0.050614
[2025-04-30 19:28:39.528] Epoch 63/80, Batch 1/3, Batch Loss (Unit B): 0.056948
[2025-04-30 19:28:39.532] Epoch 63/80, Batch 3/3, Batch Loss (Unit B): 0.008766
[2025-04-30 19:28:39.537] Epoch 64/80, Batch 1/3, Batch Loss (Unit B): 0.035959
[2025-04-30 19:28:39.541] Epoch 64/80, Batch 3/3, Batch Loss (Unit B): 0.045349
[2025-04-30 19:28:39.546] Epoch 65/80, Batch 1/3, Batch Loss (Unit B): 0.081538
[2025-04-30 19:28:39.557] Epoch 65/80, Batch 3/3, Batch Loss (Unit B): 0.038598
[2025-04-30 19:28:39.565] Epoch 66/80, Batch 1/3, Batch Loss (Unit B): 0.034866
[2025-04-30 19:28:39.570] Epoch 66/80, Batch 3/3, Batch Loss (Unit B): 0.018459
[2025-04-30 19:28:39.576] Epoch 67/80, Batch 1/3, Batch Loss (Unit B): 0.037314
[2025-04-30 19:28:39.581] Epoch 67/80, Batch 3/3, Batch Loss (Unit B): 0.049537
[2025-04-30 19:28:39.589] Epoch 68/80, Batch 1/3, Batch Loss (Unit B): 0.053582
[2025-04-30 19:28:39.594] Epoch 68/80, Batch 3/3, Batch Loss (Unit B): 0.006253
[2025-04-30 19:28:39.598] Epoch 69/80, Batch 1/3, Batch Loss (Unit B): 0.077748
[2025-04-30 19:28:39.605] Epoch 69/80, Batch 3/3, Batch Loss (Unit B): 0.015635
[2025-04-30 19:28:39.609] Epoch 70/80, Batch 1/3, Batch Loss (Unit B): 0.013586
[2025-04-30 19:28:39.613] Epoch 70/80, Batch 3/3, Batch Loss (Unit B): 0.068908
[2025-04-30 19:28:39.616] Epoch 71/80, Batch 1/3, Batch Loss (Unit B): 0.039603
[2025-04-30 19:28:39.623] Epoch 71/80, Batch 3/3, Batch Loss (Unit B): 0.046966
[2025-04-30 19:28:39.660] Epoch 71/80, Average Loss (Unit B): 0.044148, Mean Absolute Error (Unit B): 0.151845
[2025-04-30 19:28:39.671] Epoch 72/80, Batch 1/3, Batch Loss (Unit B): 0.049912
[2025-04-30 19:28:39.678] Epoch 72/80, Batch 3/3, Batch Loss (Unit B): 0.084681
[2025-04-30 19:28:39.690] Epoch 73/80, Batch 1/3, Batch Loss (Unit B): 0.053532
[2025-04-30 19:28:39.720] Epoch 73/80, Batch 3/3, Batch Loss (Unit B): 0.040791
[2025-04-30 19:28:39.757] Epoch 74/80, Batch 1/3, Batch Loss (Unit B): 0.061172
[2025-04-30 19:28:39.796] Epoch 74/80, Batch 3/3, Batch Loss (Unit B): 0.065021
[2025-04-30 19:28:39.802] Epoch 75/80, Batch 1/3, Batch Loss (Unit B): 0.042270
[2025-04-30 19:28:39.806] Epoch 75/80, Batch 3/3, Batch Loss (Unit B): 0.065686
[2025-04-30 19:28:39.811] Epoch 76/80, Batch 1/3, Batch Loss (Unit B): 0.057508
[2025-04-30 19:28:39.828] Epoch 76/80, Batch 3/3, Batch Loss (Unit B): 0.016697
[2025-04-30 19:28:39.846] Epoch 77/80, Batch 1/3, Batch Loss (Unit B): 0.052568
[2025-04-30 19:28:39.858] Epoch 77/80, Batch 3/3, Batch Loss (Unit B): 0.078373
[2025-04-30 19:28:39.863] Epoch 78/80, Batch 1/3, Batch Loss (Unit B): 0.076261
[2025-04-30 19:28:39.870] Epoch 78/80, Batch 3/3, Batch Loss (Unit B): 0.008336
[2025-04-30 19:28:39.876] Epoch 79/80, Batch 1/3, Batch Loss (Unit B): 0.067805
[2025-04-30 19:28:39.880] Epoch 79/80, Batch 3/3, Batch Loss (Unit B): 0.017356
[2025-04-30 19:28:39.893] Epoch 80/80, Batch 1/3, Batch Loss (Unit B): 0.027317
[2025-04-30 19:28:39.898] Epoch 80/80, Batch 3/3, Batch Loss (Unit B): 0.021536
[2025-04-30 19:28:39.908] Epoch 80/80, Average Loss (Unit B): 0.038751, Mean Absolute Error (Unit B): 0.145033
[2025-04-30 19:28:39.911] Model B training completed
[2025-04-30 19:28:39.949] Model B Final Predictions Shape: 16,1
[2025-04-30 19:28:39.954] Model B Final Predictions (First few): [1.578467, 1.4229388, 1.7788396, 1.6860995, 1.6775295, 1.6334094, 1.7543508, 1.5669016, 1.7304193, 1.4110415...]
[2025-04-30 19:28:39.980] Model B Final Mean Absolute Error: 0.145033
[2025-04-30 19:28:39.988] Step 7 (Unit B) - Model B trained and saved to RuntimeProcessingContext and Results Store.
[2025-04-30 19:28:39.992] Workflow Session 1: Step 8 (Unit B) - Generating future performance projection for customer 1.
[2025-04-30 19:28:39.995] Workflow Session 1: Step 8 (Unit B) - Future performance projection completed: PerformanceProjection_B_Cust_1_Outcome_StableB_ComplexModelB_Score_0.9420_TrainError_0.1450
[2025-04-30 19:28:39.999] Workflow Session 1: Workflow (Unit B) completed for customer 1 with final score 0.9420
[2025-04-30 19:28:40.004] Workflow Session 1: Workflow (Unit B) completed with result: Workflow_B_Complete_Cust_1_FinalScore_0.9420
[2025-04-30 19:28:40.215] Workflow Session 1: Parallel Processing Unit B finished.
[2025-04-30 19:28:40.220] Workflow Session 1: Parallel Processing Units A and B completed. Starting Sequential Final Processing Unit (D).
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\AutoGen.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
[2025-04-30 19:28:40.387] Workflow Session 1: Starting Sequential Final Processing Unit D (Actual Model D Concept with AutoGen) for customer 1.
[2025-04-30 19:28:40.391] Workflow Session 1: SequentialFinalProcessingUnitD: Initializing...
[2025-04-30 19:28:40.451] Workflow Session 1: SequentialFinalProcessingUnitD: Retrieving model outputs and parameters from parallel units...
[2025-04-30 19:28:40.455] Workflow Session 1: Attempting to retrieve Model A parameters from RuntimeContext.
[2025-04-30 19:28:40.459] Workflow Session 1: Successfully retrieved Model A combined parameters (61444 bytes) from RuntimeContext.
[2025-04-30 19:28:40.462] Workflow Session 1: Attempting to retrieve Model B parameters from RuntimeContext.
[2025-04-30 19:28:40.469] Workflow Session 1: Successfully retrieved Model B combined parameters (4484 bytes) from RuntimeContext.
[2025-04-30 19:28:40.475] Workflow Session 1: Attempting to retrieve Model A predictions from Unit A results dictionary.
[2025-04-30 19:28:40.510] Workflow Session 1: Successfully retrieved Model A predictions (16 values) from Unit A results.
[2025-04-30 19:28:40.545] Workflow Session 1: Model A Predictions (first 10): [2.7504, 2.7034, 2.9581, 2.5593, 2.7367, 2.6735, 2.2127, 3.0339, 2.6607, 2.6618...]
[2025-04-30 19:28:40.560] Workflow Session 1: Attempting to retrieve Model B predictions from Unit B results dictionary.
[2025-04-30 19:28:40.565] Workflow Session 1: Successfully retrieved Model B predictions (16 values) from Unit B results.
[2025-04-30 19:28:40.572] Workflow Session 1: Model B Predictions (first 10): [1.5785, 1.4229, 1.7788, 1.6861, 1.6775, 1.6334, 1.7544, 1.5669, 1.7304, 1.4110...]
[2025-04-30 19:28:40.586] Workflow Session 1: Retrieved Model A Training Error: 1.627609
[2025-04-30 19:28:40.590] Workflow Session 1: Retrieved Model B Training Error: 0.145033
[2025-04-30 19:28:40.595] Workflow Session 1: SequentialFinalProcessingUnitD: Initiating AutoGen Agent Collaboration for Comprehensive Analysis.
[2025-04-30 19:28:40.610] Agent Collaboration: System provides independent training performance metrics to agents.
[2025-04-30 19:28:40.614] Agent Collaboration: AgentA reacting to training metrics.
'Base_Pre.Server.exe' (CoreCLR: clrhost): Loaded 'E:\Development_Sandbox\Projects\Base_Pre\Base_Pre.Server\bin\Debug\net8.0\JsonSchema.Net.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
[2025-04-30 19:28:41.106] Agent Collaboration: AgentA reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.109] Agent Collaboration: AgentB reacting to training metrics.
[2025-04-30 19:28:41.112] Agent Collaboration: AgentB reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.120] Agent Collaboration: System provides prediction arrays and instructs detailed comparative analysis.
[2025-04-30 19:28:41.123] Agent Collaboration: AgentA performing and discussing comparative analysis.
[2025-04-30 19:28:41.127] Agent Collaboration: AgentA reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.130] Agent Collaboration: AgentB performing and discussing comparative analysis.
[2025-04-30 19:28:41.140] Agent Collaboration: AgentB reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.144] Agent Collaboration: C# logic performing statistical analysis and finding most similar index based on agents' instructions.
[2025-04-30 19:28:41.162] Helper_D: Most similar prediction pair found at index 6 with absolute difference 0.458382.
[2025-04-30 19:28:41.166] Agent Collaboration: C# logic calculated stats and found most similar index 6. System reporting this to agents.
[2025-04-30 19:28:41.199] Agent Collaboration: Agents interpreting detailed statistical results.
[2025-04-30 19:28:41.205] Agent Collaboration: AgentA reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.209] Agent Collaboration: AgentB reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.214] Agent Collaboration: C# logic performing simulated inference on a small validation set (4 samples) using trained model parameters.
[2025-04-30 19:28:41.223] Helper_D: Simulating Model A inference...
[2025-04-30 19:28:41.227] Helper_D: Deserialized 15361 float parameters for Model A.
[2025-04-30 19:28:41.230] Helper_D: Inferred hidden layer size: 960 for Model A.
[2025-04-30 19:28:41.237] Helper_D: Simulated Model A inference complete for 4 samples. Returning predictions.
[2025-04-30 19:28:41.240] Helper_D: Simulating Model B inference...
[2025-04-30 19:28:41.243] Helper_D: Deserialized 1121 float parameters for Model B.
[2025-04-30 19:28:41.246] Helper_D: Inferred hidden layer size: 70 for Model B.
[2025-04-30 19:28:41.281] Helper_D: Simulated Model B inference complete for 4 samples. Returning predictions.
[2025-04-30 19:28:41.300] Agent Collaboration: C# logic completed simulated inference. Average Simulated Output A: 2.549922, Average Simulated Output B: 1.638919.
[2025-04-30 19:28:41.305] Agent Collaboration: Simulated Inference Comparison Metrics:
  - MAE (Simulated): 0.911003
  - Correlation (Simulated): 0.889562
  - MSE (Simulated): 0.883968
  - RMS (Simulated): 0.940195
  - Coefficient of Variation (Simulated Differences): 25.5178%
[2025-04-30 19:28:41.324] Agent Collaboration: System reports simulated inference results and metrics to agents.
[2025-04-30 19:28:41.327] Agent Collaboration: Agents providing final assessment and summary.
[2025-04-30 19:28:41.330] Agent Collaboration: AgentA reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.334] Agent Collaboration: AgentB reply received. Content: Default reply is not set. Please pass a default reply to assistant agent
[2025-04-30 19:28:41.350] Agent Collaboration: C# logic determining overall summary based on all metrics.
[2025-04-30 19:28:41.359] Agent Collaboration: Final Overall Summary: Significant Full Prediction Differences | Lower Simulated Inference Consistency | Both Models Showed Higher Individual Training Error | Combined Confidence: 32%.
[2025-04-30 19:28:41.363] Agent Collaboration: AutoGen workflow completed. Overall summary: Significant Full Prediction Differences | Lower Simulated Inference Consistency | Both Models Showed Higher Individual Training Error | Combined Confidence: 32%
[2025-04-30 19:28:41.366] Workflow Session 1: SequentialFinalProcessingUnitD: Attempting conceptual model merge.
[2025-04-30 19:28:41.373] Workflow Session 1: Conceptually merged Model A (61444 bytes) and Model B (4484 bytes) parameters. Merged data size: 65928 bytes.
[2025-04-30 19:28:41.376] Workflow Session 1: Stored conceptual merged model data (65928 bytes) in RuntimeContext.
[2025-04-30 19:28:41.379] Workflow Session 1: SequentialFinalProcessingUnitD: Updating CoreMlOutcomeRecord with final details.
[2025-04-30 19:28:41.382] Workflow Session 1: Final Outcome Record Details:
  - RecordIdentifier: 3
  - AssociatedCustomerIdentifier: 1
  - OutcomeGenerationTimestamp: 4/30/2025 7:28:41 PM
  - CategoricalClassificationIdentifier: 250
  - CategoricalClassificationDescription:  (Full Data Processed, Analysis: Significant Full Prediction Differences | Lower Simulated Inference Consistency | Both Models Showed Higher Individual Training Error | Combined Confidence: 32%)
  - SerializedSimulatedModelData Size: 3840 bytes
  - AncillaryBinaryDataPayload Size: 260 bytes
  - DerivedProductFeatureVector: ModelA_Preds_Count:16_BestMatchIdx:6_InputUsed:0.000000
  - DerivedServiceBenefitVector: ModelB_Preds_Count:16_SimOutputA:2.549922_SimOutputB:1.638919
[2025-04-30 19:28:41.456] Workflow Session 1: SequentialFinalProcessingUnitD: Attempting to save final CoreMlOutcomeRecord to simulated persistence.
[2025-04-30 19:28:41.461] Workflow Session 1: Final CoreMlOutcomeRecord (ID: 3) state saved successfully to simulated persistent storage.
[2025-04-30 19:28:41.465] Workflow Session 1: Sequential Final Processing Unit D (Actual Model D Concept with AutoGen) completed all processing steps successfully.
[2025-04-30 19:28:41.472] Workflow Session 1: Sequential Final Processing Unit D (Actual Model D Concept with AutoGen) finished execution.
[2025-04-30 19:28:41.475] Workflow Session 1: ML Outcome Generation workflow completed successfully.
[2025-04-30 19:28:41.479] Workflow Session 1: Returning final CoreMlOutcomeRecord (ID: 3) for customer 1.
[2025-04-30 19:28:41.483] Workflow Session 1: Associated actual ML session resources cleaned up.
```
