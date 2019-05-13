(*Copyright (c) 2019-present, Jet.com, Inc.
This source code is licensed under the Apache 2.0 license found in the
LICENSE file in the root directory of this source tree.*)
module StubForPostWithBodyPattern
  open System.Web
  open System.IO
  open WireMockAPIClient
  open WireMockAPIClient.WireMockDataTypes
  
  type SearchMock() =
    inherit WireMock("http://localhost:8080/")
    
    static member mock = SearchMock()
  
    static member searchMockRequest = 
      let matchesJsonPath = "$.productSearchQuery[?(@.categoriesFilter[0] == '123')]"
      let bodyPatterns =  {BodyPattern.matchesJsonPath = matchesJsonPath; equalToJson = null }             
      let searchRequest = WireMockRequest.create WIREMOCK_POST "/v1/search" [|bodyPatterns|]
      let mockSearchResponse = """{ "Response": "OK" }"""
      let headerJsonContentType =  "Content-Type: application/json" 
      let wireMockSearchResponse = WireMockResponse.create 200 mockSearchResponse headerJsonContentType None
      WireMockHttpRequest.create(searchRequest, wireMockSearchResponse)
  
    static member setUpSearch(request: WireMockHttpRequest) =
      SearchMock.mock.setMappings request
    
    static member resetSearch() =
      SearchMock.mock.resetRequest() |> ignore
      SearchMock.mock.resetMappings()
      
    static member getMappings() =
      SearchMock.mock.mappings()
