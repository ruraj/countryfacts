'use strict';

var rowTemplate = "<tr><td>{nm}</td><td>{val}</td></tr>";

$(document).ready(() => {
    
    var tableBody = $("#tablebody")

    $("#dataArea").addClass("loading");

    // Let's just call the API once for now
    $.get("/Service1.svc/GetData").then(
        (data) => {
            data = JSON.parse(data.d);
            data.forEach(d => {
                var row = $(rowTemplate.replace("{nm}", d.Name).replace("{val}", d.Val));
                tableBody.append(row);
            });
            $("#dataArea").removeClass("loading");
        },
        (err) => {
            console.log("Error getting data");
        }
        );
});