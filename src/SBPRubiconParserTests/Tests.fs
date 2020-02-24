namespace SBPRubiconParserTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open SBPRubiconParser.RubiconParser
open FParsec.CharParsers


[<TestClass>]
type TestClass () =
    let test = "
copyright alll rights etc etc...
# Test
## Properties
Leverage agile frameworks to provide a robust synopsis for high level overviews. Iterative approaches to corporate 
strategy foster collaborative thinking to further the overall value proposition.
Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.
## Overview
ring to the table win-win survival strategies to ensure proactive domination. At the end of the day, going forward, a new normal that has evolved from generation X is on the runway heading towards a streamlined cloud solution. User generated content in real-time will have multiple touchpoints for offshoring.
## Applicability conditions
Capitalize on low hanging fruit to identify a ballpark value added activity to beta test. Override the digital divide with additional clickthroughs from DevOps. Nanotechnology immersion along the information highway will close the loop on focusing solely on the bottom line.
* testing
* testing 2

## Additional reading links
https://idsgn.dropmark.com/107/1130454
## Notes for evaluator
Podcasting operational change management inside of workflows to establish a framework. Taking seamless key performance indicators offline to maximise the long tail. Keeping your eye on the ball while performing a deep dive on the start-up mentality to derive convergence on cross-platform integration.
"
    [<TestMethod>]
    member this.TestMethodDisplay () =
        match (run pRubicon test) with
        | Success(result, _, _)   -> printfn "Success: %s" (ObjectDumper.Dump result)
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue (
            match (run pRubicon test) with
            | Success(_, _, _) -> true
            | Failure(_, _, _) -> false)
    
    [<TestMethod>]
    member this.TestIdea () =
        Assert.AreEqual("test", "  test  ".Trim() )
