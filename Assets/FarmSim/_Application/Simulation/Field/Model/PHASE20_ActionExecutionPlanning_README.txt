PHASE 20 — Action Execution Planning (DATA ONLY)

Goal
- Define deterministic, inspectable execution plans derived from a single action intent + planning context.
- Freeze execution scope (targets) and preconditions at plan creation time.
- Provide stable identity + traceability for replay/analytics.
- NO execution, NO scheduling, NO persistence, NO Unity behaviours.

Key Types
- FieldPatchActionExecutionPlanModel
  - ExecutionPlanId
  - PlanScopeSource
  - PlanScope (frozen targets)
  - ExecutionPreconditionsSnapshot
  - ExecutionPlan (immutable)

- FieldPatchActionExecutionPlanTraceModel
  - ExecutionPlanTraceLink (correlation to upstream audit/gate/batch IDs)

- FieldPatchActionExecutionPlanScopeResolutionModel
  - ScopeResolution (records how targets were resolved, plus final resolved target set)

- FieldPatchActionExecutionPlanBuildInputsModel
  - BuildInputs (forces explicit inputs; prevents implicit inference)

- FieldPatchActionExecutionPlanBuilderModel
  - Build(BuildInputs) -> ExecutionPlan
  - Converts ScopeResolution -> PlanScope without lookup/traversal

- FieldPatchActionExecutionPlanFactoryModel
  - CreatePlan / CreateScope* / CreatePreconditionsSnapshot (helpers; data-only)

- FieldPatchActionExecutionPlanReadModel
  - ReadModel (light projection for later UI/analytics systems)

Contracts
- IFieldPatchActionExecutionPlanner (data-only interface)

Hard Constraints (enforced by design)
- No world traversal / lookup logic in these types.
- No gate evaluation logic.
- No mutation logic.
- All collections are frozen/copies exposed as ReadOnlyCollection.

Next Phase Expectations
- Later phases may implement a runtime planner that assembles BuildInputs
  from already-resolved state, then calls the Builder.
- Execution systems must validate PreconditionsSnapshot before applying any effects.
