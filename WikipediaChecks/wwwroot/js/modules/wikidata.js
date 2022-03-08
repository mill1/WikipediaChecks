import { getUrlBase, validInput, isClickedManually, fetchUrl, createTable } from './util.js';
import { DateArguments } from './dateArguments.js';

let url_base = getUrlBase() + "wikidata";

// voor nu alleen aangeroepen vanuit test
export async function showDeceasedOfDay(event) {

    if (!isClickedManually(event))
        return;

    if (!(validInput("inputYear") && validInput("inputMonthId") && validInput("inputDay")))
        return;

    let container = document.getElementById("container");
    container.innerHTML = "<i>please be patient...</i>";

    let deceased = await getDeceasedPerDay(new DateArguments());    
    var table = createTable(deceased, 'table-striped');

    container.innerHTML = "";
    container.appendChild(table);
}

async function getDeceasedPerDay(dateArgs) {

    let url = url_base + "/deceased/" + dateArgs.year + "-" + dateArgs.monthId + "-" + dateArgs.day;
    return fetchUrl(url);
}

