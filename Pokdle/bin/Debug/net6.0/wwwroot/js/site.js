// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SearchPokemon(expand) {
    var input = $("#searchPokemon").val().trim().toLowerCase();
    var generation = $("#GenSelectDropdownButton").html();
    SetInputSearchSpinner();
    $.ajax({
        type: 'GET',
        url: 'Main/SearchPokemon?searchTerm=' + input + '&seed=' + $("#randomSeed").val() + '&generation=' + generation,
        success: function (response) {
            $("#SearchPokemonDropdownList").html(response);
            if (expand) {
                ExpandDropdown("#SearchPokemonDropdownList", "#searchPokemon")
            }            
        }
    });
}

function SubmitGuess() {
    var id = $("#selectedPokemonDropdown").val();
    var seed = $("#randomSeed").val();
    $.ajax({
        type: 'GET',
        url: 'Main/TryGuess?id=' + id + "&seed=" + seed,
        success: function (response) {
            $("#guessTbody").html(response);
            $("#selectedPokemonDropdown").val("0");
            $("#SearchPokemonDropdownList").html("");
            $("#searchPokemon").val("");
            var triesLeft = 3 - $("#guessTbody tr").length;
            if (triesLeft > 1) {
                $("#triesLeft").text(triesLeft + " tries left to unlock this clue")
            }
            else {
                $("#triesLeft").text(triesLeft + " try left to unlock this clue")
            }
            if ($("#guessTbody tr").length >= 3) {
                $("#cryBtn").removeAttr("disabled");
                $("#cryBtn").addClass("btn-primary");
                $("#triesLeft").remove();
            }
            SearchPokemon(false);
            CheckSuccess();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function CheckSuccess() {
    if ($("#isSuccess").val() == 1) {
        location.reload();
        startConfetti();
      //  var confettiTimeout = setTimeout(stopConfetti(), 1000000);
     //   clearTimeout(confettiTimeout);
    }
}

function SetPokemon(id, name) {
    $("#selectedPokemonDropdown").val(id);
    $("#searchPokemon").val("#" + id + ' - ' + name);
    HideDropdown("SearchPokemonDropdownList", "searchPokemon");
}

function SetGeneration(gen) {
    $("#GenSelectDropdownButton").html(gen);
  /*  $("#GenSelectDropdown").find().each(function (index, element) {
        if ($(element).hasClass("active")) {
            $(element).removeClass("active");
        }
    });
    $("#generation-" + gen).addClass("active"); */
    SearchPokemon(false);
    HideDropdown("GenSelectDropdown", "GenSelectDropdownButton");
}

$("#searchPokemon").click(function () { Toggle("SearchPokemonDropdownList"); Toggle("searchPokemon"); });
$("#GenSelectDropdownButton").click(function () { Toggle("GenSelectDropdown"); HideDropdown("SearchPokemonDropdownList", "searchPokemon") });

/*$("#searchPokemon").on("blur", function () {
    HideDropdown();
})*/

function Toggle(id1) {
    var element = $("#" + id1);
    if (element.hasClass("show")) {
        element.removeClass("show");
    }
    else {
        element.addClass("show");
    }
}

function ExpandDropdown(dropdownId, inputId) {
    if (!$(dropdownId).hasClass("show")) {
        $(dropdownId).addClass("show");
        $(inputId).addClass("show");
        $(dropdownId).css("position", "absolute");
        $(dropdownId).css("inset", "0px auto auto 0px");
        $(dropdownId).css("margin", "0px");
        $(dropdownId).css("transform", "translate(0px, 36px)");
    }
}

function HideDropdown(id1, id2) {
    $("#" + id1).removeClass("show");
    $("#" + id2).removeClass("show");
}

function debounce(func, timeout = 1000) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const pokeSearch = debounce(() => SearchPokemon(true));

function SetInputSearchSpinner() {
    var spinnerHtml = '<li class="text-center show" id="pokeSearchLoadingSpinner"><div class="spinner-border text-secondary" role="status">  <span class="visually-hidden">Loading...</span></div></li>';
    $("#SearchPokemonDropdownList").html(spinnerHtml);
}

function playCry(pokemon) {
    var url = "https://play.pokemonshowdown.com/audio/cries/" + pokemon + ".mp3";
    var a = new Audio(url);
    a.play();
}

function GetSuccessLink(tries) {
    var end = "";
    if (tries == 1) {
        end = "try";
    }
    else {
        end = "tries";
    }
    var seed = $("#randomSeed").val();
    var mode = "Pokemon of the Day"
    if (seed != "0") {
        mode = "Random Pokemon"
    }
    var html = "I guessed the #Pokdle " + mode + " in " + tries + " " + end;
    var rows = $("#guessTbody tr");
    for (var i = 0; i < rows.length; i++) {
        html += '<br/>';
        $(rows[i]).children().each(function (index, tableCell) {
            if ($(tableCell).find('img').length > 0) {
                var img = $(tableCell).find('img')[0]
                var imgName = $(img).attr('src');
                if (imgName.includes("up")) {
                    html += "&#x1F53C;";
                }
                else if (imgName.includes("down")) {
                    html += "&#x1F53D;";
                }
            }
            else {
                if ($(tableCell).hasClass("greencard")) {
                    html += "&#x1F7E2;";
                }
                else if ($(tableCell).hasClass("yellowcard")) {
                    html += "&#x1F7E1;";
                }
                else if ($(tableCell).hasClass("redcard")) {
                    html += "&#x1F534;";
                }  
            }     
            html += "&nbsp;";
        });
    }
    return html += "<br/> <br/>" + $("#baseUrl").val();
}

$("#copyButton").click(function copyFormatted() {
    // Create container for the HTML
    // [1]
    var tries = $("#guessCount").val();
    var html = GetSuccessLink(tries)
    var container = document.createElement('div')
    container.innerHTML = html

    // Hide element
    // [2]
    container.style.position = 'fixed'
    container.style.pointerEvents = 'none'
    container.style.opacity = 0

    // Detect all style sheets of the page
    var activeSheets = Array.prototype.slice.call(document.styleSheets)
        .filter(function (sheet) {
            return !sheet.disabled
        })

    // Mount the container to the DOM to make `contentWindow` available
    // [3]
    document.body.appendChild(container)

    // Copy to clipboard
    // [4]
    window.getSelection().removeAllRanges()

    var range = document.createRange()
    range.selectNode(container)
    window.getSelection().addRange(range)

    // [5.1]
    document.execCommand('copy')

    // [5.2]
    for (var i = 0; i < activeSheets.length; i++) activeSheets[i].disabled = true

    // [5.3]
    document.execCommand('copy')

    // [5.4]
    for (var i = 0; i < activeSheets.length; i++) activeSheets[i].disabled = false

    // Remove the container
    // [6]
    document.body.removeChild(container)
});

$(document).ready(function () {
    if ($("#isSuccess").val() == 1) {
        startConfetti();


        // Edit given parameters

        //  var confettiTimeout = setTimeout(stopConfetti(), 1000000);
        //   clearTimeout(confettiTimeout);
    }
});