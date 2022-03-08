import { getUrlBase, validInput, isClickedManually, fetchUrl, createLink, isCountCell } from './util.js';
import { DateArguments } from './dateArguments.js';

let url_base = getUrlBase() + "wikipedia";
const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

export async function showDeceasedOfDay(event) {

    if (!isClickedManually(event))
        return;
    
    if (!(validInput("inputYear") && validInput("inputMonthId") && validInput("inputDay")))
        return;

    document.getElementById("result").innerHTML = "<i>one moment...</i>";

    let deceased = await getDeceasedPerDay(new DateArguments());
    createCustomTable(deceased);
}

export async function showDeathCountsPerDay(event) {

    if (!isClickedManually(event))
        return;

    let deceased;

    if (document.getElementById("chkProcessYear").checked) {

        if (!validInput("inputYear"))
            return;

        document.getElementById("result").innerHTML = "<i>please wait...</i>";
        deceased = await getDeathsCountPerDayByYear(new DateArguments());
    }
    else {

        if (!(validInput("inputYear") && validInput("inputMonthId")))
            return;

        document.getElementById("result").innerHTML = "<i>hold on...</i>";
        deceased = await getDeathsCountPerDayByMonth(new DateArguments());
    }
    createCustomTable(deceased);
}

async function getDeathsCountPerDayByMonth(dateArgs) {
     
    let url = url_base + "/deathscountperday/" + dateArgs.year + "/" + dateArgs.monthId;
    return fetchUrl(url);
}

async function getDeathsCountPerDayByYear(dateArgs) {

    let url = url_base + "/deathscountperday/" + dateArgs.year;
    return fetchUrl(url);
}

async function getDeceasedPerDay(dateArgs) {

    let url = url_base + "/deceased/" + dateArgs.year + "-" + dateArgs.monthId + "-" + dateArgs.day;
    return fetchUrl(url);
}

// Not used
async function getDeceasedPerMonth(dateArgs) {

    let url = url_base + "/deceased/" + dateArgs.year + "-" + dateArgs.monthId;
    return fetchUrl(url);
}

function createCustomTable(children) {

    var table = document.createElement('table');

    table.setAttribute('class', 'table-striped table-hover');

    for (var i = 0; i < children.length; i++) {
        if (i === 0)
            addHeaders(table, Object.keys(children[i]));

        processChild(children[i], table);
    }

    var divResult = document.getElementById("result");
    divResult.innerHTML = "";
    divResult.appendChild(table);
}

function processChild(child, table) {
    
    var row = table.insertRow();

    Object.keys(child).forEach(function (k) {

        var cell = row.insertCell();

        if (k.toLowerCase() === 'linked_name') {
            var link = createLink("https://en.wikipedia.org/wiki/" + child[k], child[k]);
            cell.appendChild(link);
        }
        else {
            if (k === 'day')
                cell.setAttribute("style", "text-align:right;font-size: 1.2em;color:darkblue");

            if (isCountCell(k))
                SetCountCellStyle(child[k], cell);

            var textNode = document.createTextNode(child[k] === null ? "" : child[k]);
            cell.appendChild(textNode);
        }
    })
}

function SetCountCellStyle(count, cell) {

    switch (count) {
        case 0:
            cell.setAttribute("style", "background-color:purple;color:white;text-align:right");
            break;
        case 1:
            cell.setAttribute("style", "background-color:red;color:white;text-align:right");
            break;
        case 2:
            cell.setAttribute("style", "background-color:orange;text-align:right");
            break;
        case 3:
            cell.setAttribute("style", "background-color:yellow;text-align:right");
            break;
        default:
            cell.setAttribute("style", "text-align:right");
    }
}

function addHeaders(table, keys) {

    var row = table.insertRow();
    const dateArgs = new DateArguments();

    for (var i = 0; i < keys.length; i++) {

        var cell = row.insertCell();

        if (isCountCell(keys[i])) {
            cell.setAttribute("style", "width:45px;text-align:right");

            let monthId;

            if (document.getElementById("chkProcessYear").checked)
                monthId = i;
            else
                monthId = dateArgs.monthId;

            let link = createLink("https://en.wikipedia.org/wiki/Deaths_in_" + monthNames[monthId - 1] + "_" + dateArgs.year, keys[i].toUpperCase());
            cell.appendChild(link);
        }
        else if (keys[i].toLowerCase() === 'date_of_death') {

            let link = createLink("https://en.wikipedia.org/wiki/Deaths_in_" + monthNames[dateArgs.monthId - 1] + "_" + dateArgs.year + "#" + dateArgs.day, keys[i].toUpperCase());
            cell.appendChild(link);
        }
        else {
            if (keys[i] === 'day')
                cell.setAttribute("style", "text-align:right;font-size: 1.2em;color:darkblue");
        
            cell.appendChild(document.createTextNode(keys[i].toUpperCase()));
        }
    }
}
