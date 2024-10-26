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
                        data: data,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(255, 159, 64, 0.2)',
                            'rgba(255, 205, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(201, 203, 207, 0.2)'
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)'
                        ],
                        borderWidth: 1
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

$.getJSON("/DataApi/AvgActiveHours", data => {
    new Chart(
        $("#AvgActiveHours")[0].getContext("2d"),
        {
            type: "bar",
            data: {
                labels: Array.from(Array(24).keys()).map(h => {
                    var d = new Date()
                    d.setUTCHours(h)
                    return d.getHours().toString() + ":00"
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
                        text: "Most active hours"
                    }
                }
            }
        }
    )
})
