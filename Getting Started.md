# Getting Started
This tutorial demonstrates how to auto-generate a C# client SDK for Stanlab using the OpenAPI v3 specification. Open source and commercial tools for client/server SDK generation are available for the most popular programming languages, e.g. https://swagger.io/tools/swagger-codegen.

## 1. Download and install NPM (Node Package Manager)
   
Read more https://www.npmjs.com/get-npm

## 2.. Install NSwag CLI (Command Line Interface) from NPM

Open your terminal and enter the following command to install NSwag on your system globally.

```
npm install nswag -g
```

## 3. Generate client code

Open your terminal and enter the following command to generate the client code in the current directory. The command will generate one C# class file in the specified namespace

```
nswag swagger2csclient /input:https://stanlab-api.demo.miljoeportal.dk/openapi/v1/stanlab-gateway.json /classname:StanlabClient /namespace:Dmp.Examples /output:StanlabClient.cs
```

For more configuration, see the NSwag command line interface documentation.

https://github.com/RSuter/NSwag/wiki/CommandLine


These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

## 4. Open and run the project
Open the project in Visual Studio and run the example code.


# Support

Please contact Danmarks Miljøportal support at support@miljoeportal.dk
