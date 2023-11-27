# Settlement Booking System

### Prerequisites

Before you start, ensure you have the following tools and software installed on your system:

- **Visual Studio 2022:** You'll need visual studio 2022 community installed.
- **Visual Studio Code:** Alternative you can download visual studio code.

You should be able to browse to the application by using the below URL :

```
SettlementBookingSystem API : https://localhost:44355
```

## Versions

1. Basic Implementation

`BasicBookingController` provides an implementation within the controller that is complete but poorly written.

2. Mediator Implementation

`MediatorBookingController` provides an implementation using the mediator pattern and the data is stored in an [Interval Tree](https://en.wikipedia.org/wiki/Interval_tree) for O(log m + n) access.
This implementatrion also provides unit tests with a mocked repository.

## How to execute

1. Basic:

```
curl --request POST \
  --url https://localhost:44355/BasicBooking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "09:00",
	"name": "Keith"
}'
```

2. Mediator

```
curl --request POST \
  --url https://localhost:44355/MediatorBooking \
  --header 'Content-Type: application/json' \
  --data '{
	"bookingTime": "09:30",
	"name": "John"
}'
```
