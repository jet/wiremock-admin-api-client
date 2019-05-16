(*Copyright (c) 2019-present, Jet.com, Inc.
This source code is licensed under the Apache 2.0 license found in the
LICENSE file in the root directory of this source tree.*)
module WireMockAPIClient
    
  open System
  open System.Text
  open System.Net.Http
  open System.Web
  open System.Json
  open Fleece.SystemJson
  open Fleece.SystemJson.Operators
  
  // The following module contains the types that can be used to define behavior on wiremock by means of an HTTP request.
  module WireMockDataTypes =
    
    let inline isNull value = obj.ReferenceEquals(value, null)
    
    let [<Literal>] WIREMOCK_GET = "GET"
    let [<Literal>] WIREMOCK_POST = "POST"
  
    type Count = { count: int64 }
  
    type BodyPattern = 
      {
        matchesJsonPath : string
        equalToJson: string       
      }
      static member ToJson (x: BodyPattern) =
        if isNull x.equalToJson && not (isNull x.matchesJsonPath) then 
          jobj [ 
            "matchesJsonPath" .= x.matchesJsonPath
          ]
        else if isNull x.matchesJsonPath && not (isNull x.equalToJson) then 
          jobj [ 
            "equalToJson" .= x.equalToJson
          ]
        else 
          jobj [ 
            "matchesJsonPath" .= x.matchesJsonPath
            "equalToJson" .= x.equalToJson
          ]
              
    type WireMockRequest = 
      {
        method : string
        url : string
        bodyPatterns: BodyPattern []
      }
    with 
      static member create method url bodyPatterns =
        {
          WireMockRequest.method = method
          url = url
          bodyPatterns = bodyPatterns
        }
      static member ToJson (x: WireMockRequest) =
        jobj [ 
          "method" .= x.method
          "url" .= x.url
          "bodyPatterns" .= x.bodyPatterns
        ]
    
    type WireMockResponse = 
      {
        status: int
        body: string
        headers: string
      }
    with     
      static member create status body headers _delay =
        {
          WireMockResponse.status = status
          body = body
          headers = headers
        }
      static member ToJson (x: WireMockResponse) =
        jobj [ 
          "status" .= x.status
          "body" .= x.body
          "headers" .= x.headers
        ]
    
    type WireMockHttpRequest = 
      {
        request : WireMockRequest
        response : WireMockResponse
      }
    with    
      static member create(request,response) =
        {
          WireMockHttpRequest.request = request
          response = response
        }
      static member ToJson (x: WireMockHttpRequest) =
        jobj [ 
          "request" .= x.request
          "response" .= x.response
        ]
    
  open WireMockDataTypes
  open FSharp.Data
  open FSharp.Data.JsonExtensions
  [<AbstractClass>]
  type WireMock (host: string) =
  
    let [<Literal>] WIREMOCK_MAPPING_API_PATH = "__admin/mappings"
    let [<Literal>] WIREMOCK_COUNT_API_PATH = "__admin/requests/count"
    let [<Literal>] WIREMOCK_REQUEST_API_PATH = "__admin/requests"
    let [<Literal>] WIREMOCK_RESET_PATH = "__admin/mappings/reset"
  
    member x.resetRequest() =
      Http.RequestString( host + WIREMOCK_REQUEST_API_PATH, 
        headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
        httpMethod = "DELETE")
         
    member x.resetMappings() =
      Http.RequestString( host + WIREMOCK_RESET_PATH, 
        headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
        body = TextRequest "")
  
    member x.setMappings(request: WireMockDataTypes.WireMockHttpRequest) =
      let serializedRequest = WireMockHttpRequest.ToJson request |> string
      Http.RequestString( host + WIREMOCK_MAPPING_API_PATH, 
        headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
        body = TextRequest serializedRequest)
  
    member x.mappings() =
      Http.RequestString(host + WIREMOCK_MAPPING_API_PATH)
       
    member x.count(request: WireMockDataTypes.WireMockHttpRequest) =
      let serializedRequest = WireMockHttpRequest.ToJson request |> string
      let result = Http.RequestString( host + WIREMOCK_COUNT_API_PATH, 
                     headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
                     body = TextRequest serializedRequest)
                   |> JsonValue.Parse
      let count = result?count
      count.AsInteger() 
      
