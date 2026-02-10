---
name: Foundations for new feature
about: Model, repository, service and REST API endpoints for a new feature
title: ''
labels: ''
assignees: ''

---

## Description
Implement the foundational data model, repository, service layer, and REST API endpoints for [FEATURE_NAME]. This establishes the core CRUD operations and database schema required for [FEATURE_PURPOSE].

This foundation enables other features to [KEY_BENEFIT] and serves as the base layer for more advanced [FEATURE_NAME] functionality.

## Scope
**Includes:**
- [Entity_Name] entity with all core properties
- Repository with standard CRUD operations
- Service layer with validation logic
- REST API controller with endpoints
- DTOs for API requests/responses
- Database migration

**Excludes:**
- Advanced filtering/search (separate issue)
- External API imports (separate issue)
- Integration with other systems (separate issues)
- Complex business logic (separate issues)

## Tasks
- [ ] Create [Entity_Name] entity with properties
- [ ] Implement [Entity_Name]Repository with CRUD methods
- [ ] Implement [Entity_Name]Service with validation
- [ ] Create [Entity_Name]Controller with REST endpoints
- [ ] Create DTOs ([Entity_Name]CreateDto, [Entity_Name]ResponseDto, etc.)
- [ ] Configure database relationships in DbContext
- [ ] Generate and apply EF Core migration
- [ ] Test all operations in Bruno
- [ ] Update API documentation

## Endpoints
- `GET /api/[resource]` - List all [resources]
- `GET /api/[resource]/{id}` - Get specific [resource]
- `POST /api/[resource]` - Create new [resource]
- `PUT /api/[resource]/{id}` - Update [resource]
- `DELETE /api/[resource]/{id}` - Delete [resource]

## Acceptance Criteria
- [ ] All CRUD operations work correctly in Bruno
- [ ] Data persists correctly to database
- [ ] API returns properly formatted DTOs
- [ ] Validation prevents invalid data

## Dependencies
[List any prerequisite issues, or "None" if this is foundational]

## Technical Notes
[Any specific implementation details, constraints, or decisions]
