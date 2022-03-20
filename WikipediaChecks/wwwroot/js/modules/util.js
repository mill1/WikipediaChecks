export function getUrlBase() {
    //######################### E N V I R O N M E N T #########################
    var dev = false;
    //#########################################################################
    return (dev ? "https://localhost:44341/" : "http://sonictonic.nl/");
}

export function validInput(elementId) {

    const inpObj = document.getElementById(elementId);
    let validationElement = "validationMessage" + elementId;

    if (!inpObj.checkValidity()) {
        document.getElementById(validationElement).innerHTML = inpObj.validationMessage;
        return false
    }
    else {
        document.getElementById(validationElement).innerHTML = "";
    }
    return true;
}

export async function getAssemblyInfo() {
    let url_base = getUrlBase() + "assembly";

    var info = await fetchUrl(url_base);

    var divInfo = document.getElementById("assemblyInfo");
    divInfo.innerHTML = info.value;

    return info.value;
}

export async function fetchUrl(url) {

    return fetch(url)
        .then(response => response.json())
        .catch(error => alert(error))
}

export function isClickedManually(event) {

    if (!event.isTrusted) {
        event.preventDefault();
        alert('no cigar');
        return false;
    }
    return true;
}

export function createLink(href, text) {

    var link = document.createElement("a");
    link.setAttribute("href", href)
    link.setAttribute("target", "_blank");
    var linkText = document.createTextNode(text);
    link.appendChild(linkText);

    return link;
}

export function isCountCell(columnName) {

    if (columnName === 'count' ||
        columnName === 'jan' ||
        columnName === 'feb' ||
        columnName === 'mar' ||
        columnName === 'apr' ||
        columnName === 'may' ||
        columnName === 'jun' ||
        columnName === 'jul' ||
        columnName === 'aug' ||
        columnName === 'sep' ||
        columnName === 'oct' ||
        columnName === 'nov' ||
        columnName === 'dec')
    {
        return true;
    }
        
    
    return false;
}

export function createTable(children, styleClass) {

    function addHeaders(t, keys) {
        var r = t.insertRow();

        for (let key of keys) {
            var cell = r.insertCell();
            cell.appendChild(document.createTextNode(key));
        }
    }

    var table = document.createElement('table');
    table.setAttribute('class', styleClass + ' table-hover');

    for (var i = 0; i < children.length; i++) {

        var child = children[i];
        if (i === 0) {
            addHeaders(table, Object.keys(child));
        }
        var row = table.insertRow();
        Object.keys(child).forEach(function (k) {

            var cell = row.insertCell();

            if (k.toLowerCase() === 'articlename') {
                var link = createLink("https://en.wikipedia.org/wiki/" + child[k], child[k]);
                cell.appendChild(link);
            }
            else {
                cell.appendChild(document.createTextNode(child[k]));
            }
        })
    }
    return table;    
}