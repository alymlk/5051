﻿/// <reference path ="../scripts/jquery/index.d.ts"/>

/*
 * 
 * Global Variables
 * 
 */

// Track the Current Iteration.  
// Use the Iteraction Number to determine, if new data refresh is needed, a change in number means yes, refresh data
// Start at -1, so first time run, always feteches data.
var CurrentIterationNumber = -1;

// Hold the Server's Iteration
var ServerIterationNumber = 0;

// The Student Id is stored in the Dom, need to fetch it on page load
var StudentId = $("#StudentId").val();

// Refresh rate is the rate to refresh the game in miliseconds
var ServerRefreshRate = 1000;

// Game Update Timmer fires every RefeshRate
var GameUpdateTimer;

/* 
 * 
 * Data Structures 
 * 
 */

// The Data structor for the IterationNumber Data set
interface IJsonDataIterationNumberHeader{
    Error: string[];
    Msg: string[];
    Data: number;
}

// The result data
interface iJsonDataResult {
    Name: string[];
}

// The Data structor for the Result Data set Header
interface IJsonDataResultHeader {
    Error: string[];
    Msg: string[];
    Data: iJsonDataResult;
}

// The Data structor for the Result Data set Header
interface IJsonDataRefreshRateHeader {
    Error: string[];
    Msg: string[];
    Data: number;
}

/* 
 * 
 * Data Functions
 * 
 */

// Parses the Data Structure and returns the Iteration Number
function DataLoadIterationNumber(data: IJsonDataIterationNumberHeader): number {
    console.log ("Data load: " +
        " Error: " + data.Error +
        " Msg: " + data.Msg +
        " Data: " + data.Data);

    var IterationNumber = data.Data;
    return IterationNumber;
}

// Does a fetch to the server, and returns the Iteration Number
async function GetIterationNumber() {
    $.ajax("/Game/GetIterationNumber",
        {
            cache: false,
            dataType: 'json',
            type: 'POST',
            success: function (data: any) {
                // Update the Global Server Iteration Number
                ServerIterationNumber = DataLoadIterationNumber(<IJsonDataIterationNumberHeader>data);
            }
        })
        .fail(function () {
            console.log("Error Get Iteration Number");
        });
}

// Parses the Data Structure and returns the Iteration Number
function DataLoadGameResults(data: IJsonDataIterationNumberHeader): number {
    console.log("Data load: " +
        " Error: " + data.Error +
        " Msg: " + data.Msg +
        " Data: " + data.Data);

    var IterationNumber = data.Data;
    return IterationNumber;
}

// Does a fetch to the server, and returns the Iteration Number
function GetGameResults(): number {
    $.ajax("/Game/GetResults/"+StudentId,
        {
            cache: false,
            dataType: 'json',
            type: 'POST',
            async: false,
            success: function (data: any) {
                // console.log(data);
                var IterationNumber = DataLoadIterationNumber(<IJsonDataIterationNumberHeader>data);
                return IterationNumber;
            }
        })
        .fail(function () {
            console.log("Results error");
            return 0;
        });

    return 0;
}

// Get the Refresh rate for the page
// Returns the number of miliseconds to refresh
async function GetRefreshRate(){
    // Set the Global Refresh rate
    await GetGameRefreshRate();
}

// Parses the Data Structure and returns the Iteration Number
function DataLoadRefreshRate(data: IJsonDataRefreshRateHeader): number {
    console.log("Data load: " +
        " Error: " + data.Error +
        " Msg: " + data.Msg +
        " Data: " + data.Data);

    var rate = data.Data;
    return rate;
}

// Does a fetch to the server, and returns the Iteration Number
async function GetGameRefreshRate(){
    $.ajax("/Game/GetRefreshRate/",
        {
            cache: false,
            dataType: 'json',
            type: 'POST',
            success: function (data: any) {
                ServerRefreshRate = DataLoadRefreshRate(<IJsonDataRefreshRateHeader>data);
            }
        })
        .fail(function () {
            console.log("Error Get Refresh Rate");
            return;
        });
}

// Refresh the Game
async function RefreshGame() {

    // Force a call to Simulation
    await GetIterationNumber();

    // Check if Game Version > current version, if so do update sequence
    if (ServerIterationNumber> CurrentIterationNumber) {

        // Get New Data
        GetGameResults();

        // Refresh Game Display
        RefreshGameDisplay();

        // Update Iteration Number
        CurrentIterationNumber = ServerIterationNumber;
    }
}

/*
 * Application Starts Here
 * 
 * Load the Page
 * Initialize the Game
 * Do a fetch to the server to get IterationNumber, Refresh Rate
 * 
 * Set Refresh Rate on a Timmer
 * 
 * At the refresh go to the server and check for Iteration
 * 
 * If Current Iteration < Server Iteration Number, then refetch Data
 * 
 * Update Display for new Data
 * 
 */

// Get Refresh Rate
GetRefreshRate();

// Make Timmer to call refresh
setInterval(function () {
    RefreshGame();
}, ServerRefreshRate);


/*
 *
 *
 * 
 * Game Layout Starts Here
 * 
 * 
 * 
 * 
 */

// Refresh Game display
function RefreshGameDisplay() {
    // Use the current data structure

    // For all the elements in the Game, make a call and refresh them

    // Show Iteration Number (debugging)
    $("#IterationNumber").text(CurrentIterationNumber);

    // Show Game Data
    $("#GameData").text("Game Data Goes Here");
}