(*Copyright (c) 2019-present, Jet.com, Inc.
This source code is licensed under the Apache 2.0 license found in the
LICENSE file in the root directory of this source tree.*)
module testing
open FSharp.Data
open StubForPostWithBodyPattern
open WireMockAPIClient

open WireMockDataTypes
[<EntryPoint>]
let main _ =   
  printfn "set mapping:\n %s" (SearchMock.setUpSearch SearchMock.searchMockRequest) 
  printfn "get mappings:\n %s" (SearchMock.getMappings())   
  printfn "reset:\n %s" (SearchMock.resetSearch())   
  printfn "get mappings:\n %s" (SearchMock.getMappings())
  0
  