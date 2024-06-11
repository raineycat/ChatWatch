$.getJSON("/DataApi/DailyMessageCount", dmcData => {
    new Chart(
        $("#chartMessageTypesToday")[0].getContext("2d"),
        {
            type: "doughnut",
            data: {
                labels: [ "Chat messages", "Private messages" ],
                datasets: [{
                    label: "Daily",
                    data: [dmcData.numChatMessages, dmcData.numPrivateMessages],
                    backgroundColor: [
                        'rgb(54, 162, 235)',
                        'rgb(255, 205, 86)'
                    ],
                    hoverOffset: 5
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: "Messages sent today"
                    }
                }
            }
        }
    )
})

$.getJSON("/DataApi/WeeklyMessageCount", wmcData => {
    new Chart(
        $("#chartMessageTypesWeekly")[0].getContext("2d"),
        {
            type: "line",
            data: {
                labels: [6, 5, 4, 3, 2, 1, 0].map(i => {
                    let date = new Date()
                    var dateOffset = (24*60*60*1000) * i
                    date.setTime(date.getTime() - dateOffset)
                    return date.toLocaleDateString("en-gb", { weekday: "short", day: "numeric", month: "short" })
                }),
                datasets: [
                    {
                        label: "Combined",
                        data: wmcData.map(e => e.numChatMessages + e.numPrivateMessages),
                        tension: 0.1
                    },
                    {
                        label: "Chat",
                        data: wmcData.map(e => e.numChatMessages),
                        tension: 0.1
                    },
                    {
                        label: "Private",
                        data: wmcData.map(e => e.numPrivateMessages),
                        tension: 0.1
                    },
                ]
            },
            options: {
                maintainAspectRatio: false,
                plugins: {
                    title: {
                        display: true,
                        text: "Messages sent per weekday"
                    }
                }
            }
        }
    )
})

$.getJSON("/DataApi/MostActivePlayers", mapData => {
    new Chart(
        $("#chartMostActivePlayers")[0].getContext("2d"),
        {
            type: "doughnut",
            data: {
                labels: Object.keys(mapData),
                datasets: [
                    {
                        label: "Total messages",
                        data: Object.values(mapData).map(v => v.numChatMessages + v.numPrivateMessages),
                        hoverOffset: 5
                    }
                    // {
                    //     label: "Chat",
                    //     data: Object.values(mapData).map(v => v.numChatMessages),
                    //     hoverOffset: 5
                    // },
                    // {
                    //     label: "Private",
                    //     data: Object.values(mapData).map(v => v.numPrivateMessages),
                    //     hoverOffset: 5
                    // }
                ]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: "Most active players this month"
                    }
                }
            }
        }
    )
})
