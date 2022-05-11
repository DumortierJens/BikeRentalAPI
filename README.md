#  Bike Rental API
**Bike Rental API** is a school project for the module **Backend Development** of my study at **MCT Howest (Kortrijk)**. This project is an API to manage renting out bikes at bike rentals.

###  How does it work?
You can add/update bikes, rental locations and prices for a bike at a location. If the bike exists in the rental location you can start/stop a rental and it calulates the total price for the rental.

###  What did I use?
* **API technology:** .NET 6.0 Minimal API
* **Test Framework:** XUnit (integration/unit tests)
* **Docker**
* **GraphQL**
* **MongoDB**

###  How to get started
1. **Configuration**
	Edit the config file: `/BikeRentalAPI/appsettings.Docker.json`

	**Secret:** 256-bit hex string
	

	    {
	        "Logging": {
	            "LogLevel": {
	                "Default": "Information",
	                "Microsoft.AspNetCore": "Warning"
	            }
	        },
	        "AllowedHosts": "*",
	        "MongoConnection": {
	            "ConnectionString": "mongodb://root:example@mongo:27017",
	            "DatabaseName": "BikeRentalsDb",
	            "BikeCollection": "Bikes",
	            "BikePriceCollection": "BikePrices",
	            "LocationCollection": "Locations",
	            "RentalCollection": "Rentals"
	        },
	        "AuthenticationSettings": {
	            "SecretForKey": "<<secret>>",
	            "Issuer": "http://localhost:3000",
	            "Audience": "bike_rental_api_users"
	        }
	    }

2. **Startup Docker Compose**
	This will start MongoDB & the API
	Start the Docker compose file: `/docker-compose.yml`
	