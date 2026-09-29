# Restaurant System

A full-stack restaurant application built with Angular and ASP.NET Core Web API.

## Overview

Restaurant System is a full-stack web application that provides a restaurant interface for displaying restaurant information and menu items, with an Angular frontend connected to an ASP.NET Core backend API.

## Technologies

### Frontend

* Angular
* TypeScript
* Angular Router
* HTML / CSS

### Backend

* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQL Server

### Architecture

The backend follows a layered architecture:

* Domain
* Application
* Infrastructure
* Server

## Project Structure

RestaurantSystem/
│
├── Client/             # Angular frontend
├── Server/             # ASP.NET Core Web API
├── Application/        # Application logic, DTOs and services
├── Domain/             # Domain entities
├── Infrastructure/    # Database and infrastructure services
│
├── RestaurantSystem.sln
├── .gitignore
└── README.md

## Main Features

* Restaurant interface
* Restaurant menu
* Today's Special section
* Menu item details
* Category-based menu display
* Angular routing
* REST API integration
* Responsive user interface
* Backend API built with ASP.NET Core
* SQL Server database integration


## Getting Started

### Prerequisites

Make sure you have the following installed:

* Node.js
* Angular CLI
* .NET SDK
* SQL Server

### Run the Backend

Navigate to the backend project:

cd Server

Restore dependencies:

dotnet restore

Run the API:

dotnet run

The API will start on the URL displayed by ASP.NET Core.

### Run the Frontend

Open another terminal and navigate to the Angular project:

cd Client

Install dependencies:

npm install

Start the Angular development server:

ng serve

Then open:

http://localhost:4200

## API Integration

The Angular frontend communicates with the ASP.NET Core Web API to retrieve and manage restaurant-related data.

Make sure the backend is running before using the frontend.

## Author

alaa yasser