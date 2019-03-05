# Stanlab
Stanlab is an open standard for exchanging laboratory related data between parties. The standard defines a set of service oriented data models and operations governed by Danmarks Miljøportal. The data models and service operations are designed as contract first/schema first by using the OpenAPI Specification (OAS), which provides a standard format to unify how an industry defines and describes RESTful APIs.

The OpenAPI Specification is available on the following URL:
https://stanlab.test.miljoeportal.dk/openapi/v1/stanlab.json

## Security and Authorization
The are no requirements for authorization of Stanlab compatible services, however we recommend securing the services using OAuth 2.0, which is the industry-standard protocol for authorization of RESTful services.

## Stanlab Gateway
The Stanlab Gateway is a middleware that acts as a reverse proxy for redirecting requests to registered Stanlab compatible services, such as laboratory systems, specialized systems etc. The purpose is to provide a single endpoint for data exchange between trusted parties.

Read more about reverse-proxy on Wikipedia https://en.wikipedia.org/wiki/Reverse_proxy.

The Stanlab Gateway and a Swagger UI is available on the following URL:
https://stanlab.test.miljoeportal.dk/swagger

The following systems are currently registered in the Stanlab Gateway

| System | Endpoint | Responsible          |
| ------ | -------- | -------------------- |
| PULS   | `puls`   | Danmarks Miljøportal |

The Stanlab Gateway is secured using Danmarks Miljøportal's identity provider, that requires a set of client credentials. Please contact Danmarks Mlijøportal's support at support@miljoeportal.dk to get a client id and client secret for authorization.

## Danmarks Miljøportal's Identity Provider
Danmarks Miljøportal's identity provider supports OpenID Connect, a simple identity layer on top of the OAuth 2.0 protocol, which allows computing clients to verify the identity of an end-user based on the authentication performed by an authorization server, as well as to obtain basic profile information about the end-user in an interoperable and REST-like manner. In technical terms, OpenID Connect specifies a RESTful HTTP API, using JSON as a data format.

OpenID Connect allows a range of clients, including Web-based, mobile, and JavaScript clients, to request and receive information about authenticated sessions and end-users. The specification suite is extensible, supporting optional features such as encryption of identity data, discovery of OpenID Providers, and session management.

OpenID Connect defines a discovery mechanism, called OpenID Connect Discovery, where an OpenID server publishes its metadata at a well-known URL. The discovery documents are available on the following URL's for the test and production environment respectively.

https://log-in.test.miljoeportal.dk/runtime/oauth2/.well-known/openid-configuration

https://log-in.miljoeportal.dk/runtime/oauth2/.well-known/openid-configuration

The identity provider supports the following OAuth 2.0 / OpenID Connect flows:

* Client credentials
* Implicit
* Authorization code

