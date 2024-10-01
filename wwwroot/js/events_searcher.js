const datePicker = $("#pick-evlog-date")
datePicker.val($.query.get("SelectedDay"))
datePicker.on("change", () => {
    var date = datePicker.val()
    console.log(date)
    window.location.search = $.query.set("SelectedDay", date)
})

function doSearchFor(text) {
    var terms = text.toLowerCase().trim().split(" ")

    console.log(terms)
    $("#evlog-table-body").children("tr").each((i, row) => {
        var match = false
        var compared = ""
        $(row).children("td[data-searchable=yes]").each((j, td) => {
            compared += $(td).text().toLowerCase()
        })

        if (terms.every(st => {
            return compared.indexOf(st) > -1
        })) {
            $(row).show()
        } else {
            $(row).hide()
        }
    })
}

const searchBox = $("#evlog-search")
var lastSearch = localStorage.getItem("CWLastSearch")
if (lastSearch !== null) {
    searchBox.val(lastSearch)
    doSearchFor(lastSearch)
}

searchBox.on("keyup", () => {
    localStorage.setItem("CWLastSearch", searchBox.val())
    doSearchFor(searchBox.val())
})