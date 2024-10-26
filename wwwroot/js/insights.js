$.getJSON("/DataApi/AvgActiveDaysChat", data => {
    new Chart(
        $("#AvgActiveDaysChat")[0].getContext("2d"),
        {
            type: "bar",
            data: {
                labels: [6, 5, 4, 3, 2, 1, 0].map(i => {
                    let date = new Date()
                    var dateOffset = (24*60*60*1000) * i
                    date.setTime(date.getTime() - dateOffset)
                    return date.toLocaleDateString("en-gb", { weekday: "short", day: "numeric", month: "short" })
                }),
                datasets: [
                    {
                        label: "Chat",
                        data: data
                    },
                ]
            },
            options: {
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: "Most active days"
                    }
                }
            }
        }
    )
})