const monthPicker = $("#insights-month-select")
const monthRefresher = $("#insights-refresh")
monthPicker.change(e => {
    console.log("Changed insights month to", e.target.value)
    let monthText = monthPicker.val().toString()
    if(monthText === undefined) return
    refreshInsightsCharts(monthText)
})

monthRefresher.click(e => {
    let monthText = monthPicker.val().toString()
    if(monthText === undefined) return
    refreshInsightsCharts(monthText)
})

const insightsRefreshInterval = setInterval(e => refreshInsightsCharts(monthPicker.val().toString()), 15000)
let insightsActiveCharts = []

function refreshInsightsCharts(monthBase) 
{
    insightsActiveCharts.forEach(c => c.destroy())
    insightsActiveCharts = []
    
    $.getJSON("/DataApi/AvgActiveDaysChat?BasedOnMonth=" + encodeURIComponent(monthBase), data => {
        insightsActiveCharts.push(new Chart(
            $("#AvgActiveDaysChat")[0].getContext("2d"),
            {
                type: "bar",
                data: {
                    labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"],
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
        ))
    })

    $.getJSON("/DataApi/AvgActiveHours?BasedOnMonth=" + encodeURIComponent(monthBase), data => {
        insightsActiveCharts.push(new Chart(
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
        ))
    })
}
