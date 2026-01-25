# ConfigVault

## Purpose

ConfigVault is a backend-focused project built to practice **clean architecture**, **domain-driven design**, and **explicit state management**.

The goal is to model a **configuration lifecycle** where invalid transitions are impossible by design, and where business rules are enforced at the **domain level**, not in the API.

This project is intentionally small and focused on **correctness and predictability**, not feature count.

---

## Problem

In real systems, configuration changes are risky:
- activating an unvalidated configuration can cause outages
- unclear lifecycle rules lead to inconsistent states
- configuration issues are hard to diagnose without traceability

ConfigVault addresses this by enforcing a **strict lifecycle** and explicit rules.

---

## Core Domain

### Configuration Lifecycle

A configuration version follows this lifecycle:


Rules enforced by the domain:
- a configuration always starts as `Draft`
- only an `Approved` configuration can be activated
- an `Active` configuration can be archived
- invalid transitions are rejected explicitly

All rules live inside the **domain model**, not in the API.

---

## Architecture

The project follows a layered architecture inspired by DDD:


The domain layer has no dependency on the API or infrastructure.

---

## Persistence

ConfigVault uses **EF Core** with **SQLite** to persist configuration versions.

- the domain model (`ConfigVersion`) is mapped directly through `ConfigVaultDbContext`
- the repository interface (`IConfigVersionRepository`) has two possible implementations:
  - an in-memory repository (used initially)
  - an EF Core repository (`EfCoreConfigVersionRepository`) backed by SQLite

The goal of this persistence layer is not to model a complex schema, but to validate that the domain model and architecture hold up once data is stored and reloaded.

---

## Logging

Basic structured logging is implemented at the application layer using `ILogger<T>`.

Logs capture:
- the execution of application commands
- successful lifecycle transitions
- rejected operations (e.g. missing configuration or invalid state)

The domain layer remains free of logging or infrastructure concerns.

---

## API Usage

### Create a configuration

```bash
curl -X POST http://localhost:5251/config-versions

Respone

{
  "id": "uuid",
  "status": "Draft"
}
````

### Approve a configuration

```bash
curl -X POST http://localhost:5251/config-versions/{id}/approve

Respone

204 No Content
````

### Activate a configuration

```bash
curl -X POST http://localhost:5251/config-versions/{id}/activate

Respone

204 No Content
````

### Get configuration status

```bash
curl -X POST http://localhost:5251/config-versions/{id}/activate

Respone

204 No Content
````

## Error Handling

Invalid operations are rejected explicitly by the domain:
- activating a configuration that has not been approved
- invalid state transitions
- unknown configuration identifiers

Business rule violations are represented as domain exceptions and mapped to HTTP responses.
The goal is to ensure **predictable failures** rather than silent or implicit behavior.

---

## Testing

The domain layer is covered by unit tests using xUnit.

Tests focus on:
- valid and invalid state transitions
- enforcement of domain invariants

The API layer remains thin and delegates all business logic to the domain.
Testing is centered on **behavior correctness**, not on exhaustive coverage.

---

## Planned Improvements

The following improvements are planned to strengthen the system from an operational perspective:

- structured logging of configuration lifecycle events
- basic metrics (e.g. number of transitions, rejected operations)
- improved traceability of configuration changes

These additions aim to improve **observability** without increasing architectural complexity.

---

## Project Status

ConfigVault is currently in a **stable state**.

Future changes will focus on:
- observability
- documentation clarity
- small refinements to existing behavior

No feature expansion is planned beyond what is necessary to support these goals.

