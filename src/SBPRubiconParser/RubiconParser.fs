namespace SBPRubiconParser

open System
open System.Collections.Generic

type ISection = 
    abstract member Heading: string
    abstract member Text: string

type IConditions =
    inherit ISection
    abstract member When: IEnumerable<string>

type Title(heading, text) =
    interface ISection with
        member this.Heading = heading
        member this.Text = text

type Properties(heading, text) =
    interface ISection with
        member this.Heading = heading
        member this.Text = text
        
type Overview(heading, text) =
    interface ISection with
        member this.Heading = heading
        member this.Text = text

type Conditions(heading, text, conditions:string seq) =
    interface IConditions with
        member this.Heading = heading
        member this.Text = text
        member this.When = Seq.map (fun (x:string) -> x.Trim()) conditions 
       
type Links(heading, text) =
    interface ISection with
        member this.Heading = heading
        member this.Text = text
        
type Notes(heading, text) =
    interface ISection with
        member this.Heading = heading
        member this.Text = text

[<AllowNullLiteral>]
type BestPractice(title:ISection, properties:ISection, overview:ISection, conditions:IConditions, links:ISection, notes:ISection) =
    member this.Title = title
    member this.Properties = properties
    member this.Overview = overview
    member this.Conditions = conditions
    member this.Links = links
    member this.Notes = notes

module RubiconParser =
    open FParsec
    open FParsec.CharParsers

    let pCopyright = charsTillString "# " true 50000
    let pTitle = pCopyright >>. preturn "Title" .>>. regex ".+" .>> spaces >>= fun (x,y) -> preturn (Title(x,  y))

    let pPropertiesHead = pstring "## " >>. pstring "Properties" .>> spaces
    let pOverviewHead = pstring "## " >>. pstring "Overview" .>> spaces
    let pConditionsHead = pstring "## " >>. pstring "Applicability conditions" .>> spaces >>% "Conditions"
    let pLinksHead = pstring "## " >>. pstring "Additional reading links" .>> spaces >>% "Links"
    let pNotesHead = pstring "## " >>. pstring "Notes for evaluator" .>> spaces >>% "Notes"

    let pPropertiesValue = regex "[^#]+"
    let pOverviewValue = regex "[^#]+"
    
    let pCondition = pstring "* " >>. regex "[^*#]+"
    let pConditionsList = many pCondition
    let pConditionpreamble = regex "[^*#]+"
    let pConditionsValue = pConditionpreamble .>>. pConditionsList
    let pLinksValue = regex "[^#]+"
    let pNotesValue = regex ".+" .>> eof

    let pProperties = pPropertiesHead .>>. pPropertiesValue >>= fun (x,y) -> preturn (Properties(x,  y))
    let pOverview = pOverviewHead .>>. pOverviewValue >>= fun (x,y) -> preturn (Overview(x,  y))
    let pConditions = pConditionsHead .>>. pConditionsValue >>= fun (x, (y, z)) -> preturn (Conditions(x, y, z))
    let pLinks = pLinksHead .>>. pLinksValue >>= fun (x,y) -> preturn (Links(x,  y))
    let pNotes = pNotesHead .>>. pNotesValue >>= fun (x,y) -> preturn (Notes(x,  y))

    let pRubicon = pTitle .>>. pProperties .>>. pOverview .>>. pConditions .>>. pLinks .>>. pNotes  >>= fun (((((r, t), p), c), l), n) -> preturn (BestPractice(r, t, p, c, l, n))

    let Parse (practice:string) =
        run pRubicon practice

namespace SBPRubiconParser
open FParsec

type  ParseResult(success:bool, practice:BestPractice, error:string) = 
    member this.Success = success    
    member this.Practice = practice
    member this.Error = error

type Parser() =  
    member this.Parse (practice:string) =
        match (run RubiconParser.pRubicon practice) with
        | Success(result, _, _)   -> ParseResult(true, result, "")
        | Failure(errorMsg, _, _) -> ParseResult(false, null, errorMsg)
