# GRPC Open Telemetry Exception Logging Example

This is a simple example of how to use Open Telemetry to log exceptions in GRPC.

## Running the example

To run the example, you need to have docker and docker-compose installed.

Then, run the following command:
`docker-compose up --build`

This will build the docker images and run the example.

## Using the example

Go to swagger at http://localhost:5001/swagger/index.html

Click on the `GET /Hello` endpoint and click `Try it out` and then enter a name and click `Execute`.

Click on the `Get /Hello/error` endpoint and click `Try it out` and then enter a name and click `Execute`.

## Viewing the logs

Go to http://localhost:16686/ and click on `Find Traces` and then click on the `api-grpc-client` service.

View the error trace and see the exception logged in the grpc-server service.