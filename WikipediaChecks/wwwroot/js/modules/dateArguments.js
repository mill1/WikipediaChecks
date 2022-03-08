export class DateArguments {

    constructor() {
        this.year = document.getElementById("inputYear").value;
        this.monthId = document.getElementById("inputMonthId").value;
        this.day = document.getElementById("inputDay").value;
    }

    get year() {
        return this._year;
    }
    set year(value) {
        this._year = value;
    }
    get monthId() {
        return this._monthId;
    }
    set monthId(value) {
        this._monthId = value;
    }
    get day() {
        return this._day;
    }
    set day(value) {
        this._day = value;
    }
}