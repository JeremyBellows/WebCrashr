﻿namespace WebCrashr
open workKnob
open inputCommon
open webPageWork
open FSharp.Data
open System.IO

    module xmlInputReader =
        
        type inputXmlFormat = XmlProvider<"
        <WebCrashrConfiguration>
            <config>
                <rpmInMilliseconds>1000</rpmInMilliseconds>
                <executionTimeInMilliseconds>10000</executionTimeInMilliseconds>
                <amountOfWorkersToOrder>10</amountOfWorkersToOrder>
            </config>
            <work>
                <url>http://www.google.com</url>
		        <percentage>50</percentage>
	        </work>
            <work>
                <url>http://www.google.com</url>
		        <percentage>50</percentage>
	        </work>
        </WebCrashrConfiguration>">

        let private getXmlStringFromFile (xmlFilePath : string) =
            use streamReader = new StreamReader (xmlFilePath)
            streamReader.ReadToEnd ()

        let private getWorkListFromXml (inputXml : inputXmlFormat.WebCrashrConfiguration) = 
            inputXml.Works 
            |> Array.map (fun work -> work.Url |> webPageWork |> workKnob.createKnob <| work.Percentage) 
            |> workKnob.getWorkList
        
        let private getExecutionTimeInMsFromXml (inputXml : inputXmlFormat.WebCrashrConfiguration) =
            inputXml.Config.ExecutionTimeInMilliseconds

        let private getAmountOfWorkersToOrderFromXml (inputXml : inputXmlFormat.WebCrashrConfiguration) =
            inputXml.Config.AmountOfWorkersToOrder
                    
        let private getRpmInMillisecondsFromXml (inputXml : inputXmlFormat.WebCrashrConfiguration) =
            int64(inputXml.Config.RpmInMilliseconds)

        let getPropertiesFromXml xmlFilePath =
            let inputXml = xmlFilePath |> getXmlStringFromFile |> inputXmlFormat.Parse
            {
                workList = inputXml |> getWorkListFromXml
                executionTimeInMilliseconds =  inputXml |> getExecutionTimeInMsFromXml
                amountOfWorkersToOrder = inputXml |> getAmountOfWorkersToOrderFromXml
                rpmInMilliseconds = inputXml |> getRpmInMillisecondsFromXml
            }