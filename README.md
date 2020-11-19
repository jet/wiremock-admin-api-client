NOTICE: SUPPORT FOR THIS PROJECT ENDED ON 18 November 2020

This projected was owned and maintained by Jet.com (Walmart). This project has reached its end of life and Walmart no longer supports this project.

We will no longer be monitoring the issues for this project or reviewing pull requests. You are free to continue using this project under the license terms or forks of this project at your own risk. This project is no longer subject to Jet.com/Walmart's bug bounty program or other security monitoring.


## Actions you can take

We recommend you take the following action:

  * Review any configuration files used for build automation and make appropriate updates to remove or replace this project
  * Notify other members of your team and/or organization of this change
  * Notify your security team to help you evaluate alternative options

## Forking and transition of ownership

For [security reasons](https://www.theregister.co.uk/2018/11/26/npm_repo_bitcoin_stealer/), Walmart does not transfer the ownership of our primary repos on Github or other platforms to other individuals/organizations. Further, we do not transfer ownership of packages for public package management systems.

If you would like to fork this package and continue development, you should choose a new name for the project and create your own packages, build automation, etc.

Please review the licensing terms of this project, which continue to be in effect even after decommission.

ORIGINAL README BELOW

----------------------


# F# WireMock Admin API Client

This repository contains a WireMock Admin API client developed in F#.
This client supports stubbing, in which it is possible to configure WireMock responses for given HTTP requests.
For more information about WireMock, visit : http://wiremock.org/

## How to use
First of all, define your Mock that inherits from WireMock and indicate where WireMock server is running. In this example it is in localhost:8080.
Secondly, create a WireMockHttpRequest, which contains a WireMockRequest and a WireMockResponse. These two types define the input that WireMock server will expect and the response that the WireMock server will return for that given request.
Finally, we can create a function to configure the mapping.
```
    type SearchMock() =
    inherit WireMock("http://localhost:8080/")
    
    static member mock = SearchMock()
  
    static member searchMockRequest = 
      let searchRequest = WireMockRequest.create WIREMOCK_POST "/v1/search" [||]
      let mockSearchResponse = """{ "Response": "OK" }"""
      let headerJsonContentType =  "Content-Type: application/json" 
      let wireMockSearchResponse = WireMockResponse.create 200 mockSearchResponse headerJsonContentType None
      WireMockHttpRequest.create(searchRequest, wireMockSearchResponse)
  
    static member setUpSearch(request: WireMockHttpRequest) =
      SearchMock.mock.setMappings request
```
An easy way to test this mock is by means of starting a WireMock server, for instance by using the following Docker image.
```
docker run -p 8080:8080 rodolpheche/wiremock
```
Once 'setMappings' function is executed, we should see the stubbed request by means of reaching http://localhost:8080/__admin .
``` json
{
  "mappings" : [ {
    "id" : "04b918b6-9512-4708-a8d3-1350aa02108d",
    "request" : {
      "url" : "/v1/search",
      "method" : "POST"
    },
    "response" : {
      "status" : 200,
      "body" : "{ \"Response\": \"OK\" }",
      "headers" : {
        "Content-Type" : "application/json"
      },
      "fixedDelayMilliseconds" : 0
    },
    "uuid" : "04b918b6-9512-4708-a8d3-1350aa02108d"
  } ],
  "meta" : {
    "total" : 1
  }
}
```
The source code contains other examples under the samples folder.
## Security
This repository is actively monitored by Jet Engineers and the Jet Security team. Please monitor this repo for security updates and advisories. For more information and contacts, please see [SECURITY.md].
