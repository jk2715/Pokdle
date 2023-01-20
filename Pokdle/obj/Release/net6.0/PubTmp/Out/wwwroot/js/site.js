// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SearchPokemon() {
    var input = $("#searchPokemon").val().trim();
    SetInputSearchSpinner();
    console.log($.ajax.url);
    $.ajax({
        type: 'GET',
        url: 'Home/SearchPokemon?searchTerm=' + input,
        success: function (response) {
            $("#SearchPokemonDropdownList").html(response);
            ExpandDropdown("#SearchPokemonDropdownList", "#searchPokemon")
        }
    });
}

function SubmitGuess() {
    var id = $("#selectedPokemonDropdown").val();
    var seed = $("#randomSeed").val();
    $.ajax({
        type: 'GET',
        url: 'Home/TryGuess?id=' + id + "&seed=" + seed,
        success: function (response) {
            $("#guessTbody").html(response);
            $("#searchPokemon").val("");
            $("#SearchPokemonDropdownList").html("");
            if ($("#guessTbody tr").length >= 3) {
                $("#cryBtn").removeAttr("disabled");
            }
            CheckSuccess();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function SetPokemon(id, name) {
    $("#selectedPokemonDropdown").val(id);
    $("#searchPokemon").val("#" + id + ' - ' + name);
    HideDropdown();
}

$("#searchPokemon").click(function () { Toggle("SearchPokemonDropdownList", "searchPokemon"); });

/*$("#searchPokemon").on("blur", function () {
    HideDropdown();
})*/

function Toggle(id1, id2) {
    var element = $("#" + id1);
    var inpElement = $("#" + id2)
    if (inpElement.hasClass("show") || element.hasClass("show")) {
        element.removeClass("show");
        inpElement.removeClass("show")
    }
    else {
        element.addClass("show");
        inpElement.addClass("show");
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

function HideDropdown() {
    $("#SearchPokemonDropdownList").removeClass("show");
    $("#searchPokemon").removeClass("show");
}

function debounce(func, timeout = 1000) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

const pokeSearch = debounce(() => SearchPokemon());

function SetInputSearchSpinner() {
    var spinnerHtml = '<li class="text-center show" id="pokeSearchLoadingSpinner"><div class="spinner-border text-secondary" role="status">  <span class="visually-hidden">Loading...</span></div></li>';
    $("#SearchPokemonDropdownList").html(spinnerHtml);
}

function CheckSuccess() {
    if ($("#successHidden").val() == 1) {
        $("$pokeSearchDiv").remove();
        $("#submitGuess").remove();
        $("#cluesDiv").remove();
        $.ajax({
            type: 'GET',
            url: 'Home/Success?' + 'mode=' + $("#changeMode")
        });
    }
}

$(document).on("ready", function () {
    if ($("#successHidden").val() == 1) {
        startConfetti();
        var confettiTimeout = setTimeout(stopConfetti(), 1000000);
        clearTimeout(confettiTimeout);
    }
});
function playCry(pokemon) {
    var url = "https://play.pokemonshowdown.com/audio/cries/" + pokemon + ".mp3";
    var a = new Audio(url);
    a.play();
}
