Aroundu.AppHost - local orchiestrator for the system
Aroundu.ServiceDefaults - setup for aspire
Aroundu.SharedKernel - shared data for whole system across all serivces
Aroundu.ApiGateway - Entry of the backend system
Aroundu.View - forntend app 

run.sh - startup command for faster startup // Note: find better way of doing this 

AuthService - Auth for the system 

Each service looks like this;

Aroundu.x.Serivce - service folder

Aroundu.x.Service.SharedKernel - shared data between layers in service

Aroundu.x.Serivce.Contracts - NuGet app for managing contracts between the services

Aroundu.x.Service.Domain - buissness Domain entities 

Aroundu.x.Service.Application - commands and so on - only the logic

Aroundu.x.Service.Infrastructure - external systems implementation and implementation of the cqrs
Aroundu.x.Service.Persistence - implementation of db 

Aroundu.x.Service.ComposistionRoot - layer between projcets to distinguish buissense logic from the consumers in the service

Aroundu.x.Service.Api - main consumer for the data exchange with the view though yarp in api gateway 
Aroundu.x.Serivce.MigrationWorker - managed by apphost for enuring proper migration setup in the service dbs
Aroundu.x.Serivce.ServiceWorker - for jobs and other long taking operations

Aroundu.x.Service.Tests - test subfolder

Aroundu.x.Service.Tests.Unit - unit tests
Aroundu.x.Service.Tests.Integration - intrgration tests
Aroundu.x.Service.Tests.e2e - e2e tests
