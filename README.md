# ConfigVault V1

ConfigVault is a domain driven system built in dotnet to prevent wrong configuration in critical system.

## Why this project 

Configuration error in Telemetry system can cause outage, alert, etc  
Telemetry configurations define how raw technical data is interpreted, monitored, and acted upon in critical systems.  
In industries like aerospace and telecom, misconfigured telemetry can lead to false alarms, missed failures, or invalid test results.

## V1 Scope
- Configuration versioning
- Explicit workflow: Draft → Approved → Active → Archived
- Strict domain rules

## Core rules

- a rule must be a draft at the beginning
- the draft must be approved 
- once the draft has been approved it can be activated
- an ctive rule cant be modified
- only an active reule can be archived

## How rules are enforced

Business rules are enforced directly in the domain model:

- `ConfigVersion` exposes methods (`Approve`, `Activate`, `Archive`) instead of public setters.
- Invalid transitions throw a `DomainException`.
- Domain tests (in `ConfigVault.Domain.Tests`) prove that forbidden state transitions cannot happen.

The next iteration of ConfigVault will focus on exposing the domain in a controlled and explicit way, without compromising its integrity.

## Soon

### Application Layer
- Introduce explicit use cases (ApproveConfiguration, ActivateConfiguration, ArchiveConfiguration)
- Ensure all orchestration logic lives outside the domain
- Keep the domain model free of technical concerns

### API Layer
- Minimal HTTP API to trigger use cases
- No business rules in controllers
- Domain exceptions translated into meaningful API responses

### Audit & Traceability
- Centralized audit trail for all state transitions
- Clear trace of who performed which action and when
